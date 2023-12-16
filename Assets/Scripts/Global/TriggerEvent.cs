using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HowDoYouFeel.Global
{
    public class TriggerEvent : MonoBehaviour
    {
        public List<string> tags = new List<string> { "Player" };
        public UnityEvent onEnter, onExit;
        public UnityEvent onStay; //Uncomment for onStay behavior

        private void OnTriggerEnter(Collider other)
        {
            if (tags.Contains(other.tag)) { onEnter.Invoke(); }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (tags.Contains(collision.tag)) { onEnter.Invoke(); }
        }

        private void OnTriggerExit(Collider other)
        {
            if (tags.Contains(other.tag)) { onExit.Invoke(); }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (tags.Contains(collision.tag)) { onExit.Invoke(); }
        }
   
        private void OnTriggerStay(Collider other)
        {
            if (tags.Contains(other.tag)) { onStay.Invoke(); }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (tags.Contains(collision.tag)) { onStay.Invoke(); }
        }        
    }
}
