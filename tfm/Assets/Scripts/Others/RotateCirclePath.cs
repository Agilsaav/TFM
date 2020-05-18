using UnityEngine;


//Simple class to rotate the Circle Prefab used for the Path Element.
public class RotateCirclePath : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    void Update()
    {
        transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f, Space.Self);
    }
}
