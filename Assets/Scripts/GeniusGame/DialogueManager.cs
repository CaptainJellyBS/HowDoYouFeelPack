using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class DialogueManager : MonoBehaviour
    {
        public Transform speechBubble;
        public TextMeshProUGUI dialogueTextMesh;
        public float hidingHeight = 15.0f;
        public float characterLength = 0.2f;
        public float waitUntilHide = 2.0f;
        public float hidingTime = 0.5f;
        public float pauseBetweenDialogue = 1.0f;
        Coroutine dialogueRoutine;
        public LayerMask speechBubbleBoxcastMask;

        public Coroutine PlayDialogue(string dialogueText, Transform targetSpeaker)
        {
            if(dialogueRoutine != null) { StopCoroutine(dialogueRoutine); }
            dialogueRoutine = StartCoroutine(PlayDialogueC(dialogueText, targetSpeaker));
            return dialogueRoutine;
        }

        IEnumerator PlayDialogueC(string dialogueText, Transform targetSpeaker)
        {
            dialogueTextMesh.text = string.Empty;

            speechBubble.position = targetSpeaker.position + Vector3.up * hidingHeight;
            speechBubble.gameObject.SetActive(true);

            for (float t = 0; t < 1.0f; t+=Time.deltaTime / hidingTime)
            {
                yield return null;
                speechBubble.position = Vector3.MoveTowards(speechBubble.position, 
                    CalculateSBPos(targetSpeaker.position + Vector3.up * Mathf.Lerp(hidingHeight, 0, t)), 50.0f * Time.deltaTime);
            }

            //yield return new WaitForSeconds(0.5f);
            for (float t = 0; t < 0.5f; t+=Time.deltaTime)
            {
                yield return null;
                speechBubble.position = Vector3.MoveTowards(speechBubble.position, CalculateSBPos(targetSpeaker.position), 10.0f * Time.deltaTime);
            }

            for (int i = 0; i < dialogueText.Length; i++)
            {
                char c = dialogueText[i];
                float t = getCharWaitTime(c);
                dialogueTextMesh.text = 
                    dialogueText.Substring(0, i + 1) 
                    + "<color=#00000000>"
                    + dialogueText.Substring(Mathf.Min(i+1, dialogueText.Length-1), dialogueText.Length-(i+1))
                    + "</color>";

                while(t > 0.0f)
                {
                    speechBubble.position = Vector3.MoveTowards(speechBubble.position, CalculateSBPos(targetSpeaker.position), 10.0f * Time.deltaTime);

                    yield return null;
                    t -= Time.deltaTime;                    
                }
            }

            dialogueTextMesh.text = dialogueText;
            for (float t = 0; t < waitUntilHide; t+=Time.deltaTime)
            {
                speechBubble.position = Vector3.MoveTowards(speechBubble.position, CalculateSBPos(targetSpeaker.position), 10.0f * Time.deltaTime);
                yield return null;
            }
           
            for (float t = 0; t < 1.0f; t += Time.deltaTime / hidingTime)
            {
                yield return null;
                speechBubble.position = Vector3.MoveTowards(speechBubble.position, CalculateSBPos(targetSpeaker.position + Vector3.up * Mathf.Lerp(0, hidingHeight, t)), 50.0f * Time.deltaTime);
            }

            dialogueTextMesh.text = string.Empty;
            speechBubble.gameObject.SetActive(false);
            yield return new WaitForSeconds(pauseBetweenDialogue);

            dialogueRoutine = null;
        }

        float getCharWaitTime(char c)
        {
            switch(c)
            {
                case ' ': return characterLength/4.0f;
                case ',': return characterLength * 2.0f;
                case '.':
                case '?':
                case '!': return characterLength * 4.0f;
                default: return characterLength;
            }
        }

        Vector3 CalculateSBPos(Vector3 targetPos)
        {
            //DEBUG

            RaycastHit hit;
            if(Physics.BoxCast(targetPos + new Vector3(-0.43f, 9.83f, -2.46f), 
                new Vector3(0.1f, 2.3f, 3.8f), Vector3.down, out hit, speechBubble.rotation, 5.5f, speechBubbleBoxcastMask))
            {
                return new Vector3(targetPos.x, Mathf.Max(targetPos.y, hit.point.y), targetPos.z);
            }
            return targetPos;
        }
    }
}
