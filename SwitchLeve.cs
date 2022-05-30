using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLeve : MonoBehaviour
{
    public Animator anims;
    public CharacterSelector character;
    EnemyAudio AudioManager;
    private int LevelToLoad = 0;

    public void Awake()
    {
        AudioManager = FindObjectOfType<EnemyAudio>();
    }
    public void FadeToLevel(int LevelIndex)
    {
        if (AudioManager!=null)
        {
            AudioManager.PlaySound("ButtonClick");
        }
        
        LevelToLoad = LevelIndex;
        if (anims!=null)
        {
            anims.SetTrigger("Fade");
        }
        
        
    }

    public void FadeToLevelIfUnlocked(int LevelIndex)
    {
        if (AudioManager!=null)
        {
            AudioManager.PlaySound("ButtonClick");
        }
        
        if (character.Character[character.ActiveCharacter].Unlocked)
        {
           
            LevelToLoad = LevelIndex;
            if (anims!=null)
            {
                anims.SetTrigger("Fade");
                
            }
           
        }
      
    }

    void OnFaceComplete()
    {
        if (AudioManager != null)
        {
            AudioManager.PlaySound("ButtonClick");
        }
       
        SceneManager.LoadScene(LevelToLoad);
    }
}
