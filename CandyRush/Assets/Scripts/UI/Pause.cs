using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer.UI
{
    public class Pause : MonoBehaviour
    {
        public void pause()
        {
            Time.timeScale = 0f;
        }
    }
}