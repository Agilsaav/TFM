using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject waveObject;
    [SerializeField] int numberOfWaves = 5;
    [SerializeField] float secondsBetweenWaves = 0.5f;
    [SerializeField] bool activateCollisions = false;
    [SerializeField] float timeBetweenWavesCollision = 5.0f;
    [SerializeField] bool SpawnInTime = false;
    [SerializeField] float secondsBetweenSpawn = 3.0f;

    float timer = 0.0f;

    private void Start()
    {
        if(activateCollisions) timer = timeBetweenWavesCollision;
        if (SpawnInTime) timer = secondsBetweenSpawn;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(!activateCollisions && !SpawnInTime)
                StartCoroutine(SpawnWaves());
        }

        if(SpawnInTime && timer >= secondsBetweenSpawn)
        {
            timer = 0.0f;
            StartCoroutine(SpawnWaves());          
        }

        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activateCollisions && timer >= timeBetweenWavesCollision)
        {
            timer = 0.0f;
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        for (int i = 0; i < numberOfWaves; i++)
        {
            Instantiate(waveObject, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(secondsBetweenWaves);
        }
        
    }
}
