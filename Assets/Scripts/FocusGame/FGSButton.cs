using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{

    public class FGSButton : MonoBehaviour, IFGSelectable
    {
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void OnEnable()
        {
            StartCoroutine(IHateRaceConditionsWithAPassionC());
        }

        IEnumerator IHateRaceConditionsWithAPassionC()
        {
            for (int i = 0; TaskManager.Instance == null; i++)
            {
                yield return null;
                if (i > 5) { Debug.LogWarning("Something weird is happening, why is there no task manager?"); }
            }
            TaskManager.Instance.InstantiateTaskList();
            TaskManager.Instance.activeFGSelectables.Add(this);
        }

        private void OnDisable()
        {
            TaskManager.Instance.activeFGSelectables.Remove(this);
        }

        private void OnDestroy()
        {
            TaskManager.Instance.activeFGSelectables.Remove(this);
        }

        public Vector3 GetSelectDirection()
        {
            return (transform.position - TaskManager.Instance.brain.transform.position).normalized;
        }
    }
}
