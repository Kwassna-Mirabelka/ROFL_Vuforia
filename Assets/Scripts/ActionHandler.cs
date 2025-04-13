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
    public List<cardstats> cardtypes = new List<cardstats>();
    public string Actiontype;
    void ParseAction(string actcard, string chcard, string vcard)
    {
        
    }

}
