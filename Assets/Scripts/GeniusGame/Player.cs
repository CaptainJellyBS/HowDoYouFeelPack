using HowDoYouFeel.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HowDoYouFeel.GeniusGame
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(ControlSwitchPlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        public float rotateSpeed, walkSpeed, jumpSpeed; 
        bool isGrounded = false;

        Animator animator;
        Rigidbody rb;

        Vector2 moveInput;
        Quaternion targetRot;

        private void Awake()
        {
            if(Instance != null) { Destroy(Instance.gameObject); Debug.LogWarning("Had to destroy an old Player object. If nothing is broken, that's fine"); }
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            targetRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(moveInput.x * walkSpeed, rb.velocity.y, moveInput.y * walkSpeed);
            }

            if (moveInput.magnitude != 0 && isGrounded)
            {
                targetRot = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y));
            }
            else
            {
                if (Quaternion.Angle(transform.rotation, targetRot) > 30)
                {
                    for (float a = 0; a < 360.0f; a += 45.0f)
                    {
                        if (Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(a, Vector3.up)) < Quaternion.Angle(transform.rotation, targetRot))
                        {
                            targetRot = Quaternion.AngleAxis(a, Vector3.up);
                        }
                    }
                }
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        void OnWalk(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            animator.SetBool("IsWalking", moveInput.magnitude != 0);
        }

        void OnJump(InputValue value)
        {
            if (!isGrounded) { return; }
            isGrounded = false;
            animator.SetTrigger("Jump");
            rb.velocity += Vector3.up * jumpSpeed;            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.SetBool("IsGrounded", true);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = false;
                animator.SetBool("IsGrounded", false);
            }
        }
    }
}
