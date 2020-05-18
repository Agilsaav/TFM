using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


//Custom Effect: Blur effect class
[Serializable]
[PostProcess(typeof(BlurEffectRenderer), PostProcessEvent.AfterStack, "PeerPlay/BlurEffect")]
public sealed class BlurEffect : PostProcessEffectSettings
{
    [Range(0.1f, 20f), Tooltip("Noise Scale.")]
    public FloatParameter noiseScale = new FloatParameter { value = 1.0f };

    [Range(0.1f, 20f), Tooltip("Noise Frequency.")]
    public FloatParameter noiseFreq = new FloatParameter { value = 1.0f };

    [Range(0.1f, 30f), Tooltip("Noise Speed.")]
    public FloatParameter noiseSpeed = new FloatParameter { value = 10.0f };

    [Range(0.001f, 0.1f), Tooltip("PixelOffset.")]
    public FloatParameter pixelOffset = new FloatParameter { value = 0.01f };
}


public sealed class BlurEffectRenderer : PostProcessEffectRenderer<BlurEffect>
{
    Material m_Material;

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("PeerPlay/BlurEffect"));
        sheet.properties.SetFloat("_NoiseScale", settings.noiseScale);
        sheet.properties.SetFloat("_NoiseFreq", settings.noiseFreq);
        sheet.properties.SetFloat("_NoiseSpeed", settings.noiseSpeed);
        sheet.properties.SetFloat("_PixelOffset", settings.pixelOffset);

        if (Shader.Find("PeerPlay/UnderwaterImageEffect") != null)
        {
            m_Material = new Material(Shader.Find("PeerPlay/BlurEffect"));
            m_Material.SetFloat("_NoiseScale", settings.noiseScale);
            m_Material.SetFloat("_NoiseFreq", settings.noiseFreq);
            m_Material.SetFloat("_NoiseSpeed", settings.noiseSpeed);
            m_Material.SetFloat("_PixelOffset", settings.pixelOffset);
        }

        context.command.Blit(context.source, context.destination, m_Material);

        //context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
