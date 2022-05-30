using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCharacterMove : MonoBehaviour
{
    [Range(-5f,5f)]
    public float EffectIntensity;
    public PostProcessingChangeScript PostProcess;
    public GameObject AttackButton;
    public GameObject PauseButton;
    EnemyAudio AudioManager;
    public GameObject HitSpawn;
    public float WalkSpeed = 0.1f;
    public float RunSpeed = 0.2f;
    public float DamageArea = 1;
    public float ForwardDamageDistance = 1;
    Rigidbody RigidbodyPlayer;
    public LayerMask EnemyMask;
    public LayerMask CharacterMask;
    public float DamageDealt = 50;
    public MovingSkills MoveSkills;
    public AttackSkils AttackSkills;
    Animator anims;
    float MoveTimer;
    public Vector3 randoRot;

    Transform cam;
    Vector3 CamForward;
    Vector3 Move;
    Vector3 MoveInput;
    Vector3 movement;
    Vector3 LockedMovement;
    private Vector3 MovementInput;

    public float health = 100;

    [SerializeField] public Joystick joystick;
    [SerializeField] public Joystick joystickRight;

    bool Attacking;
    float forwardAmount;
    float TurnAmount;
    public float speed = 1;
    int currentAttackNum = 1;
    float currentAttackCounter = 1;
    public float AttackMod;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<EnemyAudio>();
        RigidbodyPlayer = GetComponent<Rigidbody>();
        anims = GetComponent<Animator>();

        cam = Camera.main.transform;
       // health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (health>0)
        {
            float horizontal = 0;
            float Vertical = 0;
          
            horizontal = joystick.Horizontal;
            Vertical = joystick.Vertical;
            Vector3 Aim = new Vector3(horizontal, 0, Vertical);
            Vector3 LookDirection = (transform.position + Aim) - transform.position;
            LookDirection.y = 0;
            transform.LookAt(transform.position + LookDirection, Vector3.up);
            WalkSpeed = 0.4f;
            RunSpeed = 1f;
        }
        
    }
    public void ExecuteCoolAttack()
    {
        if (AttackSkills!=null && health>0)
        {
            AttackSkills.AttackSkill(this.transform);
        }
        
    }
    private void FixedUpdate()
    {
        if (health>0)
        {
            float horizontal = joystick.Horizontal;
            float Vertical = joystick.Vertical;


            if (cam != null)
            {
                CamForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
                Move = Vertical * CamForward + horizontal * cam.right;
            }
            else
            {
                Move = Vertical * Vector3.forward + horizontal * Vector3.right;
            }

            if (Move.magnitude > 1)
            {
                Move.Normalize();
            }

            PlayerMove(Move);

            //  Debug.Log(movement);
            movement = new Vector3(horizontal, 0, Vertical);
            if (MoveTimer>Time.deltaTime)
            {
                CallMoveSpeed();
            }
            else
            {
                RigidbodyPlayer.AddForce(movement * speed / Time.deltaTime);
                MoveTimer -= Time.deltaTime;
            }

        }
        else
        {
            TakeDamage(20,this.transform);
        }
      
        
    }

    public void CallMoveSpeed()
    {
        if (PostProcess != null)
        {
            PostProcess.DistortIntensity(-EffectIntensity);
        }
        MoveTimer -= Time.deltaTime;
        RigidbodyPlayer.AddForce(movement *speed* MoveSkills.BoostAmount / Time.deltaTime);
        MoveSkills.SkilledMove(transform);
    }

    public void SetMoveTimer(int value)
    {
        MoveTimer = value;
    }

    void PlayerMove(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this.MoveInput = move;

        ConverMoveInput();
        UpdateAnimation();
    }

    void ConverMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(MoveInput);
        TurnAmount = localMove.x;

        forwardAmount = localMove.z;
    }
    void UpdateAnimation()
    {
       // AttackMode();
        anims.SetFloat("Forwards", forwardAmount, 0.1f, Time.deltaTime);
        anims.SetFloat("Turn", TurnAmount, 0.1f, Time.deltaTime);
        
    }

    void AttackMode()
    {
        if (Attacking)
        {
            if (PostProcess != null)
            {
                PostProcess.DistortIntensity(-EffectIntensity*0.5f);
               // Vector2 AttackDir = new Vector2()
            //    PostProcess.ChangeCenter();
            }
            AudioManager.PlaySound("Woosh" + UnityEngine.Random.Range(1, 3).ToString());
            if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                anims.SetBool("BottomAttack", true);
            }
            else
            {
                anims.SetBool("BottomAttack", false);
            }
            anims.SetInteger("AttackCounter", currentAttackNum);
            anims.SetTrigger("Attack");
            currentAttackNum++;
            if (currentAttackNum > 4)
            {
                currentAttackNum = 0;
            }
            currentAttackCounter = 1f;
            /*  if (currentAttackCounter<Time.deltaTime)
              {
                //  DealSomeDamage();

              }
              else
              {
                  currentAttackCounter -= Time.deltaTime;
              }*/


        }
    }

    public void addHealth()
    {
        if (health<8)
        {
            health++;
            if (PostProcess != null)
            {
                PostProcess.DistortIntensity(-EffectIntensity);
                PostProcess.SetVignetteColor(Color.white);
            }
        }
        
    }

    public void TakeDamage(float DamageAmount, Transform attacker)
    {
        if (PostProcess!=null)
        {
            PostProcess.DistortIntensity(EffectIntensity);
            PostProcess.SetVignetteColor(Color.red);
        }
        if (Vector3.Distance(transform.position, attacker.position)<5)
        {
            if (health>=0)
            {
                AudioManager.PlaySound("HitCartoon");
                health -= DamageAmount;
                if (health <= 0)
                {
                    Die();
                }
            }
           
        }
        
    }

    void DealSomeDamage()
    {
        
        RigidbodyPlayer.AddForce(transform.forward* DamageDealt*5);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward* ForwardDamageDistance, DamageArea, EnemyMask);
       // Debug.Log(hitColliders[0]);
        if (hitColliders.Length!=0)
        {
            AudioManager.PlaySound("HitCartoon" + UnityEngine.Random.Range(1, 5).ToString());
            // AudioManager.PlaySound("Sword" + UnityEngine.Random.Range(1, 15).ToString());
            for (int i = 0; i < hitColliders.Length; i++)
            {
                
                if (hitColliders[i].GetComponent<EnemyAi>())
                {
                    hitColliders[i].GetComponent<EnemyAi>().TakeDamage(DamageDealt + AttackMod, this.transform);
                    if (HitSpawn != null)
                    {
                        Vector3 random = new Vector3(randoRot.x * UnityEngine.Random.Range(0, 360), randoRot.y * UnityEngine.Random.Range(0, 360), randoRot.z * UnityEngine.Random.Range(0, 360));

                        Destroy(Instantiate(HitSpawn, hitColliders[i].transform.position, this.transform.rotation * HitSpawn.transform.rotation * Quaternion.Euler(random)), 3f);
                    }
                }
            }
        }
       
        Attacking = false;
        Debug.Log(DamageDealt + AttackMod);
    }

   public void CallAnAttack()
    {
        Attacking = true;
        AttackMode();
    }

    

    private void Die()
    {
        anims.SetTrigger("Death");
        joystick.gameObject.SetActive(false);
        PauseButton.SetActive(false);
        AttackButton.SetActive(false);
    }
}
