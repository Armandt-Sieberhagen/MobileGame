using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

public class ProceduralMapGen : MonoBehaviour
{
    public HexValues WorldInfo;
    public float HexScale =1;
    public GameObject HexTile;
    public GameObject[,] Hextiles;
    public LandScapeGen[,] HexTileValues;
    public Vector3 offset;
    public Vector3 StartOffset;
    public int GridX = 2000;
    public int GridY = 2000;
    public float DrawDistance = 25;
    public int LoadValue = 5;
    [Header("LandScape")]
    [Range(0,50)]
   [SerializeField] public float PerlinScaleX=1;
    [Range(0, 50)]
    [SerializeField] public float PerlinScaleZ=1;
    [Range(0, 25)]
    [SerializeField] public float Scale=1;
    [Range(0, 25)]
    [SerializeField] public float PerlinOffsetX=1;
    [Range(0, 25)]
    [SerializeField] public float PerlinOffsetY=1;
    [Range(0, 25)]
    [SerializeField] public float MountanRanges=1;
    [Range(0, 25)]
    [SerializeField] public float WaterRanges=1;
    [Header("Enviroment")]
    [Range(0, 50)]
    [SerializeField] public float EnviromentPerlinScaleX = 1;
    [Range(0, 50)]
    [SerializeField] public float EnviromentPerlinScaleZ = 1;
    [Range(0, 5)]
    [SerializeField] public float EnviromentScale = 1;
    [Range(0, 25)]
    [SerializeField] public float EnviromentPerlinOffsetX = 1;
    [Range(0, 25)]
    [SerializeField] public float EnviromentPerlinOffsetY = 1;
    [SerializeField] public GameObject Player;
    private Vector3 CachedPiece;
    private PlayerLocation locationOfPlayer;
    [SerializeField] bool Loaded;
    [SerializeField] float TimeSinceLastLoad;
    private float ExplosionTimeLoad =2;
    private bool DrawDistanceLoad;
    public PlayerInfo Info;







    public void Start()
    {
       
        if (WorldInfo!=null)
        {
            HexScale = WorldInfo.HexScale;
            HexTile = WorldInfo.HexTile;
            StartOffset = WorldInfo.StartOffset;
            offset = WorldInfo.offset;
            GridX = WorldInfo.GridX;
            GridY = WorldInfo.GridY;
            LoadValue = WorldInfo.LoadValue;
            DrawDistance = WorldInfo.DrawDistance;
            PerlinScaleX = WorldInfo.PerlinScaleX;
            PerlinScaleZ = WorldInfo.PerlinScaleZ;
            Scale = WorldInfo.Scale;
            PerlinOffsetX = WorldInfo.PerlinOffsetX;
            PerlinOffsetY = WorldInfo.PerlinOffsetY;
            MountanRanges = WorldInfo.MountanRanges;
            WaterRanges = WorldInfo.WaterRanges;
            EnviromentPerlinScaleX = WorldInfo.EnviromentPerlinScaleX;
            EnviromentPerlinScaleZ = WorldInfo.EnviromentPerlinScaleZ;
            EnviromentScale = WorldInfo.EnviromentScale;
            EnviromentPerlinOffsetX = WorldInfo.EnviromentPerlinOffsetX;
            EnviromentPerlinOffsetY = WorldInfo.EnviromentPerlinOffsetY;
            HexTile.GetComponent<LandScapeGen>().CorruptedMaterial = WorldInfo.DarkSide;
            HexTile.GetComponent<LandScapeGen>().CleanMaterial = WorldInfo.LightSide;
            


        }
        if (Info!=null)
        {
            if (Info.IsWorldCorrupted)
            {
                HexTile.GetComponent<LandScapeGen>().Corrupted = false;
            }
            else
            {
                HexTile.GetComponent<LandScapeGen>().Corrupted = true;
            }
        }
        
        CachedPiece = new Vector3(2000, 2000, 2000);
        Hextiles = new GameObject[GridX, GridY];
        HexTileValues = new LandScapeGen[GridX, GridY];
       
        Hextiles[1000, 1000] = Instantiate(HexTile, new Vector3(0, 0, 0), Quaternion.identity, transform);
        HexTileValues[1000, 1000] = Hextiles[1000, 1000].GetComponent<LandScapeGen>();
        HexTileValues[1000, 1000].MapInfo = this;

        float Height = Mathf.PerlinNoise((float)1000f / PerlinScaleX * Scale + Player.transform.position.x / PerlinOffsetY, (float)1000f / PerlinScaleZ * Scale + +Player.transform.position.z / PerlinOffsetX);
        float Type = Mathf.PerlinNoise((float)1000f / EnviromentPerlinScaleX * EnviromentScale + Player.transform.position.x / EnviromentPerlinOffsetY, (float)1000f / EnviromentPerlinScaleZ * EnviromentScale + +Player.transform.position.z / EnviromentPerlinOffsetX);
        Hextiles[1000, 1000].transform.position = (StartOffset + new Vector3(1000 * offset.x , Height * offset.y, 1000 * offset.z));

        HexTileValues[1000, 1000].GenerateLandScape(Height * offset.y, Type);
        HexTileValues[1000, 1000].HexIDx = 1000;
        HexTileValues[1000, 1000].HexIdY = 1000;
        Player.transform.position = Hextiles[1000, 1000].transform.position + new Vector3(0, 3, 0);
        locationOfPlayer = Player.GetComponent<PlayerLocation>();
        TimeSinceLastLoad = 5;
        Hextiles[1000, 1000].transform.localScale = new Vector3(HexScale, HexScale, HexScale);
        

    }

