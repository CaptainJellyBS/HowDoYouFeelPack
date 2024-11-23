using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HowDoYouFeel.UI
{
    [RequireComponent(typeof(Button))]
    public class CancelButton : MonoBehaviour
    {
        bool wasEnabledThisFrame = false;

        private void OnEnable()
        {
            wasEnabledThisFrame = true;
        }

        private void LateUpdate()
        {
            wasEnabledThisFrame = false;
        }

        public void OnCancel()
        {
            Button button = GetComponent<Button>();
            if (button.isActiveAndEnabled && !wasEnabledThisFrame)
            {
                button.onClick.Invoke();
            }
        }
    }
}
