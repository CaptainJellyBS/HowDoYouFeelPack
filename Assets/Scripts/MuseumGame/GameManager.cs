using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

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

        public int endRoom;

        public List<GameObject> specialRoomsList;
        public List<int> specialRoomIndices;
        public Dictionary<int, GameObject> specialRooms;

        [Header("UI Elements")]
        public GameObject giveUpPanel;
        public Image fadePanel;
        public TextMeshProUGUI fadeText0, fadeText1;
        public GameObject buttonMenu, buttonReload;

        public GameObject endingPanel;
        public Image endingFadePanel;
        public TextMeshProUGUI endingText;

        float giveUpTimer = 0.0f;

        bool endingReached = false, playerGaveUp = false;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destroy a still existing Museum GameManager Instance"); }
            Instance = this;

            if(specialRoomsList.Count != specialRoomIndices.Count) { Debug.LogError("Special Rooms and Special Room Indices are not the same;"); }

            specialRooms = new Dictionary<int, GameObject>();
            for (int i = 0; i < specialRoomIndices.Count; i++)
            {
                specialRooms.Add(specialRoomIndices[i], specialRoomsList[i]);
            }
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
            Room r;
            if(specialRooms.ContainsKey(roomCounter))
            {
                r = Instantiate(specialRooms[roomCounter], new Vector3(0,100,0), Quaternion.identity).GetComponent<Room>();
            }
            else
            {
                r = roomPool.SpawnRandom(new Vector3(0, 100, 0), Quaternion.identity).GetComponent<Room>();
            }
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
                roomCounter++; roomCounter = Mathf.Min(roomCounter, endRoom);
                ppHandler.UpdatePostProcessing(roomCounter);
                //UpdatePostProcessing();
            }
            lastCountedRoom = activeRoom;
        }
        #endregion

        public void ShowGiveUpPanel()
        {
            if(giveUpTimer > 0.0f) { giveUpTimer = Mathf.Max(giveUpTimer, 1.0f); return; }
            StartCoroutine(ShowGiveUpPanelC(1.0f, 0.0f));;
        }

        public void ShowGiveUpPanel(float duration, float delay)
        {
            if (giveUpTimer > 0.0f) { giveUpTimer = 1.0f; return; }
            StartCoroutine(ShowGiveUpPanelC(duration, delay));
        }

        IEnumerator ShowGiveUpPanelC(float duration, float delay)
        {
            giveUpTimer = duration;

            yield return new WaitForSeconds(delay);

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
            playerGaveUp = true;
            if (endingReached) {  yield break; } //Should not play this animation if the ending has been reached.
            float t = 0.0f;
            fadePanel.color = Color.clear;
            buttonMenu.SetActive(false);
            buttonReload.SetActive(false);

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
            buttonMenu.GetComponent<Button>().interactable = false;
            buttonReload.GetComponent<Button>().interactable = false;
            yield return new WaitForSeconds(2.0f);
            buttonMenu.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            buttonReload.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            GlobalManager.Instance.CursorVisible = true;

            buttonMenu.GetComponent<Button>().interactable = true;
            buttonReload.GetComponent<Button>().interactable = true;

            buttonReload.GetComponent<Selectable>().Select();
        }

        public void Ending()
        {
            if (endingReached) { return; }
            StartCoroutine(EndingC());
        }

        IEnumerator EndingC()
        {
            if (endingReached) { yield break; }
            endingReached = true;

            MakeGameUnplayable();

            float gt = 28.0f;
            while (gt >= 0.0f && !playerGaveUp)
            {
                yield return null;
                gt -= Time.deltaTime;
            }

            if (!playerGaveUp)
            {
                Player.Instance.OnGiveUp();
                yield return new WaitForSeconds(5.5f);
            }

            endingFadePanel.color = Color.clear;
            endingText.color = Color.clear;
            endingPanel.gameObject.SetActive(true);
            
            float t = 0.0f;
            while (t <= 2.0f)
            {
                endingFadePanel.color = Color.Lerp(Color.clear, Color.black, Mathf.Min(1.0f, t));
                endingText.color = Color.Lerp(Color.clear, Color.white, Mathf.Clamp(t - 0.75f, 0.0f, 1.0f));
                yield return null;
                t += Time.deltaTime / 3.0f;
            }
            endingFadePanel.color = Color.black;
            endingText.color = Color.white;
            yield return new WaitForSeconds(3.0f);

            t = 0.0f;
            while(t<=1.0f)
            {
                endingText.color = Color.Lerp(Color.white, Color.clear, t);
                yield return null;
                t += Time.deltaTime / 3.0f;
            }

            yield return new WaitForSeconds(1.0f);

            GlobalManager.Instance.CursorVisible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);

        }

        private void OnApplicationQuit()
        {
            if (endingReached)
            {
                MakeGameUnplayable();
            }
        }

        void MakeGameUnplayable()
        {
            string path = Application.persistentDataPath + "/MG_ReadBeforeDeleting.txt";
            if (File.Exists(path)) { return; }

            StreamWriter stream = new StreamWriter(path);
            stream.WriteLine("Will deleting this file make the Museum game playable again?");
            stream.WriteLine("Yes. Deleting this file will make the Museum game playable again.");
            stream.WriteLine("No, you probably should not do it.");
            stream.WriteLine("The game is not meant to be replayed. If you want to revisit the experience, use your memory.");
            stream.WriteLine("The effect of your personal opinions and biases on that memory are far more beautiful than experiencing this game again.");
            stream.WriteLine("There is nothing to be gained by playing it again.");
            stream.WriteLine("");
            stream.WriteLine("But I want to make someone else play/record footage/have amnesia/have a seriously valid reason to need to replay this beyond wanting to experience it again!");
            stream.WriteLine("If that is genuinely true, go ahead!");
            stream.WriteLine("");
            stream.WriteLine("I'm a rebel, your opinion does not matter to me, I am sure I want to replay this game!");
            stream.WriteLine("I cannot tell you what to do. I can only inform you about your choices.");
            stream.Close();
        }
    }
}
