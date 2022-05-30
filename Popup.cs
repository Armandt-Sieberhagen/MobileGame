using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject Object;
    EnemyAudio AudioManager;

    public void Awake()
    {
        AudioManager = FindObjectOfType<EnemyAudio>();
        
    }

    public void ActivateOrUnactive()
    {
        if (Object.activeSelf==false)
        {
            Object.SetActive(true);
        }
        else
        {
            Object.SetActive(false);
        }
        AudioManager.PlaySound("ButtonClick");
    }

}
