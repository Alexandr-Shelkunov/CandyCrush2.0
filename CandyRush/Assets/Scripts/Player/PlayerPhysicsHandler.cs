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
            playerModel.DistanceScore = playerT.position.z * SCORE_DISTANCE_MOVEMENT_COEFF; 
        }

        private void CheckObstacleCollision()
        {
            float rayDistance = 1.5f; 
            Vector3 rayOrigin = playerT.position + Vector3.up * 0.5f; 

            Debug.DrawRay(rayOrigin, Vector3.forward * rayDistance, Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag("obstacle"))
                {
                    CollidedWithObstacle?.Invoke();
                    Time.timeScale = 0; 
                }
            }
        }

        private void CheckCandyPickup()
        {
            float pickupRadius = 1.0f; 
            Collider[] hitColliders = Physics.OverlapSphere(playerT.position, pickupRadius);

            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Candy"))
                {
                    playerModel.CandyCount++;
                    Object.Destroy(collider.gameObject);
                    playerModel.Weight += CANDY_WEIGHT;
                }
            }
        }
    }
}
