using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple class to use the mouse to move the camer. Used for test only when the VR Hardware was not available.
public class MouseInput : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    float yaw = -90.0f;
    float pitch = 0.0f;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        yaw += speed * Input.GetAxis("Mouse X");
        pitch -= speed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
