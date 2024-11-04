using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HowDoYouFeel.FocusGame
{
    public interface IFGSelectable
    {
        public GameObject GetGameObject();

        public Vector3 GetSelectDirection();
    }
}