    public void FixedUpdate()
    {
        
        LoadTerrain();
    }

    void LoadTerrain()
    {
        int CurrentMaxX = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationX + LoadValue);
        int CurrentMinX = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationX - LoadValue);
        int CurrentMaxY = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationY + LoadValue);
        int CurrentMinY = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationY - LoadValue);

        
        Loaded = false;
        DrawDistanceLoad = false;
        for (int i = CurrentMinX; i < CurrentMaxX; i++)
        {
            for (int j = CurrentMinY; j < CurrentMaxY; j++)
            {

                
                if (HexTileValues[i, j] == null && Loaded == false)
                {
                    float PerlinX = PerlinScaleX * Scale / PerlinOffsetX;
                    float PerlinY = PerlinScaleZ * Scale / PerlinOffsetY;
                    float EnviromentPerlinX = EnviromentPerlinScaleX * EnviromentScale / EnviromentPerlinOffsetX;
                    float EnviromentPerlinY = EnviromentPerlinScaleZ * EnviromentScale / EnviromentPerlinOffsetY;
                    float Height = Mathf.PerlinNoise((float)i / PerlinX, (float)j / PerlinY);
                    float Type = Mathf.PerlinNoise((float)i / EnviromentPerlinX, (float)j / EnviromentPerlinY);
                    Hextiles[i, j] = Instantiate(HexTile, new Vector3(0, 0, 0), Quaternion.identity, transform);
                    HexTileValues[i, j] = Hextiles[i, j].GetComponent<LandScapeGen>();
                    HexTileValues[i, j].MapInfo = this;


                    if (i % 2 == 0)
                    {
                        Hextiles[i, j].transform.position = (StartOffset + new Vector3(i * offset.x , Height * offset.y, j * offset.z));

                    }
                    else
                    {
                        Hextiles[i, j].transform.position = (new Vector3(i * offset.x, Height * offset.y, j * offset.z));

                    }

                    HexTileValues[i, j].HexIDx = i;
                    HexTileValues[i, j].HexIdY = j;
                    Hextiles[i, j].transform.localScale = new Vector3(HexScale, HexScale, HexScale);
                    HexTileValues[i, j].TerrainID = Height;
                    HexTileValues[i, j].ChoiceOfTerrainID = Type;
                    Loaded = true;
                    TimeSinceLastLoad = 5;
                    HexTileValues[i, j].GenerateLandScape(HexTileValues[i, j].TerrainID * offset.y, HexTileValues[i, j].ChoiceOfTerrainID);
                }
                if (HexTileValues[i, j]!=null)
                {
                    if (Vector3.Distance(Player.transform.position, Hextiles[i, j].transform.position) > DrawDistance)
                    {
                        Hextiles[i, j].SetActive(false);

                    }
                    else
                    {

                        Hextiles[i, j].SetActive(true);
                    }

                }
                


            }
        }
        if (Loaded==false && TimeSinceLastLoad<Time.deltaTime)
        {
            int factor = 10;
            //float x = Mathf.Round(input.x / factor) * factor;
            CurrentMaxX = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationX/ (float)factor) * factor + factor;
            CurrentMinX = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationX/ (float)factor) * factor - factor;
            CurrentMaxY = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationX/ (float)factor) * factor + factor;
            CurrentMinY = Mathf.RoundToInt(locationOfPlayer.CurrentHexLocationX/ (float)factor) * factor - factor;
       
            for (int i = CurrentMinX; i < CurrentMaxX; i++)
            {
                for (int j = CurrentMinY; j < CurrentMaxY; j++)
                {

                    
                    if (HexTileValues[i, j] == null && Loaded==false)
                    {
                        float PerlinX = PerlinScaleX * Scale / PerlinOffsetX;
                        float PerlinY = PerlinScaleZ * Scale / PerlinOffsetY;
                        float EnviromentPerlinX = EnviromentPerlinScaleX * EnviromentScale / EnviromentPerlinOffsetX;
                        float EnviromentPerlinY = EnviromentPerlinScaleZ * EnviromentScale / EnviromentPerlinOffsetY;
                        float Height = Mathf.PerlinNoise((float)i / PerlinX, (float)j / PerlinY);
                        float Type = Mathf.PerlinNoise((float)i / EnviromentPerlinX, (float)j / EnviromentPerlinY);
                        Hextiles[i, j] = Instantiate(HexTile, new Vector3(0, 0, 0), Quaternion.identity, transform);
                        HexTileValues[i, j] = Hextiles[i, j].GetComponent<LandScapeGen>();
                        HexTileValues[i, j].MapInfo = this;


                        if (i % 2 == 0)
                        {
                            Hextiles[i, j].transform.position = (StartOffset + new Vector3(i * offset.x * HexScale, Height * offset.y, j * offset.z));

                        }
                        else
                        {
                            Hextiles[i, j].transform.position = (new Vector3(i * offset.x, Height * offset.y, j * offset.z));

                        }

                        HexTileValues[i, j].HexIDx = i;
                        HexTileValues[i, j].HexIdY = j;
                        Hextiles[i, j].transform.localScale = new Vector3(HexScale, HexScale, HexScale);
                        Loaded = true;
                        Hextiles[i, j].SetActive(false);

                        TimeSinceLastLoad = 2;

                    }
                    if (Loaded==true)
                    {
                        break;
                    }
                }
                if (Loaded == true)
                {
                    break;
                }
            }
            if (Loaded==false)
            {
                ExplosionTimeLoad++;
            }
            else
            {
                ExplosionTimeLoad = 2;
            }
        }
        else
        {
            TimeSinceLastLoad -= Time.deltaTime;
        }


    }
   


}
/*if (UseJobs)
        {
            NativeList<int> iValue = new NativeList<int>(1, Allocator.TempJob);
            NativeList<int> jValue = new NativeList<int>(1, Allocator.TempJob);
            NativeList<Vector3> Positions = new NativeList<Vector3>(1, Allocator.TempJob);
            int counter = 0;
            for (int i = CurrentMinX; i < CurrentMaxX; i++)
            {
                for (int j = CurrentMinY; j < CurrentMaxY; j++)
                {

                   
                    if (HexTileValues[i, j] == null)
                    {
                        Hextiles[i, j] = Instantiate(HexTile, new Vector3(0, 0, 0), Quaternion.identity, transform);
                        HexTileValues[i, j] = Hextiles[i, j].GetComponent<LandScapeGen>();
                        HexTileValues[i, j].MapInfo = this;
                        Positions.Add(Hextiles[i, j].transform.position);
                        iValue.Add(i);
                        jValue.Add(j);
                       


                        HexTileValues[i, j].HexIDx = i;
                        HexTileValues[i, j].HexIdY = j;
                        counter++;
                    }

                    if (Vector3.Distance(Player.transform.position, Hextiles[i, j].transform.position) > DrawDistance)
                    {
                        Destroy(Hextiles[i, j].gameObject);

                    }
                   
                    if (HexTileValues[i, j].reloadCounter < 1)
                    {
                        HexTileValues[i, j].GenerateLandScape(Random.Range(0f,1f), Random.Range(0f, 1f));
                        HexTileValues[i, j].reloadCounter++;
                    }


                }
            }
            if (counter>0)
            {
                BuildNewLand newLand = new BuildNewLand
                {
                    JobPerlinScaleX = PerlinOffsetX,
                    JobScale = Scale,
                    Jobi = iValue,
                    Jobj = jValue,
                    JobPerlinOffsetY = PerlinOffsetY,
                    JobPerlinScaleY = PerlinScaleZ,
                    JobPerlinOffsetX = PerlinOffsetX,
                    JobStartOffset = StartOffset,
                    Joboffset = offset,
                    JobPositions = Positions,
                };
                JobHandle jobHandle = newLand.Schedule<BuildNewLand>(counter, 100);
                jobHandle.Complete();
                for (int i = 0; i < Positions.Capacity; i++)
                {
                    Hextiles[iValue[i], jValue[i]].transform.position = Positions[i];
                }
            }
            Positions.Dispose();
            jValue.Dispose();
            iValue.Dispose();


        }
        else
        
     
     
     
     
     
     public struct BuildNewLand : IJobParallelFor
{
    public float JobPerlinScaleX;
    public float JobScale;
    public NativeList<int> Jobi;
    public NativeList<int> Jobj;
    public float JobPerlinOffsetY;
    public float JobPerlinScaleY;
    public float JobPerlinOffsetX;
    public Vector3 JobStartOffset;
    public Vector3 Joboffset;
    public NativeList<Vector3> JobPositions;
    public void Execute(int Index)
    {
        if (JobPositions[Index]!=null || JobPositions[Index] != Vector3.zero)
        {
            float Height = Mathf.PerlinNoise((float)Jobi[Index] / JobPerlinScaleX * JobScale / JobPerlinOffsetX, (float)Jobj[Index] / JobPerlinScaleY * JobScale / JobPerlinOffsetY);
            if (Jobi[Index] % 2 == 0)
            {
                JobPositions[Index] = (JobStartOffset + new Vector3(Jobi[Index] * Joboffset.x, Height * Joboffset.y, Jobj[Index] * Joboffset.z));

            }
            else
            {
                JobPositions[Index] = (new Vector3(Jobi[Index] * Joboffset.x, Height * Joboffset.y, Jobj[Index] * Joboffset.z));

            }
        }
       
    }
} */



