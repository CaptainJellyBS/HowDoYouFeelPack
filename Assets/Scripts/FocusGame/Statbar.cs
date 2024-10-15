using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class Statbar : MonoBehaviour
    {
        public RectTransform bar;

        float maxValue, currentValue;

        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; UpdateBar(); }
        }

        public float Value
        {
            get { return currentValue; }
            set { currentValue = value; UpdateBar(); }
        }

        void UpdateBar()
        {
            bar.localScale = new Vector3(Mathf.InverseLerp(0.0f, MaxValue, Value), 1.0f, 1.0f);
        }
    }
}
