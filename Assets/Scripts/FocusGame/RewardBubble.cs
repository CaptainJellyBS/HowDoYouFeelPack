using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HowDoYouFeel.FocusGame
{
    public class RewardBubble : MonoBehaviour
    {
        public Image rewardImage;
        public StatParticleType particleType;
        public Sprite[] spriteArr;

        public void Initialize(StatParticleType _type)
        {
            particleType = _type;
            if (particleType == StatParticleType.ScoreUp) { Debug.LogError("Score should never be in a reward bubble"); return; }
            rewardImage.sprite = spriteArr[(int)particleType];
            gameObject.SetActive(true);
        }

        public bool PopBubble(StatParticleType _type)
        {
            if (!gameObject.activeSelf) { return false; }
            if(_type != particleType) { return false; }
            gameObject.SetActive(false);
            return true;
        }
    }
}
