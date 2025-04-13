using UnityEngine;

public class RuneScript : MonoBehaviour
{
    Vector3 startposition;
    float offset;

    Vector3 dir;
    void Start()
    {
        startposition = transform.localPosition;
        dir.x = Random.Range(-0.1f,0.1f);
        dir.y = 0.5f;
        dir.z = Random.Range(-0.1f,0.1f);
        offset = Random.Range(0,1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = startposition + dir*Mathf.Sin(Time.time*1.3f + offset * 6.28f)*0.05f;
    }
}
