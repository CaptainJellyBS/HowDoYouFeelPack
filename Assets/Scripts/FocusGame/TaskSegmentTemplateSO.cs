using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    [CreateAssetMenu(fileName = "TaskSegment", menuName = "ScriptableObjects/FocusGame/TaskSegment")]
    public class TaskSegmentTemplateSO : ScriptableObject, IComparable
    {
        public int dopamineReward, energyReward, healthReward, scoreReward;
        public int index;

        public int CompareTo(object obj)
        {
            return index.CompareTo(((TaskSegmentTemplateSO)obj).index);
        }
    }
}
