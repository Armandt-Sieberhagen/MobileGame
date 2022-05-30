using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(TrailRenderer), typeof(BoxCollider))]
public class Coin : MonoBehaviour
{
    public int Value;
    public PlayerInfo info;
    GameObject Player;
    bool GoToPlayer;
    float TimeThing;
    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        if (info==null)
        {
            info = Player.GetComponent<characterAssigner>().Info;
        }
        GoToPlayer = false;
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddExplosionForce(500,this.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3)),5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeThing += Time.deltaTime;
 
        if (Vector3.Distance(Player.transform.position,transform.position)<4f)
        {
            GoToPlayer = true;
        }
        if (GoToPlayer)
        {
            transform.position = Vector3.Slerp(transform.position,Player.transform.position,Time.deltaTime*10);
        }
        if (Vector3.Distance(Player.transform.position, transform.position) < 0.5f)
        {
            FindObjectOfType<EnemyAudio>().PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
            info.IncreaseCoins(Value);
            Destroy(this.gameObject);
            
        }
    }
}
