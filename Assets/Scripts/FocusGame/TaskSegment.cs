using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class TaskSegment : MonoBehaviour, IStatParticleSpawner
    {
        public int dopamineReward, energyReward, healthReward, scoreReward;
        bool completed = false;

        public void Complete()
        {
            if (completed) { return; }
            completed = true;

            StartCoroutine(CompleteC());
        }

        IEnumerator CompleteC()
        {
            #region emit Particles
            while (dopamineReward > 0)
            {
                dopamineReward--;
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 32.0f, TaskManager.Instance.brain, StatParticleType.DopamineUp, 1, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }

            while (energyReward > 0)
            {
                energyReward--;
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 32.0f, TaskManager.Instance.brain, StatParticleType.EnergyUp, 1, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }

            while (healthReward > 0)
            {
                energyReward--;
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 32.0f, TaskManager.Instance.brain, StatParticleType.HealthUp, 1, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }

            while (dopamineReward < 0)
            {
                dopamineReward++;
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 32.0f, TaskManager.Instance.brain, StatParticleType.DopamineDown, -1, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }

            while (energyReward < 0)
            {
                energyReward++;
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 32.0f, TaskManager.Instance.brain, StatParticleType.EnergyDown, -1, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }

            while (healthReward < 0)
            {
                energyReward++;
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 32.0f, TaskManager.Instance.brain, StatParticleType.HealthDown, -1, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }

            if (scoreReward > 0)
            {
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 0.0f, TaskManager.Instance.brain, StatParticleType.ScoreUp, scoreReward, 1.0f);
                yield return new WaitForSeconds(0.2f);
            }
            #endregion

            //Sprite update here!
        }
    }
}
