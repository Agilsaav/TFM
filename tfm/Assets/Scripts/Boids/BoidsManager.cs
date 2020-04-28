using UnityEngine;


namespace Boids
{
    public class BoidsManager : MonoBehaviour
    {
        [SerializeField] ComputeShader computeShader;
        [SerializeField] BoidAgentSettings settings;

        BoidAgent[] boids;
        PredatorAgent[] predators;
        int numBoidAgents, numPredAgents;
        bool bTarget = false;

        const int threadGroupSize = 1024;

        //Compute Buffer:
        public struct agentDataBuffer
        {
            public int NNCount;
            public int avoidCount;
            public int predNN;

            public Vector3 position;
            public Vector3 forwardDir;

            public Vector3 alignmentMove; //Mean of directions
            public Vector3 cohesionMove;  //Mean of positions
            public Vector3 avoidanceMove; //Mean of difference between Position
            public Vector3 runDirection;
        }

        public struct predDataBuffer
        {
            public Vector3 position;
        }

        public void SetTarget(bool active)
        {
            bTarget = active;
        }

        private void Start()
        {
            boids = FindObjectsOfType<BoidAgent>();
            predators = FindObjectsOfType<PredatorAgent>();
            numBoidAgents = boids.Length;
            numPredAgents = predators.Length;

            foreach (BoidAgent boidAgent in boids)
            {
                boidAgent.InitializeAgent(settings);
            }
        }

        private void Update()
        {
            agentDataBuffer[] buffer = new agentDataBuffer[numBoidAgents];
            predDataBuffer[] predBuffer = new predDataBuffer[numPredAgents];

            for (int i = 0; i < numBoidAgents; i++)
            {
                buffer[i].position = boids[i].Position;
                buffer[i].forwardDir = boids[i].ForwardDir;
            }

            for (int j = 0; j < numPredAgents; j++)
            {
                predBuffer[j].position = predators[j].Position;
            }

            //Set compute shader properties and data:
            int size = 3 * 6 * sizeof(float) + 3 * sizeof(int); //size of data buffer
            int size2 = 3 * sizeof(float);

            ComputeBuffer computeBuffer = new ComputeBuffer(numBoidAgents, size);
            computeBuffer.SetData(buffer);

            ComputeBuffer computeBuffer2 = null;

            if (numPredAgents > 0)
            {
                computeBuffer2 = new ComputeBuffer(numPredAgents, size2);
                computeBuffer2.SetData(predBuffer);
                computeShader.SetBuffer(0, "predData", computeBuffer2);
            }

            computeShader.SetBuffer(0, "boidsData", computeBuffer);
            computeShader.SetInt("numAgents", numBoidAgents);
            computeShader.SetInt("numPred", numPredAgents);
            computeShader.SetFloat("predRadius", settings.predatorRadius);
            computeShader.SetFloat("viewRadius", settings.viewRadius);
            computeShader.SetFloat("avoidRadius", settings.avoidRadius);

            int threadGroups = Mathf.CeilToInt(numBoidAgents / (float)threadGroupSize);
            computeShader.Dispatch(0, threadGroups, 1, 1);

            computeBuffer.GetData(buffer);

            for (int i = 0; i < numBoidAgents; i++)
            {
                boids[i].ComputeAgentBehavior(buffer[i].NNCount, buffer[i].avoidCount, buffer[i].predNN, buffer[i].alignmentMove, buffer[i].avoidanceMove, buffer[i].cohesionMove, buffer[i].runDirection, bTarget); //Compute vel
                boids[i].UpdateAgent();
            }

            computeBuffer.Release();
            if (numPredAgents > 0) computeBuffer2.Release();
        }
    }
}
