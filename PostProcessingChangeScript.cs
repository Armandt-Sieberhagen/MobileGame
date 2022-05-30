using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class PostProcessingChangeScript : MonoBehaviour
{
    [SerializeField] private Volume _postProcessVolume;
    LensDistortion lensdistort;
    Vignette vig;

    float DistortionIntesnity;
    // Start is called before the first frame update
    void Start()
    {
        _postProcessVolume = GetComponent<Volume>();
        LensDistortion lens;
        Vignette vige;
        if (_postProcessVolume.profile.TryGet<LensDistortion>(out lens))
        {
            lensdistort = lens;
        }if (_postProcessVolume.profile.TryGet<Vignette>(out vige))
        {
            vig = vige;
        }
        
    }

    public void FixedUpdate()
    {
        lensdistort.intensity.value = Mathf.Lerp(lensdistort.intensity.value,0,Time.deltaTime*0.5f);
        vig.color.value = Color.Lerp(vig.color.value,Color.black,Time.deltaTime*0.5f);
    }

    public void DistortIntensity(float Intensity)
    {
        
        lensdistort.intensity.value = Intensity;
    }public void SetVignetteColor(Color colorChoice)
    {

        vig.color.value = colorChoice;
    }
 
}
