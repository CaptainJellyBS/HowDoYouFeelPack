using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.MuseumGame
{
    public class Room : MonoBehaviour
    {
        public ArtStyle roomStyle;
        public Transform[] artPoints;
        public List<ShiftingArt> artObjects;
        public Transform[] connectionPoints;
        public GameObject models;
        [Range(0.0f, 1.0f)]
        public float artSpawnChance;

        public void SpawnRoomAtPoint(Transform doorConnectionPoint)
        {
            roomStyle = (ArtStyle)Random.Range(0, 5);

            models.SetActive(false);
            if(connectionPoints.Length <= 0) { throw new System.Exception("Room has no connection points. Fix your shit."); }

            int connectionIndex = Random.Range(0, connectionPoints.Length);

            //Quaternion dif = Quaternion.Inverse(doorConnectionPoint.rotation) * connectionPoints[connectionIndex].rotation;
            //transform.rotation = dif * transform.rotation;
            Quaternion targetRot = doorConnectionPoint.rotation;
            Quaternion childRot = connectionPoints[connectionIndex].rotation;
            transform.rotation = transform.rotation * (Quaternion.Inverse(childRot) * targetRot);

            if(connectionPoints[connectionIndex].rotation.eulerAngles != doorConnectionPoint.rotation.eulerAngles)
            {
                Debug.LogError("WHY THO");
            }
            transform.position = doorConnectionPoint.position + (transform.position - connectionPoints[connectionIndex].position);

            for (int i = 0; i < connectionPoints.Length; i++)
            {
                if(i == connectionIndex) { continue; }
                SpawnDoor(connectionPoints[i]);
            }

            SpawnArt();

            models.SetActive(true);
        }

        public void SpawnDoor(Transform connectionPoint)
        {
            Door d = GameManager.Instance.doorPool.Spawn(connectionPoint.position, connectionPoint.rotation).GetComponent<Door>();
            d.insideRoom = this;
        }

        public void InitializeAsStartRoom()
        {
            //Debug.LogWarning("ToDo: Uncomment artstyle variety in Room.cs, in BOTH initialize functions");
            roomStyle = (ArtStyle)Random.Range(0, 5);
            for (int i = 0; i < connectionPoints.Length; i++)
            {
                SpawnDoor(connectionPoints[i]);
            }

            SpawnArt();

            models.SetActive(true);
        }

        void SpawnArt()
        {
            artObjects = new List<ShiftingArt>();

            for (int i = 0; i < artPoints.Length; i++)
            {
                if (!artPoints[i].gameObject.activeSelf) { continue; }
                if(Random.Range(0.0f, 1.0f) >= artSpawnChance) { continue; }
                ShiftingArt art = GameManager.Instance.artPool.Spawn(artPoints[i].position, artPoints[i].rotation).GetComponent<ShiftingArt>();
                art.Initialize(roomStyle);
                artObjects.Add(art);
            }
        }
        
        public void CleanupArt()
        {
            if(artObjects == null) { return; }
            
            foreach (ShiftingArt art in artObjects)
            {
                GameManager.Instance.artPool.Store(art.gameObject);
            }
            
        }
    }
}
