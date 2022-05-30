using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPiece : MonoBehaviour
{
    public bool dead;
    public Vector3 AssignedPosition;
    public float UpdateTimer;
    public Material ChosenMat;
    public Vector3 AssignedSize;
    public float SizeTimer;

    public void Awake()
    {
        this.transform.rotation = Quaternion.Euler(0, Random.Range(0,360),0);
        this.transform.localScale = new Vector3(0, 0, 0);
        SizeTimer = 0;
    }

    public void FixedUpdate()
    {
        if (UpdateTimer>Time.deltaTime)
        {
            SizeTimer += Time.deltaTime;
            UpdateTimer -= Time.deltaTime;
            if (dead)
            {
                DieJustDie(AssignedPosition, AssignedSize);
            }
            else
            {
                grow(AssignedPosition, AssignedSize);
            }
        }
       
    }
    public void DieJustDie(Vector3 currentPosition, Vector3 Size)
    {
        Destroy(this.gameObject, 3f);
        if (SizeTimer < 0.5f)
        {
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, new Vector3(Size.x - Size.x * 0.5f, Size.y + Size.y * 2f, Size.z - Size.z * 0.5f), Time.deltaTime * 4);
        }
        else
        {
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*2);
        }
       
    }
    public void grow(Vector3 TargetLocation, Vector3 Size)
    {
        if (SizeTimer < 0.25f)
        {
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, new Vector3(Size.x+ Size.x*0.5f, Size.y + Size.y * 0.5f, Size.z + Size.z * 0.5f), Time.deltaTime*4);
        }
        else if(SizeTimer < 0.75f)
        {
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, new Vector3(Size.x - Size.x * 0.2f, Size.y + Size.y * 1, Size.z - Size.z * 0.2f), Time.deltaTime*3);
        }else 
        {
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, Size, Time.deltaTime*3);
        }
        
     
    }
}
