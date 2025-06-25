using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HowDoYouFeel.GeniusGame
{
    public class NPC_Pathwalker : MonoBehaviour
    {
        public float moveSpeed = 4.0f;
        public float rotateSpeed = 720.0f;
        Queue<Transform> walkQueue;
        public Animator npcAnimator;
        public UnityEvent onDestReached;

        public void StartPathWalk()
        {
            npcAnimator.SetBool("IsGrounded", true);
            StartCoroutine(PathWalkC());
        }

        public void AddPath(List<Transform> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                walkQueue.Enqueue(path[i]);
            }
        }

        public void AddPath(Transform[] path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                walkQueue.Enqueue(path[i]);
            }
        }


        IEnumerator PathWalkC()
        {
            walkQueue = new Queue<Transform>();
            while(true)
            {
                npcAnimator.SetBool("IsWalking", walkQueue.Count > 0);

                while (walkQueue.Count <= 0)
                {
                    yield return null;
                }

                npcAnimator.SetBool("IsWalking", true);

                while (walkQueue.Count > 0)
                {
                    Transform t = walkQueue.Dequeue();
                    yield return StartCoroutine(MoveToPointC(t));
                }

                onDestReached.Invoke();

                yield return null;
            }
        }

        IEnumerator MoveToPointC(Transform target)
        {
            //Rotate (over Y axis) to look at target
            Quaternion targetRot = Quaternion.LookRotation(
                Vector3.Scale(target.position - transform.position, new Vector3(1, 0, 1)), Vector3.up) ;

            while (Quaternion.Angle(transform.rotation, targetRot) > 5.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.rotation = targetRot;

            //Walk to target

            while(Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = target.position;

            //Rotate to match target rotation
            while (Quaternion.Angle(transform.rotation, target.rotation) > 5.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.rotation = target.rotation;
        }
    }
}
