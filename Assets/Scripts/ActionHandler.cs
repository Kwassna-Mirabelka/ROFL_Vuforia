using UnityEngine;
using System.IO;
using System.Collections.Generic;
[System.Serializable]
public class ActionHandler : MonoBehaviour
{
    Animator buttanimator;

    public GameObject atkbutton;
    public GameObject healbutton;
    public GameObject shieldbutton;
    public char Actionchance;
    public char Actionvalue;
    void Start()
    {
        buttanimator = GameObject.Find("CastButtons").GetComponent<Animator>();
    }
    public void SetRune(char actcard, char chcard, char vcard)
    {
        Actionchance = int.Parse(chcard)*10;
        Actionvalue = int.Parse(vcard);
        if (actcard == '0')
        {
            buttanimator.SetInteger("State", 0);
        } 
        else if(actcard == 'A')
        {
            buttanimator.SetInteger("State", 2);
            atkbutton.SetActive(true);
            healbutton.SetActive(true);
            shieldbutton.SetActive(true);
        }
        else
        {
            buttanimator.SetInteger("State", 1);
            atkbutton.SetActive(false);
            healbutton.SetActive(false);
            shieldbutton.SetActive(false);
            if(actcard == 'K') atkbutton.SetActive(true);
            if(actcard == 'J') shieldbutton.SetActive(true);
            if(actcard == 'Q') healbutton.SetActive(true);
        }

    }

    

}
