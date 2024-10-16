using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class TestButton : MonoBehaviour, IStatParticleSpawner
    {
        public GameObject t;

        public void OnClick()
        {
            IStatReceptable target = t.GetComponent<IStatReceptable>();
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.DopamineDown, -1, 1.0f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.DopamineDown, -1, 1.2f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.DopamineDown, -1, 1.4f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.DopamineDown, -1, 1.6f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.DopamineDown, -1, 1.8f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.DopamineDown, -1, 2.0f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.EnergyDown, -1, 2.2f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 64.0f, target, StatParticleType.EnergyDown, -1, 2.4f);
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 0.0f, target, StatParticleType.ScoreUp, 100, 2.6f);
            Destroy(gameObject);
        }
    }
}
