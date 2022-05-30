using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    EnemyAudio AudioManager;
    public PlayerInfo Info;
    public List<CharacterInfo> Character;
    public Animator anims;
    public List<GameObject> SceneDecor;
    public CharacterName CharacterName;
    public CharacterName Price;
    public CharacterName Currency;
    public CharacterName AttackSkill;
    public CharacterName MoveSkill;
    public Material DarkSideMat;
    public Material LightSideMat;
    public int ActiveCharacter;
    public GameObject LockedOrUnlocked;
    public GameObject Cart;
    private Vector3 LockedOrUnlockedScale;
    private Vector3 CartScale;
    public int currencyHolder;
    GameObject CurrentlyActive;
    AttackSkils CurrentlyActiveAttackSkill;
    MovementSkills CurrentlyActiveMoveSkill;

    GameObject CharacterEnviromentEffect;
    public GameObject CharacterChangeEffect;
    public Material SkinColorOne;
    public Material SkinColorTwo;
    public Material SkinColorThree;
    int CurrentlyActiveSkin;
    int CurrentValue;
    float lerp = 0f, duration = 2f;

    private void Start()
    {
        Info.Load();
        AudioManager = FindObjectOfType<EnemyAudio>();
        LockedOrUnlockedScale = LockedOrUnlocked.transform.localScale;
        CartScale = Cart.transform.localScale;
        anims.GetComponent<Animator>();
        ActiveCharacter = 0;
        CallChange();
        SetInfo();
        CurrentlyActiveSkin = Info.CurrentlyActiveSkinColor;
        currencyHolder = 0;
        CurrentValue = Info.currency + (int)Info.CachedCoins;
        Debug.Log(CurrentValue);
        Debug.Log(currencyHolder + (int)Info.CachedCoins);
         Info.CachedCoins = 0;
        Info.Save();
    }

    public void NextCharacter()
    {
        Character[ActiveCharacter].gameObject.SetActive(false);
        ActiveCharacter++;
        CallChange();
        AudioManager.PlaySound("ButtonClick");

    }

    private void FixedUpdate()
    {
        Currency.SetName(currencyHolder.ToString());
        if (CurrentValue > currencyHolder)
        {

            lerp += Time.deltaTime / duration;
            currencyHolder = (int)Mathf.Lerp(currencyHolder, CurrentValue, lerp);
            
        }
        else if (CurrentValue < currencyHolder)
        {

            lerp -= Time.deltaTime / duration;
            currencyHolder = (int)Mathf.Lerp(currencyHolder, CurrentValue, lerp);
        }


        Info.currency = currencyHolder;
        if (CurrentlyActiveSkin ==1)
        {
            Info.Mat = SkinColorOne;
           ChangeCharacterColors(SkinColorOne);
        }
        if (CurrentlyActiveSkin ==2)
        {
            Info.Mat = SkinColorTwo;
            ChangeCharacterColors(SkinColorTwo);
        }
        if (CurrentlyActiveSkin ==3)
        {
            Info.Mat = SkinColorThree;
            ChangeCharacterColors(SkinColorThree);
        }

        if (Character[ActiveCharacter].Unlocked)
        {
            LockedOrUnlocked.transform.localScale = Vector3.Slerp(LockedOrUnlocked.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*5);
            Cart.transform.localScale = Vector3.Slerp(Cart.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*5);
        }
        else
        {
            LockedOrUnlocked.transform.localScale = Vector3.Slerp(LockedOrUnlocked.transform.localScale, LockedOrUnlockedScale, Time.deltaTime*5);
            Cart.transform.localScale = Vector3.Slerp(Cart.transform.localScale, CartScale, Time.deltaTime*5);
        }
    }

    public void ChangeToColorTo(int num)
    {
        CurrentlyActiveSkin = num;
        Info.CurrentlyActiveSkinColor = CurrentlyActiveSkin;
        AudioManager.PlaySound("ButtonClick");
    }
  

    void ChangeCharacterColors(Material Mat)
    {
        Character[ActiveCharacter].GetComponent<SkinnedMeshRenderer>().material = Mat;
    }

    public void PreviousCharacter()
    {
        Character[ActiveCharacter].gameObject.SetActive(false);
        ActiveCharacter--;
        CallChange();
        AudioManager.PlaySound("ButtonClick");
    }

    private void ChangeSKills()
    {
        if (AttackSkill!=null && Character[ActiveCharacter].AttackSkill!=null)
        {
            Info.AttackSkill = Character[ActiveCharacter].AttackSkill;
            AttackSkill.SetName(Character[ActiveCharacter].AttackSkill.SkillName);
        }
        
        if (MoveSkill!= null && Character[ActiveCharacter].MoveSkill != null)
        {
            Info.MoveSkill = Character[ActiveCharacter].MoveSkill;
            MoveSkill.SetName(Character[ActiveCharacter].MoveSkill.MoveName);
        }
        
        
    }

    private void CallChange()
    {
        if (ActiveCharacter >=Character.Count)
        {
            ActiveCharacter = 0;
        }
        else if (ActiveCharacter < 0)
        {
            ActiveCharacter = Character.Count - 1;
        }
        ChangeSKills();
        anims.SetInteger("PrefferedAnim", Random.Range(0,9));
        Changescene(Character[ActiveCharacter].DarkSide);
        CharacterName.SetName(Character[ActiveCharacter].Name);
        Price.SetName(Character[ActiveCharacter].Price.ToString());
        
        
        Character[ActiveCharacter].gameObject.SetActive(true);
        if (CharacterEnviromentEffect!=null)
        {
           
            CharacterEnviromentEffect = null;
         
        }

        if (CurrentlyActive!=null)
        {
            
            if (Character[ActiveCharacter].SpecialArea != CurrentlyActive)
            {
                CurrentlyActive.SetActive(false);
            }
        }

        if (Character[ActiveCharacter].SpecialArea!=null)
        {
            CurrentlyActive = Character[ActiveCharacter].SpecialArea;
            if (CurrentlyActive.activeSelf==false)
            {
                CurrentlyActive.SetActive(true);
            }
            
        }

      
        Destroy(Instantiate(CharacterChangeEffect,transform.position, CharacterChangeEffect.transform.rotation,transform).gameObject,5f);

      
        GetInfo();
        Info.Save();
    }
    public void Changescene(bool DarkSide)
    {
        if (DarkSide)
        {
            for (int i = 0; i < SceneDecor.Count; i++)
            {
                if (SceneDecor[i] != null)
                {
                    SceneDecor[i].GetComponent<MeshRenderer>().material = DarkSideMat;
                }
                
            }
        }
        else
        {
            for (int i = 0; i < SceneDecor.Count; i++)
            {
                if (SceneDecor[i] != null)
                {
                    SceneDecor[i].GetComponent<MeshRenderer>().material = LightSideMat;
                }
                
            }
        }
    }


    public void BuyCharacter()
    {
        AudioManager.PlaySound("ButtonClick");
        if (currencyHolder>= Character[ActiveCharacter].Price)
        {
            if (Character[ActiveCharacter].Unlocked == false)
            {
                CurrentValue -= Character[ActiveCharacter].Price;
                Character[ActiveCharacter].Unlocked = true;
                Info.ListOfCurrentlyUnlocked[ActiveCharacter] = true;
                AudioManager.PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
                AudioManager.PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
                AudioManager.PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
                AudioManager.PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
            }
            
        }
        CallChange();
    }

    void GetInfo()
    {
        SetActiveOrUnactive();
        Info.chosenCharacter = ActiveCharacter;
        Info.IsWorldCorrupted = Character[ActiveCharacter].DarkSide;
       // Info.currency = currencyHolder;
        Info.ListOfCurrentlyUnlocked = new bool[Character.Count];
        string LoadedOrUnloaded = "";
        for (int i = 0; i < Info.ListOfCurrentlyUnlocked.Length; i++)
        {
            Info.ListOfCurrentlyUnlocked[i] = Character[i].Unlocked;
            
        }
        
        Info.AttackSkill = Character[ActiveCharacter].AttackSkill;
        Info.MoveSkill = Character[ActiveCharacter].MoveSkill;
        
    }

    void SetInfo()
    {
        ActiveCharacter = Info.chosenCharacter;
        for (int i = 0; i < Character.Count; i++)
        {
            Character[i].Unlocked = Info.ListOfCurrentlyUnlocked[i];
        }

    }

    void SetActiveOrUnactive()
    {
        for (int i = 0; i < Character.Count; i++)
        {
            Character[i].Unlocked = Info.ListOfCurrentlyUnlocked[i];
        }
    }
}
