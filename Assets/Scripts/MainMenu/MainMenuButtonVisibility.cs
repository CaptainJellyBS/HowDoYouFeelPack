using HowDoYouFeel.Global;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HowDoYouFeel.MainMenu
{
    public class MainMenuButtonVisibility : MonoBehaviour
    {
        public Selectable introductionButton;
        public Selectable[] gameButtons;
        public Selectable conclusionButton;
        public TextMeshProUGUI conclusionText;
        public Selectable settingsButton;
        public Color conclusionColor;

        public void UpdateMainMenu()
        {
            GlobalManager.Instance.Load();
            bool introDone = GlobalManager.Instance.IntroductionFinished;
            bool conclusionAvailable = CheckConclusion();

            if (!introDone)
            {
                for (int i = 0; i < gameButtons.Length; i++)
                {
                    gameButtons[i].gameObject.SetActive(false);
                }
                LinkSelectables(introductionButton, settingsButton);
            }
            else
            {
                for (int i = 0; i < gameButtons.Length; i++)
                {
                    gameButtons[i].gameObject.SetActive(true);
                }

                LinkSelectables(introductionButton, gameButtons[0]);
                LinkSelectables(gameButtons[gameButtons.Length - 1], settingsButton);
            }

            if (conclusionAvailable)
            {
                if (!introDone) { Debug.LogError("UH OH SPAGHETTIO. THE INTRO IS NOT DONE BUT THE CONCLUSION HAS BEEN REACHED? IMPOSSIBLE!"); }
                conclusionButton.gameObject.SetActive(true);
                conclusionText.gameObject.SetActive(false);

                LinkSelectables(gameButtons[gameButtons.Length - 1], conclusionButton);
                LinkSelectables(conclusionButton, settingsButton);
            }
            else
            {
                float r = ConclusionPercentage();
                conclusionButton.gameObject.SetActive(false);
                conclusionText.gameObject.SetActive(true);

                Color c = Color.Lerp(Color.clear, conclusionColor, r);
                conclusionText.color = c;
                conclusionText.GetComponent<LetterScatter>().letterColor = c;
            }
        }

        void LinkSelectables(Selectable upper, Selectable lower)
        {
            SetSelectableDown(upper, lower);
            SetSelectableUp(lower, upper);
        }

        void SetSelectableUp(Selectable selectable, Selectable newTarget)
        {
            Navigation nav = selectable.navigation;
            nav.selectOnUp = newTarget;
            selectable.navigation = nav;
        }

        void SetSelectableDown(Selectable selectable, Selectable newTarget)
        {
            Navigation nav = selectable.navigation;
            nav.selectOnDown = newTarget;
            selectable.navigation = nav;
        }


        bool CheckConclusion()
        {
            return GlobalManager.Instance.IntroductionFinished &&
                GlobalManager.Instance.MuseumGameFinished &&
                GlobalManager.Instance.FocusGameFinished &&
                GlobalManager.Instance.WordsInWordsGameFinished &&
                GlobalManager.Instance.GeniusGameFinished;
        }

        float ConclusionPercentage()
        {
            int totalAmount = 5;
            int currentAmount = 0;

            if(GlobalManager.Instance.IntroductionFinished) { currentAmount++; }
            if(GlobalManager.Instance.MuseumGameFinished) { currentAmount++; }
            if (GlobalManager.Instance.FocusGameFinished) { currentAmount++; }
            if (GlobalManager.Instance.WordsInWordsGameFinished) { currentAmount++; }
            if (GlobalManager.Instance.GeniusGameFinished) { currentAmount++; }

            return (float)currentAmount / (float)totalAmount;
        }
    }
}
