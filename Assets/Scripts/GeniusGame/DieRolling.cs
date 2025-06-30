using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    [RequireComponent(typeof(Renderer))]
    public class DieRolling : MonoBehaviour
    {
        Rigidbody rb;
        public float rollTime = 1.0f;
        public AnimationCurve rollCurve;
        bool canRoll = true;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Roll()
        {
            StartCoroutine(RollC());
        }

        IEnumerator RollC()
        {
            if (!canRoll) { yield break; }

            canRoll = false;
            float offset = 1.75f * transform.localScale.x; //scale should never be non-uniform anyway

            Vector3 dir = transform.position - Player.Instance.transform.position;
            Vector3 dirOnAxis = new Vector3(dir.x / Mathf.Abs(dir.x), 0, dir.z / Mathf.Abs(dir.z)).normalized; //Should be a vector on the closest angle??? Hopefully?????

            RaycastHit hit;
            if (rb.SweepTest(dirOnAxis, out hit, offset, QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.collider.name);
                canRoll = true;
                yield break;
            }

            Vector3 oldP = transform.position;
            Quaternion oldR = transform.rotation;

            transform.RotateAround(transform.position + dirOnAxis * (offset / 2.0f) + Vector3.down * (offset / 2.0f), Vector3.Cross(dirOnAxis, Vector3.up), -90);

            Vector3 newP = transform.position;
            Quaternion newR = transform.rotation;

            transform.position = oldP;
            transform.rotation = oldR;

            for (float t = 0; t < 1.0f; t+=Time.deltaTime / rollTime)
            {
                float ct = rollCurve.Evaluate(t);
                transform.position = Vector3.Slerp(oldP, newP, ct);
                transform.rotation = Quaternion.Slerp(oldR, newR, ct);
                yield return null;
            }

            transform.position = newP;
            transform.rotation = newR;

            canRoll = true;
        }
    }
}
