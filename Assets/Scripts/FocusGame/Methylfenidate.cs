using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class Methylfenidate : MonoBehaviour, IStatParticleSpawner
    {
        public int dopamineAmount = 6, timerLength = 6;
        int timer = 0;

        public GameObject buttonObject;
        public GameObject disabledObject;
        public Brain brain;
        public TaskTemplateSO funTTS;

        public void Initialize()
        {
            float angle = TaskManager.Instance.GetAngle();
            transform.rotation = Quaternion.Euler(0, 0, angle);

            ActivateButton();

            gameObject.SetActive(true);
        }

        public void UseMethylfenidate()
        {
            if(timer > 0) { Debug.LogWarning("Should not be able to use speed while timer active"); return; }
            StartCoroutine(SpawnParticlesC());

            Debug.LogWarning("KNOWN BUG: This doesn't always work");
            if (TaskManager.Instance.activePriorities.Count > 0)
            {
                List<Priority> completedPriorities = new List<Priority>();
                foreach (Priority p in TaskManager.Instance.activePriorities)
                {
                    if (p.attachedTask.myTemplate == funTTS) { completedPriorities.Add(p); }
                }

                foreach (Priority p in completedPriorities)
                {
                    TaskManager.Instance.activePriorities.Remove(p); Destroy(p.gameObject);
                }
            }

            DeactivateButton();
        }

        IEnumerator SpawnParticlesC()
        {
            for (int i = 0; i < dopamineAmount; i++)
            {
                (this as IStatParticleSpawner).SpawnParticle(buttonObject.transform.position, 32.0f, brain, StatParticleType.DopamineUp, 1, 0.5f);
                yield return new WaitForSeconds(0.1f);
            }
        }

        void ActivateButton()
        {
            timer = 0;
            buttonObject.SetActive(true);
            disabledObject.SetActive(false);
        }

        void DeactivateButton()
        {
            timer = timerLength;
            buttonObject.SetActive(false);
            disabledObject.SetActive(true);
        }

        public void ProgressTime()
        {
            timer--;
            if(timer == 0)
            {
                ActivateButton();
                (this as IStatParticleSpawner).SpawnParticle(buttonObject.transform.position, 0.0f, brain, StatParticleType.EnergyDown, -1, 0.0f);
            }
        }

        public void ProgressDay()
        {
            ActivateButton();
        }
    }
}
