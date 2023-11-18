using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    public enum ControlSchemeType { Keyboard, Gamepad }
    public class ControlChangeToggleActive : MonoBehaviour
    {
        public ControlSchemeType activeOn;


        public void ActivateGameObject()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateGameObject()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            switch (activeOn)
            {
                case ControlSchemeType.Keyboard:
                    FindObjectOfType<ControlSwitchEventManager>()?.onKeyboard.RemoveListener(ActivateGameObject);
                    FindObjectOfType<ControlSwitchEventManager>()?.onGamepad.RemoveListener(DeactivateGameObject);
                    break;
                case ControlSchemeType.Gamepad:
                    FindObjectOfType<ControlSwitchEventManager>()?.onGamepad.RemoveListener(ActivateGameObject);
                    FindObjectOfType<ControlSwitchEventManager>()?.onKeyboard.RemoveListener(DeactivateGameObject);
                    break;
            }
        }
    }
}
