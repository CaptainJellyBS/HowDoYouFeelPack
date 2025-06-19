using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;

namespace HowDoYouFeel.GeniusGame
{
    public class LegoBrickMaterialSetter : MonoBehaviour
    {
        public Material[] legoMaterials;

        [ContextMenu("Update Bricks")]
        public void SetMaterials()
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material = Utility.Pick(legoMaterials);
            }
        }
    }
}
