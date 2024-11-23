using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HowDoYouFeel.WordsInWordsGame
{
    public class GameManager : MonoBehaviour
    {
        public DialogueChoiceSO[] sectionOneManaDialogue;
        public DialogueChoiceSO[] customerDialogue;
        public DialogueSO[] sectionTwoManaDialogue;
        public DialogueSO lastReply = null;
        [Header("Gameobject References")]
        public DialogueBox customerSpeechBubble;
        public DialogueBox manaSpeechBubble;
        public DialogueBox selfSpeechBubble;
        public GameObject buttonsPanel;
        public DialogueChoiceButton button0, button1, button2;

        public Transform debugCameraPos;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destoy old GameManager"); }
            Instance = this;
        }

        private void Start()
        {
            buttonsPanel.SetActive(false);
            customerSpeechBubble.gameObject.SetActive(false);
            manaSpeechBubble.gameObject.SetActive(false);
            selfSpeechBubble.gameObject.SetActive(false);
            lastReply = null;
            //   StartCoroutine(GameplayLoopC());
        }

        public void Reply(DialogueSO reply)
        {
            lastReply = reply;
        }

        public void StartGameplayLoop()
        {
            StartCoroutine(GameplayLoopC());
        }

        IEnumerator GameplayLoopC()
        {
            Animator animator = GetComponent<Animator>();

            buttonsPanel.SetActive(false);
            customerSpeechBubble.gameObject.SetActive(false);
            manaSpeechBubble.gameObject.SetActive(false);
            selfSpeechBubble.gameObject.SetActive(false);
            lastReply = null;

            for (int i = 1; i < sectionOneManaDialogue.Length; i++)
            {
                customerSpeechBubble.gameObject.SetActive(true);
                yield return customerSpeechBubble.ShowDialogue(customerDialogue[i].dialogueOptions[0], false, 0);

                selfSpeechBubble.gameObject.SetActive(false);
                buttonsPanel.SetActive(true);
                button0.SetDialogue(sectionOneManaDialogue[i].dialogueOptions[0], false, 0);
                button1.SetDialogue(sectionOneManaDialogue[i].dialogueOptions[1], false, 0);
                button2.SetDialogue(sectionOneManaDialogue[i].dialogueOptions[2], false, 0);
                EventSystem.current.SetSelectedGameObject(button0.gameObject);

                while(lastReply == null)
                {
                    yield return null;
                }

                sectionTwoManaDialogue[i] = lastReply;

                buttonsPanel.SetActive(false);
                customerSpeechBubble.gameObject.SetActive(false);
                selfSpeechBubble.gameObject.SetActive(true);
                yield return selfSpeechBubble.ShowDialogue(lastReply, false, 0);
                lastReply=null;

                yield return new WaitForSeconds(0.5f);
            }

            animator.SetTrigger("SwitchCamera");
            buttonsPanel.SetActive(false);
            customerSpeechBubble.gameObject.SetActive(false);
            manaSpeechBubble.gameObject.SetActive(false);
            selfSpeechBubble.gameObject.SetActive(false);
            lastReply = null;

            yield return new WaitForSeconds(4.0f);

            for (int i = 0; i < customerDialogue.Length; i++)
            {
                if(i == 0) { yield return new WaitForSeconds(0.2f); }
                selfSpeechBubble.gameObject.SetActive(false);

                buttonsPanel.SetActive(true);
                button0.SetDialogue(customerDialogue[i].dialogueOptions[0], true, 1);
                button1.SetDialogue(customerDialogue[i].dialogueOptions[1], true, 1);
                button2.SetDialogue(customerDialogue[i].dialogueOptions[2], true, 1);

                EventSystem.current.SetSelectedGameObject(button0.gameObject);

                while (lastReply == null)
                {
                    yield return null;
                }

                if (i == 1) { animator.SetTrigger("ManaTurnFront"); }

                manaSpeechBubble.gameObject.SetActive(false);
                buttonsPanel.SetActive(false);
                selfSpeechBubble.gameObject.SetActive(true);
                yield return selfSpeechBubble.ShowDialogue(lastReply, true, 1);
                lastReply = null;
                yield return new WaitForSeconds(0.5f);
                if (i == 0) { animator.SetTrigger("ManaTurnBack"); }
                manaSpeechBubble.gameObject.SetActive(true);
                yield return manaSpeechBubble.ShowDialogue(sectionTwoManaDialogue[i], true, 1);
            }

            animator.SetTrigger("GameEnd");
            Debug.LogWarning("GAME END NOT IMPLEMENTED");
            //Game end sequence here
        }
    }
}
