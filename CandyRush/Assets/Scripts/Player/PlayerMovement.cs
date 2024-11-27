using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class PlayerMovement : IUpdatable
    {
        private const float RAY_LENGTH_Y = 3.0F;

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
            Vector3 rayOrigin = playerT.position + Vector3.up * RAY_LENGTH_Y;
            // TODO: to const
            float raycastRadius = 0.5F;
            float rayLength = RAY_LENGTH_Y - raycastRadius;
            Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

            // TODO: get from main collider
            var hits = Physics.SphereCastAll(rayOrigin, raycastRadius, Vector3.down, rayLength);

            float minDelta = float.MaxValue;
            bool isAnySurface = false;

            hitPoint = default;
            foreach (var hit in hits)
            {
                var surface = hit.collider.GetComponent<Surface>();
                if (surface != null)
                {
                    isAnySurface = true;
                    float deltaY = (rayOrigin.y - hit.point.y) - rayLength;

                    if (deltaY < minDelta)
                    {
                        hitPoint = hit.point;
                        minDelta = deltaY;
                    }
                }
            }

            return isAnySurface;
        }

        public void DoUpdate()
        {
            speed += acceleration * Time.deltaTime;
            movementVelocity.z = speed;

            if (IsGrounded(out var hitPoint))
            {
                isSwipedDown = false;

                var playerPosition = playerT.position;
                playerPosition.y = hitPoint.y;
                playerT.position = playerPosition;

                movementVelocity.y = 0;

                if (SwipeController.swipeUp)
                {
                    movementVelocity.y = jumpForce;
                }
            }
            else
            {
                float velocityDeltaY = Time.deltaTime * Physics.gravity.y;
                movementVelocity.y += velocityDeltaY;

                if (SwipeController.swipeDown)
                {
                    isSwipedDown = true;
                }

                if (isSwipedDown)
                {
                    movementVelocity.y += fallForce * Physics.gravity.y * Time.deltaTime;
                }
            }

            if (SwipeController.swipeRight)
            {
                if (currentLine < 2)
                {
                    currentLine++;
                }
            }
            else if (SwipeController.swipeLeft)
            {
                if (currentLine > 0)
                {
                    currentLine--;
                }
            }

            //movementVelocity.x ;

            //Vector3 targetPosition = playerT.position.z * playerT.forward + playerT.position.y * playerT.up;
            //if (currentLine == 0)
            //{
            //    targetPosition += Vector3.left * lineDistance;
            //}
            //else if (currentLine == 2)
            //{
            //    targetPosition += Vector3.right * lineDistance;
            //}

            //if (playerT.position != targetPosition)
            //{
            //    Vector3 directionFromPlayerToTarget = (targetPosition - playerT.position).normalized;
            //    Vector3 moveDir = 50 * Time.deltaTime * directionFromPlayerToTarget;

            //    if (moveDir.sqrMagnitude < directionFromPlayerToTarget.sqrMagnitude)
            //    {
            //        controller.Move(moveDir);
            //    }
            //    else
            //    {
            //        controller.Move(directionFromPlayerToTarget);
            //    }
            //}

            playerT.position += movementVelocity * Time.deltaTime;
        }
    }
}
