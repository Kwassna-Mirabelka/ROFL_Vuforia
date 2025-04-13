using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public Vector3 dir;
    void Update()
    {
        var vec = dir * Time.deltaTime;
        transform.Rotate(dir.x,dir.y,dir.z);
    }
}
