using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAssigner : MonoBehaviour
{
    public List<GameObject> Character;
    int ActiveCharacter;
    public PlayerInfo Info;
    MovingSkills MoveSkills;
    public GameObject Root;
    float Timer;
    MiniCharacterMove Player;

    public void Start()
    {
        Player = this.GetComponent<MiniCharacterMove>();
        ActiveCharacter = Info.chosenCharacter;

        Character[ActiveCharacter].SetActive(true);
        if (Info.Mat!=null)
        {
            Character[ActiveCharacter].GetComponent<SkinnedMeshRenderer>().material = Info.Mat;
        }
       
        if (Info.AttackSkill!=null)
        {
            Player.AttackSkills = Info.AttackSkill;
        }
        if (Info.MoveSkill!=null)
        {
            Player.MoveSkills = Info.MoveSkill;
        }
        this.GetComponent<PlayerLocation>().GivesOffCorruption = Info.IsWorldCorrupted;

        if (Character[ActiveCharacter].GetComponent<CharacterInfo>() && Character[ActiveCharacter].GetComponent<CharacterInfo>().SpecialArea!=null)
        {
            Character[ActiveCharacter].GetComponent<CharacterInfo>().SpecialArea.SetActive(true);
            Character[ActiveCharacter].GetComponent<CharacterInfo>().SpecialArea.GetComponent<MeshRenderer>().material = Info.Mat;
        }
        if (Character[ActiveCharacter].GetComponent<CharacterInfo>() && Character[ActiveCharacter].GetComponent<CharacterInfo>().SecondSpecialArea!=null)
        {
            Character[ActiveCharacter].GetComponent<CharacterInfo>().SecondSpecialArea.SetActive(true);
            Character[ActiveCharacter].GetComponent<CharacterInfo>().SecondSpecialArea.GetComponent<MeshRenderer>().material = Info.Mat;
        }
        if (Character[ActiveCharacter].GetComponent<CharacterInfo>() && Character[ActiveCharacter].GetComponent<CharacterInfo>().SpecialEffect != null)
        {
            Character[ActiveCharacter].GetComponent<CharacterInfo>().SpecialEffect.SetActive(true);
           
        }
        if (Character[ActiveCharacter].GetComponent<CharacterInfo>() && Character[ActiveCharacter].GetComponent<CharacterInfo>().HitSpawn != null)
        {
            Info.HitSpawn = Character[ActiveCharacter].GetComponent<CharacterInfo>().HitSpawn;
            Player.HitSpawn = Info.HitSpawn;
        }
        if (Character[ActiveCharacter].GetComponent<CharacterInfo>() && Character[ActiveCharacter].GetComponent<CharacterInfo>().randomHitRotation != null)
        {

            Player.randoRot = Character[ActiveCharacter].GetComponent<CharacterInfo>().randomHitRotation;
        }
        Info.AttackMod = 0;
        
        MoveSkills = Player.MoveSkills;
        Timer = 0;
    }

    public void FixedUpdate()
    {
        Player.AttackMod = Info.AttackMod;
        if (MoveSkills!=null)
        {
            if (Timer > Time.deltaTime)
            {
                MoveSkills.CharacterDissapear(Character[ActiveCharacter]);
                MoveSkills.CharacterDissapear(Root);
                Timer -= Time.deltaTime;
            }
            else
            {


                MoveSkills.DestroyInstantiated(Character[ActiveCharacter]);
                MoveSkills.DestroyInstantiated(Root);
            }
        }
    
    }

    public void CallSkills(int value)
    {
        Timer = value;
    }
}
