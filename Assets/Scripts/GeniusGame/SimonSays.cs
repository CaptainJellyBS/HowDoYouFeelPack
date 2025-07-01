using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HowDoYouFeel.GeniusGame
{ 
    public class SimonSays : MonoBehaviour
    {
        [Tooltip("List of elements that light up")] 
        public List<SimonSaysElement> elements; 
        [Tooltip("Length of each sequence. List length determines amount of sequences")] 
        public List<int> sequenceLengths;
        [Tooltip("Should the sequence be the same and become longer, or should a new sequence be randomly generated every time?")]
        public bool extendSequence = true;
        [Tooltip("Should game restart on failure?")]
        public bool restartOnFail = true;

        public float elementLightTime = 1.0f, restBetweenCycles = 3.0f;

        List<int[]> sequence;
        int lastButtonPress;

        Coroutine currentGame;

        public UnityEvent onDone, onCorrect, onFail;

        public Coroutine StartPlay(float startDelay)
        {
            if(currentGame != null) { StopCoroutine(currentGame); }
            GenerateSequence();
            currentGame = StartCoroutine(SimonSaysC(startDelay));
            return currentGame;
        }

        IEnumerator SimonSaysC(float startDelay)
        {
            yield return new WaitForSeconds(startDelay);
            for (int i = 0; i < sequence.Count; i++)
            {
                yield return new WaitForSeconds(elementLightTime + 1.0f);
                lastButtonPress = -1;
                while (lastButtonPress < 0)
                {
                    for (int j = 0; j < sequence[i].Length && lastButtonPress < 0; j++)
                    {
                        float lt = 0.0f;
                        elements[sequence[i][j]].Activate(elementLightTime);
                        while(lastButtonPress < 0 && lt <= elementLightTime + 0.2f)
                        {
                            yield return null;
                            lt += Time.deltaTime;
                        }
                    }

                    float wt = 0.0f;
                    while(lastButtonPress < 0 && wt < restBetweenCycles)
                    {
                        yield return null;
                        wt += Time.deltaTime;
                    }
                }

                foreach(SimonSaysElement e in elements) { e.Stop(); }

                int[] input = new int[sequence[i].Length];
                for (int j = 0; j < sequence[i].Length; j++)
                {
                    while(lastButtonPress < 0) { yield return null; }

                    input[j] = lastButtonPress;
                    elements[lastButtonPress].Activate(elementLightTime);

                    lastButtonPress = -1;
                }

                if(!EvaluateSequence(sequence[i], input))
                {
                    onFail.Invoke();
                    if(restartOnFail) { i = -1; /* ew */ } else { currentGame = null; yield break; }                    
                }
                else 
                { 
                    onCorrect.Invoke(); 
                }
            }

            onDone.Invoke();
            currentGame = null;
        }

        public void GenerateSequence()
        {
            sequence = new List<int[]>();
            for (int i = 0; i < sequenceLengths.Count; i++)
            {
                sequence.Add(new int[sequenceLengths[i]]);
                int j = 0;
                if (i > 0 && extendSequence)
                {
                    while (j < sequence[i - 1].Length && j < sequence[i].Length)
                    {
                        sequence[i][j] = sequence[i - 1][j];
                        j++;
                    }
                }

                while(j < sequence[i].Length)
                {
                    sequence[i][j] = Random.Range(0, elements.Count);
                    j++;
                }
            }

            Debug.LogWarning("DEBUG ACTIVE");

            for (int i = 0; i < sequence.Count; i++)
            {
                string s = string.Empty;
                for (int j = 0; j < sequence[i].Length; j++)
                {
                    s += sequence[i][j] + " -> ";
                }
                Debug.Log(s);
            }
        }

        bool EvaluateSequence(int[] correctSequence, int[] input)
        {
            for (int i = 0; i < correctSequence.Length; i++)
            {
                if(correctSequence[i] != input[i]) { return false; }
            }
            return true;
        }

        public void PressButton(int buttonIndex)
        {
            lastButtonPress = buttonIndex;
        }

    }
}
