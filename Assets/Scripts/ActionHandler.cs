using UnityEngine;
using System.IO;
using System.Collections.Generic;
[System.Serializable]
public struct cardstats
{
    public int name;
    public int value;
    public int chance;
}
public class ActionHandler : MonoBehaviour
{
    Animator buttanimator;

    public GameObject atkbutton;
    public GameObject healbutton;
    public GameObject shieldbutton;
    public List<cardstats> cardtypes = new List<cardstats>();
    public string Actiontype;
    public string Actionchance;
    public string Actionvalue;
    void Start()
    {
        buttanimator = GameObject.Find("CastButtons").GetComponent<Animator>();
        SetRune("K","0","0");
    }
    void SetRune(string actcard, string chcard, string vcard)
    {
        Actionchance = actcard;
        Actionchance = chcard;
        Actionvalue = vcard;
        if (actcard == "0")
        {
            buttanimator.SetInteger("State", 0);
        } 

    }

    

}
