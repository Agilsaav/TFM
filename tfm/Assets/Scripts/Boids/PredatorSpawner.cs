using UnityEngine;

namespace Boids
{
    public class PredatorSpawner : MonoBehaviour
    {
        [SerializeField] int numberOfAgents = 10;
        [SerializeField] float spawnRadius = 10.0f;
        [SerializeField] PredatorAgent agent;

        private void Awake()
        {
            for (int i = 0; i < numberOfAgents; i++)
            {
                Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
                PredatorAgent agentInst = Instantiate(agent, pos, Quaternion.identity);
                agentInst.transform.parent = gameObject.transform;
                agentInst.transform.forward = Random.insideUnitSphere;
            }
        }
    }
}
