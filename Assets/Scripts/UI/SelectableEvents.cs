using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HowDoYouFeel.UI
{
    [RequireComponent(typeof(Selectable))]
    public class SelectableEvents : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent onSelect, onDeselect;

        public void OnSelect(BaseEventData eventData)
        {
            onSelect.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onSelect.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect.Invoke();
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            onDeselect.Invoke();
        }
    }
}
