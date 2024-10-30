using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class SleepParticleSpawner : MonoBehaviour, IStatParticleSpawner
    {
        public int energyParticleAmount, dopamineParticleAmount;
        public Brain brain;

        public Coroutine SpawnSleepParticles()
        {
            return StartCoroutine(SpawnSleepParticlesC());
        }

        IEnumerator SpawnSleepParticlesC()
        {
            int actualEnergyParticleAmount = Mathf.Min(energyParticleAmount, GameManager.Instance.maxEnergy + 5 - GameManager.Instance.Energy);

            for (int i = 0; i < 3 - GameManager.Instance.Health; i++)
            {
                (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, brain, StatParticleType.HealthUp, 1, 1.0f);
                actualEnergyParticleAmount -= 3;
                yield return new WaitForSeconds(0.05f);
            }

            for (int i = 0; i < Mathf.Max(actualEnergyParticleAmount, dopamineParticleAmount); i++)
            {
                if(i< actualEnergyParticleAmount)
                {
                    (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, brain, StatParticleType.EnergyUp, 1, 1.0f);
                    yield return new WaitForSeconds(0.05f);
                }
                if (i < dopamineParticleAmount)
                {
                    (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, brain, StatParticleType.DopamineUp, 1, 1.0f);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            yield break;
        }
    }
}
