using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public Sounds[] sounds;

    public bool AdsIsOn;


    // Start is called before the first frame update
    void Awake()
    {
        AdsIsOn = false;
        foreach (Sounds sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.clip;

            sound.Source.volume = sound.volume;
            sound.Source.pitch = sound.pitch;
            sound.Source.loop = sound.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sounds s = Array.Find(sounds, sounds => sounds.name == name);
        if (s==null)
        {
            Debug.Log("Sound "+ name + " Was not found");
        }
        else
        {
            if (name== "ButtonClick" && AdsIsOn == false)
            {
                s.Source.volume = UnityEngine.Random.Range(1f, 2f);
                s.Source.pitch = UnityEngine.Random.Range(.75f, 1.2f);
                s.Source.Play();
            }
            else if (!s.Source.isPlaying && AdsIsOn ==false)
            {
                s.Source.volume = UnityEngine.Random.Range(1f, 2f);
                s.Source.pitch = UnityEngine.Random.Range(.75f, 1.2f);
                s.Source.Play();
            }
           
        }
        
    }
    public void PlaySoundSpecial(string name)
    {
        Sounds s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " Was not found");
        }
        else
        {
            if (AdsIsOn == false)
            {
                s.Source.volume = UnityEngine.Random.Range(1f, 2f);
                s.Source.pitch = UnityEngine.Random.Range(.75f, 1.2f);
                s.Source.Play();
            }
            

        }

    }
}
