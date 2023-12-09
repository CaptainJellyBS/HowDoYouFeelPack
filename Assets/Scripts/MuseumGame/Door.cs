using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.MuseumGame
{
    public class Door : MonoBehaviour
    {
        [Header("External GameObjects")]
        public Transform insideConnectionPoint;
        public Transform outsideConnectionPoint;

        public Room insideRoom;
        public Room outsideRoom;

        [Header("Internal Gameobjects")]
        public GameObject doorObject;
        public AudioSource doorAudio;
        public AudioClip doorOpenClip, doorCloseClip;

        public bool playerIsInDoor { get; private set; }
        bool playerInInsideTrigger, playerInOutsideTrigger;

        bool isAnimationPlaying = false;        

        //This function is meant to be called through UnityEvents.
        public void SetInsideTrigger(bool value)
        {
            playerInInsideTrigger = value;
            SetDoorValue();
        }

        //This function is meant to be called through UnityEvents
        public void SetOutsideTrigger(bool value)
        {
            playerInOutsideTrigger = value;
            SetDoorValue();
        }

        void SetDoorValue()
        {
            bool isIn = playerInInsideTrigger || playerInOutsideTrigger;

            if(playerInInsideTrigger && !playerInOutsideTrigger) { GameManager.Instance.activeRoom = insideRoom; }
            if (!playerInInsideTrigger && playerInOutsideTrigger) { GameManager.Instance.activeRoom = outsideRoom; }

            if(isIn && !playerIsInDoor)
            {
                playerIsInDoor = isIn;
                StartCoroutine(DoorAnimationC());
            }
            playerIsInDoor = isIn;
        }

        IEnumerator DoorAnimationC()
        {
            if (isAnimationPlaying) { yield break; }
            isAnimationPlaying = true;
            
            if(insideRoom == null)
            {
                insideRoom = GameManager.Instance.SpawnRoom(insideConnectionPoint);
            }

            if(outsideRoom == null)
            {
                outsideRoom = GameManager.Instance.SpawnRoom(outsideConnectionPoint);
            }

            doorObject.SetActive(false);

            doorAudio.Stop();
            doorAudio.clip = doorOpenClip;
            doorAudio.Play();

            while(playerIsInDoor)
            {
                yield return null;
            }

            doorAudio.Stop();
            doorAudio.clip = doorCloseClip;
            doorAudio.Play();
            doorObject.SetActive(true);

            if(outsideRoom != GameManager.Instance.activeRoom)
            {
                GameManager.Instance.StoreRoom(outsideRoom);
                outsideRoom = null;
            }

            if(insideRoom != GameManager.Instance.activeRoom)
            {
                GameManager.Instance.StoreRoom(insideRoom);
                insideRoom = null;
            }

            isAnimationPlaying = false;
        }

        /// <summary>
        /// Check if either of the two rooms this door is a part of is active. If not, return the door to the object pool
        /// </summary>
        public void CheckDoorAlive()
        {
            bool insideAlive = insideRoom != null ? insideRoom.gameObject.activeSelf : false;
            bool outsideAlive = outsideRoom != null ? outsideRoom.gameObject.activeSelf : false;

            if(!insideAlive && !outsideAlive) 
            {
                insideRoom = null;
                outsideRoom = null;
                GameManager.Instance.doorPool.Store(gameObject); 
            }
        }
    }
}
