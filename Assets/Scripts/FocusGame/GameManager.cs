using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using TMPro;

namespace HowDoYouFeel.FocusGame
{
    public class GameManager : MonoBehaviour
    {
        [Header("GameObject References")]
        public List<ObjectPool> particlePools;
        public Statbar energyBar, dopamineBar, healthBar;
        public TextMeshProUGUI scoreText;
        public static GameManager Instance { get; private set; }

        [Header("Stats")]
        public int maxEnergy;
        public int maxDopamine, maxHealth;
        int energy, dopamine, health;
        int score;

        public int Energy
        {
            get { return energy; }
            set { energy = Mathf.Clamp(value,0,maxEnergy); energyBar.Value = energy; }
        }

        public int Dopamine
        {
            get { return dopamine; }
            set { dopamine = Mathf.Clamp(value, 0, maxDopamine); dopamineBar.Value = dopamine; }
        }

        public int Health
        {
            get { return health; }
            set 
            { 
                health = Mathf.Clamp(value, 0, maxHealth); 
                healthBar.Value = health; 
                energyBar.Cap = health;
            }
        }

        public int Score
        {
            get { return score; }
            set { score = value; scoreText.text = score.ToString(); }
        }

        public float particleSpeed = 1.0f;

        private void Awake()
        {
            if(Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destroy an old GameManager reference"); }

            Instance = this;
        }

        private void Start()
        {
            healthBar.MaxValue = maxHealth;
            Health = maxHealth;

            energyBar.MaxValue = maxEnergy;
            Energy = maxEnergy;

            dopamineBar.MaxValue = maxDopamine;
            Dopamine = maxDopamine;

            Score = 0;
        }

        public ObjectPool GetParticlePool(StatParticleType _type)
        {
            return particlePools[(int)_type];
        }

    }
}
