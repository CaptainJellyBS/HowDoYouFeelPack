using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using TMPro;

namespace HowDoYouFeel.FocusGame
{
    public enum StatParticleType { DopamineUp, DopamineDown, EnergyUp, EnergyDown, HealthUp, HealthDown, ScoreUp }
    public class StatParticle : MonoBehaviour
    {
        public StatParticleType statType;

        IStatReceptable target;
        public float speed;
        float lifeTime;
        float delay;
        public int statValue; //Note, for Down particles, this value should be negative
        Vector3 targetPos;
        ObjectPool pool;
        bool initialized = false;

        public void Initialize(IStatReceptable _target, int _statValue, float _speed, ObjectPool _pool, float _delay = 1.0f)
        {
            target = _target;
            targetPos = _target.GetTargetPosition();
            speed = _speed;
            statValue = _statValue;
            pool = _pool;
            lifeTime = 0.0f;
            delay = _delay;
            initialized = true;

            if(statType == StatParticleType.ScoreUp)
            {
                GetComponent<TextMeshProUGUI>().text = statValue.ToString();
            }
        }

        private void Update()
        {
            if (!initialized) { return; }

            lifeTime += Time.deltaTime;
            if(lifeTime < delay) { return; }

            float distance = Vector3.Distance(targetPos, transform.position);

            if(distance < 0.1f)
            {
                target.ReceiveParticle(this);
                StoreParticle();
                return;
            }

            float curSpeed = Mathf.Clamp(lifeTime - delay, 0.0f, 1.0f) * speed;

            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position += direction * Mathf.Min(curSpeed * Time.deltaTime, distance);
            //See FocusGame.txt
        }

        public void StoreParticle()
        {
            pool.Store(gameObject);
            initialized = false;
        }
    }
}
