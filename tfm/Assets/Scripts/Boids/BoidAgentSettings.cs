﻿using UnityEngine;

namespace Boids
{
    [CreateAssetMenu(menuName = "Boids/AgentSettings")]
    public class BoidAgentSettings : ScriptableObject
    {
        // Settings of a Boid Agent
        public float minSpeed = 0.0f;
        public float maxSpeed = 5.0f;
        public float viewRadius = 2.5f;
        public float avoidRadius = 1.0f;

        public float avoidanceWeight = 1.0f;
        public float alignmentWeight = 1.0f;
        public float cohesionWeight = 1.0f;
        public float cohesionSmoothTime = 0.5f;

        public float collisionWeight = 1.0f;
        public float collisionAvoidDistance = 2.0f;
        public float sphereCastRadius = 0.1f;

        public float predatorRadius = 5.0f;
        public float predatorWeight = 1.0f;

        public float targetDist = 50.0f;
        public float targetWeight = 1.0f;

        public LayerMask obstaclesLayer;
    }
}
