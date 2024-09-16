using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HowDoYouFeel.MainMenu
{
    public class Credits : MonoBehaviour
    {
        Animator animator;
        Dictionary<int, List<string>> credits;

        [TextArea] [Header("Main Credits")]
        public string mainCreditsTitle = "Credits";
        [TextArea]
        public List<string> mainCredits;

        [TextArea] [Header("Art Credits")]
        public string artTitle = "Art";
        [TextArea]
        public List<string> artCredits;

        [TextArea] [Header("Special Thanks")]
        public string specialTitle = "Special Thanks";
        [TextArea]
        public List<string> specialCredits;

        [TextArea] [Header("Music Credits")]
        public string musicTitle = "Music";
        [TextArea]
        public List<string> musicCredits;

        [TextArea] [Header("SFX Credits")]
        public string sfxTitle = "Sound Effects";
        [TextArea]
        public List<string> sfxCredits;

        public TextMeshProUGUI creditsTopHeaderText, creditsTopContentText, creditsBottomHeaderText, creditsBottomContentText, titleText;

        int currentTitleIndex = -1, currentCreditsIndex = 0;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if(animator == null) { animator = GetComponent<Animator>(); }
            credits = new Dictionary<int, List<string>>();
            credits.Add(0, mainCredits);
            credits.Add(1, artCredits);
            credits.Add(2, musicCredits);
            credits.Add(3, sfxCredits);
            credits.Add(4, specialCredits);
        }

        public void ResetCredits()
        {
            currentTitleIndex = -1;
            currentCreditsIndex = 0;
        }

        public void UpdateTopText()
        {
            UpdateText(creditsTopHeaderText, creditsTopContentText);
        }

        public void UpdateBottomText()
        {
            UpdateText(creditsBottomHeaderText, creditsBottomContentText);
        }

        public void UpdateText(TextMeshProUGUI header, TextMeshProUGUI content)
        {
            string[] splitCredits = credits[currentTitleIndex][currentCreditsIndex].Split('|');
            if(splitCredits.Length != 2) { Debug.LogError("Credits formatted wrong. Problem lies in: " + currentTitleIndex + ", " + currentCreditsIndex); }

            header.text = splitCredits[0];
            content.text = splitCredits[1];

            currentCreditsIndex++;
            if(currentCreditsIndex >= credits[currentTitleIndex].Count) { animator.SetBool("HeaderNeedsUpdating", true); currentCreditsIndex = 0; }
        }

        public void UpdateHeader()
        {
            currentTitleIndex++; currentTitleIndex %= 5;
            switch (currentTitleIndex)
            {
                case 0: titleText.text = mainCreditsTitle; break;
                case 1: titleText.text = artTitle; break;
                case 2: titleText.text = musicTitle; break;
                case 3: titleText.text = sfxTitle; break;
                case 4: titleText.text = specialTitle; break;
                default: titleText.text = "Credits"; Debug.LogWarning("Credits ran out of bounds???"); break;
            }

            animator.SetBool("HeaderNeedsUpdating", false);
            currentCreditsIndex = 0;
        }
    }
}
