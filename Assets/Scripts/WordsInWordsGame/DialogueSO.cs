using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.WordsInWordsGame
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/WordsInWordsGame/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        public string dialogueSpoken = "\"PLACEHOLDER\"";
        public string dialogueTone = "PLACEHOLDER";
        public string dialogueBodyLanguage = "PLACEHOLDER";
        public string dialogueSectionOneOverride = string.Empty;
        public string dialogueSectionTwoOverride = string.Empty;
    }
}
