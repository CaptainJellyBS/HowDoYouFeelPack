using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HowDoYouFeel.MuseumGame
{
    public class EndingLookTrigger : MonoBehaviour
    {
        public void TriggerEnding()
        {
            Debug.Log("Saw the ending?");
            GameManager.Instance.Ending();
        }
    }
}
