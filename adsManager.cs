using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class adsManager : MonoBehaviour, IUnityAdsListener
{
    // public PlayerInfo info;
    public BroadCastSwitch Switcher;
    public AudioSource MainSound;
    public EnemyAudio Sounds;
    int ChosenLevel;
    public bool AdsDisabled;
    void Start()
    {
        Advertisement.Initialize("4714714");
        Advertisement.AddListener(this);
    }

    public void PlayAd(int Level)
    {
        if (AdsDisabled==false)
        {
            ChosenLevel = Level;
            Sounds.PlaySound("ButtonClick");
            if (Advertisement.IsReady("Interstitial_iOS"))
            {
                Advertisement.Show("Interstitial_iOS");
                Switcher.CallSwitch(Level);
            }
        }
        else
        {
            Switcher.CallSwitch(Level);
        }
      
    }

    public void PlayRewardedAd(int Level)
    {
        ChosenLevel = Level;
        Sounds.PlaySound("ButtonClick");
        if (Advertisement.IsReady("Rewarded_iOS"))
        {
            MainSound.volume = 0;
            Sounds.AdsIsOn = true;
            Advertisement.Show("Rewarded_iOS");
        }
        else
        {
            Debug.Log("Rewarded ad is not rwady");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("ads ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("ads error");
        Switcher.CallSwitch(ChosenLevel);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("ads started");
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId== "Rewarded_iOS" && showResult == ShowResult.Finished)
        {
            int value = FindObjectOfType<characterAssigner>().Info.CachedCoins;
            FindObjectOfType<characterAssigner>().Info.CachedCoins = value + value;
            Debug.Log("I have been called" + value +"    "+ FindObjectOfType<characterAssigner>().Info.CachedCoins);
        }
        Switcher.CallSwitch(ChosenLevel);
    }
}
