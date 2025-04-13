using UnityEngine;
using System.Collections.Generic;
public class CanvasManager : MonoBehaviour
{
    public List<GameObject> playerboards;
    public GameObject bossbar;
    public PlayerStats bossstats;
    public List<PlayerStats> stats;
    public GameManager2s gameman;
    void Start()
    {
        foreach(var board in playerboards) board.SetActive(false);

        for (int i = 0; i < gameman.playersNum; i++)
        {
            playerboards[i].SetActive(true);
            stats.Add(playerboards[i].GetComponent<PlayerStats>());
            stats[i].maxhp = gameman.maxPlayerHP;
            stats[i].hp = stats[i].maxhp;
        }

        bossstats = bossbar.GetComponent<PlayerStats>();
        bossbar.SetActive(false);
        ActivateBoss();
    }

    void ActivateBoss()
    {
        bossbar.SetActive(true);
        bossstats.maxhp = gameman.initialBossHP;
        bossstats.hp = bossstats.maxhp;
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < gameman.playersNum; i++)
            stats[i].hp = gameman.playerHPs[i];
        bossstats.hp = gameman.BossHP;
    }
}
