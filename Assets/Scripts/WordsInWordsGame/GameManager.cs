using HowDoYouFeel.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HowDoYouFeel.WordsInWordsGame
{
    public class GameManager : MonoBehaviour
    {
        public AudioSource musicAS;
        public AudioClip backgroundMusicSec0, backupMusicSec0, backgroundMusicSec1, backupMusicSec1;
        float origVolume;

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

        Coroutine musicRoutine;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destoy old GameManager"); }
            Instance = this;
        }

        private void Start()
        {
            GlobalManager.Instance.CursorVisible = true;
            origVolume = musicAS.volume;
            buttonsPanel.SetActive(false);
            customerSpeechBubble.gameObject.SetActive(false);
            manaSpeechBubble.gameObject.SetActive(false);
            selfSpeechBubble.gameObject.SetActive(false);
            lastReply = null;
            musicRoutine = StartCoroutine(MusicC(backgroundMusicSec0, backupMusicSec0));
        }

        public void Reply(DialogueSO reply)
        {
            lastReply = reply;
        }

        public void StartGameplayLoop()
        {
            StartCoroutine(GameplayLoopC());
        }

        public void EndGame()
        {
            //Permanent data stuff here
            SceneManager.LoadScene(0);
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

            float t = 0.0f;
            while(t <= 1.0f)
            {
                yield return null;
                t += Time.deltaTime / 2.0f;
                musicAS.volume = Mathf.Lerp(origVolume, 0.0f, t);
            }

            StopCoroutine(musicRoutine);
            musicRoutine = StartCoroutine(MusicC(backgroundMusicSec1, backupMusicSec1));

            yield return new WaitForSeconds(2.0f);

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

            GlobalManager.Instance.UpdateWordsInWordsGameFinished(true);
            animator.SetTrigger("GameEnd");

            float ti = 0.0f;
            while(ti<=1.0f)
            {
                yield return null;
                ti += Time.deltaTime / 2.0f;
                musicAS.volume = Mathf.Lerp(origVolume, 0.0f, ti);
            }

            StopCoroutine(musicRoutine);
            musicAS.Stop();
            //Debug.LogWarning("GAME END NOT IMPLEMENTED");
            //Game end sequence here
        }

        IEnumerator MusicC(AudioClip music, AudioClip backup)
        {
            musicAS.Stop();
            musicAS.volume = 0.0f;
            musicAS.clip = music;
            musicAS.Play();

            float t = 0.0f;
            while(t<=1.0f)
            {
                yield return null;
                t += Time.deltaTime /2.0f;
                musicAS.volume = Mathf.Lerp(0.0f, origVolume, t);
            }

            while(true)
            {
                if (!musicAS.isPlaying)
                {
                    musicAS.clip = backup;
                    musicAS.Play();
                }
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
