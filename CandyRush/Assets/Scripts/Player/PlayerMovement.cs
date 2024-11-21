using Unity.VisualScripting;
using UnityEngine;

namespace Alexender.Runer
{
    public class PlayerMovement : IUpdatable
    {
        private readonly CharacterController controller;
        private readonly float lineDistance;
        private readonly Transform playerT;
        private readonly float jumpForce;
        private readonly float fallForce;
        private readonly float speed;

        // Собственные поля
        private int currentLine;
        private float fallCoeff;
        private Vector3 velocity;

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

        public void DoUpdate()
        {
            velocity.z = speed;

            if (!controller.isGrounded)
            {
                if (SwipeController.swipeDown)
                {
                    fallCoeff = fallForce;
                }
            }
            else
            {
                fallCoeff = 1.0F;

                if (SwipeController.swipeUp)
                {
                    velocity += jumpForce * -Physics.gravity.normalized;
                }
            }

            velocity += fallCoeff * Time.deltaTime * Physics.gravity;

            // Управление движением
            if (SwipeController.swipeRight)
            {
                if (currentLine < 2) currentLine++;
            }
            else if (SwipeController.swipeLeft)
            {
                if (currentLine > 0) currentLine--;
            }

            controller.Move(velocity * Time.deltaTime);
            Debug.Log(controller.isGrounded);

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
                Vector3 moveDir = 25 * Time.deltaTime * diff.normalized;

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
