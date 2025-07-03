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
        //public float waitUntilHide = 2.0f;
        public float hidingTime = 0.5f;
        public float pauseBetweenDialogue = 1.0f;
        Coroutine dialogueRoutine;
        public LayerMask speechBubbleBoxcastMask;

        public Coroutine PlayDialogue(string dialogueText, Transform targetSpeaker, Animator characterAnimator = null, float waitUntilHide = 2.0f, Coroutine waitUntilCoroutine = null)
        {
            if(dialogueRoutine != null) { StopCoroutine(dialogueRoutine); }
            dialogueRoutine = StartCoroutine(PlayDialogueC(dialogueText, targetSpeaker, waitUntilHide, waitUntilCoroutine, characterAnimator));
            return dialogueRoutine;
        }

        IEnumerator PlayDialogueC(string dialogueText, Transform targetSpeaker, float waitUntilHide, Coroutine waitUntilCoroutine, Animator characterAnimator)
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
                float t = GetCharWaitTime(c);
                if(characterAnimator != null) { AnimateCharacter(characterAnimator, c); }
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

            //istg I'm going to stab someone so bad. I hate this this is stupid this is terrible pain pain pain incredible agony and pain.
            bool[] waiter = new bool[] { false };
            WaitForCoroutine(waitUntilCoroutine, waiter);

            for (float t = 0; t < waitUntilHide || !waiter[0]; t+=Time.deltaTime)
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

        void AnimateCharacter(Animator animator, char c)
        {
            switch (c)
            {
                case ' ': 
                case ',': 
                case '.':
                case '?':
                case '!': return;
                default: animator.SetTrigger("Talk"); return;
            }
        }

        float GetCharWaitTime(char c)
        {
            switch(c)
            {
                case ' ': return characterLength*1.25f;
                case ',': return characterLength * 2.0f;
                case '.':
                case '?':
                case '!': return characterLength * 4.0f;
                default: return characterLength;
            }
        }

        public float GetDialogueTime(string dialogue)
        {
            float time = 0.0f;
            foreach(char c in dialogue)
            {
                time += GetCharWaitTime(c);
            }

            return time;
        }

        //WORSE pain, suffering, and agony. Incredibly stupid. I hate it I hate it I hate it.
        void WaitForCoroutine(Coroutine c, bool[] b)
        {
            StartCoroutine(WaitForCoroutineC(c, b));
        }

        //pain, suffering, and agony
        IEnumerator WaitForCoroutineC(Coroutine c, bool[] b)
        {
            if(c == null) { b[0] = true; yield break; }
            yield return c;
            b[0] = true;
        }

        Vector3 CalculateSBPos(Vector3 targetPos)
        {
            //DEBUG

            RaycastHit hit;
            if(Physics.BoxCast(targetPos + new Vector3(-0.43f, 9.83f, -2.46f), 
                new Vector3(0.1f, 2.3f, 3.2f), Vector3.down, out hit, speechBubble.rotation, 5.5f, speechBubbleBoxcastMask))
            {
                return new Vector3(targetPos.x, Mathf.Max(targetPos.y, hit.point.y), targetPos.z);
            }
            return targetPos;
        }
    }
}
