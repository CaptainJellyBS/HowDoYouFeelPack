using HowDoYouFeel.Global;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public interface IStatParticleSpawner
    {
        public virtual void SpawnParticle(Vector3 spawnPosition, float maxDeviance, IStatReceptable target, StatParticleType particleType, int statValue, float delay = 1.0f)
        {
            ObjectPool pool = GameManager.Instance.GetParticlePool(particleType);
            float speed = GameManager.Instance.particleSpeed;
            Vector3 spawnPos = spawnPosition + new Vector3(Random.Range(-maxDeviance, maxDeviance), Random.Range(-maxDeviance, maxDeviance), 0);

            StatParticle p = pool.Spawn(spawnPos, Quaternion.identity).GetComponent<StatParticle>();
            p.Initialize(target, statValue, speed, pool, delay);
        }
    }
}
