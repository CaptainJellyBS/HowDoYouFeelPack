using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class GameManager : MonoBehaviour
    {
        public float maxEnergy, maxDopamine, maxHealth;
        float energy, dopamine, health;
        int score;

        public float Energy
        {
            get { return energy; }
            set { energy = Mathf.Clamp(value,0.0f,maxEnergy); }
        }

        public float Dopamine
        {
            get { return dopamine; }
            set { dopamine = Mathf.Clamp(value, 0.0f, maxDopamine); }
        }

        public float Health
        {
            get { return health; }
            set { health = Mathf.Clamp(value, 0.0f, maxHealth); }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
    }
}
