using UnityEngine;
using UnityEngine.SceneManagement;
public class GameInfo : MonoBehaviour
{
    public int playerCount;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetGameScene(int players)
    {
        playerCount = players;
        SceneManager.LoadScene(2);
    }
}
