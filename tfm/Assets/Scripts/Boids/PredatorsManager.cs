using UnityEngine;

namespace Boids
{
    public class PredatorsManager : MonoBehaviour
    {
        [SerializeField] ComputeShader computeShader;
        [SerializeField] PredatorAgentSettings settings;

        PredatorAgent[] predators;
        int numPredAgents;

        const int threadGroupSize = 1024;

        //Compute Buffer:
        public struct agentDataBuffer
        {
            public int NNCount;
            public int avoidCount;

            public Vector3 position;
            public Vector3 forwardDir;

            public Vector3 alignmentMove; //Mean of directions
            public Vector3 cohesionMove;  //Mean of positions
            public Vector3 avoidanceMove; //Mean of difference between Position

        }

        private void Start()
        {
            predators = FindObjectsOfType<PredatorAgent>();
            numPredAgents = predators.Length;

            foreach (PredatorAgent predAgent in predators)
            {
                predAgent.InitializeAgent(settings);
            }
        }

        private void Update()
        {
            agentDataBuffer[] buffer = new agentDataBuffer[numPredAgents];

            for (int i = 0; i < numPredAgents; i++)
            {
                buffer[i].position = predators[i].Position;
                buffer[i].forwardDir = predators[i].ForwardDir;
            }

            //Set compute shader properties and data:
            int size = 3 * 5 * sizeof(float) + 2 * sizeof(int); //size of data buffer

            var computeBuffer = new ComputeBuffer(numPredAgents, size);
            computeBuffer.SetData(buffer);

            computeShader.SetBuffer(0, "boidsData", computeBuffer);
            computeShader.SetInt("numAgents", numPredAgents);
            computeShader.SetFloat("viewRadius", settings.viewRadius);
            computeShader.SetFloat("avoidRadius", settings.avoidRadius);

            int threadGroups = Mathf.CeilToInt(numPredAgents / (float)threadGroupSize);
            computeShader.Dispatch(0, threadGroups, 1, 1);

            computeBuffer.GetData(buffer);

            for (int i = 0; i < numPredAgents; i++)
            {
                predators[i].ComputeAgentBehavior(buffer[i].NNCount, buffer[i].avoidCount, buffer[i].alignmentMove, buffer[i].avoidanceMove, buffer[i].cohesionMove); //Compute vel
                predators[i].UpdateAgent();
            }

            computeBuffer.Release();
        }
    }
}
