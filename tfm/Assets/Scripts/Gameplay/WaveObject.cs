using System.Collections;
using WavesBehavior;
using UnityEngine;

public class WaveObject : MonoBehaviour
{
    MeshRenderer mesh;
    bool active = false;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wave" && !active )
        {
            if(other.GetComponent<WaveBehavior>().GetMainWaveBool())
            {
                mesh.enabled = true;
                active = true;
            }
        }
    }
}
