using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HowDoYouFeel.UI
{
    [RequireComponent(typeof(Button))]
    public class CancelButton : MonoBehaviour
    {
        public void OnCancel()
        {
            Button button = GetComponent<Button>();
            if (button.isActiveAndEnabled)
            {
                button.onClick.Invoke();
            }
        }
    }
}
