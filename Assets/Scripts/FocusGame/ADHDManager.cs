using HowDoYouFeel.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public class ADHDManager : MonoBehaviour
    {
        public bool inPanicMode = false;
        bool hasPanicked = false;
        public TaskTemplateSO funTaskTemplate;
        List<Friend> friendsList; //funniest variable name
        int daysPanicking;
        public GameObject friendPrefab;
        public Transform friendsParent;
        public Methylfenidate methylfenidate;
        bool spawnMF = false;

        // Update is called once per frame
        void Update()
        {
            SetFunPriority();
        }

        void SetFunPriority()
        {
            if (inPanicMode) { return; }

            if (GameManager.Instance.Dopamine <= 0)
            {
                if (TaskManager.Instance.activePriorities.Count > 0) { return; }

                Task funTask = TaskManager.Instance.activeTasks.Find(x => x.myTemplate == funTaskTemplate);
                if (funTask != null)
                {
                    TaskManager.Instance.MakeTaskPriority(funTask);
                }
            }
        }

        public void ProgressTasks()
        {
            if(friendsList == null) { friendsList = new List<Friend>(); }

            for (int i = friendsList.Count-1; i >= 0; i--)
            {
                friendsList[i].ProgressFriend();
            }

            if (inPanicMode)
            {
                if (TaskManager.Instance.activePriorities.Count < 1)
                {
                    TaskManager.Instance.MakeTaskPriority(TaskManager.Instance.activeTasks[0]);
                }

                TaskManager.Instance.activePriorities[0].SetTask(Utility.Pick(TaskManager.Instance.activeTasks));
                return;
            }

            methylfenidate.ProgressTime();
        }

        public void ProgressDay()
        {
            if(spawnMF)
            {
                spawnMF = false;
                methylfenidate.Initialize();
            }

            methylfenidate.ProgressDay();

            if (inPanicMode) { daysPanicking++; }            
        }

        public void ProgessDayAfterTaskSpawning()
        {
            if ((daysPanicking >= 3 || GameManager.Instance.Health <= 4) && inPanicMode)
            {
                inPanicMode = false;

                //Set the priority to the selfcare task
                if (TaskManager.Instance.selfcareActive)
                {
                    Task sct = TaskManager.Instance.activeTasks.Find(x => x.myTemplate == TaskManager.Instance.selfCareTaskTemplate);
                    if (sct != null)
                    {
                        if (TaskManager.Instance.activePriorities.Count >= 1)
                        {
                            TaskManager.Instance.activePriorities[0].SetTask(sct);
                        }
                        else
                        {
                            TaskManager.Instance.MakeTaskPriority(sct);
                        }
                    }
                }
                else
                {
                    List<Priority> completedPriorities = new List<Priority>();

                    foreach (Priority p in TaskManager.Instance.activePriorities)
                    {
                       completedPriorities.Add(p);
                    }

                    foreach (Priority p in completedPriorities)
                    {
                        TaskManager.Instance.activePriorities.Remove(p); Destroy(p.gameObject);
                    }
                }

                //Spawn a Friend for each chore
                foreach (Task t in TaskManager.Instance.activeTasks)
                {
                    if (t.isChore)
                    {
                        Friend f = Instantiate(friendPrefab, t.headSegment.transform.position + t.headSegment.transform.up * 190.0f, Quaternion.identity, friendsParent).GetComponent<Friend>();
                        f.Initialize(t, this);
                        friendsList.Add(f);
                    }
                }

                //Next day: spawn the medicine tasks
                spawnMF = true;
            }
        }

        public void StartPanicMode()
        {
            if (hasPanicked) { return; }
            inPanicMode = true;
            hasPanicked = true;
        }

        public void FriendDone(Friend f)
        {
            friendsList.Remove(f);
        }
    }
}
