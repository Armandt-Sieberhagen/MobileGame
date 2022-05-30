using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public int chosenCharacter;
    public List<EnemyAi> DarkSideEnemy;
    public List<EnemyAi> LightSideEnemy;
    public List<EnemyAi> DarkSideBosses;
    public List<EnemyAi> LightSideBosses;
    public List<EnemyAi> LightCompanions;
    public List<EnemyAi> DarkCompanions;
    public bool IsWorldCorrupted;
    public int currency;
    public bool[] ListOfCurrentlyUnlocked;
    public int CurrentlyActiveSkinColor;
    public Material Mat;
    public AttackSkils AttackSkill;
    public MovingSkills MoveSkill;
    public float AttackMod;
    public int CachedCoins;
    public GameObject HitSpawn;

    public void IncreaseAttackMod(float Increase)
    {
        AttackMod += Increase;
    }
    public void resetAttackMod()
    {
        AttackMod = 0;
    }
    
    public void IncreaseCoins(int amount)
    {
        CachedCoins += amount;
    }

    public void Save()
    {
        string LoadedOrUnloaded = "";
        for (int i = 0; i < ListOfCurrentlyUnlocked.Length; i++)
        {
            if (ListOfCurrentlyUnlocked[i])
            {
                LoadedOrUnloaded = LoadedOrUnloaded + "1";
            }
            else
            {
                LoadedOrUnloaded = LoadedOrUnloaded + "0";
            }
        }
        PlayerPrefs.SetString("Unlocked", LoadedOrUnloaded);
        PlayerPrefs.SetInt("Currency", currency+CachedCoins);
        PlayerPrefs.Save();
       
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("Unlocked") && PlayerPrefs.HasKey("Currency"))
        {
            string UnlockedOrLocked = PlayerPrefs.GetString("Unlocked");
            char[] LockedOrUnlockedValues = UnlockedOrLocked.ToCharArray();
            for (int i = 0; i < ListOfCurrentlyUnlocked.Length; i++)
            {
                if (LockedOrUnlockedValues[i]=='1')
                {
                    ListOfCurrentlyUnlocked[i] = true;
                }
                else
                {
                    ListOfCurrentlyUnlocked[i] = false;
                }
              
            }
            currency = PlayerPrefs.GetInt("Currency");
            Debug.Log(PlayerPrefs.GetString("Unlocked"));
            Debug.Log(PlayerPrefs.GetInt("Currency"));
        }
        else
        {
            Save();
        }
      
    }

}
