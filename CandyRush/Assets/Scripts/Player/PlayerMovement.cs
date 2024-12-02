using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class PlayerMovement : IUpdatable
    {
        private const float RAY_LENGTH_Y = 3.0F;
        private const float RAYCAST_RADIUS = 0.5F;
        private const float MAX_SPEED = 20.0F;

        private readonly float lineDistance;
        private readonly Transform playerT;
        private readonly float jumpForce;
        private readonly float fallForce;
        private readonly float acceleration;

        // Собственные поля
        private int currentLine;
        private Vector3 movementVelocity;
        private float speed;
        private bool isSwipedDown;

        public LayerMask groundLayer;

        public Vector3 Velocity => movementVelocity;

        public PlayerMovement(float lineDistance,
            Transform playerT,
            float jumpForce,
            float fallForce,
            float intialSpeed)
        {
            this.lineDistance = lineDistance;
            this.playerT = playerT;
            this.jumpForce = jumpForce;
            this.fallForce = fallForce;

            speed = intialSpeed;
            acceleration = 1.0F;
            currentLine = 1;
        }

        private bool IsGrounded(out Vector3 hitPoint)
        {
            groundLayer = LayerMask.GetMask("Ground");
            Vector3 rayOrigin = playerT.position + Vector3.up * RAY_LENGTH_Y;
            float rayLength = RAY_LENGTH_Y - RAYCAST_RADIUS;

            Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

            if (Physics.SphereCast(rayOrigin, RAYCAST_RADIUS, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
            {
                hitPoint = hit.point;
                return true;
            }

            hitPoint = default;
            return false;
        }

        public void DoUpdate()
        {
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Min(speed, MAX_SPEED);
            movementVelocity.z = speed;

            if (IsGrounded(out var hitPoint))
            {
                isSwipedDown = false;

                playerT.position = new Vector3(playerT.position.x, hitPoint.y, playerT.position.z);
                movementVelocity.y = 0;

                if (SwipeController.swipeUp)
                {
                    movementVelocity.y = jumpForce;
                }
            }
            else
            {
                movementVelocity.y += Time.deltaTime * Physics.gravity.y;

                if (SwipeController.swipeDown)
                {
                    isSwipedDown = true;
                }

                if (isSwipedDown)
                {
                    movementVelocity.y += fallForce * Physics.gravity.y * Time.deltaTime * 5f;
                }
            }


            HandleLaneChange();

            playerT.position += movementVelocity * Time.deltaTime;
        }

        private void HandleLaneChange()
        {
            int targetLine = currentLine;

            if (SwipeController.swipeRight && currentLine < 2)
            {
                targetLine++;
            }
            else if (SwipeController.swipeLeft && currentLine > 0)
            {
                targetLine--;
            }

            if (targetLine != currentLine)
            {
                currentLine = targetLine;
                Vector3 targetPosition = GetLinePosition(targetLine);
                MoveToLine(targetPosition);
            }
        }

        private Vector3 GetLinePosition(int lineIndex)
        {
            float offsetX = (lineIndex - 1) * lineDistance;
            return new Vector3(offsetX, playerT.position.y, playerT.position.z);
        }

        private void MoveToLine(Vector3 targetPosition)
        {
            float moveSpeed = 10.0f;
            Vector3 newPosition = Vector3.Lerp(playerT.position, targetPosition, moveSpeed * Time.deltaTime);
            playerT.position = newPosition;
        }
    }
}
