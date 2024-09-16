using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HowDoYouFeel.UI
{
    [RequireComponent(typeof(Selectable))]
    public class SelectOnMouseEnter : MonoBehaviour, IPointerEnterHandler
    {
        Selectable selectable;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(selectable == null) { selectable = GetComponent<Selectable>(); }
            if (selectable.interactable)
            {
                selectable.Select();
            }
        }
    }
}