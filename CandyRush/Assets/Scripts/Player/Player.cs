using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class Player : MonoBehaviour, IUpdatable
    {
        // �����������
        [SerializeField] private LoopController loopController;
        [SerializeField] private Transform sceneRoot;

        // ��������� ��������
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        [SerializeField] private float gravity;
        [SerializeField] private float lineDistance;
        [SerializeField] private float speed;

        public event Action CollidedWithObstacle;

        public PlayerModel Model { get; set; }

        // ����� ��������� ����������� ������
        private PlayerPhysicsHandler physicsHandler;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            Model = new PlayerModel();

            physicsHandler = new PlayerPhysicsHandler(Model, transform);
            playerMovement = new PlayerMovement(lineDistance, transform, jumpForce, fallForce, speed);
        }

        private void Start()
        {
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