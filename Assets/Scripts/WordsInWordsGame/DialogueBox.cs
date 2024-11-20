using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HowDoYouFeel.WordsInWordsGame
{
    public class DialogueBox : MonoBehaviour
    {
        public Color dialogueSpokenColor, dialogueToneColor, dialogueBodyLanguageColor;
        public TextMeshProUGUI textMesh;
        public float textDelay = 0.05f;
        public DialogueSO debugDialogue;

        Coroutine dialogueRoutine;

        public void ShowDebugDialogue()
        {
            ShowDialogue(debugDialogue, true, 0);
        }

        public void ShowDialogue(DialogueSO dialogue, bool showToneAndBodyLanguage, int section)
        {
            if(section != 0 && section != 1) { Debug.LogError("Section should be 0 or 1"); }
            if(dialogueRoutine != null) { StopCoroutine(dialogueRoutine); }

            string d = dialogue.dialogueSpoken;
            if(dialogue.dialogueSectionOneOverride.Length > 0 && section == 0) { d = dialogue.dialogueSectionOneOverride; }
            if(dialogue.dialogueSectionTwoOverride.Length > 0 && section == 1) { d = dialogue.dialogueSectionTwoOverride; }

            dialogueRoutine = StartCoroutine(PlayDialogueC(d, dialogue.dialogueTone, dialogue.dialogueBodyLanguage, showToneAndBodyLanguage));
        }

        IEnumerator PlayDialogueC(string spoken, string tone, string bodyLanguage, bool showToneAndBodyLanguage)
        {
            int spokenIndex = 0;
            int bodyLanguageIndex = 0;
            textMesh.text = CreateDialogue(spoken, tone, bodyLanguage, showToneAndBodyLanguage, spokenIndex, bodyLanguageIndex);

            while(showToneAndBodyLanguage && bodyLanguageIndex < bodyLanguage.Length)
            {
                textMesh.text = CreateDialogue(spoken, tone, bodyLanguage, showToneAndBodyLanguage, spokenIndex, bodyLanguageIndex);
                yield return new WaitForSeconds(textDelay/4);
                bodyLanguageIndex++;
            }

            while(spokenIndex <= spoken.Length)
            {
                textMesh.text = CreateDialogue(spoken, tone, bodyLanguage, showToneAndBodyLanguage, spokenIndex, bodyLanguageIndex);
                yield return new WaitForSeconds(textDelay);
                spokenIndex++;
            }
        }

        string CreateDialogue(string spoken, string tone, string bodyLanguage, bool showToneBodyLanguage, int spokenIndex, int bodyLanguageIndex)
        {
            string clearColor = ColorUtility.ToHtmlStringRGBA(Color.clear);
            string toneColor = showToneBodyLanguage && spokenIndex > 0 ? ColorUtility.ToHtmlStringRGBA(dialogueToneColor) : clearColor;
            string bodyLanguageColor = ColorUtility.ToHtmlStringRGBA(dialogueBodyLanguageColor);
            string spokenColor = ColorUtility.ToHtmlStringRGBA(dialogueSpokenColor);

            string toneString = "<i><color=#" + toneColor + ">" + tone + "</color></i> ";
            string spokenString = "<color=#" + spokenColor + ">" + spoken.Substring(0,spokenIndex) + "</color>" +
                "<color=#" + clearColor + ">" + spoken.Substring(spokenIndex, spoken.Length-spokenIndex) + "</color> ";
            string bodyLanguageString = 
                showToneBodyLanguage 
                ?
                "<i><color=#" + bodyLanguageColor + ">" + bodyLanguage.Substring(0, bodyLanguageIndex) + "</color>" +
                "<color=#" + clearColor + ">" + bodyLanguage.Substring(bodyLanguageIndex, bodyLanguage.Length - bodyLanguageIndex) + "</color></i>" 
                :
                "<color=#" + clearColor + ">" + bodyLanguage + "</color></i>"
                ;

            string finalString = toneString + spokenString + bodyLanguageString;
            return finalString;
        }
    }
}
