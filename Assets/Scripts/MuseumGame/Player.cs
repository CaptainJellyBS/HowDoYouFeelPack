using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using UnityEngine.InputSystem;

namespace HowDoYouFeel.MuseumGame
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(ControlSwitchPlayerInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        [Header("Gameobject References")]
        public Transform cameraPivot;
        public AudioSource leftFootAS, rightFootAS;

        [Header("Stats")]
        public float forwardMoveSpeed;
        public float backwardMoveSpeed, strafeSpeed;
        public float lookSpeed;
        [Range(0.0f, 1.0f)]
        public float leftFootSpeedModifier, rightFootSpeedModifier;
        public float minAngleY = -60, maxAngleY = 60, minAngleX = -360, maxAngleX = 360;

        Vector2 lookInput, moveInput;
        int currentFoot = -1; //-1 = left foot, 1 = right foot
        bool canMove = true, canLook = true, canGiveUp = true;

        PlayerInput playerInput;
        Animator animator;
        Rigidbody rb;

        private void Awake()
        {
            if(Instance != null) { Destroy(Instance.gameObject); }
            Instance = this;
        }

        private void Start()
        {
            canMove = false;
            canLook = false;
            canGiveUp = false;
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;
            GlobalManager.Instance.CursorVisible = false;

            currentFoot = -1;
        }

        public void EnableInteractivity()
        {
            canMove = true;
            canLook = true;
            canGiveUp = true;
            GameManager.Instance.ShowGiveUpPanel(3.5f, 1.0f);
        }

        private void FixedUpdate()
        {
            HandleRotation();
            HandleMovement();

        }

        #region movement controls

        void HandleRotation()
        {
            if (!canLook) { return; }
            float rotationX = lookInput.x * lookSpeed;
            float rotationY = lookInput.y * lookSpeed;

            if(playerInput.currentControlScheme == "Gamepad")
            {
                rotationX *= Time.fixedDeltaTime;
                rotationY *= Time.fixedDeltaTime;
            }

            float x = cameraPivot.localRotation.eulerAngles.x;
            float y = transform.localRotation.eulerAngles.y;

            x -= rotationY;
            x = ClampAngle(x, minAngleY, maxAngleY);

            y += rotationX;
            y = ClampAngle(y, minAngleX, maxAngleX);

            transform.localRotation = Quaternion.Euler(transform.localRotation.x, y, transform.localRotation.z);
            cameraPivot.localRotation = Quaternion.Euler(x, cameraPivot.localRotation.y, cameraPivot.localRotation.z);
        }

        public void OnLook(InputValue value)
        {
            lookInput = value.Get<Vector2>();
        }

        float ClampAngle(float angle, float min, float max)
        {
            while (angle < -180F) { angle += 360F; }
            while (angle > 180F) { angle -= 360F; }
            return Mathf.Clamp(angle, min, max);
        }

        void HandleMovement()
        {
            if (!canMove) { rb.velocity = Vector3.Scale(rb.velocity, Vector3.up); return; }

            if(moveInput == Vector2.zero)
            {
                animator.SetBool("isWalking", false);
                rb.velocity = Vector3.Scale(rb.velocity, Vector3.up); //Set our forward and sideways movement to 0
                return;
            }

            animator.SetBool("isWalking", true);

            float currentForwardMoveMod = 0.0f;
            if(currentFoot == -1) { currentForwardMoveMod = leftFootSpeedModifier; }
            if(currentFoot == 1) { currentForwardMoveMod = rightFootSpeedModifier; }

            float z = moveInput.y * (moveInput.y > 0 ? currentForwardMoveMod * forwardMoveSpeed : backwardMoveSpeed);
            float x = moveInput.x * strafeSpeed;

            //rb.velocity = new Vector3(x, rb.velocity.y, z);
            Vector3 vel = transform.forward * z + transform.right * x;
            rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);

            //Ok lets write this out

            //If the player has stopped moving, we want to have them set their foot down (play sound), and transition to an idle animation
            //The player always steps left foot first after stopping
            //While the player is moving forward, we kind of want the animator to signal to us that a stride is completed, and switch feet

        }

        public void SetFoot(int foot)
        {
            currentFoot = foot;
            PlayCurrentFootSound();
        }

        public void PlayCurrentFootSound()
        {
            if (currentFoot == -1) { leftFootAS.Play(); }
            if (currentFoot == 1) { rightFootAS.Play(); }
        }

        public void PlayOtherFootSound()
        {
            if (currentFoot == 1) { leftFootAS.Play(); }
            if (currentFoot == -1) { rightFootAS.Play(); }
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        #endregion

        public void OnGiveUp(InputValue value)
        {
            if (!canGiveUp) { return; }
            Debug.Log("Player gave up");
            canMove = false; canLook = false; canGiveUp = false;
            animator.SetTrigger("giveUpTrigger");
            GameManager.Instance.giveUpPanel.SetActive(false);
        }

        #region animation triggered functions

        public void ActivateDeathPostProcessing()
        {
            GameManager.Instance.ppHandler.Die();
        }

        public void ActivateGameManagerFadeOut()
        {
            GameManager.Instance.GiveUpFadeOut();
        }

        #endregion
    }
}
