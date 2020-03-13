using UnityEngine;

namespace WavesBehavior
{
    public class HumanSoundSpawner : MonoBehaviour
    {
        [SerializeField] HumanSoundBehavior humanSoundBehavior;
        [SerializeField] float spawnRadius = 10.0f;

        public void InstantiateSound()
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            HumanSoundBehavior agentInst = Instantiate(humanSoundBehavior, pos, Quaternion.identity);
            agentInst.transform.parent = gameObject.transform;
        }
    }
}
