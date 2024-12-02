using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class Player : MonoBehaviour, IUpdatable
    {
        // Зависимости
        [SerializeField] private LoopController loopController;
        [SerializeField] private Transform sceneRoot;

        // Параметры движения
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        [SerializeField] private float gravity;
        [SerializeField] private float lineDistance;
        [SerializeField] private float speed;

        public event Action CollidedWithObstacle;

        public PlayerModel Model { get; private set; }

        // Новый экземпляр класса движения
        private PlayerMovement playerMovement;

        private void Awake()
        {
            // Инициализация модели и движения
            Model = new PlayerModel();
            playerMovement = new PlayerMovement(lineDistance, transform, jumpForce, fallForce, speed);
        }

        private void Start()
        {
            // Регистрируем Player в LoopController
            if (loopController != null)
            {
                loopController.Register(this);
            }
        }

        // Обновление объекта (вызов в LoopController)
        public void DoUpdate()
        {
            // Выполняем движение и логику взаимодействия
            playerMovement.DoUpdate();
            CheckCandyPickup();
            CheckObstacleCollision();
        }

        // Проверка конфет
        private void CheckCandyPickup()
        {
            float pickupRadius = 1.0f; // Радиус проверки конфет
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRadius);

            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Candy"))
                {
                    Model.CandyCount++;
                    Destroy(collider.gameObject);
                    Model.Weight += 3; // Используем CANDY_WEIGHT из PlayerPhysicsHandler
                }
            }
        }

        // Проверка препятствий
        private void CheckObstacleCollision()
        {
            float rayDistance = 1.5f; // Дистанция проверки перед игроком
            Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // Смещение вверх

            Debug.DrawRay(rayOrigin, Vector3.forward * rayDistance, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag("obstacle"))
                {
                    CollidedWithObstacle?.Invoke();
                    Time.timeScale = 0; // Останавливаем игру
                }
            }
        }
    }
}