using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InvertPass : ScriptableRenderPass
{
    static readonly string renderPassTag = "Invert";

    private InvertVolume invertVolume;
    Material invertMaterial;

    private int testRTID;

    public InvertPass(RenderPassEvent evt, Shader invertShader)
    {
        renderPassEvent = evt;
        var shader = invertShader;
        if (shader == null)
        {
            Debug.LogError("no shader");
            return;
        }
        invertMaterial = CoreUtils.CreateEngineMaterial(invertShader);
    }


    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (invertMaterial == null)
        {
            Debug.LogError("Ciao");
            return;
        }

        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        VolumeStack stack = VolumeManager.instance.stack;
        invertVolume = stack.GetComponent<InvertVolume>();

        var cmd = CommandBufferPool.Get(renderPassTag);   
        Render(cmd, ref renderingData);                
        context.ExecuteCommandBuffer(cmd);            
        
        CommandBufferPool.Release(cmd); 

    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (invertVolume.IsActive() == false) return;
        invertVolume.load(invertMaterial, ref renderingData);

        RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTargetHandle;  
        
        RenderTextureDescriptor thresholdRT = renderingData.cameraData.cameraTargetDescriptor;
        
        int thresholdRTID = Shader.PropertyToID("thresholdRT");
        
        cmd.GetTemporaryRT(thresholdRTID, thresholdRT.width, thresholdRT.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBFloat);
        cmd.Blit(source, thresholdRTID);
        
        cmd.Blit(thresholdRTID, source, invertMaterial, -1);
        
        cmd.ReleaseTemporaryRT(thresholdRTID);
    }

}