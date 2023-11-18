using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HowDoYouFeel.UI
{
    public class CancelHandler : MonoBehaviour
    {
        void OnCancel()
        {
            FindObjectOfType<CancelButton>()?.OnCancel();
        }
    }
}
