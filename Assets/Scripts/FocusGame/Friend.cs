using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HowDoYouFeel.FocusGame
{
    [RequireComponent(typeof(Image))]
    public class Friend : MonoBehaviour, IStatParticleSpawner
    {
        public Task myTask { get; private set; }
        int progressDone = 0;
        Image myImage;
        ADHDManager adhdManager;
        bool destroyed = false;

        public void Initialize(Task t, ADHDManager manager)
        {
            myTask = t;
            progressDone = t.Progress;
            adhdManager = manager;

            foreach(TaskSegment ts in myTask.taskSegments)
            {
                ts.dopamineReward = Mathf.Max(0, ts.dopamineReward); //POSSIBLE TODO: REFACTOR SO THE PARTICLES GO TO THE FRIEND.
            }

            destroyed = false;
        }

        public void ProgressFriend()
        {
            if(destroyed) { return; }
            if (!TaskManager.Instance.brain.CanProgressTask()) { return; }
            if(myTask == null) 
            {
                StartCoroutine(DestroyMeC());
                return;
            }

            progressDone++;
            (this as IStatParticleSpawner).SpawnParticle(transform.position + transform.right * 16.0f,
                0.0f,myTask, StatParticleType.EnergyUp, 1, 0.0f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position - transform.right * 16.0f,
                0.0f,myTask, StatParticleType.DopamineUp, 1, 0.0f);

            StartCoroutine(FlashImageC());

            if (progressDone >= myTask.MaxProgress || myTask.Progress >= myTask.MaxProgress) 
            {
                StartCoroutine(DestroyMeC());
            }

        }

        IEnumerator FlashImageC()
        {
            if(myImage == null) { myImage = GetComponent<Image>(); }
            myImage.transform.localScale = Vector3.one * 1.5f;
            yield return new WaitForSeconds(0.15f);
            myImage.transform.localScale = Vector3.one;
        }

        IEnumerator DestroyMeC()
        {
            if (destroyed) { yield break; }
            destroyed = true;

            yield return new WaitForSeconds(0.5f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position + transform.right * 16.0f,
    0.0f, TaskManager.Instance.brain, StatParticleType.DopamineUp, 1, 0.2f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position - transform.right * 16.0f,
                0.0f, TaskManager.Instance.brain, StatParticleType.HealthUp, 1, 0.4f);

            yield return StartCoroutine(FlashImageC());
            Destroy(gameObject); 
        }

        private void OnDestroy()
        {
            adhdManager.FriendDone(this);
        }
    }
}
