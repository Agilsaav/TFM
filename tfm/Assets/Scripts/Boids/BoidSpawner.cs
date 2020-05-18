﻿using UnityEngine;


//Boid Spawner class used to spawn boids randomly in a radius.
namespace Boids
{
    public class BoidSpawner : MonoBehaviour
    {
        [SerializeField] int numberOfAgents = 10;
        [SerializeField] float spawnRadius = 10.0f;
        [SerializeField] BoidAgent agent;

        private void Awake()
        {
            for (int i = 0; i < numberOfAgents; i++)
            {
                Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
                BoidAgent agentInst = Instantiate(agent, pos, Quaternion.identity);
                agentInst.transform.parent = gameObject.transform;
                agentInst.transform.forward = Random.insideUnitSphere;
            }
        }
    }
}
