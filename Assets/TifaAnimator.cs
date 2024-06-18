using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TifaAnimator : MonoBehaviour
{
    private TifaVolume tifaVolume;
    public float rotationAmount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Volume volume = gameObject.GetComponent<Volume>();
 
        if(volume.profile.TryGet<TifaVolume>( out var tmp ) )
        {
            tifaVolume = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tifaVolume.rotationAmount.value = rotationAmount;
    }
}
