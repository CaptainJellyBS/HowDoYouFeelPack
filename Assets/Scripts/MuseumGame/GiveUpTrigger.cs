using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.MuseumGame
{
    public class GiveUpTrigger : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.ShowGiveUpPanel(1.0f, 2.0f);
            }
        }
    }
}
