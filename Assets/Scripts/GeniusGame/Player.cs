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
        public Transform groundedRaycastOrigin;
        public LayerMask groundedRaycastLayerMask;
        public float groundedRaycastDistance = 1.125f;
        public float stepDownDistance = 0.55f;
        public Vector3 groundedBoxExtents;
        public float maxSlopeAngle = 45.0f;

        bool isGrounded = false;
        bool canDoubleJump;
        Vector3 groundNormal;

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
            groundNormal = Vector3.up;
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            targetRot = transform.rotation;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            CheckGrounded();
            HandleInput();
        }

        void HandleInput()
        {
            if (isGrounded)
            {
                //rb.velocity = new Vector3(moveInput.x * walkSpeed, rb.velocity.y, moveInput.y * walkSpeed);
                rb.velocity = Vector3.ProjectOnPlane(new Vector3(moveInput.x * walkSpeed, 0, moveInput.y * walkSpeed), groundNormal);
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

        void CheckGrounded()
        {
            RaycastHit hit;
            float ed = isGrounded ? stepDownDistance : 0.0f;
            if(Physics.BoxCast(groundedRaycastOrigin.position, groundedBoxExtents, Vector3.down, out hit, 
                transform.rotation, groundedRaycastDistance + ed, groundedRaycastLayerMask))
            {
                SetGrounded(Vector3.Angle(Vector3.up, hit.normal) <= maxSlopeAngle, hit.normal);

                if (isGrounded)
                {
                    //eww why
                    transform.Translate(Vector3.up * Mathf.Max(stepDownDistance * -1,
                       hit.point.y - (groundedRaycastOrigin.position.y - groundedRaycastDistance - groundedBoxExtents.y + 0.025f) ));
                }
            }
            else
            {
                SetGrounded(false, Vector3.up);
            }
        }

        void SetGrounded(bool g, Vector3 normal)
        {
            isGrounded = g;
            animator.SetBool("IsGrounded", isGrounded);
            rb.useGravity = !isGrounded;
            if (isGrounded) { rb.velocity = Vector3.Scale(rb.velocity, new Vector3(1.0f, 0.0f, 1.0f)); }
            groundNormal = isGrounded ? normal : Vector3.up; //lol this is so ugly
            canDoubleJump = canDoubleJump || isGrounded;
        }

        void OnWalk(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            animator.SetBool("IsWalking", moveInput.magnitude != 0);
        }

        void OnJump(InputValue value)
        {
            if (!isGrounded) { return; }
            SetGrounded(false, Vector3.up);
            animator.SetTrigger("Jump");
            rb.velocity += Vector3.up * jumpSpeed;
            transform.Translate(Vector3.up * jumpSpeed * Time.fixedDeltaTime * 2.0f);
        }
    }
}
