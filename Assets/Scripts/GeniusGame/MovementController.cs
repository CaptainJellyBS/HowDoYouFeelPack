using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.GeniusGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour
    {
        public float speedFwd;
        public float rotationSpeed;
        public float maxJumpHeight;
        public float maxFallSpeed;
        public float maxSlopeAngle;
        public float gravityScale;
        public Transform groundedOrigin;
        public Vector3 groundedBoxExtents;
        public LayerMask groundedMask;

        Vector3 groundNormal;

        Vector3 velocity;
        bool isGrounded;

        Rigidbody rb;
        Quaternion targetRotation;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (!rb.isKinematic) { Debug.LogError("Rigidbody should be kinematic"); }
            targetRotation = rb.rotation;
            groundNormal = Vector3.up;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            //Before any of this: calculate a new velocity based on inputs???
                //Which also applies gravity like this:
                //velocity += Vector3.down * Time.fixedDeltaTime * gravityScale;

            UpdateGrounded();
            UpdateRotation();
            UpdateVertical();
            UpdateWalk();
        }

        void UpdateRotation()
        {
            if (!isGrounded) { return; }
            Quaternion initRot = rb.rotation;
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            
            RaycastHit hit;
            if(rb.SweepTest(Vector3.zero, out hit, 0.0f))
            {
                rb.rotation = initRot;
            }
        }

        void UpdateVertical()
        {
            Vector3 vertMov = Vector3.Project(velocity, Vector3.up);

            RaycastHit hit;
            if(rb.SweepTest(vertMov.normalized, out hit, vertMov.magnitude))
            {
                rb.MovePosition(rb.position + vertMov.normalized * hit.distance);
                velocity = Vector3.Scale(velocity, new Vector3(1.0f,0,1.0f));
            }
            else
            {
                rb.MovePosition(rb.position + vertMov);
            }            
        }

        void UpdateGrounded()
        {
            //Check boxcast from origin
            //If any hits where the Angle(hit normal) < maxSlopeAngle -> Grounded
            //if there was a hit, but we're not grounded, move with the normal of the collider we hit (to bounce off steep slopes)
            RaycastHit hit;
            if(Physics.BoxCast(groundedOrigin.position, groundedBoxExtents , Vector3.down, out hit, rb.rotation, groundedBoxExtents.y, groundedMask))
            {
                if(Vector3.Angle(Vector3.down, hit.normal) < maxSlopeAngle)
                {
                    isGrounded = true;
                    groundNormal = hit.normal;
                }
                else
                {
                    isGrounded = false;
                    groundNormal = Vector3.up;
                    Vector3 movement = Vector3.ProjectOnPlane(hit.normal, Vector3.up) * Vector3.Project(velocity, Vector3.down).magnitude * Time.fixedDeltaTime;
                    
                    RaycastHit sweephit;
                    if(!rb.SweepTest(movement.normalized, out sweephit, movement.magnitude ))
                    {
                        rb.MovePosition(rb.position + movement);
                    }
                }
            }
            else
            {
                isGrounded = false;
                groundNormal = Vector3.up;
            }
        }

        void UpdateWalk()
        {
            Vector3 velOnGround = Vector3.ProjectOnPlane(velocity, groundNormal);
            
        }
    }
}
