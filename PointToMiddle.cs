using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToMiddle : MonoBehaviour
{
    public GameObject Lookat;
    float Right;
    float Up;

    // Update is called once per frame
    private void Start()
    {
        transform.right = transform.forward;
        transform.forward = transform.right;
    }
    void FixedUpdate()
    {
        
        transform.LookAt(Lookat.transform.position );
    }
}
