using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool paused;
    public void PauseOrUnpause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
        }
    }

  
}
