using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoJump : MonoBehaviour
{
    public float Radius;
    public LayerMask Ground;
   public  float Forward;
   public  float upAmount;
    Rigidbody Body;
    private void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * Forward + transform.up*upAmount, Radius, Ground);
        if (colliders.Length>0)
        {
            Body.AddForce(transform.up *50f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(transform.position + transform.forward * Forward + transform.up * upAmount, Radius);
    }
}
