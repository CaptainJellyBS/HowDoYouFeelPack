using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.MuseumGame
{
    [RequireComponent(typeof(AudioSource))]
    public class LampBuzz : MonoBehaviour
    {
        AudioSource audioSource;

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if(audioSource == null) { return; }
            float t = (float)(GameManager.Instance.roomCounter) / (float)(GameManager.Instance.endRoom);
            audioSource.minDistance = Mathf.Lerp(0.0f, 0.25f, t);
            audioSource.maxDistance = Mathf.Lerp(3.5f, 9.0f,t);
        }
    }
}
