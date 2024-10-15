using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public enum StatParticleType { DopamineUp, DopamineDown, EnergyUp, EnergyDown, HealthUp, HealthDown, ScoreUp }
    public class StatParticle : MonoBehaviour
    {
        public StatParticleType statType;

        IStatReceptable target;
        float speed;
        int statValue;

        public void Initialize(IStatReceptable _target, int _statValue, float _speed = 1.0f)
        {
            target = _target;
            speed = _speed;
            statValue = _statValue;
        }

        private void Update()
        {
            //Move towards target. If target is reached, destroy particle and invoke ReceiveParticle function.
            //See FocusGame.txt
        }
    }
}
