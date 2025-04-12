using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    Transform cam;
    void Start()
    {
        cam = GameObject.Find("ARCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = (transform.position - cam.position);
        vec = new Vector3(vec.x, 0, vec.z);
        transform.right = vec;
        transform.Rotate(-90,0,0);
        
    }
}
