using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public int currentPlayerFloor = 0;
        DialogueManager dm;


        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(Instance.gameObject);
                Debug.LogWarning("Had to destroy an old GameManager. If nothing is broken, this is fine");
            }
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            Debug.LogWarning("Note to self: Make sure colliders are not moved up with hiding objects!");
            dm = GetComponent<DialogueManager>();

        }

        public void SetPlayerFloor(int floor)
        {
            currentPlayerFloor = floor;
        }
    }
}
