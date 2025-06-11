using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public enum WallDirection { NorthWest, NorthEast, SouthWest, SouthEast }
    public enum WallSegmentType { Wall, Door, None }

    [RequireComponent(typeof(HidingObject))]
    public class Wall : MonoBehaviour
    {
        public Transform wallParent;
        HidingObject hidingObject;
        public GameObject[] wallConnectors;
        public GameObject[] wallTops;
        public GameObject[] wallBottoms;
        public GameObject[] doorTops;
        public GameObject[] doorBottoms;
        public int floor = 0;

        public WallSegmentType typeSegment0 = WallSegmentType.Wall;
        public WallSegmentType typeSegment1 = WallSegmentType.Wall;
        public WallSegmentType typeSegment2 = WallSegmentType.Wall;
        public WallSegmentType typeSegment3 = WallSegmentType.Wall;

        public WallDirection wallDirection;
        public HidingAnimationType animationType;

        private void Start()
        {
            UpdateWall();
        }

        [ContextMenu("Update Wall")]
        void UpdateWall()
        {
            hidingObject = GetComponent<HidingObject>();
            UpdateRotation();
            UpdateSegmentTypes();
            UpdateHidingAnimationType();

        }

        void UpdateRotation()
        {
            switch(wallDirection)
            {
                case WallDirection.NorthEast:
                    transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    wallParent.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    break;
                case WallDirection.NorthWest:
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    wallParent.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);

                    break;
                case WallDirection.SouthEast:
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    wallParent.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                case WallDirection.SouthWest: 
                    transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    wallParent.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);

                    break;
            }
        }

        void UpdateSegmentTypes()
        {
            wallBottoms[0].SetActive(typeSegment0 == WallSegmentType.Wall); wallTops[0].SetActive(typeSegment0 == WallSegmentType.Wall); 
            wallBottoms[1].SetActive(typeSegment1 == WallSegmentType.Wall); wallTops[1].SetActive(typeSegment1 == WallSegmentType.Wall); 
            wallBottoms[2].SetActive(typeSegment2 == WallSegmentType.Wall); wallTops[2].SetActive(typeSegment2 == WallSegmentType.Wall); 
            wallBottoms[3].SetActive(typeSegment3 == WallSegmentType.Wall); wallTops[3].SetActive(typeSegment3 == WallSegmentType.Wall); 

            doorBottoms[0].SetActive(typeSegment0 == WallSegmentType.Door); doorTops[0].SetActive(typeSegment0 == WallSegmentType.Door);
            doorBottoms[1].SetActive(typeSegment1 == WallSegmentType.Door); doorTops[1].SetActive(typeSegment1 == WallSegmentType.Door);
            doorBottoms[2].SetActive(typeSegment2 == WallSegmentType.Door); doorTops[2].SetActive(typeSegment2 == WallSegmentType.Door);
            doorBottoms[3].SetActive(typeSegment3 == WallSegmentType.Door); doorTops[3].SetActive(typeSegment3 == WallSegmentType.Door);
        }

        void UpdateHidingAnimationType()
        {
                hidingObject.floor = floor;
                hidingObject.animationType = animationType;
                hidingObject.animValue = animationType == HidingAnimationType.Translation ?
                    new Vector3(0.0f,30.0f,0.0f) :
                    new Vector3(-170.0f,0.0f,0.0f);

            foreach(GameObject g in wallConnectors)
            {
                g.SetActive(animationType == HidingAnimationType.Translation);
            }
        }
    }
}
