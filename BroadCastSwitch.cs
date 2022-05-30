using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastSwitch : MonoBehaviour
{
    public SwitchLeve level;
    public void CallSwitch(int layer)
    {
        Debug.Log("Registered Click " + layer );
        level.FadeToLevel(layer);
    }
}
