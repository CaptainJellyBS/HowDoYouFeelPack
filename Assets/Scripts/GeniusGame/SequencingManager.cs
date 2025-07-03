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

        [Header("Teacher NPC")]
        public NPC_Pathwalker teacher;
        public Transform teacherDialoguePoint;
        public GameObject teacherPathsParent;
        [TextArea] public List<string> teacherDialogue;
        public Animator teacherAnimator;

        public SimonSays simonSays;        

    private void Start()
        {
            StartCoroutine(TestSequenceC());
        }

        IEnumerator TestSequenceC()
        {
            Debug.LogWarning("DEBUG SEQUENCE ACTIVE");
            NPC_Path[] teacherPaths = teacherPathsParent.GetComponentsInChildren<NPC_Path>();
            
            teacher.StartPathWalk(); 
            
            yield return new WaitForSeconds(1.0f);

            yield return WalkPath(teacher, teacherPaths[0]);

            doors[0].SetDoor(true);
            yield return WalkPath(teacher, teacherPaths[1]);

            yield return dm.PlayDialogue(teacherDialogue[0], teacherDialoguePoint, teacherAnimator);
            yield return dm.PlayDialogue(teacherDialogue[1], teacherDialoguePoint, teacherAnimator);

            Coroutine tc = DelayedCoroutine(WalkPathC(teacher, teacherPaths[2]), 1.0f, 0.0f);
                        
            yield return dm.PlayDialogue(teacherDialogue[2], teacherDialoguePoint, teacherAnimator, 2.0f, tc);

            tc = DelayedCoroutine(WalkPathC(teacher, teacherPaths[3]), dm.GetDialogueTime(teacherDialogue[3]) + 0.5f, 0.0f);
            yield return dm.PlayDialogue(teacherDialogue[3], teacherDialoguePoint, teacherAnimator, 2.0f, tc);

            yield return dm.PlayDialogue(teacherDialogue[4], teacherDialoguePoint, teacherAnimator);

            tc = simonSays.StartPlay(dm.GetDialogueTime(teacherDialogue[5]));
            yield return dm.PlayDialogue(teacherDialogue[5], teacherDialoguePoint, teacherAnimator, 2.0f, tc);

            yield return dm.PlayDialogue(teacherDialogue[6], teacherDialoguePoint, teacherAnimator);
            doors[1].SetDoor(true);

            tc = DelayedCoroutine(WalkPathC(teacher, teacherPaths[4]), dm.GetDialogueTime(teacherDialogue[7]), 0.0f);

            yield return dm.PlayDialogue(teacherDialogue[7], teacherDialoguePoint, teacherAnimator, 2.0f, tc);

            tc = teacher.Jump(teacherPaths[5].path[0], teacherPaths[5].path[1], 4.5f, 0.5f);
            yield return dm.PlayDialogue(teacherDialogue[8], teacherDialoguePoint, teacherAnimator, 2.0f, tc);
        }

        //surely we can refactor this in a way to make it NPC generic?
        Coroutine WalkPath(NPC_Pathwalker npc, NPC_Path path)
        {
            return StartCoroutine(WalkPathC(npc, path));
        }

        IEnumerator WalkPathC(NPC_Pathwalker npc, NPC_Path path)
        {
            npc.AddPath(path);
            yield return null;

            while (!npc.HasReachedDest) { yield return null; }
        }

        Coroutine DelayedCoroutine(IEnumerator c, float startDelay, float endDelay)
        {
            return StartCoroutine(DelayedCoroutineC(c, startDelay, endDelay));
        }

        IEnumerator DelayedCoroutineC(IEnumerator c, float startDelay, float endDelay)
        {
            yield return new WaitForSeconds(startDelay);

            yield return StartCoroutine(c);

            yield return new WaitForSeconds(endDelay);
        }
    }
}
