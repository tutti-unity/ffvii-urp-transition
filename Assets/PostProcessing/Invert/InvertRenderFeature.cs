using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InvertRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Shader shader;
    }
    public Settings settings = new Settings();

    InvertPass invertPass;

    public override void Create()
    {
        name = "InvertPass";
        if (settings.shader == null)
        {
            Debug.Log("no shader");
            return;
        }
        invertPass = new InvertPass(RenderPassEvent.BeforeRenderingPostProcessing, settings.shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(invertPass); 
    }
}