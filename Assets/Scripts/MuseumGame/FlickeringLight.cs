using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light l;
    public Renderer left, right;
    Coroutine flickerRoutine;
    float origIntensity;

    IEnumerator FlickerC()
    {
        origIntensity = l.intensity;
        while(true)
        {
            l.intensity = 0.0f;
            left.material.DisableKeyword("_EMISSION");
            right.material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(Random.Range(0.4f, 2.8f));

            for(int r = Random.Range(4, 18); r> 0; r--)
            {
                left.material.EnableKeyword("_EMISSION");
                right.material.EnableKeyword("_EMISSION");
                l.intensity = origIntensity + Random.Range(-0.45f, 0.45f);
                yield return new WaitForSeconds(Random.Range(0.005f, 0.10f));
            }


        }
    }

    private void OnEnable()
    {
        origIntensity = l.intensity;

        flickerRoutine = StartCoroutine(FlickerC());    
    }

    private void OnDisable()
    {
        if(flickerRoutine != null) { StopCoroutine(flickerRoutine); }
        left.material.EnableKeyword("_EMISSION");
        right.material.EnableKeyword("_EMISSION");
        l.intensity = origIntensity;
    }
}
