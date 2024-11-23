using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    // TODO: do consistent code style
    public class Player : MonoBehaviour, IUpdatable
    {
        // Зависимости
        [SerializeField] private LoopController loopController;
        [SerializeField] private Transform sceneRoot;
        private CharacterController controller;

        // Параметры движения
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        [SerializeField] private float gravity;
        [SerializeField] private float lineDistance;
        [SerializeField] private float maxSpeed;

        // TODO:The event 'Player.CollidedWithObstacle' is never used
        public event Action CollidedWithObstacle;
        public PlayerModel Model { get; set; }

        // Новый экземпляр обработчика физики
        private PlayerPhysicsHandler physicsHandler;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            Model = new PlayerModel();

            physicsHandler = new PlayerPhysicsHandler(controller, Model, transform);
            playerMovement = new PlayerMovement(controller, lineDistance, transform, jumpForce, fallForce, speed);
        }

        private void Start()
        {

            StartCoroutine(SpeedIncrease());
            // Регистрируем playerController в loopController
            if (loopController != null && this != null)
            {
                loopController.Register(this);
            }
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


        // Обновление объекта (DoUpdate) передаем в loopController
        public void DoUpdate()
        {
            playerMovement.DoUpdate();
        }

        // Обработка FixedUpdate
        private void FixedUpdate()
        {
            physicsHandler.HandleFixedUpdate(playerMovement.Velocity);
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