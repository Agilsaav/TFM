using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAgent : MonoBehaviour
{
    BoidAgentSettings agentSettings;

    Transform agentTransform;
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

    public void ComputeAgentBehavior(int NNCount, int avoidCount, int predCount, Vector3 alignmentMove, Vector3 avoidanceMove, Vector3 cohesionMove, Vector3 runDirection)
    {
        Vector3 acceleration = Vector3.zero;

        acceleration += behaviors.ComputeAlignmentBehavior(NNCount, alignmentMove, agentSettings.alignmentWeight);
        if(NNCount != 0) acceleration += behaviors.ComputeCohesionBehavior(cohesionMove, position,forwardDirection, cohesionCurrVel, agentSettings.cohesionWeight, agentSettings.cohesionSmoothTime);
        if(avoidCount != 0) acceleration += behaviors.ComputeSeparationBehavior(avoidanceMove, agentSettings.avoidanceWeight);

        acceleration += behaviors.ComputeRunFromPredatorBehavior(predCount, runDirection, agentSettings.predatorWeight);

        acceleration += behaviors.ComputeEnvironmentColisionsBehavior(position, forwardDirection, agentSettings.collisionWeight, agentSettings.collisionAvoidDistance, agentSettings.sphereCastRadius, agentSettings.obstaclesLayer);

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


    //private Vector3 ComputeSeparationBehavior(Vector3 avoidanceMove)
    //{
    //    Vector3 avoidanceAcceleration = avoidanceMove * agentSettings.avoidanceWeight;
    //    return LimitMove(avoidanceAcceleration, agentSettings.avoidanceWeight);
    //}

    //private Vector3 ComputeAlignmentBehavior(int NNCount, Vector3 alignmentMove)
    //{
    //    Vector3 alignmentAcceleration;
    //    if (NNCount != 0) alignmentAcceleration =  alignmentMove;
    //    else alignmentAcceleration = transform.forward;

    //    alignmentAcceleration *= agentSettings.alignmentWeight;
    //    return LimitMove(alignmentAcceleration, agentSettings.alignmentWeight);
    //}

    //private Vector3 ComputeCohesionBehavior(Vector3 cohesionMove)
    //{
    //    Vector3 cohesionAcceleration = cohesionMove - position; 
    //    cohesionAcceleration = Vector3.SmoothDamp(forwardDirection, cohesionAcceleration, ref cohesionCurrVel, agentSettings.cohesionSmoothTime);
    //    cohesionAcceleration *= agentSettings.cohesionWeight;

    //    return LimitMove(cohesionAcceleration, agentSettings.cohesionWeight);
    //}

    //private Vector3 ComputeEnvironmentColisionsBehavior()
    //{
    //    //Check if it is going to collide
    //    RaycastHit hit;
    //    if (Physics.SphereCast(position, agentSettings.sphereCastRadius, forwardDirection, out hit, agentSettings.collisionAvoidDistance, agentSettings.obstaclesLayer))
    //    {
    //        //Cast rays to find new direction
    //        Vector3[] rayDirections = UniformSpherePointsGenerator.directions;
    //        Vector3 noColisionDir = forwardDirection;

    //        for (int i = 0; i < rayDirections.Length; i++)
    //        {
    //            Vector3 direction = transform.TransformDirection(rayDirections[i]); //transform to world space
    //            Ray ray = new Ray(position, direction);
    //            if (!Physics.SphereCast(ray, agentSettings.sphereCastRadius, agentSettings.collisionAvoidDistance, agentSettings.obstaclesLayer))
    //            {
    //                noColisionDir = direction;
    //                break;
    //            }
    //        }

    //        noColisionDir *= agentSettings.collisionWeight;
    //        return LimitMove(noColisionDir, agentSettings.collisionWeight);
    //    }
    //    else return Vector3.zero;
    //}

    //private Vector3 LimitMove(Vector3 Move, float weight)
    //{
    //    if (Move != Vector3.zero && (Move.magnitude > weight * weight))
    //        return (Move.normalized * weight);
    //    else return Move;
    //}
}
