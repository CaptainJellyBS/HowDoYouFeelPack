using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;

namespace HowDoYouFeel.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        void Start()
        {
            GlobalManager.Instance.CursorVisible = true;
        }        
    }
}
