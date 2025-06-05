using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public enum WallDirection { NorthWest, NorthEast, SouthWest, SouthEast }
    public enum WallSegmentType { Wall, Door, None }

    public class Wall : MonoBehaviour
    {
        public Transform wallParent;
        public HidingObject[] wallSegments;
        public GameObject[] wallConnectors;
        public GameObject[] wallModels;
        public GameObject[] doorModels;
        public int floor = 0;

        public WallSegmentType typeSegment0 = WallSegmentType.Wall;
        public WallSegmentType typeSegment1 = WallSegmentType.Wall;
        public WallSegmentType typeSegment2 = WallSegmentType.Wall;
        public WallSegmentType typeSegment3 = WallSegmentType.Wall;

        public WallDirection wallDirection;
        public HidingAnimationType animationType;

        public Texture2D texture;

        [ContextMenu("Update Wall")]
        void UpdateWall()
        {
            UpdateRotation();
            UpdateSegmentTypes();
            UpdateHidingAnimationType();
            UpdateRenderers();

        }

        void UpdateRotation()
        {
            Debug.LogWarning("Updating Rotation has not been implemented yet");
        }

        void UpdateSegmentTypes()
        {
            wallModels[0].SetActive(typeSegment0 == WallSegmentType.Wall); doorModels[0].SetActive(typeSegment0 == WallSegmentType.Door);
            wallModels[1].SetActive(typeSegment1 == WallSegmentType.Wall); doorModels[1].SetActive(typeSegment1 == WallSegmentType.Door);
            wallModels[2].SetActive(typeSegment2 == WallSegmentType.Wall); doorModels[2].SetActive(typeSegment2 == WallSegmentType.Door);
            wallModels[3].SetActive(typeSegment3 == WallSegmentType.Wall); doorModels[3].SetActive(typeSegment3 == WallSegmentType.Door);
        }

        void UpdateHidingAnimationType()
        {
            foreach(HidingObject h in wallSegments)
            {
                h.floor = floor;
                h.animationType = animationType;
                h.animValue = animationType == HidingAnimationType.Translation ?
                    new Vector3(0.0f,30.0f,0.0f) :
                    new Vector3(0.0f,0.0f,170.0f);
            }

            foreach(GameObject g in wallConnectors)
            {
                g.SetActive(animationType == HidingAnimationType.Translation);
            }
        }

        void UpdateRenderers()
        {
            for (int i = 0; i < wallSegments.Length; i++)
            {
                Renderer[] renderers = wallSegments[i].GetComponentsInChildren<Renderer>(true);
                foreach(Renderer r in renderers)
                {
                    r.material.SetTexture("_Texture", texture);
                    r.material.SetFloat("_SegmentIndex", i);
                    //r.material.SetInteger("SegmentIndex", i);
                }
            }
        }
    }
}
