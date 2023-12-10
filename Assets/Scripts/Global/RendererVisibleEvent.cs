using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HowDoYouFeel.Global
{
    [RequireComponent(typeof(Renderer))]
    public class RendererVisibleEvent : MonoBehaviour
    {
        public UnityEvent onBecameVisible, onBecameInvisible;

        private void OnBecameVisible()
        {
            onBecameVisible.Invoke();
        }

        private void OnBecameInvisible()
        {
            onBecameInvisible.Invoke();
        }
    }
}