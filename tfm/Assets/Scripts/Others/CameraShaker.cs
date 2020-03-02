using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float power = 0.5f; //Effect on camera
    [SerializeField] float duration = 1.0f; //Duration of effect
    [SerializeField] float slowDownAmount = 1.0f; //Control how quickly we slow down


    Transform camera;
    Vector3 startPos;
    float initialDuration;
    public bool shouldShake = true;

    void Start()
    {
        camera = Camera.main.transform;
        startPos = camera.localPosition;
        initialDuration = duration;
    }

    void Update()
    {
        if(shouldShake)
        {
            if (duration > 0.0f) Shake();
            else StopShaking();
        }
    }

    public void SetShakeStatus(bool status)
    {
        shouldShake = status;
    }

    private void Shake()
    {
        camera.localPosition = startPos + Random.insideUnitSphere * power;
        duration -= Time.deltaTime * slowDownAmount;
    }

    private void StopShaking()
    {
        shouldShake = false;
        duration = initialDuration;
        camera.localPosition = startPos;
    }
}
