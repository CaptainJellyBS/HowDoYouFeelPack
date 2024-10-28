using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HowDoYouFeel.FocusGame
{
    [CreateAssetMenu(fileName = "Task", menuName = "ScriptableObjects/FocusGame/Task")]
    public class TaskTemplateSO : ScriptableObject
    {
        public bool isPriority = false, isChore = false, repeatsImmediately = false, repeatsDaily = false;
        public int dopamineReward, energyReward, healthReward, scoreReward;

        public TaskSegmentTemplateSO[] subRewards;

        public int progressNeeded;

        public List<TaskSegmentTemplateSO> GetSegmentList()
        {
            List<TaskSegmentTemplateSO> list = new List<TaskSegmentTemplateSO>();
            Array.Sort(subRewards);

            int i = 0;
            while(list.Count < progressNeeded)
            {
                if (i >= subRewards.Length) { list.Add(null); continue; }

                if(list.Count == subRewards[i].index)
                {
                    list.Add(subRewards[i]);
                    i++;
                    continue;
                }

                list.Add(null);
            }

            return list;
        }
    }
}
