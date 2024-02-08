using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationEffect : MonoBehaviour
{
    private Volume globalVolume;
    private ChromaticAberration chromaticAberration;
    public float effectDuration = 2.0f; // Duration of the effect in seconds

    void Start()
    {
        // Find the global volume in the scene
        globalVolume = FindObjectOfType<Volume>();

        if (globalVolume != null)
        {
            // Try to get the chromatic aberration, or add it if it's not there
            if (!globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
            {
                chromaticAberration = globalVolume.profile.Add<ChromaticAberration>(false);
            }

            chromaticAberration.active = false;
        }
    }

    void Update()
    {
    }

    public void TriggerChromaAb()
    {
        StartCoroutine(ActivateChromaticAberration());
    }

    private IEnumerator ActivateChromaticAberration()
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.active = true;
            yield return new WaitForSeconds(effectDuration);
            chromaticAberration.active = false;
        }
    }
}

