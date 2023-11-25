using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorRetainState : MonoBehaviour
    {

        //I love Unity but sometimes I want to dissect the people behind it to see if they have brains at all
        private void Awake()
        {
            GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        }
    }
}
