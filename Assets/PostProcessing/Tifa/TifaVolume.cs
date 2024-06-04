using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[System.Serializable, VolumeComponentMenu("Tifa")]
public class TifaVolume : VolumeComponent,IPostProcessComponent
{
    [FormerlySerializedAs("enableInversion")] [Tooltip("Enable Tifa")]
    public BoolParameter enableTifa = new (true);

    [FormerlySerializedAs("rotationAmount")] public ClampedFloatParameter rotationAmount = new (.0f, .0f, 2 * Mathf.PI); 

    public void load(Material material, ref RenderingData renderingData)
    {
        material.SetFloat("_rotationAmount", rotationAmount.value);
    }

    public bool IsActive() => enableTifa == true;
    public bool IsTileCompatible() => false;
}