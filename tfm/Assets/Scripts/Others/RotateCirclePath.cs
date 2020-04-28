using UnityEngine;

public class RotateCirclePath : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    void Update()
    {
        transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f, Space.Self);
    }
}
