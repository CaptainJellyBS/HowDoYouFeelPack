using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;

namespace HowDoYouFeel.MuseumGame
{
    public class GameManager : MonoBehaviour
    {
        public ObjectPool roomPool;
        public ObjectPool doorPool;

        public Room activeRoom;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destroy a still existing Museum GameManager Instance"); }
            Instance = this;
        }

        private void Start()
        {
            if (activeRoom != null)
            {
                activeRoom.InitializeAsStartRoom();
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void CleanupDoors()
        {
            foreach(GameObject g in doorPool.objects)
            {
                g.GetComponent<Door>().CheckDoorAlive();
            }
        }

        public Room SpawnRoom(Transform doorConnectionPoint)
        {
            Room r = roomPool.SpawnRandom(new Vector3(0, 100, 0), Quaternion.identity).GetComponent<Room>();
            r.SpawnRoomAtPoint(doorConnectionPoint);

            return r;
        }

        public void StoreRoom(Room r)
        {
            //If the room is part of the room pool, store it. Otherwise (if it's a special room), just destroy it.
            if (roomPool.objects.Contains(r.gameObject))
            {
                roomPool.Store(r.gameObject);
            }
            else
            {
                Destroy(r.gameObject);
            }
            CleanupDoors();
        }
    }
}