using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//VR movement class. Moves all the CameraRig with the axis input.
namespace Movement
{
    public class VRMovement : MonoBehaviour
    {
        [SerializeField] float speed = 0.5f;

        Transform cameraTransform;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        public void Move(Vector2 axis)
        {
            Vector3 move = axis.y * cameraTransform.forward + axis.x * cameraTransform.right;
            transform.position += move * speed * Time.deltaTime;
        }
    }
}
