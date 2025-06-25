using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    public class SequencingManager : MonoBehaviour
    {
        [Header("Globals")]
        public DialogueManager dm;
        public Door[] doors;

        bool teacherOnDest = false;

        [Header("Teacher NPC")]
        public NPC_Pathwalker teacher;
        public Transform teacherDialoguePoint;
        public Transform[] teacherPathm1, teacherPath0, teacherPath1, teacherPath2;

        

    private void Start()
        {
            StartCoroutine(TestSequenceC());
        }

        IEnumerator TestSequenceC()
        {
            Debug.LogWarning("DEBUG SEQUENCE ACTIVE");
            
            teacherOnDest = false;
            teacher.StartPathWalk(); 
            
            yield return new WaitForSeconds(1.0f);

            teacher.AddPath(teacherPathm1);
            teacherOnDest = false;
            while (!teacherOnDest) { yield return null; }

            doors[0].SetDoor(true);
            teacher.AddPath(teacherPath0);
            teacherOnDest = false;

            while (!teacherOnDest)
            {
                yield return null;
            }
            teacherOnDest = false;

            yield return dm.PlayDialogue("Hello there! Thank you for waiting.", teacherDialoguePoint);
            yield return dm.PlayDialogue("I am Ms. Amy. I will be your Life Tutorial teacher.", teacherDialoguePoint);
            yield return dm.PlayDialogue("First I will teach you how to walk.", teacherDialoguePoint);

            teacher.AddPath(teacherPath1);
            teacherOnDest = false;
            
            yield return dm.PlayDialogue("Like this!", teacherDialoguePoint);

            while(!teacherOnDest) { yield return null; }

            yield return dm.PlayDialogue("Well done!", teacherDialoguePoint);

            dm.PlayDialogue("Come with me!", teacherDialoguePoint);

            yield return new WaitForSeconds(2.0f);

            teacher.AddPath(teacherPath2);
            teacherOnDest = false;

            while (!teacherOnDest) { yield return null; }

            yield return dm.PlayDialogue("WOWOWOWOW THIS WORKS!", teacherDialoguePoint);

        }

        public void TeacherReachedDest()
        {
            teacherOnDest = true;
        }
    }
}
