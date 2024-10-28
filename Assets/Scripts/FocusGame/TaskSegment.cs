using HowDoYouFeel.Global;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class TaskSegment : MonoBehaviour, IStatParticleSpawner
    {
        public GameObject normalSprite, completedSprite;
        public Transform particleSpawnpoint;
        public TextMeshProUGUI scoreRewardText;
        public RewardBubble middleBubble;
        public RewardBubble[] rewardBubbles;
        public int dopamineReward, energyReward, healthReward, scoreReward;
        bool completed = false;

        public void Initialize(bool showZeroScore = false)
        {
            if(scoreReward == 0 && healthReward == 0 && energyReward == 0 && dopamineReward == 0) { return; }
            Queue<StatParticleType> statQueue = new Queue<StatParticleType>();

            #region fill queue
            for (int i = healthReward; i > 0; i--)
            {
                statQueue.Enqueue(StatParticleType.HealthUp);
            }

            for (int i = energyReward; i > 0; i--)
            {
                statQueue.Enqueue(StatParticleType.EnergyUp);
            }

            for (int i = dopamineReward; i > 0; i--)
            {
                statQueue.Enqueue(StatParticleType.DopamineUp);
            }

            for (int i = healthReward; i < 0; i++)
            {
                statQueue.Enqueue(StatParticleType.HealthDown);
            }

            for (int i = energyReward; i < 0; i++)
            {
                statQueue.Enqueue(StatParticleType.EnergyDown);
            }

            for (int i = dopamineReward; i < 0; i++)
            {
                statQueue.Enqueue(StatParticleType.DopamineDown);
            }
            #endregion

            Utility.FisherYates(ref rewardBubbles);

            if(scoreReward != 0 || showZeroScore)
            {
                scoreRewardText.text = scoreReward.ToString();
                scoreRewardText.gameObject.SetActive(true);
            }
            else
            {
                middleBubble.Initialize(statQueue.Dequeue());
            }

            for (int i = 0; i < rewardBubbles.Length && statQueue.Count > 0; i++)
            {
                rewardBubbles[i].Initialize(statQueue.Dequeue());
            }
        }

        public void Complete()
        {
            if (completed) { return; }
            completed = true;

            StartCoroutine(CompleteC());
        }

        IEnumerator CompleteC()
        {
            completedSprite.SetActive(true);
            normalSprite.SetActive(false);

            #region emit Particles
            while (dopamineReward > 0)
            {
                yield return new WaitForSeconds(0.2f);
                dopamineReward--;
                Vector3 sp = PopBubble(StatParticleType.DopamineUp);
                (this as IStatParticleSpawner).SpawnParticle(sp, 16.0f, TaskManager.Instance.brain, StatParticleType.DopamineUp, 1, 1.0f);
            }

            while (energyReward > 0)
            {
                yield return new WaitForSeconds(0.2f);
                energyReward--;
                Vector3 sp = PopBubble(StatParticleType.EnergyUp);
                (this as IStatParticleSpawner).SpawnParticle(sp, 16.0f, TaskManager.Instance.brain, StatParticleType.EnergyUp, 1, 1.0f);
            }

            while (healthReward > 0)
            {
                yield return new WaitForSeconds(0.2f);
                healthReward--;
                Vector3 sp = PopBubble(StatParticleType.HealthUp);
                (this as IStatParticleSpawner).SpawnParticle(sp, 16.0f, TaskManager.Instance.brain, StatParticleType.HealthUp, 1, 1.0f);
            }

            while (dopamineReward < 0)
            {
                yield return new WaitForSeconds(0.2f);
                dopamineReward++;
                Vector3 sp = PopBubble(StatParticleType.DopamineDown);
                (this as IStatParticleSpawner).SpawnParticle(sp, 16.0f, TaskManager.Instance.brain, StatParticleType.DopamineDown, -1, 1.0f);
            }

            while (energyReward < 0)
            {
                yield return new WaitForSeconds(0.2f);
                energyReward++;
                Vector3 sp = PopBubble(StatParticleType.EnergyDown);
                (this as IStatParticleSpawner).SpawnParticle(sp, 16.0f, TaskManager.Instance.brain, StatParticleType.EnergyDown, -1, 1.0f);
            }

            while (healthReward < 0)
            {
                yield return new WaitForSeconds(0.2f);
                healthReward++;
                Vector3 sp = PopBubble(StatParticleType.HealthDown);
                (this as IStatParticleSpawner).SpawnParticle(sp, 16.0f, TaskManager.Instance.brain, StatParticleType.HealthDown, -1, 1.0f);
            }

            if (scoreReward > 0)
            {
                yield return new WaitForSeconds(0.2f);
                (this as IStatParticleSpawner).SpawnParticle(particleSpawnpoint.position, 0.0f, TaskManager.Instance.brain, StatParticleType.ScoreUp, scoreReward, 1.0f);
            }
            #endregion


            //Sprite update here!
        }

        Vector3 PopBubble(StatParticleType _type)
        {
            for (int i = 0; i < rewardBubbles.Length; i++)
            {
                if (rewardBubbles[i].PopBubble(_type)) { return rewardBubbles[i].transform.position; }
            }
            middleBubble.PopBubble(_type);
            return particleSpawnpoint.position;
        }
    }
}
