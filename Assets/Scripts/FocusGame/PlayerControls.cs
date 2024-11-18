using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace HowDoYouFeel.FocusGame
{
    [RequireComponent(typeof(ControlSwitchPlayerInput))]
    [RequireComponent(typeof(PlayerInput))]

    public class PlayerControls : MonoBehaviour
    {
        bool isPressed = false;
        Vector3 input = Vector3.zero;

        public Transform debugObject;
        public UnityEvent gamePauseEvent;

        private void Start()
        {
            GlobalManager.Instance.CursorVisible = true;
        }

        private void Update()
        {
            if(Time.timeScale <= 0.0f) { return; }
            TaskManager.Instance.HandleInput(input, isPressed);

            debugObject.localRotation = Quaternion.Euler(0, 0, -Vector3.SignedAngle(input, Vector3.up, Vector3.forward));
        }

        void OnGamePause(InputValue value)
        {
            gamePauseEvent.Invoke();
        }

        void OnGamepadPoint(InputValue value)
        {
            Vector2 v = value.Get<Vector2>();
            input = new Vector3(v.x, v.y, 0);
        }

        void OnMousePoint(InputValue value)
        {
            Vector2 v = value.Get<Vector2>() - new Vector2(transform.position.x, transform.position.y);
            input = new Vector3(v.x, v.y, 0);
        }

        void OnPerformTask(InputValue value)
        {
            isPressed = value.isPressed;
        }
    }
}
