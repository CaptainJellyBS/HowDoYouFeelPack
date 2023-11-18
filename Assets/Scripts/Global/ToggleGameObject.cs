using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    public class ToggleGameObject : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
