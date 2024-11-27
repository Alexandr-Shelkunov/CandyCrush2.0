using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Alexender.Runer
{
    public class PlayerPhysicsHandler
    {
        private const int CANDY_WEIGHT = 3;
        private const float SCORE_DISTANCE_MOVEMENT_COEFF = 0.5F;

        private readonly PlayerModel playerModel;
        private readonly Transform playerT;

        public event Action CollidedWithObstacle;

        // Инициализация
        public PlayerPhysicsHandler(PlayerModel playerModel,
            Transform playerT)
        {
            this.playerModel = playerModel;
            this.playerT = playerT;
        }

        public void HandleFixedUpdate(Vector3 movementDirection)
        {
            playerModel.DistanceScore = playerT.position.z * SCORE_DISTANCE_MOVEMENT_COEFF; // Обновление счета
        }

        public void HandleControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("obstacle"))
            {
                CollidedWithObstacle?.Invoke();
                Time.timeScale = 0; // Остановка игры при столкновении
            }
        }

        public void HandleTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Candy"))
            {
                return;
            }

            playerModel.CandyCount++;
            Object.Destroy(other.gameObject);
            playerModel.Weight += CANDY_WEIGHT; // Увеличение веса при сборе конфет
        }
    }
}
