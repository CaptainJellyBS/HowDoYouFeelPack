using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.WordsInWordsGame
{
    [CreateAssetMenu(fileName = "DialogueChoice", menuName = "ScriptableObjects/WordsInWordsGame/DialogueChoice")]
    public class DialogueChoiceSO : ScriptableObject
    {
        public DialogueSO[] dialogueOptions;
    }
}
