using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HowDoYouFeel.FocusGame
{
    public class TaskManager : MonoBehaviour
    {
        public static TaskManager Instance { get; private set; }
        public Brain brain;
        public Transform playerControls;
        public List<Task> activeTasks;
        bool playerIsPerformingTask = false;
        public Task currentTask = null;
        public Transform taskParent;

        public GameObject taskPrefab;
        public GameObject taskSegmentPrefab, taskSegmentRewardPrefab;

        public TaskTemplateSO[] testTasks;

        private void Awake()
        {
            if(Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destroy an old TaskManager"); }
            Instance = this;

            InstantiateTaskList();
        }

        private void Start()
        {
            TestTaskInit();
        }

        void TestTaskInit()
        {
            Debug.LogWarning("Test task init active");
            float f = 80.0f;

            foreach(TaskTemplateSO tts in testTasks)
            {
                Task t = Instantiate(taskPrefab, taskParent).GetComponent<Task>();
                t.transform.localPosition = Vector3.zero;
                t.transform.localRotation = Quaternion.AngleAxis(f, Vector3.forward);

                t.Initialize(tts);

                f -= 20.0f;
            }
        }

        public void InstantiateTaskList()
        {
            if (activeTasks == null)
            { activeTasks = new List<Task>(); }
        }

        public void HandleInput(Vector3 input, bool pressing)
        {
            //Task Manager receives an Angle from the PlayerInput class
            //(either the normalized vector Brain -> Mouseposition, or the controller stick angle)
            //And whether the player is pressing the button

            //If the player is pressing, don't handle angle input, but tell the currently selected task (if any) to progress (with a delay that shortens)
            //Otherwise, every task has an angle vector. Find the task with the angle closest to the input vector (as well as a limit),
            //and then select that task

            //Select a task by keeping a CurrentTask internally (to send press events to), but also by updating the Eventsystem
            //(to send OnSelect and OnDeselect to all selectables (including tasks)



            if(pressing)
            {
                if (!playerIsPerformingTask)
                {
                    playerIsPerformingTask = true;
                    StartCoroutine(PerformTaskC());
                }
                return;
            }
            playerIsPerformingTask = false;

            if (input == Vector3.zero)
            {
                currentTask = null;
                EventSystem.current.SetSelectedGameObject(null);
                return;
            }

            input = input.normalized;

            Task curTask = null;
            float curAngle = 15.0f; //Determines the maximum angle

            foreach(Task t in activeTasks)
            {
                //float a = Mathf.Abs(t.transform.localRotation.eulerAngles.z - Vector3.SignedAngle(input, Vector3.up, Vector3.forward));
                float a = Mathf.Abs(Vector3.SignedAngle(input, t.transform.up, Vector3.forward));
                if (a < curAngle)
                {
                    curAngle = a;
                    curTask = t;
                }
            }

            currentTask = curTask;
            EventSystem.current.SetSelectedGameObject(curTask ? curTask.gameObject : null);
        }

        IEnumerator PerformTaskC()
        {
            if(currentTask == null)
            {
                playerIsPerformingTask = false;
                yield break;
            }

            ProgressTask(currentTask);

            float t = 0.0f;
            while(t <= 1.0f && playerIsPerformingTask)
            {
                yield return null;
                t += Time.deltaTime;
            }

            if (currentTask == null)
            {
                playerIsPerformingTask = false;
                yield break;
            }
            if (!playerIsPerformingTask) { yield break; }
            ProgressTask(currentTask);

            for (int i = 0; i < 4 && playerIsPerformingTask; i++)
            {
                t = 0.0f;
                while (t <= 0.5f && playerIsPerformingTask)
                {
                    yield return null;
                    t += Time.deltaTime;
                }

                if (currentTask == null)
                {
                    playerIsPerformingTask = false;
                    yield break;
                }
                if (!playerIsPerformingTask) { yield break; }
                ProgressTask(currentTask);
            }

            while(currentTask != null && playerIsPerformingTask)
            {
                t = 0.0f;
                while (t <= 0.25f && playerIsPerformingTask)
                {
                    yield return null;
                    t += Time.deltaTime;
                }

                if (currentTask == null)
                {
                    playerIsPerformingTask = false;
                    yield break;
                }
                if (!playerIsPerformingTask) { yield break; }
                ProgressTask(currentTask);
            }
        }

        void ProgressTask(Task t)
        {
            if(Mathf.Max(t.CurrentEnergy, t.CurrentDopamine) >= t.MaxProgress) { return; }
            t.FlashOutline();
            brain.ProgressTask(t);
        }
    }
}
