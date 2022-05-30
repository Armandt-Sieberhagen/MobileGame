using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfPatrolLocations : MonoBehaviour
{
    public Transform[] PatrolLocations;
    public AoEPieces Pieces;

    public void Start()
    {
        if (GetComponent<characterAssigner>().Info!=null)
        {
            if (GetComponent<characterAssigner>().Info.AttackSkill!=null)
            {
                if (GetComponent<characterAssigner>().Info.AttackSkill.AoEObject != null)
                {
                    Pieces = GetComponent<characterAssigner>().Info.AttackSkill.AoEObject;
                    Pieces.StartAssign(this);
                }
            }
        }
       
       
    }
}
