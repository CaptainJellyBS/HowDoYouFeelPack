using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HowDoYouFeel.FocusGame
{
    public class Task : MonoBehaviour, IStatReceptable, IStatParticleSpawner, ISelectHandler, IDeselectHandler, IFGSelectable
    {
        public Transform statReceptablePoint, priorityPoint;
        public GameObject outline;
        public GameObject choreOutline;

        public bool isPriority = false, isChore = false, repeatsImmediately = false, repeatsDaily = false;
        public int MaxProgress { get; private set; }
        public int Progress { get; private set; }

        public int expectedProgress;

        public int DaysNotCompleted { get; private set; }

        public List<TaskSegment> taskSegments;

        public TaskSegment bottomSegment, headSegment;
        public Transform segmentParent;

        public int CurrentEnergy { get; private set; } = 0;
        public int CurrentDopamine { get; private set; } = 0;

        public TaskTemplateSO myTemplate { get; private set; }

        private void Start()
        {
            //Initialize(); //DEBUG
        }

        public void Initialize(TaskTemplateSO taskTemplate)
        {
            myTemplate = taskTemplate;

            isChore = taskTemplate.isChore;
            repeatsDaily = taskTemplate.repeatsDaily;
            repeatsImmediately = taskTemplate.repeatsImmediately;
            isPriority = taskTemplate.isPriority;

            expectedProgress = 0;
            List<TaskSegmentTemplateSO> segments = taskTemplate.GetSegmentList();

            taskSegments.Add(bottomSegment);

            float segmentSize = Mathf.Clamp(520.0f / (float)taskTemplate.progressNeeded, 3.0f, 24.0f);
            float segmentYPos = bottomSegment.GetComponent<RectTransform>().rect.height;

            for (int i = 1; i < segments.Count-1; i++)
            {
                TaskSegment t = GenerateSegment(segments[i], segmentSize);
                //t.GetComponent<RectTransform>().position = Vector3.up * segmentYPos;
                t.transform.localPosition = Vector3.up * segmentYPos;
                t.transform.localRotation = Quaternion.identity;
                segmentYPos += t.GetComponent<RectTransform>().rect.height;
                taskSegments.Add(t);
            }

            headSegment.transform.localPosition = Vector3.up * segmentYPos;
            headSegment.dopamineReward = taskTemplate.dopamineReward;
            headSegment.energyReward = taskTemplate.energyReward;
            headSegment.healthReward = taskTemplate.healthReward;
            headSegment.scoreReward = taskTemplate.scoreReward;
            taskSegments.Add(headSegment);

            if (TaskManager.Instance == null) { Debug.LogError("TASK MANAGER DOES NOT EXIST"); return; }
            TaskManager.Instance.InstantiateTaskList(); //Classic Unity race conditions tomfoolery
            TaskManager.Instance.activeTasks.Add(this);
            TaskManager.Instance.activeFGSelectables.Add(this);

            foreach (TaskSegment segment in taskSegments)
            {
                //if(segment == headSegment) { segment.Initialize(true); continue; }
                segment.Initialize();
            }

            MaxProgress = taskSegments.Count;
            if (isPriority) { TaskManager.Instance.MakeTaskPriority(this); }
        }

        public TaskSegment GenerateSegment(TaskSegmentTemplateSO segmentTemplate, float segmentSize)
        {
            TaskSegment result;
            if (segmentTemplate == null)
            {
                result = Instantiate(TaskManager.Instance.taskSegmentPrefab,segmentParent).GetComponent<TaskSegment>();
                //result.GetComponent<RectTransform>().rect.Set(0, 0, 64, segmentSize);
                result.GetComponent<RectTransform>().sizeDelta = new Vector2(64, segmentSize);
                result.dopamineReward = 0;
                result.energyReward = 0;
                result.healthReward = 0;
                result.scoreReward = 0;
                return result;
            }

            result = Instantiate(TaskManager.Instance.taskSegmentRewardPrefab,segmentParent).GetComponent<TaskSegment>();
            //result.GetComponent<RectTransform>().rect.Set(0, 0, 64, Mathf.Clamp(segmentSize * 2.0f, 52.0f, 64.0f));
            result.GetComponent<RectTransform>().sizeDelta = new Vector2(64, Mathf.Clamp(segmentSize * 2.0f, 52.0f, 64.0f));
            result.dopamineReward = segmentTemplate.dopamineReward;
            result.energyReward = segmentTemplate.energyReward;
            result.healthReward = segmentTemplate.healthReward;
            result.scoreReward = segmentTemplate.scoreReward;
            return result;
        }

        void UpdateProgress()
        {
            if(Progress >= MaxProgress) { return; }
            Progress = Mathf.Min(CurrentDopamine, CurrentEnergy);
            for(int i = 0; i < Mathf.Min(Progress, taskSegments.Count); i++)
            {
                taskSegments[i].Complete();
            }

            StartCoroutine(CheckCompleteC());
        }

        IEnumerator CheckCompleteC()
        {
            if(Progress < MaxProgress) { yield break; }

            TaskManager.Instance.tasksBeingRemoved.Add(this);

            float s = 
                (Mathf.Abs(headSegment.dopamineReward) * 0.2f) + 
                (Mathf.Abs(headSegment.energyReward) * 0.2f) + 
                (Mathf.Abs(headSegment.healthReward) * 0.2f) +
                1.2f;
            yield return new WaitForSeconds(s);

            TaskManager.Instance.tasksBeingRemoved.Remove(this);
            TaskManager.Instance.TaskCompleted(transform.rotation.eulerAngles.z, myTemplate, this);

            Destroy(gameObject);
        }

        public void ReceiveParticle(StatParticle particle)
        {
            switch(particle.statType)
            {
                case StatParticleType.DopamineUp: CurrentDopamine += particle.statValue; UpdateProgress(); break;
                case StatParticleType.EnergyUp: CurrentEnergy += particle.statValue; UpdateProgress(); break;
                default: Debug.LogWarning("Tasks should not receive particles other than DopamineUp or EnergyUp"); break;
            }
        }

        public Vector3 GetTargetPosition()
        {
            return statReceptablePoint.position;
        }

        public void OnSelect(BaseEventData eventData)
        {
            outline.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            outline.SetActive(false);
        }

        private void OnEnable()
        {
            //if(TaskManager.Instance == null) { return; }
            //TaskManager.Instance.InstantiateTaskList(); //Classic Unity race conditions tomfoolery
            //TaskManager.Instance.activeTasks.Add(this);
        }

        private void OnDisable()
        {
            TaskManager.Instance.activeTasks.Remove(this);
            TaskManager.Instance.activeFGSelectables.Remove(this);
            if(TaskManager.Instance.currentlySelected == this) { TaskManager.Instance.currentlySelected = null; }   
        }

        private void OnDestroy()
        {
            TaskManager.Instance.activeTasks.Remove(this);
            TaskManager.Instance.activeFGSelectables.Remove(this);
            if (TaskManager.Instance.currentlySelected == this) { TaskManager.Instance.currentlySelected = null; }
        }

        public void FlashOutline()
        {
            StartCoroutine(FlashOutlineC());
        }

        IEnumerator FlashOutlineC()
        {
            outline.transform.localScale = Vector3.one * 1.2f;
            yield return new WaitForSeconds(0.1f);
            outline.transform.localScale = Vector3.one;
        }

        public Coroutine ProgressDay()
        {
            return StartCoroutine(ProgressDayC());
        }

        IEnumerator ProgressDayC()
        {
            DaysNotCompleted++;

            if (!isChore) { yield break; }

            for (int i = 0; i < DaysNotCompleted && i < 3; i++)
            {
                (headSegment as IStatParticleSpawner).SpawnParticle(headSegment.particleSpawnpoint.position, 0.0f, TaskManager.Instance.brain, StatParticleType.HealthDown, -1, 0.0f);

                choreOutline.transform.localScale = Vector3.one * 1.2f;
                yield return new WaitForSeconds(0.1f);
                choreOutline.transform.localScale = Vector3.one;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Vector3 GetSelectDirection()
        {
            return transform.up;
        }
    }
}
