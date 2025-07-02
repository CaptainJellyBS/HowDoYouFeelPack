using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HowDoYouFeel.GeniusGame
{
    public enum HidingObjectType { Floor, Wall }
    public enum HidingAnimationType { Translation, Rotation }

    public class HidingObject : MonoBehaviour
    {
        public Transform hidingObject;
        public HidingObjectType hoType = HidingObjectType.Wall;
        public HidingAnimationType animationType = HidingAnimationType.Translation;
        public Vector3 animValue;
        [Range(0.1f, 3.0f)]
        public float hideTime = 1.0f;
        public int floor = 0;
        public bool overrideParentFloorNumber = false;
        public bool matchChildrenFloorOnStartup = false;
        public bool hideChildRenderersWhenHiding = false;
        List<Renderer> childRenderers;
        
        float t;
        Vector3 startLocalPos;
        Quaternion startLocalRot;

        private void Awake()
        {
            if (matchChildrenFloorOnStartup) { MatchChildrenFloor(); }
        }

        private void Start()
        {
            if (hideChildRenderersWhenHiding) 
            {
                childRenderers = new List<Renderer>();
                GetComponentsInChildren<Renderer>(false, childRenderers);

                for (int i = childRenderers.Count-1; i >= 0; i--)
                {
                    if (!childRenderers[i].enabled) { childRenderers.RemoveAt(i); }
                }
            }

            t = 0.0f;
            startLocalPos = hidingObject.localPosition;
            startLocalRot = hidingObject.localRotation;
        }

        private void Update()
        {
            t += (Time.deltaTime / hideTime) * (isHidden() ? 1.0f : -1.0f);
            UpdateChildRenderers();
            //hidingObject.gameObject.SetActive(t < 1.0f);
            t = Mathf.Clamp(t, 0.0f, 1.0f);

            switch(animationType)
            {
                case HidingAnimationType.Translation: hidingObject.localPosition = Vector3.Lerp(startLocalPos, animValue, t); break;
                case HidingAnimationType.Rotation: hidingObject.localRotation =
                        Quaternion.Euler(
                            Mathf.Lerp(startLocalRot.eulerAngles.x, animValue.x, t), 
                            Mathf.Lerp(startLocalRot.eulerAngles.y, animValue.y, t),
                            Mathf.Lerp(startLocalRot.eulerAngles.z, animValue.z, t)); //istg I hate rotations                        
                        break;
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

        void UpdateChildRenderers()
        {
            if (!hideChildRenderersWhenHiding) { return; }
            if (t > 1.0f)
            {
                if (childRenderers[0].enabled)
                {
                    foreach (Renderer r in childRenderers)
                    {
                        r.enabled = false;
                    }
                }
            }

            if (t < 1.0f)
            {
                if (!childRenderers[0].enabled)
                {
                    foreach (Renderer r in childRenderers)
                    {
                        r.enabled = true;
                    }
                }
            }


        }

        [ContextMenu("Match Children Floor")]
        void MatchChildrenFloor()
        {
            foreach(HidingObject h in GetComponentsInChildren<HidingObject>(true))
            {
                if (h.overrideParentFloorNumber || h == this) { continue; }
                h.floor = floor;
            }

            foreach(Wall w in GetComponentsInChildren<Wall>(true))
            {
                w.floor = w.GetComponent<HidingObject>().floor;
                w.UpdateWall();
            }
        }
    }
}