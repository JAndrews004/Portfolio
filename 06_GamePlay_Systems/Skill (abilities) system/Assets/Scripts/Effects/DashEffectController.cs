using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class DashEffectController : MonoBehaviour
{
    public Volume volume; // assign your Global Volume in the Inspector

    private MotionBlur motionBlur;
    private ChromaticAberration chromaticAberration;
    //private Vignette vignette;

    private void Start()
    {
        // Grab overrides from the Volume Profile
        volume.profile.TryGet(out motionBlur);
        volume.profile.TryGet(out chromaticAberration);
        //volume.profile.TryGet(out vignette);
    }

    public void PlayDashEffect(float duration, float intensity = 0.5f)
    {
        StartCoroutine(DashEffectRoutine(duration, intensity));
    }

    private IEnumerator DashEffectRoutine(float duration, float intensity)
    {
        if (motionBlur != null) motionBlur.intensity.value = intensity;
        if (chromaticAberration != null) chromaticAberration.intensity.value = 0.3f;
        //if (vignette != null) vignette.intensity.value = 0.4f; // darker edges

        yield return new WaitForSeconds(duration);

        // Reset all back to 0
        if (motionBlur != null) motionBlur.intensity.value = 0f;
        if (chromaticAberration != null) chromaticAberration.intensity.value = 0f;
        //if (vignette != null) vignette.intensity.value = 0f;
    }
}
