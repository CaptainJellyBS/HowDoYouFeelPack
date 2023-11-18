using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.Global
{


    public class ControlSwitchPlayerInput : MonoBehaviour
    {
        ControlSwitchEventManager manager;

        void OnControlsChanged()
        {
            if(manager == null) { manager = FindObjectOfType<ControlSwitchEventManager>(); }
            manager.OnControlsChanged();
        }
    }
}



