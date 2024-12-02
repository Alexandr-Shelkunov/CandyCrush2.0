using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alexender.Runer.UI
{
    public class MuteAudio : MonoBehaviour
    {
        private bool isMuted = false;

        public void ToggleAudio()
        {
            isMuted = !isMuted; 
            AudioListener.volume = isMuted ? 0f : 1f; 

            Debug.Log("Audio is " + (isMuted ? "Muted" : "Unmuted"));
        }
    }
}
