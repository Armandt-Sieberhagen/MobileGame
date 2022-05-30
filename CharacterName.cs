using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterName : MonoBehaviour
{
    public void SetName(string Name)
    {
       
        this.GetComponent<TextMeshProUGUI>().text = Name;
    }
}
