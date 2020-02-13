using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    [SerializeField] ComputeShader computeShader;
    [SerializeField] BoidAgentSettings settings;

    BoidAgent[] boids;
    int numBoidAgents;

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
        boids = FindObjectsOfType<BoidAgent>();
        numBoidAgents = boids.Length;

        foreach(BoidAgent boidAgent in boids)
        {
            boidAgent.InitializeAgent(settings);
        }
    }

    private void Update()
    {
        agentDataBuffer[] buffer = new agentDataBuffer[numBoidAgents];

        for (int i = 0; i < numBoidAgents; i++)
        {
            buffer[i].position = boids[i].Position;
            buffer[i].forwardDir = boids[i].ForwardDir;
        }

        //Set compute shader properties and data:
        int size = 3 * 5 * sizeof(float) + 2*sizeof(int); //size of data buffer

        var computeBuffer = new ComputeBuffer(numBoidAgents, size);
        computeBuffer.SetData(buffer);

        computeShader.SetBuffer(0, "boidsData", computeBuffer);
        computeShader.SetInt("numAgents", numBoidAgents);
        computeShader.SetFloat("viewRadius", settings.viewRadius);
        computeShader.SetFloat("avoidRadius", settings.avoidRadius);

        int threadGroups = Mathf.CeilToInt(numBoidAgents / (float)threadGroupSize);
        computeShader.Dispatch(0, threadGroups, 1, 1);

        computeBuffer.GetData(buffer);

        for (int i = 0; i<numBoidAgents; i++)
        {        
            boids[i].ComputeAgentBehavior(buffer[i].NNCount, buffer[i].avoidCount, buffer[i].alignmentMove, buffer[i].avoidanceMove, buffer[i].cohesionMove); //Compute vel
            boids[i].UpdateAgent(); 

        }

        computeBuffer.Release();
    }

}
