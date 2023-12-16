using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using UnityEngine.UI;
using TMPro;

namespace HowDoYouFeel.MuseumGame
{
    public class GameManager : MonoBehaviour
    {
        public ObjectPool roomPool;
        public ObjectPool doorPool;
        public ObjectPool artPool;
        
        public PostProcessingHandler ppHandler;

        public Room activeRoom, lastCountedRoom;
        public int roomCounter;
        public float maxArtXSize = 3.0f, maxArtYSize = 3.5f, minArtXSize = 0.25f, minArtYSize = 0.25f;
        
        [Header("UI Elements")]
        public GameObject giveUpPanel;
        public Image fadePanel;
        public TextMeshProUGUI fadeText0, fadeText1;        

        float giveUpTimer = 0.0f;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destroy a still existing Museum GameManager Instance"); }
            Instance = this;
        }

        private void Start()
        {
            giveUpPanel.SetActive(false);
            roomCounter = 0;
            if (activeRoom != null)
            {
                activeRoom.InitializeAsStartRoom();
            }

            fadePanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        #region Room spawning stuff
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
            r.CleanupArt();
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

        public void ExitedDoor()
        {
            if(lastCountedRoom != activeRoom) 
            { 
                roomCounter++;
                ppHandler.UpdatePostProcessing(roomCounter);
                //UpdatePostProcessing();
            }
            lastCountedRoom = activeRoom;
        }
        #endregion

        public void ShowGiveUpPanel()
        {
            if(giveUpTimer > 0.0f) { giveUpTimer = 1.0f; return; }
            StartCoroutine(ShowGiveUpPanelC());
        }

        IEnumerator ShowGiveUpPanelC()
        {
            giveUpTimer = 1.0f;

            giveUpPanel.transform.localScale = new Vector3(1, 0, 1);
            giveUpPanel.SetActive(true);

            float t = 0.0f;
            while(t<=1.0f)
            {
                giveUpPanel.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, t), 1);

                yield return null;
                t += Time.deltaTime * 8.0f;
            }

            giveUpPanel.transform.localScale = Vector3.one;

            while (giveUpTimer > 0.0f)
            {
                giveUpPanel.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, Mathf.Clamp(giveUpTimer * 8, 0.0f, 1.0f)), 1);

                yield return null;
                giveUpTimer -= Time.deltaTime;
            }

            giveUpPanel.transform.localScale = new Vector3(1, 0, 1);
            giveUpPanel.SetActive(false);
        }

        public void GiveUpFadeOut()
        {
            StartCoroutine(GiveUpFadeOutC());
        }

        IEnumerator GiveUpFadeOutC()
        {
            float t = 0.0f;
            fadePanel.color = Color.clear;

            Color origTextColor = fadeText0.color;
            fadeText0.color = Color.clear;
            fadeText1.color = Color.clear;
            fadeText0.gameObject.SetActive(true);
            fadeText1.gameObject.SetActive(true);

            fadePanel.gameObject.SetActive(true);
            while(t<=1.0f)
            {
                fadePanel.color = Color.Lerp(Color.clear, Color.black, t);
                yield return null;
                t += Time.deltaTime / 3.0f;
            }
            fadePanel.color = Color.black;

            t = 0.0f;

            while(t <= 3.0f)
            {
                fadeText0.color = Color.Lerp(Color.clear, origTextColor, Mathf.Clamp(t, 0.0f, 1.0f));
                fadeText1.color = Color.Lerp(Color.clear, origTextColor, Mathf.Clamp(t-2.0f, 0.0f, 1.0f));

                yield return null;
                t += Time.deltaTime;
            }
        }
    }
}
