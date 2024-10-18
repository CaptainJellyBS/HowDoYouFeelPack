using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HowDoYouFeel.FocusGame
{
    public class Task : MonoBehaviour, IStatReceptable, IStatParticleSpawner, ISelectHandler, IDeselectHandler
    {
        public Transform statReceptablePoint;
        public GameObject outline;

        public bool isPriority = false, isChore = false, repeatsImmediately = false, repeatsDaily = false;
        public int MaxProgress { get; private set; }
        public int Progress { get; private set; }

        public int DaysNotCompleted { get; private set; }

        public List<TaskSegment> taskSegments;

        int currentEnergy = 0, currentDopamine = 0;

        private void Start()
        {
            if (TaskManager.Instance == null) { return; }
            TaskManager.Instance.InstantiateTaskList(); //Classic Unity race conditions tomfoolery
            TaskManager.Instance.activeTasks.Add(this);
        }

        void UpdateProgress()
        {
            Progress = Mathf.Min(currentDopamine, currentEnergy);
            for(int i = 0; i < Mathf.Min(Progress, taskSegments.Count); i++)
            {
                taskSegments[i].Complete();
            }
        }

        public void ReceiveParticle(StatParticle particle)
        {
            switch(particle.statType)
            {
                case StatParticleType.DopamineUp: currentDopamine++; UpdateProgress(); break;
                case StatParticleType.EnergyUp: currentEnergy++; UpdateProgress(); break;
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
            if(TaskManager.Instance.currentTask == this) { TaskManager.Instance.currentTask = null; }   
        }

        private void OnDestroy()
        {
            TaskManager.Instance.activeTasks.Remove(this);
            if (TaskManager.Instance.currentTask == this) { TaskManager.Instance.currentTask = null; }
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
    }
}
