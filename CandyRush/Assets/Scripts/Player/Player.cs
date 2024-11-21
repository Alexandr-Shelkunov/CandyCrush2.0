using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    // TODO: do consistent code style
    public class Player : MonoBehaviour, IUpdatable
    {
        // �����������
        [SerializeField] private LoopController loopController;
        [SerializeField] private Transform sceneRoot;
        private CharacterController controller;

        // ��������� ��������
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        [SerializeField] private float gravity;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float lineDistance;

        public event Action CollidedWithObstacle;
        public PlayerModel Model { get; set; }

        // ����� ��������� ����������� ������
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

            // ������������ playerController � loopController
            if (loopController != null && this != null)
            {
                loopController.Register(this);
            }
        }

        // ���������� ������� (DoUpdate) �������� � loopController
        public void DoUpdate()
        {
            playerMovement.DoUpdate();
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

        // ��������� FixedUpdate
        private void FixedUpdate()
        {
            physicsHandler.HandleFixedUpdate(playerMovement.Velocity);
        }

        // ��������� OnControllerColliderHit
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            physicsHandler.HandleControllerColliderHit(hit);
        }

        // ��������� OnTriggerEnter
        private void OnTriggerEnter(Collider other)
        {
            physicsHandler.HandleTriggerEnter(other);
        }
    }
}