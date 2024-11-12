using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alexender.Runer.UI
{
    public class Continue : MonoBehaviour
    {
        public void ContinueGame()
        {
            Time.timeScale = 1.0F;
        }
    }
}