using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public GameObject playerPanel;
    int playerCount = 2;
    public List<PlayerStats> players = new List<PlayerStats>();
    void Start()
    {
        playerCount = GameObject.Find("GameInfo").GetComponent<GameInfo>().playerCount;
        float y = 5;
        for (int i = 0; i < playerCount; i++)
        {
            GameObject obj = Instantiate(playerPanel, Vector3.zero, Quaternion.identity);
            obj.transform.position = new Vector3(0,y,0);
            obj.transform.parent = transform.parent;
            obj.transform.localScale = Vector3.one;
            var stats = obj.GetComponent<PlayerStats>();
            stats.playername = "Player " + i.ToString();
            stats.maxhp = 50;
            stats.hp = 50;
            players.Add(stats);
            y-= 2.5f;
        }
            
    }

    // Update is called once per frame
    void Update()
    {

    }
}
