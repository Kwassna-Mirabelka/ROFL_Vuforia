using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
[System.Serializable]
public class ActionHandler : MonoBehaviour
{
    public GameObject atkbutton;
    public GameObject healbutton;
    public GameObject shieldbutton;
    public int Actionchance;
    public int Actionvalue;
    void Start()
    {

    }
    public void SetRune(char actcard, char chcard, char vcard)
    {
        Actionchance = Int32.Parse(chcard+"")*10;
        Actionvalue = Int32.Parse(vcard+"");
        //atkbutton.SetActive(false);
        //healbutton.SetActive(false);
        //shieldbutton.SetActive(false);
        if (actcard == '0')
        {

        } 
        else if(actcard == 'A')
        {
            atkbutton.SetActive(true);
            healbutton.SetActive(true);
            shieldbutton.SetActive(true);
        }
        else
        {
            if(actcard == 'K') atkbutton.SetActive(true);
            if(actcard == 'J') shieldbutton.SetActive(true);
            if(actcard == 'Q') healbutton.SetActive(true);
        }

    }

    

}
