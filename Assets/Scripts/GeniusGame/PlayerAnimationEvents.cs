using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        Player player;

        private void Start()
        {
            player = Player.Instance;
        }

        public void Interact()
        {
            player.Interact();
        }

    }
}
