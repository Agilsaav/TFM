using UnityEngine.Rendering.PostProcessing;
using UnityEngine;


//Effects modifier class: It modifies the light in the scene and all the other custom effects.
public class EffectsModifier : MonoBehaviour
{
     public PostProcessProfile ppProfile;
    [SerializeField] Color ambientColor;

    [Header("Fog Effect Settings")]
    [SerializeField] Color fogColorStart;
    [SerializeField] float fogStart = -20.0f;
    [SerializeField] float fogDistanceStart = 35.0f; //35!
    [SerializeField] float fogDistanceEnd = 30.0f;
    [SerializeField] float maxDepth = 10.0f;

    [Header("Underwater Effect Settings")]
    [SerializeField] float noiseScale = 2.5f;
    [SerializeField] float noiseFreq = 5.0f;
    [SerializeField] float noiseSpeed = 5.0f;
    [SerializeField] float pixelOffset = 0.001f; //0.003f 
    [SerializeField] float depthStart = 0.0f;
    [SerializeField] float depthDist = 20.0f;

    PostProcessVolume newVolume;
    Transform camera;
    UWFogEffect fogEffect;
    UWEffect uwEffect;
    BlurEffect blurEffect;
    float initialDepth;
    float totDepth;

    private void Awake()
    {
        camera = Camera.main.transform;
        fogEffect = ppProfile.GetSetting<UWFogEffect>();
        uwEffect = ppProfile.GetSetting<UWEffect>();
        blurEffect = ppProfile.GetSetting<BlurEffect>();
    }

    void Start()
    {
        initialDepth = camera.position.y;
        totDepth = initialDepth - maxDepth;
        StartFogEffect();
        StartUWEffect();
        StartBlurEffect();
    }

    void Update()
    {
        float param = GetInterpolationParameter();
        UpdateFogColor(param);
        UpdateAmbientLight(param);
    }

    public void ActivateBlurEffect(float newScale, float newFreq, float newSpeed, float newPixelOffset)
    {
        blurEffect.enabled.Override(true);
        blurEffect.noiseScale.Override(newScale);
        blurEffect.noiseFreq.Override(newFreq);
        blurEffect.noiseSpeed.Override(newSpeed);
        blurEffect.pixelOffset.Override(newPixelOffset);
    }

    public void DeactivateBlur()
    {
        blurEffect.enabled.Override(false);
    }

    public void ReduceBlur(float step)
    { 
        float offset = blurEffect.pixelOffset.value;
        if(offset > 0)       
            blurEffect.pixelOffset.Override(offset - step);
    }

    private void StartFogEffect()
    {
        fogEffect.enabled.Override(true);
        fogEffect.fogColor.Override(fogColorStart);
        fogEffect.fogStart.Override(fogStart);
        fogEffect.fogDistance.Override(fogDistanceStart);
    }

    private void StartUWEffect()
    {
        uwEffect.enabled.Override(true);
        uwEffect.noiseScale.Override(noiseScale);
        uwEffect.noiseFreq.Override(noiseFreq);
        uwEffect.noiseSpeed.Override(noiseSpeed);
        uwEffect.pixelOffset.Override(pixelOffset);
        uwEffect.depthStart.Override(depthStart);
        uwEffect.depthDistance.Override(depthDist);
    }

    private void StartBlurEffect()
    {
        blurEffect.enabled.Override(false);
    }

    private float GetInterpolationParameter()
    {
        float interpolParam;

        //Set interpolation Parameter using the camera y position
        if (camera.position.y > initialDepth) interpolParam = 0.0f;
        else
        {
            interpolParam = (Mathf.Abs(camera.position.y - initialDepth)) / totDepth;
            if (interpolParam > 1.0f) interpolParam = 1.0f;
            if (interpolParam < 0.0f) interpolParam = 0.0f;
        }

        return interpolParam;
    }

    private void UpdateFogColor(float interpolParam)
    {
        fogEffect.fogColor.Override(Color.Lerp(fogColorStart, Color.black, interpolParam));
    }

    private void UpdateFogDistance(float interpolParam)
    {
        fogEffect.fogDistance.Override(Mathf.Lerp(fogDistanceStart, fogDistanceEnd, interpolParam));
    }
    
    private void UpdateAmbientLight(float interpolParam)
    {
        RenderSettings.ambientLight = Color.Lerp(Color.white, ambientColor, interpolParam);
    }


}
