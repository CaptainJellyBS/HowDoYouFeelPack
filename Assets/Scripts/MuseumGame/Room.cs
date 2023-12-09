using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.MuseumGame
{
    public class Room : MonoBehaviour
    {
        public Transform[] connectionPoints;
        public GameObject models;

        public void SpawnRoomAtPoint(Transform doorConnectionPoint)
        {
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
            models.SetActive(true);
        }

        public void SpawnDoor(Transform connectionPoint)
        {
            Door d = GameManager.Instance.doorPool.Spawn(connectionPoint.position, connectionPoint.rotation).GetComponent<Door>();
            d.insideRoom = this;
        }

        public void InitializeAsStartRoom()
        {
            for (int i = 0; i < connectionPoints.Length; i++)
            {
                SpawnDoor(connectionPoints[i]);
            }
            models.SetActive(true);
        }
    }
}
