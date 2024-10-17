using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class Brain : MonoBehaviour, IStatReceptable, IStatParticleSpawner
    {
        public StatConverter statConverter;
        Coroutine particleQueueRoutine;
        Queue<StatParticleQueueElement> particleQueue;

        public Vector3 GetTargetPosition()
        {
            return transform.position;
        }

        public void ReceiveParticle(StatParticle particle)
        {
            switch(particle.statType)
            {
                case StatParticleType.DopamineUp:
                case StatParticleType.DopamineDown:
                    int rd = (GameManager.Instance.Dopamine + particle.statValue) * -1;
                    GameManager.Instance.Dopamine += particle.statValue;
                    while(rd > 0)
                    {
                        rd--;
                        particleQueue.Enqueue(new StatParticleQueueElement(statConverter, -1, StatParticleType.DopamineDown));
                    }
                    break;
                case StatParticleType.EnergyUp:                    
                case StatParticleType.EnergyDown:
                    int re = (GameManager.Instance.Energy + particle.statValue) * -1;
                    int rc = (GameManager.Instance.Energy + particle.statValue) - GameManager.Instance.Health;
                    GameManager.Instance.Energy = Mathf.Min(GameManager.Instance.Energy + particle.statValue, GameManager.Instance.Health);
                    while (re > 0)
                    {
                        re--;
                        particleQueue.Enqueue(new StatParticleQueueElement(statConverter, -1, StatParticleType.EnergyDown));
                    }
                    while (rc > 0)
                    {
                        rc--;
                        particleQueue.Enqueue(new StatParticleQueueElement(statConverter, 1, StatParticleType.EnergyUp));
                    }
                    break;
                case StatParticleType.HealthUp: 
                case StatParticleType.HealthDown:
                    GameManager.Instance.Health += particle.statValue;
                    while(GameManager.Instance.Energy > GameManager.Instance.Health)
                    {
                        GameManager.Instance.Energy--;
                        particleQueue.Enqueue(new StatParticleQueueElement(statConverter, 1, StatParticleType.EnergyUp));
                    }
                    break;
                case StatParticleType.ScoreUp:
                    GameManager.Instance.Score += particle.statValue;
                    break;
            }
        }

        private void OnEnable()
        {
            if (particleQueue == null) { particleQueue = new Queue<StatParticleQueueElement>(); }
            particleQueueRoutine = StartCoroutine(ParticleQueueR());
        }

        private void OnDisable()
        {
            StopCoroutine(particleQueueRoutine);
        }

        IEnumerator ParticleQueueR()
        {
            while(true)
            {
                while(particleQueue.Count <= 0) { yield return null; }

                while(particleQueue.Count > 0)
                {
                    StatParticleQueueElement stats = particleQueue.Dequeue();
                    (this as IStatParticleSpawner).SpawnParticle(transform.position, 0, stats.target, stats.statType, stats.statValue, 0);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

    }
}
