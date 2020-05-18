using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


//Custom Effect: Underwater Distortion effect class
[Serializable]
[PostProcess(typeof(UWEffectRenderer), PostProcessEvent.AfterStack, "PeerPlay/UnderwaterImageEffect")]
public sealed class UWEffect : PostProcessEffectSettings
{
    [Range(0.1f, 20f), Tooltip("Noise Scale.")]
    public FloatParameter noiseScale = new FloatParameter { value = 1.0f };

    [Range(0.1f, 20f), Tooltip("Noise Frequency.")]
    public FloatParameter noiseFreq = new FloatParameter { value = 1.0f };

    [Range(0.1f, 30f), Tooltip("Noise Speed.")]
    public FloatParameter noiseSpeed = new FloatParameter { value = 10.0f };

    [Range(0.001f, 0.1f), Tooltip("PixelOffset.")]
    public FloatParameter pixelOffset = new FloatParameter { value = 0.01f };

    [Range(-20f, 20f), Tooltip("Depth Start")]
    public FloatParameter depthStart = new FloatParameter { value = 0.0f };


    [Range(1f, 1000f), Tooltip("Depth Distance")]
    public FloatParameter depthDistance = new FloatParameter { value = 1.0f };

}


public sealed class UWEffectRenderer : PostProcessEffectRenderer<UWEffect>
{
    Material m_Material;

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("PeerPlay/UnderwaterImageEffect"));
        sheet.properties.SetFloat("_NoiseScale", settings.noiseScale);
        sheet.properties.SetFloat("_NoiseFreq", settings.noiseFreq);
        sheet.properties.SetFloat("_NoiseSpeed", settings.noiseSpeed);
        sheet.properties.SetFloat("_PixelOffset", settings.pixelOffset);

        if (Shader.Find("PeerPlay/UnderwaterImageEffect") != null)
        {
            m_Material = new Material(Shader.Find("PeerPlay/UnderwaterImageEffect"));
            m_Material.SetFloat("_NoiseScale", settings.noiseScale);
            m_Material.SetFloat("_NoiseFreq", settings.noiseFreq);
            m_Material.SetFloat("_NoiseSpeed", settings.noiseSpeed);
            m_Material.SetFloat("_PixelOffset", settings.pixelOffset);
            m_Material.SetFloat("_DepthStart", settings.depthStart);
            m_Material.SetFloat("_DepthDistance", settings.depthDistance);
        }




        context.command.Blit(context.source, context.destination, m_Material);

        //context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}