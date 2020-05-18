using UnityEngine;
using WavesBehavior;


//Class used for the treasure at the end of the level: Opens the chest, play the sound and activates the spotLight
public class Treasure : MonoBehaviour
{
    [SerializeField] float distTreasure = 50.0f;
    [SerializeField] Light spotLight;
    [SerializeField] AudioClip clip;

    Camera camera;
    Animation animation;
    AudioSource audioSource;
    bool activated = false;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
        camera = Camera.main;
        audioSource.clip = clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        //No gaire bonic massa if:
        if (other.tag == "Wave" && !activated)
        {
            if (other.GetComponent<WaveBehavior>().GetMainWaveBool())
            {
                if(Vector3.Distance(camera.transform.position, transform.position) <= distTreasure)
                {
                    Activate();
                    activated = true;
                }                 
            }
        }
    }

    private void Activate()
    {
        audioSource.Play();
        animation.Play();
        spotLight.enabled = true;
    }
}
