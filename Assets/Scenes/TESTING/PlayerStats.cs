using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI nametext;
    public TextMeshProUGUI hptext;
    public string playername;
    public Slider slider;    
    public float hp = 5;
    public float maxhp;
    void Start()
    {
        nametext.text = playername;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = hp/maxhp;
        transform.position = new Vector3(transform.position.x,transform.position.y,800);
        hptext.text = hp.ToString() + '/' + maxhp.ToString();
    }
}
