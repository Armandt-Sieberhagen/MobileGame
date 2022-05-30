using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public bool PointerDown
    {
        get;
        private set;
    }

    public void OnPointerDown()
    {
        PointerDown = true;
    }

    public void OnPointerUp()
    {
        PointerDown = false;
    }
}
