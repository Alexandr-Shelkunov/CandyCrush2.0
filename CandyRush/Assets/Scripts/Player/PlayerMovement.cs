using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class PlayerMovement : MonoBehaviour, IUpdatable
    {
        private readonly CharacterController controller;
        private readonly float lineDistance;
        private readonly Transform playerT;
        private readonly float jumpForce;
        private readonly float fallForce;

        // Собственные поля
        private int currentLine;
        private float fallCoeff;
        private Vector3 velocity;
        private float speed;
        public LayerMask groundLayer;

        public Vector3 Velocity => velocity;

        public PlayerMovement(CharacterController controller,
            float lineDistance,
            Transform playerT,
            float jumpForce,
            float fallForce,
            float speed)
        {
            this.controller = controller;
            this.lineDistance = lineDistance;
            this.playerT = playerT;
            this.jumpForce = jumpForce;
            this.fallForce = fallForce;
            this.speed = speed;

            currentLine = 1;
            fallCoeff = 1.0F;
        }

        private bool IsGrounded()
        {
            Vector3 rayOrigin = playerT.position;
            float rayLength = 1.1f;
            Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
                return slopeAngle <= 45f; 
            }
            return false;
        }

        public void DoUpdate()
        {
            velocity.z = speed;
            Debug.Log($"Current Speed: {speed}");

            if (IsGrounded())
            {
                fallCoeff = 1.0F;

                if (SwipeController.swipeUp)
                {
                   
                    velocity.y = jumpForce;
                }
            }
            else
            {
               
                if (SwipeController.swipeDown)
                {
                    fallCoeff = fallForce; 
                }
            }

            velocity.y += fallCoeff * Time.deltaTime * Physics.gravity.y;
            velocity.y = Mathf.Clamp(velocity.y, -50f, 50f);

            if (SwipeController.swipeRight)
            {
                if (currentLine < 2) currentLine++;
            }
            else if (SwipeController.swipeLeft)
            {
                if (currentLine > 0) currentLine--;
            }

            controller.Move(velocity * Time.deltaTime);

            Vector3 targetPosition = playerT.position.z * playerT.forward + playerT.position.y * playerT.up;
            if (currentLine == 0)
            {
                targetPosition += Vector3.left * lineDistance;
            }
            else if (currentLine == 2)
            {
                targetPosition += Vector3.right * lineDistance;
            }

            if (playerT.position != targetPosition)
            {
                Vector3 diff = targetPosition - playerT.position;
                Vector3 moveDir = 50 * Time.deltaTime * diff.normalized;

                if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                {
                    controller.Move(moveDir);
                }
                else
                {
                    controller.Move(diff);
                }
            }
        }
    }
}
