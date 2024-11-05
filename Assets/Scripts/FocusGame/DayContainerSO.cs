using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    [CreateAssetMenu(fileName = "Task", menuName = "ScriptableObjects/FocusGame/DayContainer")]
    public class DayContainerSO : ScriptableObject
    {
        public List<TaskTemplateSO> dayTasks;
    }
}
