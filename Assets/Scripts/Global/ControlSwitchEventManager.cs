using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace HowDoYouFeel.Global
{
    public class ControlSwitchEventManager : MonoBehaviour
    {
        public UnityEvent onKeyboard, onGamepad;
        PlayerInput input;

        private void Start()
        {
            input = FindObjectOfType<PlayerInput>();
            foreach (ControlSwitchToggleActive c in FindObjectsOfType<ControlSwitchToggleActive>(true))
            {
                if (c.activeOn == ControlSchemeType.Gamepad)
                {
                    onGamepad.AddListener(c.ActivateGameObject);
                    onKeyboard.AddListener(c.DeactivateGameObject);
                }
                if (c.activeOn == ControlSchemeType.Keyboard)
                {
                    onKeyboard.AddListener(c.ActivateGameObject);
                    onGamepad.AddListener(c.DeactivateGameObject);
                }
            }

            //Debug.Log(PersistentDataObject.Instance.currentControlScheme);
            //Debug.Log(input.devices.ToArray().ToString());
            input.SwitchCurrentControlScheme(GlobalManager.Instance.currentControlScheme, input.devices.ToArray());
            OnControlsChanged();
        }

        public void OnControlsChanged()
        {
            if (!GlobalManager.Instance)
            { 
                Debug.LogWarning("Tried to reach a non-existent Global Manager. If nothing is broken, this is fine");
                return; 
            }

            if (input != null) { GlobalManager.Instance.currentControlScheme = input.currentControlScheme; }

            if (input == null) { input = FindObjectOfType<PlayerInput>(); }

            if (input.currentControlScheme == "Keyboard")
            {
                onKeyboard.Invoke();
                GlobalManager.Instance.MouseActive = true;
            }
            else
            {
                onGamepad.Invoke();
                GlobalManager.Instance.MouseActive = false;
            }
        }
    }
}
