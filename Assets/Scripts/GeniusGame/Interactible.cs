using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HowDoYouFeel.GeniusGame
{
    public class Interactible : MonoBehaviour
    {
        [SerializeField] UnityEvent onEnter, onExit, onInteract;
        // Start is called before the first frame update

        public virtual void Enter()
        {
            onEnter.Invoke();
        }

        public virtual void Exit()
        {
            onExit.Invoke();
        }

        public virtual void Interact()
        {
            onInteract.Invoke();
        }
    }
}
