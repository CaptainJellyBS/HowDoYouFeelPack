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

        public SimonSays simonSays;

        

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
            yield return StartCoroutine(WaitForTeacherToReachDestC());

            doors[0].SetDoor(true);
            teacher.AddPath(teacherPath0);
            teacherOnDest = false;

            yield return StartCoroutine(WaitForTeacherToReachDestC());
            teacherOnDest = false;

            yield return dm.PlayDialogue("Hello there! Thank you for waiting.", teacherDialoguePoint);
            yield return dm.PlayDialogue("I am Ms. Amy. I will be your Life Tutorial teacher.", teacherDialoguePoint);
            yield return dm.PlayDialogue("First I will teach you how to walk.", teacherDialoguePoint);

            teacher.AddPath(teacherPath1);
            teacherOnDest = false;
            
            yield return dm.PlayDialogue("Like this!", teacherDialoguePoint);

            yield return StartCoroutine(WaitForTeacherToReachDestC());

            yield return dm.PlayDialogue("Well done!", teacherDialoguePoint);

            teacher.AddPath(teacherPath2);
            teacherOnDest = false;
            Coroutine tc = StartCoroutine(WaitForTeacherToReachDestC());
            yield return dm.PlayDialogue("Alright. Come with me!", teacherDialoguePoint, 2.0f, tc);

            yield return dm.PlayDialogue("This is a puzzle. Go do the puzzle!", teacherDialoguePoint);

            yield return simonSays.StartPlay();

            yield return dm.PlayDialogue("Well done omg!!!!!!!!", teacherDialoguePoint);
            doors[1].SetDoor(true);

        }

        IEnumerator WaitForTeacherToReachDestC()
        {
            while (!teacherOnDest) { yield return null; }
        }

        public void TeacherReachedDest()
        {
            teacherOnDest = true;
        }
    }
}
