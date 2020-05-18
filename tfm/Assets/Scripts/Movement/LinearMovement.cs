using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class not used: Moves an object in a direction.
public class LinearMovement : MonoBehaviour
{
    [SerializeField] float distanceToMove = 5.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] Vector3 direction = new Vector3(1.0f, 0.0f, 0.0f);

    Vector3 initialPos;
    Vector3 lastPos;
    float initialPosDir;
    float distanceMoved;


    private void Start()
    {
        initialPos = transform.position;
        initialPosDir = Vector3.Dot(initialPos, direction);
        distanceMoved = 0.0f;
    }

    void Update()
    {
        lastPos = transform.position;

        if (Vector3.Dot(transform.position, direction) - initialPosDir < distanceToMove || distanceMoved < distanceToMove)
        {
            transform.position += direction * speed * Time.deltaTime;
            distanceMoved += Vector3.Distance(transform.position, lastPos);
        }           
        else 
        {
            distanceMoved = 0.0f;
            direction = -direction;
            transform.position -= direction * speed * Time.deltaTime;
            //distanceMoved += Vector3.Distance(transform.position, lastPos);
        }


    }
}
