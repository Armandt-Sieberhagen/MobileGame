
using UnityEngine;

[CreateAssetMenu(fileName = "HexValues", menuName = "HexValues")]
public class HexValues : ScriptableObject
{
    public Material DarkSide;
    public Material LightSide;
    public float HexScale = 1;
    public GameObject HexTile;
    public Vector3 offset;
    public Vector3 StartOffset;
    public int GridX = 2000;
    public int GridY = 2000;
    public float DrawDistance = 25;
    public int LoadValue = 5;
    [Header("LandScape")]
    [Range(0, 50)]
    [SerializeField] public float PerlinScaleX = 1;
    [Range(0, 50)]
    [SerializeField] public float PerlinScaleZ = 1;
    [Range(0, 25)]
    [SerializeField] public float Scale = 1;
    [Range(0, 25)]
    [SerializeField] public float PerlinOffsetX = 1;
    [Range(0, 25)]
    [SerializeField] public float PerlinOffsetY = 1;
    [Range(0, 25)]
    [SerializeField] public float MountanRanges = 1;
    [Range(0, 25)]
    [SerializeField] public float WaterRanges = 1;
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

}
