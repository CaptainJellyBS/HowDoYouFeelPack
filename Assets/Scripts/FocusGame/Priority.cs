using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HowDoYouFeel.FocusGame
{
    public class Priority : MonoBehaviour, IStatParticleSpawner
    {
        public Task attachedTask { get; private set; }

        public void SetTask(Task t)
        {
            attachedTask = t;
            transform.position = t.priorityPoint.position;
        }

        public void ProgressTask(Task t)
        {
            StartCoroutine(FlashC());
            (this as IStatParticleSpawner).SpawnParticle(transform.position, 0.0f, TaskManager.Instance.brain,
                t == attachedTask ? StatParticleType.DopamineUp : StatParticleType.DopamineDown,
                t == attachedTask ? 1 : -1,
                0.0f
                );

        // Debug.Log("TODO: RUMBLE CONTROLLER");
        }

        IEnumerator FlashC()
        {
            transform.localScale = Vector3.one * 1.5f;
            Gamepad.current.SetMotorSpeeds(0.5f, 0.25f);
            yield return new WaitForSeconds(0.15f);
            transform.localScale = Vector3.one;
            Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);

        }
    }
}
