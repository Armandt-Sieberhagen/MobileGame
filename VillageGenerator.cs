using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageGenerator : MonoBehaviour
{
    public List<Coin> coins;
    public int MaxCoinDrop = 5;
    public bool Activate;
    public bool DarkSide;
    public bool LightSide;
    PlayerInfo Info;
    public LandScapeGen ParentGenerator;
    bool Updated;
    public EnemySpawner Spawner;
    public CharacterName InfoDescriber;
    public GameObject DarkVillage;
    public GameObject LightVillage;
    public TextMesh text;
    GameObject Player;
    public bool active;
    int difficultyGen;
    EnemyAudio AudioManager;
    void Awake()
    {
        AudioManager = FindObjectOfType<EnemyAudio>();
        Player = GameObject.FindWithTag("Player");
        Info = Player.GetComponent<characterAssigner>().Info;
        Updated = false;
        active = true;
        GenerateDifficulty();
    }

    void GenerateDifficulty()
    {


        if ((ParentGenerator.HexIDx<=1050 && ParentGenerator.HexIDx >= 950) && (ParentGenerator.HexIdY <= 1050 && ParentGenerator.HexIdY >= 950))
        {
            difficultyGen = Random.Range(1,3);
        }
        else if ((ParentGenerator.HexIDx <= 1100 && ParentGenerator.HexIDx >= 900) && (ParentGenerator.HexIdY <= 1100 && ParentGenerator.HexIdY >= 900))
        {
            difficultyGen = Random.Range(2, 4);
        }
        else if ((ParentGenerator.HexIDx <= 1150 && ParentGenerator.HexIDx >= 850) && (ParentGenerator.HexIdY <= 1150 && ParentGenerator.HexIdY >= 850))
        {
            difficultyGen = Random.Range(3, 5);
        }
        else if ((ParentGenerator.HexIDx <= 1200 && ParentGenerator.HexIDx >= 800) && (ParentGenerator.HexIdY <= 1200 && ParentGenerator.HexIdY >= 800))
        {
            difficultyGen = Random.Range(4, 7);
        }
        else if ((ParentGenerator.HexIDx <= 1250 && ParentGenerator.HexIDx >= 750) && (ParentGenerator.HexIdY <= 1250 && ParentGenerator.HexIdY >= 750))
        {
            difficultyGen = Random.Range(5, 9);
        }
        else if ((ParentGenerator.HexIDx <= 1300 && ParentGenerator.HexIDx >= 700) && (ParentGenerator.HexIdY <= 1300 && ParentGenerator.HexIdY >= 700))
        {
            difficultyGen = Random.Range(6, 12);
        }
        else if ((ParentGenerator.HexIDx <= 1350 && ParentGenerator.HexIDx >= 650) && (ParentGenerator.HexIdY <= 1350 && ParentGenerator.HexIdY >= 650))
        {
            difficultyGen = Random.Range(7, 12);
        }
        else if ((ParentGenerator.HexIDx <= 1400 && ParentGenerator.HexIDx >= 600) && (ParentGenerator.HexIdY <= 1400 && ParentGenerator.HexIdY >= 600))
        {
            difficultyGen = Random.Range(9, 12);
        }
        else if ((ParentGenerator.HexIDx <= 1450 && ParentGenerator.HexIDx >= 550) && (ParentGenerator.HexIdY <= 1450 && ParentGenerator.HexIdY >= 550))
        {
            difficultyGen = Random.Range(10, 13);
        }
        else if ((ParentGenerator.HexIDx <= 1500 && ParentGenerator.HexIDx >= 500) && (ParentGenerator.HexIdY <= 1500 && ParentGenerator.HexIdY >= 500))
        {
            difficultyGen = Random.Range(10, 15);
        }
        else if ((ParentGenerator.HexIDx == 1000 && ParentGenerator.HexIDx == 1000))
        {
            difficultyGen = 1;
        }
        else
        {
            difficultyGen = Random.Range(13, 35);
        }
        Spawner.DifficultyScale = difficultyGen;
        Spawner.ActiveNess = false;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            
            text.transform.LookAt(Camera.main.transform.position);
            if (Activate)
            {
                Spawner.ActiveNess = true;
                text.characterSize = 0.1f;
                if (Spawner.Stopped)
                {
                    text.text = "Final WAVE!!!";
                }
                else
                {
                    text.text = ("Current Wave: " + Spawner.currentWave.ToString());
                }

                Spawner.ActiveNess = true;
                if (Spawner.Conquered)
                {
                    FindObjectOfType<EnemyAudio>().PlaySound("Success");
                    if (DarkSide)
                    {
                        DarkVillage.GetComponent<GrowPiece>().UpdateTimer = 5;
                        DarkVillage.GetComponent<GrowPiece>().AssignedSize = new Vector3(1, 1, 1);
                        Instantiate(DarkVillage, ParentGenerator.transform.position, Quaternion.identity, ParentGenerator.transform);
                        EnemyAi Ai = Instantiate(Info.DarkCompanions[Random.Range(0, Info.DarkCompanions.Count)],transform.position,transform.rotation);
                        Ai.PatrolLocations = Player.GetComponent<ListOfPatrolLocations>().PatrolLocations;

                    }
                    else if (LightSide)
                    {
                        LightVillage.GetComponent<GrowPiece>().UpdateTimer = 5;
                        LightVillage.GetComponent<GrowPiece>().AssignedSize = new Vector3(1,1,1);
                        Instantiate(LightVillage, ParentGenerator.transform.position, Quaternion.identity, ParentGenerator.transform);
                        EnemyAi Ai = Instantiate(Info.LightCompanions[Random.Range(0, Info.LightCompanions.Count)], transform.position, transform.rotation);
                        Ai.PatrolLocations = Player.GetComponent<ListOfPatrolLocations>().PatrolLocations;
                    }
                    Spawner.ActiveNess = false;
                    active = false;
                    this.gameObject.SetActive(false);
                    Info.IncreaseAttackMod((float)difficultyGen);
                    Player.GetComponent<MiniCharacterMove>().addHealth();
                    SpawnCoins();


                }

            }
            else
            {
                text.text = difficultyGen.ToString();
                text.characterSize = 0.5f;
                Spawner.ActiveNess = false;
                Spawner.nextWaveCounter = 5f;
            }
        }
        else
        {
            Spawner.ActiveNess = false;
            Spawner.nextWaveCounter = 1f;
        }
       
    }


    void SpawnCoins()
    {
        int randomCoinAmount = UnityEngine.Random.Range(MaxCoinDrop, (MaxCoinDrop+1)*difficultyGen);
        for (int i = 0; i < randomCoinAmount; i++)
        {
            AudioManager.PlaySound("Coin" + UnityEngine.Random.Range(1, 9).ToString());
            Instantiate(coins[UnityEngine.Random.Range(0, coins.Count)], transform.position, transform.rotation);
        }
    }
}
