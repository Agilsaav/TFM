using System;
using UnityEngine;

namespace Boids
{
    public class BoidAgent : MonoBehaviour
    {
        BoidAgentSettings agentSettings;

        Transform agentTransform, target;
        Behaviors behaviors;
        Vector3 position, velocity, forwardDirection;
        Vector3 cohesionCurrVel;

        public Vector3 ForwardDir  // property
        {
            get { return forwardDirection; }
            set { forwardDirection = value; }
        }

        public Vector3 Position  // property
        {
            get { return position; }
            set { position = value; }
        }

        private void Awake()
        {
            target = Camera.main.transform;
            agentTransform = transform;
            behaviors = GetComponent<Behaviors>();
        }

        public void InitializeAgent(BoidAgentSettings settings)
        {
            agentSettings = settings;
            position = agentTransform.position;
            forwardDirection = agentTransform.forward;
            velocity = ((settings.minSpeed + settings.maxSpeed) / 2.0f) * forwardDirection;
        }

        public void ComputeAgentBehavior(int NNCount, int avoidCount, int predCount, Vector3 alignmentMove, Vector3 avoidanceMove, Vector3 cohesionMove, Vector3 runDirection, bool bTarget)
        {
            Vector3 acceleration = Vector3.zero;

            acceleration += behaviors.ComputeAlignmentBehavior(NNCount, alignmentMove, agentSettings.alignmentWeight);
            if (NNCount != 0) acceleration += behaviors.ComputeCohesionBehavior(cohesionMove, position, forwardDirection, cohesionCurrVel, agentSettings.cohesionWeight, agentSettings.cohesionSmoothTime);
            if (avoidCount != 0) acceleration += behaviors.ComputeSeparationBehavior(avoidanceMove, agentSettings.avoidanceWeight);

            acceleration += behaviors.ComputeRunFromPredatorBehavior(predCount, runDirection, agentSettings.predatorWeight);

            acceleration += behaviors.ComputeEnvironmentColisionsBehavior(position, forwardDirection, agentSettings.collisionWeight, agentSettings.collisionAvoidDistance, agentSettings.sphereCastRadius, agentSettings.obstaclesLayer);

            if (bTarget) acceleration += behaviors.ComputeTargetBehavior(target.position, transform.position, agentSettings.targetDist, agentSettings.targetWeight);

            velocity += acceleration * Time.deltaTime;
        }

        public void UpdateAgent()
        {
            //Update agent position and velocity        
            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, agentSettings.minSpeed, agentSettings.maxSpeed);
            velocity = dir * speed;

            transform.position += velocity * Time.deltaTime;
            transform.forward = dir;
            position = transform.position;
            forwardDirection = transform.forward;
        }
    }      
}
