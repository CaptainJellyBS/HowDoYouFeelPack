using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class Door : MonoBehaviour
    {
        public Transform doorPivot;
        public GameObject doorCollider;
        public float doorOpenAngle = 155.0f;
        public float doorClosedAngle = 0.0f;
        bool isOpen = false;
        public bool startOpen = false;

        private void Start()
        {
            UpdateDoorStartStatus();
        }

        public void SetDoor(bool _isOpen)
        {
            isOpen = _isOpen;
            doorCollider.SetActive(!isOpen);
            doorPivot.transform.localRotation = Quaternion.Euler(0, 0, isOpen ? doorOpenAngle : doorClosedAngle);
        }

        [ContextMenu("Update Door")]
        public void UpdateDoorStartStatus()
        {
            SetDoor(startOpen);
        }
    }
}
