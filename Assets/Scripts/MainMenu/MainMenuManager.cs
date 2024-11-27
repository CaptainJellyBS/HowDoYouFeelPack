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
    [RequireComponent(typeof(MainMenuButtonVisibility))]
    public class MainMenuManager : MonoBehaviour
    {
        public UnityEvent denyMuseumGame;
        public Image fadePanel;
        public TextMeshProUGUI denyText;
        public Button museumButton;
        public TextMeshProUGUI versionText;
        [Range(0.0f, 1.0f)]
        public float mainMenuMusicVolume = 0.7f;
        bool isSwitching = false;
        int museumDenyCounter = 0;
        bool musicIsPlaying;

        public AudioSource drumsAS, pianoAS, guitarAS, bassAS, violinAS;
        float drumsVolume, pianoVolume, guitarVolume, bassVolume, violinVolume;

        void Start()
        {
            GlobalManager.Instance.CursorVisible = true;
            Time.timeScale = 1.0f;
            versionText.text = "v" + Application.version;

            UpdateMainMenuButtons();
        }
        
        public void FadeOutToScene(int buildindex)
        {
            StartCoroutine(FadeOutToSceneC(buildindex));
        }

        public void UpdateMainMenuButtons()
        {
            GetComponent<MainMenuButtonVisibility>().UpdateMainMenu();
            UpdateMusicVolumes();
        }

        void UpdateMusicVolumes()
        {
            drumsVolume = GlobalManager.Instance.IntroductionFinished ? mainMenuMusicVolume : 0.0f;
            pianoVolume = GlobalManager.Instance.FocusGameFinished ? mainMenuMusicVolume : 0.0f;
            guitarVolume = GlobalManager.Instance.GeniusGameFinished ? mainMenuMusicVolume : 0.0f;
            bassVolume = GlobalManager.Instance.WordsInWordsGameFinished ? mainMenuMusicVolume : 0.0f;
            violinVolume = GlobalManager.Instance.MuseumGameFinished ? mainMenuMusicVolume : 0.0f;

            PlayMusic();
        }

        void PlayMusic()
        {
            if (musicIsPlaying) { return; }
            StartCoroutine(MusicC());
        }

        IEnumerator MusicC()
        {
            if (musicIsPlaying) { yield break; }

            musicIsPlaying = true;
            drumsAS.volume = 0.0f;
            pianoAS.volume = 0.0f;
            guitarAS.volume = 0.0f;
            bassAS.volume = 0.0f;
            violinAS.volume = 0.0f;

            drumsAS.Play();
            pianoAS.Play();
            guitarAS.Play();
            bassAS.Play();
            violinAS.Play();

            float t = 0.0f;
            while(drumsAS.isPlaying || pianoAS.isPlaying || guitarAS.isPlaying || bassAS.isPlaying || violinAS.isPlaying)
            {
                yield return null;
                t += Time.deltaTime;
                drumsAS.volume = Mathf.Lerp(0.0f, drumsVolume, t/2.0f);
            }

            while(true)
            {
                drumsAS.volume = drumsVolume;
                pianoAS.volume = pianoVolume;
                guitarAS.volume = guitarVolume;
                bassAS.volume = bassVolume;
                violinAS.volume = violinVolume;

                drumsAS.Play();
                pianoAS.Play();
                guitarAS.Play();
                bassAS.Play();
                violinAS.Play();

                while(drumsAS.isPlaying || pianoAS.isPlaying || guitarAS.isPlaying || bassAS.isPlaying || violinAS.isPlaying)
                { 
                    yield return null; 
                }
            }
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

                drumsVolume = Mathf.Min(Mathf.Lerp(mainMenuMusicVolume, 0.0f, t), drumsVolume);
                pianoVolume = Mathf.Min(Mathf.Lerp(mainMenuMusicVolume, 0.0f, t), pianoVolume);
                bassVolume = Mathf.Min(Mathf.Lerp(mainMenuMusicVolume, 0.0f, t), bassVolume);
                violinVolume = Mathf.Min(Mathf.Lerp(mainMenuMusicVolume, 0.0f, t), violinVolume);
                guitarVolume = Mathf.Min(Mathf.Lerp(mainMenuMusicVolume, 0.0f, t), guitarVolume);

                drumsAS.volume = drumsVolume; pianoAS.volume = pianoVolume; bassAS.volume = bassVolume; violinAS.volume = violinVolume; guitarAS.volume = guitarVolume;

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

        public void OpenIntroduction()
        {
            GlobalManager.Instance.UpdateIntroductionFinished(true);
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
