using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public int CurrentHexLocationX = 1000;
    public int CurrentHexLocationY = 1000;
    public LayerMask mask;
    public bool GivesOffCorruption;
    LandScapeGen Gen;
    public CharacterName Counter;
    public void FixedUpdate()
    {
       
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,999, mask))
        {
            if (hit.transform.gameObject.GetComponent<LandScapeGen>())
            {
                Gen = hit.transform.gameObject.GetComponent<LandScapeGen>();
                CurrentHexLocationX = Gen.HexIDx;
                CurrentHexLocationY = Gen.HexIdY;
                if (GivesOffCorruption)
                {
                    if (Gen.Corrupted ==false)
                    {
                        Gen.PlayerWasOnMe = true;
                        Gen.GenerateDecors(Gen.DarkSideItems);
                        Gen.Corrupted = true;
                        Gen.ChangeStatus();
                    
                    }
                    
                }
                else
                {
                    if (Gen.Corrupted == true)
                    {
                        Gen.PlayerWasOnMe = true;
                        Gen.GenerateDecors(Gen.LightSideItems);
                        Gen.Corrupted = false;
                        Gen.ChangeStatus();
                     
                    }
                }
            }
        }
    }
}
