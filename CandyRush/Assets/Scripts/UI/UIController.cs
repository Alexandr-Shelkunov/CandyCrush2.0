using TMPro;
using UnityEngine;

namespace Alexender.Runer
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private PlayerControl player;

        [SerializeField] private TextMeshProUGUI candyScoreText;
        [SerializeField] private TextMeshProUGUI distanceScoreText;

        [SerializeField] private GameObject losePanel;

        private PlayerModel model;

        private void Awake()
        {
            model = player.Model;
            player.CollidedWithObstacle += PlayerCollidedWithObstacleHandler;
        }

        private void PlayerCollidedWithObstacleHandler()
        {
            losePanel.SetActive(true);
        }

        private void Update()
        {
            candyScoreText.text = "Candy score:\n" + model.CandyCount.ToString();
            distanceScoreText.text = "Distance score:\n" + model.DistanceScore.ToString("0");
        }
    }
}