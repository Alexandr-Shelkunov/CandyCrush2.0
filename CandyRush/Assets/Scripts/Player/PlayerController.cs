using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{

    // TODO: do consistent code style
    public class PlayerController : MonoBehaviour, IUpdatable
    {
        // Зависимости
        private Rigidbody rb;
        private CharacterController controller;
        public LoopController loopController;
        public PlayerController playerController;

        [SerializeField] private Transform sceneRoot;

        // Параметры движения
        [SerializeField] private float lineDistance = 4;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        [SerializeField] private float gravity;
        [SerializeField] private float weight = 40;
        [SerializeField] private float maxSpeed = 110;

        // Собственные поля
        private Vector3 movementDirection;
        private int currentLine = 1;
        private float subtraction = 0f;

        public event Action CollidedWithObstacle;
        public PlayerModel Model { get; set; }

        // Новый экземпляр обработчика физики
        private PlayerPhysicsHandler physicsHandler;

        private void Awake()
        {
            Model = new PlayerModel();
        }

        void Start()
        {
            controller = GetComponent<CharacterController>();
            rb = GetComponent<Rigidbody>();

            // Инициализация физического обработчика
            physicsHandler = new PlayerPhysicsHandler();
            physicsHandler.Init(rb, controller, Model, gravity, jumpForce, fallForce, speed);

            StartCoroutine(SpeedIncrease());

            // Регистрируем playerController в loopController
            if (loopController != null && playerController != null)
            {
                loopController.Register(playerController);
            }
        }

        // Обновление объекта (DoUpdate) передаем в loopController
        public void DoUpdate()
        {
            subtraction += Time.deltaTime;

            if (subtraction >= 5f)
            {
                subtraction = 0f;
                weight *= 0.8f;
            }

            // Управление движением
            if (SwipeController.swipeRight)
            {
                if (currentLine < 2) currentLine++;
            }

            if (SwipeController.swipeLeft)
            {
                if (currentLine > 0) currentLine--;
            }

            if (SwipeController.swipeUp && controller.isGrounded)
            {
                Jump();
            }

            if (SwipeController.swipeDown && !controller.isGrounded)
            {
                Fall();
            }

            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (currentLine == 0)
            {
                targetPosition += Vector3.left * lineDistance;
            }
            else if (currentLine == 2)
            {
                targetPosition += Vector3.right * lineDistance;
            }

            if (transform.position != targetPosition)
            {
                Vector3 diff = targetPosition - transform.position;
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

        private void Jump()
        {
            movementDirection.y = jumpForce;
        }

        private void Fall()
        {
            movementDirection.y = -fallForce;
        }

        private IEnumerator SpeedIncrease()
        {
            yield return new WaitForSeconds(1);
            if (speed < maxSpeed)
            {
                speed += 1;
                StartCoroutine(SpeedIncrease());
            }
        }

        // Обработка FixedUpdate
        private void FixedUpdate()
        {
            physicsHandler.HandleFixedUpdate(movementDirection);
        }

        // Обработка OnControllerColliderHit
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            physicsHandler.HandleControllerColliderHit(hit);
        }

        // Обработка OnTriggerEnter
        private void OnTriggerEnter(Collider other)
        {
            physicsHandler.HandleTriggerEnter(other);
        }
    }
}