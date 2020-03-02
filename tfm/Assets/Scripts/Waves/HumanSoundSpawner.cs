using UnityEngine;

namespace WavesBehavior
{
    public class HumanSoundSpawner : MonoBehaviour
    {
        [SerializeField] HumanSoundBehavior humanSoundBehavior;
        [SerializeField] float spawnRadius = 10.0f;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                InstantiateSound();
            }
        }

        private void InstantiateSound()
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            HumanSoundBehavior agentInst = Instantiate(humanSoundBehavior, pos, Quaternion.identity);
            agentInst.transform.parent = gameObject.transform;
        }
    }
}
