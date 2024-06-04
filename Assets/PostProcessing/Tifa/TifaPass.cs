using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TifaPass : ScriptableRenderPass
{
    static readonly string renderPassTag = "Tifa";

    private TifaVolume tifaVolume;
    Material tifaMaterial;

    private int testRTID;

    public TifaPass(RenderPassEvent evt, Shader tifaShader)
    {
        renderPassEvent = evt;
        var shader = tifaShader;
        if (shader == null)
        {
            Debug.LogError("no shader");
            return;
        }
        tifaMaterial = CoreUtils.CreateEngineMaterial(tifaShader);
    }


    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (tifaMaterial == null)
        {
            Debug.LogError("Ciao");
            return;
        }

        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        VolumeStack stack = VolumeManager.instance.stack;
        tifaVolume = stack.GetComponent<TifaVolume>();

        var cmd = CommandBufferPool.Get(renderPassTag);   
        Render(cmd, ref renderingData);                
        context.ExecuteCommandBuffer(cmd);            
        
        CommandBufferPool.Release(cmd); 

    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (tifaVolume.IsActive() == false) return;
        tifaVolume.load(tifaMaterial, ref renderingData);

        RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTargetHandle;  
        
        RenderTextureDescriptor thresholdRT = renderingData.cameraData.cameraTargetDescriptor;
        
        int thresholdRTID = Shader.PropertyToID("thresholdRT");
        
        cmd.GetTemporaryRT(thresholdRTID, thresholdRT.width, thresholdRT.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBFloat);
        cmd.Blit(source, thresholdRTID);
        
        cmd.Blit(thresholdRTID, source, tifaMaterial, -1);
        
        cmd.ReleaseTemporaryRT(thresholdRTID);
    }

}