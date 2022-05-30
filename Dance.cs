using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour
{
    public int Band;
    public float StartScale, ScaleMultiplier;
    public AudioPeer peer;
    public Vector3 TargetVector;
    GrowPiece Grow;
    public bool XAndZAxis;
    // Start is called before the first frame update
    void Awake()
    {
        Band = Random.Range(0,8);
        Grow = GetComponent<GrowPiece>();
    }

    // Update is called once per frame
    void Update()
    {
        if (peer==null)
        {
            peer = GameObject.FindGameObjectWithTag("MainAudio").GetComponent<AudioPeer>();
        }
        if (Grow==null)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, TargetVector, Time.deltaTime);
        }
        else
        {
            if (Grow.UpdateTimer <= Time.deltaTime)
            {
                transform.localScale = Vector3.Slerp(transform.localScale, TargetVector, Time.deltaTime);
            }
        }
        
      
   
    }

    private void FixedUpdate()
    {
        if (peer!=null)
        {
            if (XAndZAxis)
            {
                TargetVector = new Vector3((peer.frequenceBand[Band] * ScaleMultiplier) + StartScale, (peer.frequenceBand[Band] * ScaleMultiplier) + StartScale, (peer.frequenceBand[Band] * ScaleMultiplier) + StartScale);
            }
            else
            {
                TargetVector = new Vector3(StartScale, (peer.frequenceBand[Band] * ScaleMultiplier) + StartScale, StartScale);
            }
            
        }
       
    }
}
