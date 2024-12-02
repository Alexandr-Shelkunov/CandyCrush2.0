using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alexender.Runer.UI
{
    public class MainMenu : MonoBehaviour
    {

        public void startGame()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }
    }
}