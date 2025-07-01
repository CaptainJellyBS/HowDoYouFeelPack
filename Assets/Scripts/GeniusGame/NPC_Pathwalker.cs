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
        //public UnityEvent onDestReached;
        public bool HasReachedDest { get; private set; }

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

        public void AddPath(NPC_Path path)
        {
            AddPath(path.path);
        }


        IEnumerator PathWalkC()
        {
            walkQueue = new Queue<Transform>();
            while (true)
            {
                HasReachedDest = walkQueue.Count <= 0;
                npcAnimator.SetBool("IsWalking", !HasReachedDest);

                while (walkQueue.Count <= 0)
                {
                    yield return null;
                }

                HasReachedDest = false;
                npcAnimator.SetBool("IsWalking", true);

                while (walkQueue.Count > 0)
                {
                    Transform t = walkQueue.Dequeue();
                    yield return StartCoroutine(MoveToPointC(t));
                }               

                yield return null;
            }
        }

        IEnumerator MoveToPointC(Transform target)
        {
            //Rotate (over Y axis) to look at target
            Quaternion targetRot = Quaternion.LookRotation(
                Vector3.Scale(target.position - transform.position, new Vector3(1, 0, 1)), Vector3.up);

            while (Quaternion.Angle(transform.rotation, targetRot) > 5.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.rotation = targetRot;

            //Walk to target

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
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

        public Coroutine Jump(Transform startPoint, Transform endPoint, float height, float speed)
        {
            return Jump(startPoint.position, endPoint.position, height, speed);
        }

        public Coroutine Jump(Vector3 startPoint, Vector3 endPoint, float height, float speed)
        {
            return StartCoroutine(JumpC(startPoint, endPoint, height, speed));
        }

        IEnumerator JumpC(Vector3 startPoint, Vector3 endPoint, float height, float speed)
        {
            Vector3 control = startPoint + (endPoint - startPoint) / 2 + Vector3.up * height;

            npcAnimator.SetBool("IsGrounded", false);
            npcAnimator.SetTrigger("Jump");
            for (float t = 0; t <= 1.0f; t+=Time.deltaTime * speed)
            {
                yield return null;
                Vector3 m1 = Vector3.Lerp(startPoint, control, t);
                Vector3 m2 = Vector3.Lerp(control, endPoint, t);

                transform.position = Vector3.Lerp(m1, m2, t);
            }
            npcAnimator.SetBool("IsGrounded", true);
        }
    }
}
