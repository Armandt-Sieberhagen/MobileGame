using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class LandScapeGen : MonoBehaviour
{



    public Vector3 DecorSize;
    public List<GrowPiece> LightSideItems;
    public List<GrowPiece> DarkSideItems;
    public List<GameObject> Locations;
    public GrowPiece[] CurrentlyActive;
    private List<GrowPiece> CurrentlyDying;


    
    public int HexIDx;
   public int HexIdY;
    public float TerrainID;
    public float ChoiceOfTerrainID;
    public int reloadCounter = 0;

    [SerializeField] List<GameObject> Water;
    [SerializeField] List<GameObject> Mountains;
    [SerializeField] List<GameObject> Flat;
    [SerializeField] List<GameObject> Roads;

    public float MountainRange;
    public float WaterRange;

   public ProceduralMapGen MapInfo;

    public GameObject CurrentPiece;
    public GameObject PreviousPiece;
    public Material CleanMaterial;
    public Material CorruptedMaterial;
    public bool Corrupted;
    public bool IsFlat;
    AudioPeer peer;
    public VillageGenerator VillageGenerator;
    public bool VillageActive;
    int count;
    bool StopChange;
    public bool PlayerWasOnMe;

    public void Awake()
    {
        peer = GameObject.FindGameObjectWithTag("MainAudio").GetComponent<AudioPeer>();
        CurrentlyActive = new GrowPiece[Locations.Count];
        StopChange = false;
    }

    public void ChangeStatus()
    {

        if (CleanMaterial!=null && CorruptedMaterial!=null && CurrentPiece!=null && StopChange==false)
        {
            if (Corrupted && CurrentPiece.GetComponent<MeshRenderer>().material!= CorruptedMaterial)
            {
                this.GetComponent<MeshRenderer>().material = CorruptedMaterial;
                CurrentPiece.GetComponent<MeshRenderer>().material = CorruptedMaterial;
                GenerateDecors(DarkSideItems);
                
                if (PlayerWasOnMe)
                {
                    if (VillageGenerator.Activate == false)
                    {
                        VillageGenerator.DarkSide = true;
                        VillageGenerator.Activate = true;
                    }
                    else
                    {
                        VillageGenerator.DarkSide = false;
                        VillageGenerator.Activate = false;
                    }
                    
                }

            }
            else if(Corrupted==false && CurrentPiece.GetComponent<MeshRenderer>().material != CleanMaterial)
            {
                this.GetComponent<MeshRenderer>().material = CleanMaterial;
                CurrentPiece.GetComponent<MeshRenderer>().material = CleanMaterial;
                GenerateDecors(LightSideItems);

                if (PlayerWasOnMe)
                {
                    if (VillageGenerator.Activate == false)
                    {
                        VillageGenerator.LightSide = true;
                        VillageGenerator.Activate = true;
                    }
                    else
                    {
                        VillageGenerator.LightSide = false;
                        VillageGenerator.Activate = false;
                    }
                    
                }

            }

        }
    }
   
    public void GenerateDecors(List<GrowPiece> Decor)
    {
        if (IsFlat)
        {
            for (int i = 0; i < CurrentlyActive.Length; i++)
            {
                if (CurrentlyActive[i] != null)
                {
       
                    CurrentlyActive[i].dead = true;
                    CurrentlyActive[i].UpdateTimer = 5;
                    CurrentlyActive[i].AssignedPosition = CurrentlyActive[i].transform.position;
                }

            }


            CurrentlyActive = new GrowPiece[Locations.Count];
            Material ThisMat = this.GetComponent<MeshRenderer>().material;

            for (int i = 0; i < CurrentlyActive.Length; i++)
            {

                if (Random.Range(0, 10) >= 5)
                {
                    GrowPiece RandomGrowPiece = Decor[Random.Range(0, Decor.Count)];
                    GrowPiece Grow = Instantiate(RandomGrowPiece, Locations[i].transform.position, RandomGrowPiece.transform.rotation, this.transform);

                    Grow.dead = false;
                    Grow.AssignedSize = DecorSize;
                    Grow.UpdateTimer = 5;
                    Grow.AssignedPosition = Locations[i].transform.position;
                    if (Grow.GetComponent<MeshRenderer>())
                    {
                        Grow.GetComponent<MeshRenderer>().material = ThisMat;
                    }
                    CurrentlyActive[i] = Grow;
                    Grow.GetComponent<Dance>().peer = this.peer;
                }
            }
        }
    }

    public void GenerateLandScape(float TypeOfTerrain, float ChoiceOfTerrain)
    {
        if (MapInfo != null)
        {
            MountainRange = MapInfo.MountanRanges;
            WaterRange = MapInfo.WaterRanges;
        }
       
        if (TypeOfTerrain>MountainRange)
        {
            IsFlat = false;
            CreateActiveLandScapes(Mountains, ChoiceOfTerrain);
        }
        else if(TypeOfTerrain<WaterRange)
        {
            CreateActiveLandScapes(Water, ChoiceOfTerrain);
            IsFlat = false;
        }
        else
        {
            CreateActiveLandScapes(Flat, ChoiceOfTerrain);
            
            if (Random.Range(0, 100) >= 90)
            {
              
                VillageGenerator.gameObject.SetActive(true);
                VillageActive = true;
                IsFlat = false;
            }
            else
            {
                IsFlat = true;
                VillageGenerator.gameObject.SetActive(false);
                VillageActive = false;
            }
        }
        ChangeStatus();
        int RotationValue = UnityEngine.Random.Range(1, 6);
     //   Debug.Log(RotationValue);
        this.transform.rotation = Quaternion.Euler(0, RotationValue*60, 0);

    }

    public void CreateActiveLandScapes(List<GameObject> ListOfGayobjects, float ChoiceOfTerrain)
    {
        if (CurrentPiece==null)
        {
            int value = Mathf.RoundToInt(ChoiceOfTerrain * (ListOfGayobjects.Count-1));
            if (value> ListOfGayobjects.Count - 1)
            {
                value = ListOfGayobjects.Count - 1;
            }
            if (value < 0)
            {
                value = 0;
            }
            ListOfGayobjects[value].SetActive(true);
            CurrentPiece = ListOfGayobjects[value];
         
        }
        else
        {
            PreviousPiece = CurrentPiece;
            CurrentPiece = null;
            PreviousPiece.SetActive(false);
      
            int value = Mathf.RoundToInt(ChoiceOfTerrain * ListOfGayobjects.Count-1);
            if (value > ListOfGayobjects.Count - 1)
            {
                value = ListOfGayobjects.Count - 1;
            }
            if (value < 0)
            {
                value = 0;
            }
       
            ListOfGayobjects[value].SetActive(true);
            CurrentPiece = ListOfGayobjects[value];
        
        }
       


    }

    
}
