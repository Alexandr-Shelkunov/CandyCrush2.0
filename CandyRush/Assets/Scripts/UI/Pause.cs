using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
   public void pause()
    {
        Time.timeScale = 0f;
    }
}
