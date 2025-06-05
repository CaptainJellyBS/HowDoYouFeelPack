using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform target;
        public float moveSpeed = 20.0f;

        private void Start()
        {
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
