using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    public MiniCharacterMove Player;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.health<0)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(2500,2500,2500), Time.deltaTime);
            float y =  Mathf.PingPong(Time.time * 2, 6) * 6 - 4;
            Quaternion rot = Quaternion.Euler(new Vector3(0,0,y));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);

        }
    }
}
