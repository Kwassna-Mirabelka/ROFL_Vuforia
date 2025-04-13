using UnityEngine;

public class PeriodicallyResetImageTrackers : MonoBehaviour
{
    public GameObject[] ImageTargets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        ImageTargets = GameObject.FindGameObjectsWithTag("ToResetSometimes");
      //  InvokeRepeating("ResetImageTargets", 1f, 2f);
    }

    // Update is called once per frame
    public void ResetImageTargets()
    {
        foreach (var target in ImageTargets)
        {
            if (target != null)
            {
                target.SetActive(false);
                target.SetActive(true);
            }
        }

    }
}
