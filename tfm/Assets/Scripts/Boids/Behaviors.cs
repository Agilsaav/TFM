using UnityEngine;

namespace Boids
{
    public class Behaviors : MonoBehaviour
    {
        public Vector3 ComputeSeparationBehavior(Vector3 avoidanceMove, float avoidanceWeight)
        {
            Vector3 avoidanceAcceleration = avoidanceMove * avoidanceWeight;
            return LimitMove(avoidanceAcceleration, avoidanceWeight);
        }

        public Vector3 ComputeAlignmentBehavior(int NNCount, Vector3 alignmentMove, float alignmentWeight)
        {
            Vector3 alignmentAcceleration;
            if (NNCount != 0) alignmentAcceleration = alignmentMove;
            else alignmentAcceleration = transform.forward;

            alignmentAcceleration *= alignmentWeight;
            return LimitMove(alignmentAcceleration, alignmentWeight);
        }

        public Vector3 ComputeCohesionBehavior(Vector3 cohesionMove, Vector3 position, Vector3 forwardDirection, Vector3 cohesionCurrVel, float cohesionWeight, float smoothTime)
        {
            Vector3 cohesionAcceleration = cohesionMove - position;
            cohesionAcceleration = Vector3.SmoothDamp(forwardDirection, cohesionAcceleration, ref cohesionCurrVel, smoothTime);
            cohesionAcceleration *= cohesionWeight;

            return LimitMove(cohesionAcceleration, cohesionWeight);
        }

        public Vector3 ComputeRunFromPredatorBehavior(int predNN, Vector3 runDirection, float predWeight)
        {
            if (predNN != 0)
            {
                Vector3 predMove = runDirection * predWeight;
                return LimitMove(predMove, predWeight);
            }
            else return new Vector3(0.0f, 0.0f, 0.0f);
        }

        public Vector3 ComputeEnvironmentColisionsBehavior(Vector3 position, Vector3 forwardDirection, float collisionWeight, float colAvoidDist, float spehereRadius, LayerMask layer)
        {
            //Check if it is going to collide
            RaycastHit hit;
            if (Physics.SphereCast(position, spehereRadius, forwardDirection, out hit, colAvoidDist, layer))
            {
                //Cast rays to find new direction
                Vector3[] rayDirections = UniformSpherePointsGenerator.directions;
                Vector3 noColisionDir = forwardDirection;

                for (int i = 0; i < rayDirections.Length; i++)
                {
                    Vector3 direction = transform.TransformDirection(rayDirections[i]); //transform to world space
                    Ray ray = new Ray(position, direction);
                    if (!Physics.SphereCast(ray, spehereRadius, colAvoidDist, layer))
                    {
                        noColisionDir = direction;
                        break;
                    }
                }

                noColisionDir *= collisionWeight;
                return LimitMove(noColisionDir, collisionWeight);
            }
            else return Vector3.zero;
        }

        private Vector3 LimitMove(Vector3 Move, float weight)
        {
            if (Move != Vector3.zero && (Move.magnitude > weight * weight))
                return (Move.normalized * weight);
            else return Move;
        }
    }

}
