using UnityEngine;

[CreateAssetMenu(menuName = "Boids/PredatorSettings")]
public class PredatorAgentSettings : ScriptableObject
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

    public LayerMask obstaclesLayer;
}
