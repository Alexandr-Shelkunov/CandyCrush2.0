using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private bool isMuted = false;

    public void startGame()
    {
        AudioListener.volume = 1;
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Mute()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Переключение звука по нажатию кнопки M
            isMuted = !isMuted;
            if (isMuted)
            {
                AudioListener.volume = 0; // Выключить звук
            }
            else
            {
                AudioListener.volume = 1; // Включить звук
            }
        }
    }
}
