using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class Statbar : MonoBehaviour
    {
        public RectTransform valueBar;
        public RectTransform capBar;

        float maxValue, currentValue, cap;

        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; UpdateBar(); }
        }

        public float Cap
        {
            get { return cap; }
            set { cap = value; UpdateBar(); }
        }

        public float Value
        {
            get { return currentValue; }
            set { currentValue = value; UpdateBar(); }
        }

        void UpdateBar()
        {
            valueBar.localScale = new Vector3(Mathf.InverseLerp(0.0f, MaxValue, Value), 1.0f, 1.0f);
            if (capBar == null) { return; }
            capBar.localScale = new Vector3(Mathf.InverseLerp(MaxValue, 0.0f, Cap), 1.0f, 1.0f);
        }
    }
}
