using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace HowDoYouFeel.MuseumGame
{
    public class PostProcessingHandler : MonoBehaviour
    {
        public Volume startVolume, endVolume, deadVolume;
        public int endRoom = 50;
        bool isUpdating;
                
        public void UpdatePostProcessing(int roomCounter)
        {
            StartCoroutine(UpdatePostProcessingC(roomCounter));
        }

        IEnumerator UpdatePostProcessingC(int roomCounter)
        {
            if (isUpdating) { yield break; }
            isUpdating = true;

            float t = 0.0f;
            while(t<=1.0f)
            {
                float e = Mathf.Clamp((float)roomCounter / (float)endRoom, 0.0f, 1.0f);
                float s = Mathf.Clamp((float)(roomCounter-1)/(float)endRoom, 0.0f, 1.0f);
                float l = Mathf.Lerp(s, e, t);

                startVolume.weight = 1.0f - l;
                endVolume.weight = l;

                yield return null;
                t += Time.deltaTime / 4.0f;                
            }

            float i = Mathf.Clamp((float)roomCounter / (float)endRoom, 0.0f, 1.0f);
            startVolume.weight = 1.0f - i;
            endVolume.weight = i;

            isUpdating = false;
        }

        public void Die()
        {
            startVolume.gameObject.SetActive(false);
            endVolume.gameObject.SetActive(false);
            deadVolume.gameObject.SetActive(true);
        }
    }
}