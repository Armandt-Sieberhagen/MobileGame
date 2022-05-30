using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    EnemyAudio AudioManager;
    public bool Boss;
    public List<Coin> coins;
    public int MaxCoinDrop;
    NavMeshAgent agent;
    Animator anims;
    public float health = 100;
    public LayerMask LayerToAttack;
    public LayerMask CharacterLayer;
    bool Patrolling;
    public Transform[] PatrolLocations;
    public GameObject Enemy;
    int patrolCounter;
    float currentAttackCounter;
    int currentAttackNum;
    float Urgency;
    public float CheckRadius = 5;
    public float maxFollow = 25;
    float enemyHealth;
    public float DamageDealt = 5;
    float forwardAmount;
    float TurnAmount;
    public Transform attacker;
    bool KnockBack;
    float KnockBackTime;
    Rigidbody Body;
    bool EnemyIsPlayer;
    MiniCharacterMove PlayerValues;
    EnemyAi EnemyValues;
    float RandomUpdate;
    bool Poisoned;
    float PoisonedTimer;

   

    // Start is called before the first frame update
    void Awake()
    {
        AudioManager = FindObjectOfType<EnemyAudio>();
        currentAttackNum = 1;
        PatrolLocations = new Transform[1];
        PatrolLocations[0] = this.transform;
        agent = GetComponent<NavMeshAgent>();
       // this.gameObject.layer = CharacterLayer.value;
        Patrolling = true;
        anims = GetComponent<Animator>();
        Body = GetComponent<Rigidbody>();
        Poisoned = false;
    }
    public void AssignPoison(float TimeLeftToLive)
    {
        if (!Poisoned)
        {
            PoisonedTimer = TimeLeftToLive;
            Poisoned = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (KnockBack)
        {
            generateKnockBack();
        }
        

        
    }

    private void generateKnockBack()
    {
        agent.isStopped = true;
        if (KnockBackTime>Time.deltaTime)
        {
            Vector3 moveDirection = attacker.transform.position - transform.position ;
            GetComponent<Rigidbody>().AddForce(moveDirection.normalized * -100f);
            KnockBackTime -= Time.deltaTime;
        }
        
    }

    private void FixedUpdate()
    {
        if (Poisoned)
        {
            PoisonedTimer -= Time.deltaTime;
            if (PoisonedTimer <= Time.deltaTime)
            {
                TakeDamage(50000,this.transform);
            }
        }


        if (health>0 || KnockBack)
        {
            agent.isStopped = false;
            if (Patrolling && Enemy==null && PatrolLocations!=null)
            {
                agent.destination = NextPatrolDestination();
                FindNewEnemy();
                Urgency = 0.5f;
            }
            else if(Enemy!=null)
            {
                if (RandomUpdate<Time.deltaTime)
                {
                    RandomUpdate = UnityEngine.Random.Range(0.1f, 0.8f);
                    Urgency = 1f;
                    Patrolling = false;
                    /* */
                    

                    if (Enemy == null)
                    {
                        Patrolling = true;
                    }
                    else if (enemyHealth > 0)
                    {
                        if (agent.remainingDistance > maxFollow)
                        {
                            Patrolling = true;
                        }
                        if (Vector3.Distance(transform.position,Enemy.transform.position)<2 && currentAttackCounter <Time.deltaTime)
                        {
                            agent.destination = transform.position;
                            attack();
                            currentAttackCounter = 2f;
                        }
                        else
                        {
                            agent.destination = Enemy.transform.position;
                        }
                    }
                    else
                    {
                        FindNewEnemy();
                    }

                }
                else
                {
                    RandomUpdate -= Time.deltaTime;
                }
                
            }
            else
            {
                Patrolling = true;
            }

            updateAnims();

        }
        else if(health <= 0)
        {
            Debug.Log(health);
            Debug.Log(this.transform.localScale);
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, Vector3.zero, Time.deltaTime*4f);
        }
        currentAttackCounter -= Time.deltaTime;
       
        //  this.gameObject.layer = CharacterLayer.value;
    }

    private void FindNewEnemy()
    {
        Collider[] hitColliders;
        if (Enemy == null)
        {
            hitColliders = Physics.OverlapSphere(transform.position, CheckRadius, LayerToAttack);
        }
        else
        {
            hitColliders = Physics.OverlapSphere(Enemy.transform.position, CheckRadius, LayerToAttack);
        }
      
        float EnemyDistance = 2000;
        Enemy = null;
        foreach (var hitCollider in hitColliders)
        {
            if (Vector3.Distance(transform.position, hitCollider.transform.position)< EnemyDistance)
            {
                EnemyDistance = Vector3.Distance(transform.position, hitCollider.transform.position);
                Enemy = hitCollider.gameObject;
            }
        }
        if (Enemy == null)
        {
            Patrolling = true;
        }
        else
        {
            if (Enemy.GetComponent<EnemyAi>())
            {
                EnemyIsPlayer = false;
                
                EnemyValues = Enemy.GetComponent<EnemyAi>();
                enemyHealth = EnemyValues.health;
            }
            if (Enemy.GetComponent<MiniCharacterMove>())
            {
                EnemyIsPlayer = true;
                PlayerValues = Enemy.GetComponent<MiniCharacterMove>();
                enemyHealth = Enemy.GetComponent<MiniCharacterMove>().health;
            }
        }
    }

  

    private Vector3 NextPatrolDestination()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            patrolCounter++;
            if (patrolCounter > PatrolLocations.Length - 1)
            {
                patrolCounter = 0;
            }
        }
        return PatrolLocations[patrolCounter].position;
        
    }

    private void updateAnims()
    {
        if (Enemy!=null)
        {
            transform.LookAt(Enemy.transform.position);
        }
        else
        {
            transform.LookAt(transform.position+ agent.velocity);
        }

        if (agent.velocity!=Vector3.zero)
        {
            float DotResultForwards = Vector3.Dot(transform.right, transform.position + agent.velocity);
            if (DotResultForwards < 0)
            {
                forwardAmount = Urgency;
            }
            else
            {
                forwardAmount = -Urgency;
            }
            float DotResultLeftOrRight = Vector3.Dot(transform.right, transform.position + agent.velocity);
            if (DotResultLeftOrRight > 0)
            {
                TurnAmount = Urgency;
            }
            else
            {
                TurnAmount = -Urgency;
            }
         
        }
        else
        {
            forwardAmount = 0;
            TurnAmount = 0;
        }
        
        anims.SetFloat("Forwards", forwardAmount, 0.1f, Time.deltaTime);
        anims.SetFloat("Turn", TurnAmount, 0.1f, Time.deltaTime);
    }

    private void attack()
    {
        if (Boss)
        {
            anims.SetBool("BottomAttack", true);
        }
        else
        {
            if (agent.velocity != Vector3.zero)
            {
                anims.SetBool("BottomAttack", false);
            }
            else { anims.SetBool("BottomAttack", true); }
        }
        

     
        
        anims.SetInteger("AttackCounter", currentAttackNum);
        anims.SetTrigger("Attack");
        currentAttackNum++;
        if (Boss)
        {
            if (currentAttackNum > 4)
            {
                currentAttackNum = 1;
            }
        }
        else
        {
            if (currentAttackNum > 2)
            {
                currentAttackNum = 0;
            }
        }
        
        currentAttackCounter = 2f;
        if (EnemyIsPlayer)
        {
            enemyHealth = PlayerValues.health;
        }
        else
        {
            enemyHealth = EnemyValues.health;
        }
    }

    public void dealSomeDamage()
    {
        if (Enemy!=null)
        {
           
            if (Vector3.Distance(transform.position, Enemy.transform.position) < 2)
            {
                AudioManager.PlaySound("Sword" + UnityEngine.Random.Range(1, 15).ToString());
                if (Enemy.GetComponent<EnemyAi>())
                {
                    Enemy.GetComponent<EnemyAi>().TakeDamage(DamageDealt, transform);
                }
                if (Enemy.GetComponent<MiniCharacterMove>())
                {
                    Enemy.GetComponent<MiniCharacterMove>().TakeDamage(DamageDealt, transform);
                }
            }
            else
            {
                AudioManager.PlaySound("Woosh" + UnityEngine.Random.Range(1, 3).ToString());
            }
        }
       
       
    }

    public void TakeDamage(float DamageAmount , Transform DamageDealer)
    {
        AudioManager.PlaySound("VoiceHurt" + UnityEngine.Random.Range(1, 4).ToString());
        if (health>0)
        {
            if (DamageDealer.GetComponent<EnemyAi>() || DamageDealer.GetComponent<MiniCharacterMove>())
            {
                if (Enemy == null)
                {
                    Enemy = DamageDealer.gameObject;
                }
                else
                {
                    Enemy = DamageDealer.gameObject;
                }
            }
           

            attacker = DamageDealer;
            
            health -= DamageAmount;
            if (health == 0)
            {
                health -= 10;
            }
            if (DamageDealer.GetComponent<MiniCharacterMove>() && Boss == false)
            {
                if (health <= 0)
                {
                    Die();
                }
                else
                {
                    anims.SetTrigger("KnockBack");
                }

                KnockBackTime = 1;
                KnockBack = true;
            }
            else
            {
                if (health <= 0)
                {
                    Die();
                }
                else
                {
                    anims.SetTrigger("Hit");
                }

            }
        }
       
       
    }

    private void Die()
    {
        anims.SetTrigger("Death");

        Invoke("SpawnCoins",2f);
        Destroy(this.gameObject, 3f);
    }

    void SpawnCoins()
    {
        int randomCoinAmount = UnityEngine.Random.Range(0, MaxCoinDrop);
        for (int i = 0; i < randomCoinAmount; i++)
        {
            AudioManager.PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
            Instantiate(coins[UnityEngine.Random.Range(0, coins.Count)], transform.position, transform.rotation);
        }
    }

    public void SetKnockBackFalse()
    {
        KnockBack = false;
        KnockBackTime = 0;
    }
}
