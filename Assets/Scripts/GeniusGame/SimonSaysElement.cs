using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class SimonSaysElement : MonoBehaviour
    {
        public AudioSource activateAS;
        public GameObject lightObject;
        Coroutine cur;

        public void Activate(float time)
        {
            float d = 0.0f;
            if(cur != null) { Stop(); d = 0.1f; }
            cur = StartCoroutine(ActivateC(time, d));
        }

        IEnumerator ActivateC(float time, float startDelay)
        {
            yield return new WaitForSeconds(startDelay);

            lightObject.SetActive(true);
            activateAS.Play();

            yield return new WaitForSeconds(time);

            lightObject.SetActive(false);
            activateAS.Stop();

            cur = null;
        }

        public void Stop()
        {
            if(cur == null) { return; }
            StopCoroutine(cur);
            lightObject.SetActive(false);

            activateAS.Stop();

            cur = null;
        }
    }
}
