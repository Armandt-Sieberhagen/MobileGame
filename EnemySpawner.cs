using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyAi> EnemyPrefab;
    public List<EnemyAi> BossPrefab;
    public MiniCharacterMove Player;
    public int currentAmountofEnemies;
    public PlayerInfo Info;
    public EnemyAi BossInfo;
    public List<EnemyAi> ListOfPlayerEnemies;
    public float HealthMod;
    public float KillAllTimer;
    public float DifficultyScale;
    public bool Stopped;
    public float nextWaveCounter;
    public int currentWave;
    public bool Conquered;
    public bool ActiveNess;
    public int MaxWaves;

    private void Awake()
    {
        Conquered = false;
        currentWave = 1;
        ActiveNess = false;
        Player = GameObject.FindWithTag("Player").GetComponent<MiniCharacterMove>();
        Info = Player.GetComponent<characterAssigner>().Info;
        if (Info.IsWorldCorrupted)
        {
            EnemyPrefab = Info.LightSideEnemy;
            BossPrefab = Info.LightSideBosses;
        }
        else
        {
            EnemyPrefab = Info.DarkSideEnemy;
            BossPrefab = Info.DarkSideBosses;
        }
        HealthMod = 1;

        currentAmountofEnemies = 1;
        nextWaveCounter = 3f;
        KillAllTimer = 5f;
        MaxWaves = Mathf.RoundToInt(DifficultyScale)  * 2;
        if (MaxWaves>14)
        {
            MaxWaves = 14;
        }
    }

    private void FixedUpdate()
    {

        if (ActiveNess)
        {
         
            if (nextWaveCounter < Time.deltaTime && Stopped == false)
            {
                nextWaveCounter = 3f;
                int NullCounter = 0;
                for (int i = 0; i < ListOfPlayerEnemies.Count; i++)
                {
                    if (ListOfPlayerEnemies[i] == null)
                    {
                        NullCounter++;
                    }
                    else
                    {
                        if (KillAllTimer <= Time.deltaTime)
                        {
                            ListOfPlayerEnemies[i].AssignPoison(300f);
                        }
                    }


                }

                if (NullCounter == ListOfPlayerEnemies.Count)
                {
                    NullCounter = 0;
                    GenerateEnemies();
                }
                if (KillAllTimer <= Time.deltaTime)
                {
                    KillAllTimer = 5f;
                }
            }
            else
            {
                nextWaveCounter -= Time.deltaTime;
                KillAllTimer -= Time.deltaTime;
            }

            if (Stopped)
            {
                if (BossInfo.health <= 0)
                {
                    Conquered = true;
                }
            }
        }
        
       
        
    }

    void GenerateEnemies()
    {
   
        currentAmountofEnemies *= 2;
        
        if (currentWave >= MaxWaves)
        {
           
            SpawnBoss();

        }
        else
        {
            currentWave++;
            ListOfPlayerEnemies.Clear();
            float radius = 25f;
            for (int i = 0; i < currentAmountofEnemies; i++)
            {
                
        
                float angle = i * Mathf.PI * 2f / currentAmountofEnemies;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius + Random.Range(-3, 3), 20, Mathf.Sin(angle) * radius + Random.Range(-3, 3)) + Player.transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newPos, out hit, 1000f, NavMesh.AllAreas))
                {
                    
                    int randomPicker = Random.Range(0, EnemyPrefab.Count);
                    GameObject go = Instantiate(EnemyPrefab[randomPicker].gameObject, hit.position+new Vector3(0,1,0), Quaternion.identity);
                    ListOfPlayerEnemies.Add(go.GetComponent<EnemyAi>());
                    go.GetComponent<EnemyAi>().Enemy = Player.gameObject;
                    go.GetComponent<EnemyAi>().health = go.GetComponent<EnemyAi>().health * DifficultyScale;


                }
            }
        }
       
    }

    void SpawnBoss()
    {
        if (CheckSpawner())
        {
            Stopped = true;
        }
        
    }

    bool CheckSpawner()
    {
        float radius = 25f;
        bool spawned= false;
        for (int i = 0; i < currentAmountofEnemies; i++)
        {
            float angle = i * Mathf.PI * 2f / currentAmountofEnemies;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, 10f, Mathf.Sin(angle) * radius) + Player.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPos, out hit, 1000f, NavMesh.AllAreas) && spawned==false)
            {
                int randomPicker = Random.Range(0, BossPrefab.Count);
                GameObject go = Instantiate(BossPrefab[randomPicker].gameObject, hit.position, Quaternion.identity);
                BossInfo = go.GetComponent<EnemyAi>();
             
                go.GetComponent<EnemyAi>().Enemy = Player.gameObject;
                go.GetComponent<EnemyAi>().health = BossPrefab[randomPicker].health * DifficultyScale;
                i = currentAmountofEnemies;
                spawned = true;
                return true;
            }
        }
        return false;
    }


}
