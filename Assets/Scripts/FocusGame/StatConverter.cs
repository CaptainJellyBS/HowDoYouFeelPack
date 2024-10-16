using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class StatConverter : MonoBehaviour, IStatReceptable, IStatParticleSpawner
    {
        public Brain brain;
        public Transform particleInPoint, particleSpawnPoint;

        int energyStored = 0;

        Coroutine particleQueueRoutine;
        Queue<StatParticleQueueElement> particleQueue;

        public Vector3 GetTargetPosition()
        {
            return particleInPoint.position;
        }

        public void ReceiveParticle(StatParticle particle)
        {
            switch(particle.statType)
            {
                case StatParticleType.EnergyDown:
                    particleQueue.Enqueue(new StatParticleQueueElement(brain, -1, StatParticleType.HealthDown));
                    break;
                case StatParticleType.DopamineDown:
                    particleQueue.Enqueue(new StatParticleQueueElement(brain, -1, (GameManager.Instance.Energy > 0 ? StatParticleType.EnergyDown : StatParticleType.HealthDown)));
                    particleQueue.Enqueue(new StatParticleQueueElement(brain, -1, (GameManager.Instance.Energy > 1 ? StatParticleType.EnergyDown : StatParticleType.HealthDown)));
                    break;
                case StatParticleType.EnergyUp:
                    energyStored+=particle.statValue;
                    while(energyStored >= 5) 
                    { 
                        energyStored -=5; 
                        particleQueue.Enqueue(new StatParticleQueueElement(brain, 1, StatParticleType.HealthUp)); 
                    }
                    break;
                default: Debug.LogError("Shouldn't be sending other particle types to the stat converter"); break;
            }
        }


        private void OnEnable()
        {
            if(particleQueue == null) { particleQueue = new Queue<StatParticleQueueElement>(); }
            particleQueueRoutine = StartCoroutine(ParticleQueueR());
        }

        private void OnDisable()
        {
            StopCoroutine(particleQueueRoutine);
        }

        IEnumerator ParticleQueueR()
        {
            while (true)
            {
                while (particleQueue.Count <= 0) { yield return null; }

                while (particleQueue.Count > 0)
                {
                    StatParticleQueueElement stats = particleQueue.Dequeue();
                    (this as IStatParticleSpawner).SpawnParticle(particleSpawnPoint.position, 0, stats.target, stats.statType, stats.statValue, 0);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }
}