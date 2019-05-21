using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using Api;

public class RollText : MonoBehaviour
{
    public string id = "RollText";

    float t = 0;
    Client client;
    private GComponent mainUI;
    GComponent roll;
    GRichTextField text;
    // Start is called before the first frame update
    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        roll = mainUI.GetChild("rollText").asCom;
        text = roll.GetChild("text").asRichTextField;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = text.position;
        if (text.text != "")
        {
            text.SetPosition(pos.x - 1, pos.y, pos.z);
        }
        
        if (pos.x < -text.width)
        {
            text.SetPosition(roll.width, pos.y, pos.z);
        }

        t += Time.deltaTime;
        if (t>2)
        {
            t = 0;
            text.text = PlayerPrefs.GetString(id);
        }
    }
}
