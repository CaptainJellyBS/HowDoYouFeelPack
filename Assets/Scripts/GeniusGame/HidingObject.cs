using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HidingObjectType { Floor, Wall }
public enum HidingAnimationType { Translation, Rotation}

namespace HowDoYouFeel.GeniusGame
{
    public class HidingObject : MonoBehaviour
    {
        public Transform hidingObject;
        public HidingObjectType hoType = HidingObjectType.Wall;
        public HidingAnimationType animationType = HidingAnimationType.Translation;
        public Vector3 animValue;
        [Range(0.1f, 3.0f)]
        public float hideTime = 1.0f;
        public int floor = 0;
        
        float t;
        Vector3 startLocalPos;
        Quaternion startLocalRot;

        private void Start()
        {
            t = 0.0f;
            startLocalPos = hidingObject.localPosition;
            startLocalRot = hidingObject.localRotation;
        }

        private void Update()
        {
            t += (Time.deltaTime / hideTime) * (isHidden() ? 1.0f : -1.0f);
            t = Mathf.Clamp(t, 0.0f, 1.0f);

            switch(animationType)
            {
                case HidingAnimationType.Translation: hidingObject.localPosition = Vector3.Lerp(startLocalPos, animValue, t); break;
                case HidingAnimationType.Rotation: hidingObject.localRotation = Quaternion.Slerp(startLocalRot, Quaternion.Euler(animValue), t); break;
                default: throw new System.ArgumentException("WHAT DO YOU MEAN DEFAULT????");
            }
        }

        private bool isHidden()
        {
            switch (hoType)
            {
                case HidingObjectType.Floor: return floor > GameManager.Instance.currentPlayerFloor;
                case HidingObjectType.Wall: 
                    return floor == GameManager.Instance.currentPlayerFloor
                        && Vector3.Dot(transform.right, Player.Instance.transform.position - transform.position) > 0;
                default: throw new System.ArgumentException("WHAT DO YOU MEAN DEFAULT????");
            }
        }
    }
}