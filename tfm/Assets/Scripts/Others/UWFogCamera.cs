using UnityEngine;

public class UWFogCamera : MonoBehaviour
{
    //Tell camera to render its depths:
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

}
