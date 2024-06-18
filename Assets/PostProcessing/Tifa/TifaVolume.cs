using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[System.Serializable, VolumeComponentMenu("Tifa")]
public class TifaVolume : VolumeComponent,IPostProcessComponent
{
    [Tooltip("Enable Tifa")]
    public BoolParameter enableTifa = new (true);

    public ClampedFloatParameter rotationAmount = new (.0f, .0f, 2 * Mathf.PI);
    public ClampedFloatParameter density = new (.1f, .1f, 2 * Mathf.PI);
    public ClampedFloatParameter zoomAmount = new (.1f, .1f, 10);

    public void load(Material material, ref RenderingData renderingData)
    {
        material.SetFloat("_rotationAmount", rotationAmount.value);
        material.SetFloat("_density", density.value);
        material.SetFloat("_zoomAmount", zoomAmount.value);
    }

    public bool IsActive() => enableTifa == true;
    public bool IsTileCompatible() => false;
}