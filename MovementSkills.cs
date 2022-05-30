using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSkills", menuName = "PlayerSkills/MovementSkills")]
public class MovementSkills : ScriptableObject
{
    public GameObject SkillInstantiated;
    public bool CharacterMustDissapear;
    public float BoostAmount;

    private GameObject Instantiated;
    public bool CallDodgeAnimation;
    public void SkilledMove(Transform Player)
    {
        
        
        if (Instantiated==null && SkillInstantiated!=null)
        {
            Instantiated = Instantiate(SkillInstantiated, Player.transform.position,SkillInstantiated.transform.rotation, Player);
        }
    }

    public void CharacterDissapear(GameObject ActiveCharacter)
    {
        if (CharacterMustDissapear)
        {
            ActiveCharacter.SetActive(false);
        }
    }

    public void DestroyInstantiated(GameObject ActiveCharacter)
    {
        Destroy(Instantiated.gameObject);
        ActiveCharacter.SetActive(true);
    }
}
