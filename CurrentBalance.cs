using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentBalance : MonoBehaviour
{
    public PlayerInfo Info;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Info.CachedCoins.ToString();
    }
}
