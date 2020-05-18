using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


//Custom Effect: Fog effect class
[Serializable]
[PostProcess(typeof(UWFogEffectRenderer), PostProcessEvent.AfterStack, "PeerPlay/FogEffect"), ImageEffectAllowedInSceneView]
public sealed class UWFogEffect : PostProcessEffectSettings
{
    [Tooltip("Fog Color")]
    public ColorParameter fogColor = new ColorParameter();

    [Range(-50f, 20f), Tooltip("Fog Start")]
    public FloatParameter fogStart = new FloatParameter { value = 0.0f };


    [Range(1f, 100f), Tooltip("Fog Distance")]
    public FloatParameter fogDistance = new FloatParameter { value = 1.0f };
}


public sealed class UWFogEffectRenderer : PostProcessEffectRenderer<UWFogEffect>
{
    Material m_Material;

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("PeerPlay/FogEffect"));

        if (Shader.Find("PeerPlay/FogEffect") != null)
        {
            m_Material = new Material(Shader.Find("PeerPlay/FogEffect"));
            m_Material.SetColor("_FogColor", settings.fogColor);
            m_Material.SetFloat("_DepthStart", settings.fogStart);
            m_Material.SetFloat("_DepthDistance", settings.fogDistance);
        }

            context.command.Blit(context.source, context.destination, m_Material);

    }
}