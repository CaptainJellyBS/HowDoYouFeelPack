using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.IO;
using TMPro;

namespace HowDoYouFeel.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public UnityEvent denyMuseumGame;
        public Image fadePanel;
        public TextMeshProUGUI denyText;
        public Button museumButton;
        public TextMeshProUGUI versionText;
        bool isSwitching = false;
        int museumDenyCounter = 0;

        void Start()
        {
            GlobalManager.Instance.CursorVisible = true;
            Time.timeScale = 1.0f;
            versionText.text = "v" + Application.version;
        }
        
        public void FadeOutToScene(int buildindex)
        {
            StartCoroutine(FadeOutToSceneC(buildindex));
        }

        IEnumerator FadeOutToSceneC(int buildindex)
        {
            if (isSwitching) { Debug.LogWarning("Should not be able to start scene transition twice"); yield break; }
            isSwitching = true;
            EventSystem.current.SetSelectedGameObject(null);

            Color endColor = fadePanel.color;
            fadePanel.color = Color.clear;
            fadePanel.gameObject.SetActive(true);

            float t = 0.0f;
            while(t<=1.0f)
            {
                t += Time.deltaTime * 2.0f;
                fadePanel.color = Color.Lerp(Color.clear, endColor, t);
                yield return null;
            }

            fadePanel.color = endColor;
            SceneManager.LoadScene(buildindex);
        }

        public void MuseumGameCheck()
        {
            string path = Application.persistentDataPath + "/MG_ReadBeforeDeleting.txt";
            if (File.Exists(path))
            {
                UpdateDenyText();
                denyMuseumGame.Invoke();
                return;
            }

            FadeOutToScene(1);
        }

        void UpdateDenyText()
        {
            switch(museumDenyCounter)
            {
                case 0: denyText.text = "No."; break;
                case 1: denyText.text = "No!"; break;
                case 2: denyText.text = "There is nothing left to experience there."; break;
                case 3: denyText.text = "I'm serious. There is no do-over."; break;
                case 4:
                default:
                    denyText.text = "NO!!!";
                    Selectable below = museumButton.navigation.selectOnDown;
                    Selectable above = museumButton.navigation.selectOnUp;
                    Navigation nBelow = below.navigation;
                    Navigation nAbove = above.navigation;
                    
                    nBelow.selectOnUp = above;
                    below.navigation = nBelow;
                    nAbove.selectOnDown = below;
                    above.navigation = nAbove;
                    museumButton.gameObject.SetActive(false);
                    break;
            }

            museumDenyCounter++;
        }
    }
}
