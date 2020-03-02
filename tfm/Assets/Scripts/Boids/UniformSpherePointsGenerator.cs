using UnityEngine;

/* Algorithm to generate the directions of the rays using a uniform distribution of points in a spehere.
 * Based on: https://stackoverflow.com/questions/9600801/evenly-distributing-n-points-on-a-sphere ,
 *           https://github.com/SebLague/Boids/blob/master/Assets/Scripts/BoidHelper.cs        
 */

namespace Boids
{
    public static class UniformSpherePointsGenerator
    {
        const int numberOfPoints = 300;
        public static readonly Vector3[] directions;

        static UniformSpherePointsGenerator()
        {
            directions = new Vector3[UniformSpherePointsGenerator.numberOfPoints];

            float goldenRatio = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
            float angleIncrement = Mathf.PI * 2.0f * goldenRatio;

            for (int i = 0; i < numberOfPoints; i++)
            {
                float radius = (float)i / numberOfPoints;
                float phi = Mathf.Acos(1.0f - 2.0f * radius);
                float theta = angleIncrement * (float)i;

                directions[i] = new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Sin(phi) * Mathf.Sin(theta), Mathf.Cos(phi));
            }
        }
    }
}
