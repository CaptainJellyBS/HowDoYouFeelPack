using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using TMPro;
using UnityEngine.UI;

namespace HowDoYouFeel.FocusGame
{
    public class GameManager : MonoBehaviour
    {
        [Header("GameObject References")]
        public List<ObjectPool> particlePools;
        public Statbar energyBar, dopamineBar, healthBar;
        public TextMeshProUGUI scoreText;
        public static GameManager Instance { get; private set; }
        public Brain brain;
        public SleepParticleSpawner sleepParticleSpawner;
        public GameObject nightPanel;
        public Button sleepButton; 

        [Header("Stats")]
        public int maxEnergy;
        public int maxDopamine, maxHealth;
        
        int energy, dopamine, health;
        int score;

        public bool IsDaytime { get; private set; }

        public int DayNumber { get; private set; }

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

            sleepParticleSpawner.energyParticleAmount = (int)((float)maxEnergy * 0.9f);
            sleepParticleSpawner.dopamineParticleAmount = maxDopamine / 2;

            Score = 0;

            IsDaytime = true;
            DayNumber = 0;
            TaskManager.Instance.SpawnNewDayTasks(DayNumber);
        }

        public ObjectPool GetParticlePool(StatParticleType _type)
        {
            return particlePools[(int)_type];
        }

        public void ProgressDay()
        {
            StartCoroutine(ProgressDayC());
        }

        IEnumerator ProgressDayC()
        {
            //Set isDaytime to false
            //Progress day for task manager, wait for that to be finished
            //Wait until brain.particlequeuecount is empty
            //Wait another second or two
            //Spawn sleep particles and wait until finished
            //wait another second or two
            //Wait until brain.particlequeuecount is empty (again)
            //Spawn new tasks for the day

            //Set isDaytime to true

            if (!IsDaytime) { yield break; }
            IsDaytime = false;
            nightPanel.SetActive(true);
            sleepButton.interactable = false;

            yield return TaskManager.Instance.ProgressDay();
            yield return new WaitForSeconds(1.0f);

            if (brain.ParticleQueueCount > 0)
            {
                while (brain.ParticleQueueCount > 0)
                {
                    yield return new WaitForSeconds(1.0f);
                }
                yield return new WaitForSeconds(2.0f);
            }

            yield return sleepParticleSpawner.SpawnSleepParticles();
            yield return new WaitForSeconds(2.0f);

            if (brain.ParticleQueueCount > 0)
            {
                while (brain.ParticleQueueCount > 0)
                {
                    yield return new WaitForSeconds(1.0f);
                }
                yield return new WaitForSeconds(2.0f);
            }

            DayNumber++;
            yield return TaskManager.Instance.SpawnNewDayTasks(DayNumber);

            IsDaytime = true;
            nightPanel.SetActive(false);
            sleepButton.interactable = true;
        }

        public void SetCurrentDay(int d)
        {
            DayNumber = d;
        }

    }
}
