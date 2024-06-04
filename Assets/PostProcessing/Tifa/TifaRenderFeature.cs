using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TifaRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Shader shader;
    }
    public Settings settings = new Settings();

    TifaPass tifaPass;

    public override void Create()
    {
        name = "TifaPass";
        if (settings.shader == null)
        {
            Debug.Log("no shader");
            return;
        }
        tifaPass = new TifaPass(RenderPassEvent.BeforeRenderingPostProcessing, settings.shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(tifaPass); 
    }
}