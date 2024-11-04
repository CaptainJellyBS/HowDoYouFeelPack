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
        public List<IFGSelectable> activeFGSelectables;
        public List<Priority> activePriorities;
        bool playerIsPerformingTask = false;
        public IFGSelectable currentlySelected = null;
        public Transform taskParent;

        public TaskTemplateSO selfCareTaskTemplate;


        public GameObject taskPrefab, chorePrefab, priorityPrefab;
        public GameObject taskSegmentPrefab, taskSegmentRewardPrefab;

        public TaskTemplateSO[] testTasks;

        public bool canInteract = true;
        bool selfcareActive = false;

        Queue<float> selectableAngles;
        Queue<TaskTemplateSO> repeatNextDay;

        private void Awake()
        {
            if (Instance != null) { Destroy(Instance); Debug.LogWarning("Had to destroy an old TaskManager"); }
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

            foreach (TaskTemplateSO tts in testTasks)
            {
                InstantiateTask(tts);
            }
        }

        void InstantiateTask(TaskTemplateSO tts)
        {
            Task t = Instantiate(tts.isChore? chorePrefab : taskPrefab, taskParent).GetComponent<Task>();
            t.transform.localPosition = Vector3.zero;
            t.transform.localRotation = Quaternion.AngleAxis(selectableAngles.Dequeue(), Vector3.forward);

            t.Initialize(tts);
        }

        public void InstantiateTaskList()
        {
            if (activeTasks == null)
            { activeTasks = new List<Task>(); }

            if(activeFGSelectables == null)
            { activeFGSelectables = new List<IFGSelectable>(); }

            if (repeatNextDay == null)
            { repeatNextDay = new Queue<TaskTemplateSO>(); }
            
            if(activePriorities == null)
            { activePriorities = new List<Priority>(); }

            if (selectableAngles == null)
            {
                selectableAngles = new Queue<float>();
                #region angles hardcode ugliness
                selectableAngles.Enqueue(-75.0f);
                selectableAngles.Enqueue(-45.0f);
                selectableAngles.Enqueue(-15.0f);
                selectableAngles.Enqueue(15.0f);
                selectableAngles.Enqueue(45.0f);
                selectableAngles.Enqueue(75.0f);

                selectableAngles.Enqueue(-60.0f);
                selectableAngles.Enqueue(-30.0f);
                selectableAngles.Enqueue(0.0f);
                selectableAngles.Enqueue(30.0f);
                selectableAngles.Enqueue(60.0f);
                #endregion
            }
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

            if (!GameManager.Instance.IsDaytime)
            {
                currentlySelected = null;
                EventSystem.current.SetSelectedGameObject(null);
                return;
            }

            if (pressing)
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
                currentlySelected = null;
                EventSystem.current.SetSelectedGameObject(null);
                return;
            }

            input = input.normalized;

            IFGSelectable curSelected = null;
            float curAngle = 15.0f; //Determines the maximum angle

            foreach (IFGSelectable t in activeFGSelectables)
            {
                //float a = Mathf.Abs(t.transform.localRotation.eulerAngles.z - Vector3.SignedAngle(input, Vector3.up, Vector3.forward));
                float a = Mathf.Abs(Vector3.SignedAngle(input, t.GetSelectDirection(), Vector3.forward));
                if (a < curAngle)
                {
                    curAngle = a;
                    curSelected = t;
                }
            }

            currentlySelected = curSelected;
            EventSystem.current.SetSelectedGameObject(curSelected != null ? curSelected.GetGameObject() : null);
        }

        IEnumerator PerformTaskC()
        {
            if (currentlySelected == null)
            {
                playerIsPerformingTask = false;
                yield break;
            }

            ProgressTask(currentlySelected);

            float t = 0.0f;
            while (t <= 1.0f && playerIsPerformingTask)
            {
                yield return null;
                t += Time.deltaTime;
            }

            if (currentlySelected == null)
            {
                playerIsPerformingTask = false;
                yield break;
            }
            if (!playerIsPerformingTask) { yield break; }
            ProgressTask(currentlySelected);

            for (int i = 0; i < 4 && playerIsPerformingTask; i++)
            {
                t = 0.0f;
                while (t <= 0.5f && playerIsPerformingTask)
                {
                    yield return null;
                    t += Time.deltaTime;
                }

                if (currentlySelected == null)
                {
                    playerIsPerformingTask = false;
                    yield break;
                }
                if (!playerIsPerformingTask) { yield break; }
                ProgressTask(currentlySelected);
            }

            while (currentlySelected != null && playerIsPerformingTask)
            {
                t = 0.0f;
                while (t <= 0.25f && playerIsPerformingTask)
                {
                    yield return null;
                    t += Time.deltaTime;
                }

                if (currentlySelected == null)
                {
                    playerIsPerformingTask = false;
                    yield break;
                }
                if (!playerIsPerformingTask) { yield break; }
                ProgressTask(currentlySelected);
            }
        }

        void ProgressTask(IFGSelectable s)
        {
            Task t = s as Task;
            if(t == null) { return; }
            if (t.expectedProgress >= t.MaxProgress) { return; }
            if (Mathf.Max(t.CurrentEnergy, t.CurrentDopamine) >= t.MaxProgress) { return; }
            t.FlashOutline();

            foreach(Priority p in activePriorities)
            {
                p.ProgressTask(t);
            }

            brain.ProgressTask(t);
        }

        public void MakeTaskPriority(Task t)
        {
            Priority p = Instantiate(priorityPrefab, taskParent).GetComponent<Priority>();
            p.SetTask(t);
            activePriorities.Add(p);
        }

        public void TaskCompleted(float angle, TaskTemplateSO taskTemplate, Task task)
        {
            List<Priority> completedPriorities = new List<Priority>();
            foreach(Priority p in activePriorities)
            {
                if(p.attachedTask == task) { completedPriorities.Add(p); }
            }

            foreach(Priority p in completedPriorities)
            {
                activePriorities.Remove(p); Destroy(p.gameObject);
            }

            if(taskTemplate == selfCareTaskTemplate) { selfcareActive = false; }
            selectableAngles.Enqueue(angle);
            if (taskTemplate.repeatsImmediately) { InstantiateTask(taskTemplate); return; }

            if (taskTemplate.repeatsDaily) { repeatNextDay.Enqueue(taskTemplate); }
        }

        public Coroutine ProgressDay()
        {
            return StartCoroutine(ProgressDayC());
        }

        IEnumerator ProgressDayC()
        {
            foreach (Task t in activeTasks)
            {
                yield return t.ProgressDay();
            }
            yield break;
        }

        public Coroutine SpawnNewDayTasks(int day)
        {
            return StartCoroutine(SpawnNewDayTasksC(day));
        }

        IEnumerator SpawnNewDayTasksC(int day)
        {
            Debug.LogWarning("New Day Task Spawning not implemented yet");

            if(GameManager.Instance.Health < GameManager.Instance.maxHealth/2 && !selfcareActive)
            {
                InstantiateTask(selfCareTaskTemplate);
                selfcareActive = true;
            }

            while(repeatNextDay.Count > 0)
            {
                InstantiateTask(repeatNextDay.Dequeue());
                yield return new WaitForSeconds(0.5f);
            }
            yield break;
        }
    }
}
