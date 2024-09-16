using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace HowDoYouFeel.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public Image fadePanel;
        bool isSwitching = false;

        void Start()
        {
            GlobalManager.Instance.CursorVisible = true;
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
    }
}
