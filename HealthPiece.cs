using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class HealthPiece : MonoBehaviour
{
    public MiniCharacterMove Player;
    public List<GameObject> Hearts;
    float[] cachedHeights;

    public float amplitude = 10.0f;
    public int UpOrDownValue;
    public float omega = 1.0f;
    public float Speed = 1;
    public float Length = 1;
    public float WeirdValueOne = 6;
    public float WeirdValueTwo = 4;

    public float index;
    private void Start()
    {
        cachedHeights = new float[Hearts.Count];
        for (int i = 0; i < Hearts.Count; i++)
        {
            Hearts[i].transform.localScale = new Vector3(0,0,0);
            cachedHeights[i] = Hearts[i].transform.localPosition.y;
        }
        UpOrDownValue = 1;
    }
    private void FixedUpdate()
    {
        
        for (int i = 0; i < Hearts.Count; i++)
        {
            float SizePingPong = Mathf.PingPong(Time.time * Speed, Length) * WeirdValueOne - WeirdValueTwo;
            if (Player.health<i+1)
            {
                Hearts[i].transform.localScale = Vector3.Lerp(Hearts[i].transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*5);
            }
            else 
            {
                Hearts[i].transform.localScale = Vector3.Lerp(Hearts[i].transform.localScale, new Vector3(100+i*5+ SizePingPong, 100+i * 5+ SizePingPong, 100+i * 5+SizePingPong), Time.deltaTime * 5);
            }
            if (index>5)
            {
                UpOrDownValue = -1;
            }
            else if(index<-5)
            {
                UpOrDownValue = 1;
            }
            float y = cachedHeights[i] + Mathf.PingPong(Time.time * Speed, Length) * WeirdValueOne - WeirdValueTwo;
            Hearts[i].transform.localPosition = Vector3.Lerp(Hearts[i].transform.localPosition, new Vector3(Hearts[i].transform.localPosition.x, y, Hearts[i].transform.localPosition.z),Time.deltaTime);

            /*float Rotx = Hearts[i].transform.rotation.eulerAngles.x + amplitude* Mathf.Cos(omega * index) ;
            float Roty = Hearts[i].transform.rotation.eulerAngles.y + amplitude * Mathf.Cos(omega * index) ;
            float Rotz = Hearts[i].transform.rotation.eulerAngles.z + amplitude * Mathf.Cos(omega * index) ;
            Quaternion Rot = Quaternion.Euler(Rotx, Roty, Rotz);
            Hearts[i].transform.rotation = Quaternion.Lerp(Hearts[i].transform.rotation, Rot, Time.deltaTime); */
        }
    }
}
