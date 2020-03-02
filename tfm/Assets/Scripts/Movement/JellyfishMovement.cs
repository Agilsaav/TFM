using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishMovement : MonoBehaviour
{
    [SerializeField] float speed = 0.1f;
    [SerializeField] float maxMov = 2.0f;


    float maxPosition, minPosition;

    void Start()
    {
        RandomSign();
        maxPosition = transform.position.y + maxMov;
        minPosition = transform.position.y - maxMov;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position.y < minPosition || transform.position.y > maxPosition) speed = -speed;

        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void RandomSign()
    {
        float rand = Random.Range(0.0f, 1.0f);
        if (rand < 0.5f) speed = -speed;
    }
}
