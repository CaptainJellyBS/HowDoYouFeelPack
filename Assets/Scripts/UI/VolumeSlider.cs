﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HowDoYouFeel.Global;

namespace HowDoYouFeel.UI
{
    public class VolumeSlider : MonoBehaviour
    {
        void OnEnable()
        {
            GetComponent<Slider>().value = AudioListener.volume;
        }


        public void SetVolume(float volume)
        {
            AudioVolumeManager.Instance?.SetVolume(volume);
        }
    }
}
