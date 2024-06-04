using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Invert")]
public class InvertVolume : VolumeComponent,IPostProcessComponent
{
    [Tooltip("Enable Inversion")]
    public BoolParameter enableInversion = new (true);

    public ClampedFloatParameter inversionAmount = new (.0f, .0f, 1f); 

    public void load(Material material, ref RenderingData renderingData)
    {
        material.SetFloat("_inversionAmount", inversionAmount.value);
    }

    public bool IsActive() => enableInversion == true;
    public bool IsTileCompatible() => false;
}