using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBreathing : MonoBehaviour
{
    [Header("Options for Scales")]
    public bool ContiniousUpAnDownScale;
    public float ScaleLength;
    public float ScaleSpeed;
    public float ScaleMultiplier;
    [Header("Options for Positions")]
    public bool ContiniousMoveUpAndDown;
    public float UpOrDown;
    public Vector3 ModifiedVectors;
    [Header("Options for Rotations")]
    public bool ContiniousRotation;
    public float RotionValue;
    public float RotationLength;
    public Vector3 ModifiedRotationValues;



    private bool UpAnDownScale;
    private bool MoveUpAndDown;
    private bool IcanRotate;
    private float UpAndDownTimer ;
    private float ScaleUpAndDownTimer;
    private float RotationTimer;
    private Vector3 CachedPos;
    private Vector3 CachedScale;
    private Quaternion CachedRotation;
   

    private void Awake()
    {
        CachedPos = this.transform.position;
        CachedScale = this.transform.localScale;
    }

    private void FixedUpdate()
    {
        if (ContiniousUpAnDownScale || UpAnDownScale)
        {
            if (ScaleUpAndDownTimer > Time.deltaTime)
            {
           
                ScaleUpAndDownTimer -= Time.deltaTime;
                float Value = Mathf.PingPong(Time.time, ScaleLength) * ScaleLength;
                Vector3 NewScale = CachedScale + new Vector3(Value, Value, Value);
                this.transform.localScale = Vector3.Slerp(this.transform.localScale, NewScale, Time.deltaTime* ScaleSpeed);
            }
            else
            {
                if (Vector3.Distance(this.transform.localScale, CachedScale) > 0.1)
                {
                    this.transform.localScale = Vector3.Slerp(this.transform.localScale, CachedScale, Time.deltaTime * ScaleSpeed);
                }
                else
                {
                    ScaleUpAndDownTimer = ScaleLength;
                    UpAnDownScale = false;
                }
               
            }
        }
        if (ContiniousMoveUpAndDown || MoveUpAndDown)
        {
            if (UpAndDownTimer > Time.deltaTime)
            {
               
                UpAndDownTimer -= Time.deltaTime;
                float Value = Mathf.PingPong(Time.time, UpOrDown);
                Vector3 NewPos = CachedPos + new Vector3(Value* ModifiedVectors.x, Value * ModifiedVectors.y, Value* ModifiedVectors.z);
                this.transform.position = Vector3.Slerp(this.transform.position, NewPos, Time.deltaTime * UpOrDown); 
            }
            else
            {
                UpAndDownTimer = UpOrDown;
                MoveUpAndDown = false;

            }
        }
        if (ContiniousRotation || IcanRotate)
        {
            if (  RotationTimer > Time.deltaTime)
            {

                RotationTimer -= Time.deltaTime;
                float Value = Mathf.PingPong(Time.time, RotationLength);
                Quaternion NewPos = Quaternion.Euler(CachedRotation.eulerAngles + new Vector3(Value * ModifiedRotationValues.x, Value * ModifiedRotationValues.y, Value * ModifiedRotationValues.z));
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, NewPos, Time.deltaTime * RotationLength);
            }
            else
            {

                RotationTimer = RotationLength;
                IcanRotate = false;
          

            }
        }
    }

    public void ActivateScales()
    {
        if (ScaleUpAndDownTimer <= Time.deltaTime)
        {
            ScaleUpAndDownTimer = ScaleLength;
        }
        
        UpAnDownScale = true;
    }
    public void ActivateMoveUpAndDown()
    {
        MoveUpAndDown = true;
        if (UpAndDownTimer <= Time.deltaTime)
        {
            UpAndDownTimer = UpOrDown;
        }
    }
    public void ActivateRotations()
    {
        if (RotationTimer <= Time.deltaTime)
        {
            RotationTimer = RotationLength;
        }

        IcanRotate = true;
    }

}
