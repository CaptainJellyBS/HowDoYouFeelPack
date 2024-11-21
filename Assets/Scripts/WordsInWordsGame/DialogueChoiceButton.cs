using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.WordsInWordsGame
{
    public class DialogueChoiceButton : MonoBehaviour
    {
        public DialogueBox dialogueBox;

        DialogueSO myDialogue;

        public void SetDialogue(DialogueSO dialogue, bool showToneAndBodyLanguage, int section)
        {
            myDialogue = dialogue;
            dialogueBox.ShowDialogue(dialogue, showToneAndBodyLanguage, section);
        }

        public void ChooseThis()
        {
            GameManager.Instance.Reply(myDialogue);
        }
    }
}
