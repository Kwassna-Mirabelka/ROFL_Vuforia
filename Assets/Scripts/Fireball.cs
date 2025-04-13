using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject explosionvfx;
    public string bossname;
    public float startimpulse;
    public float speed;
    Transform target;
    public Rigidbody rb;
    float timealive = 0;
    void Start()
    {
        target = GameObject.Find(bossname).transform;
        rb.AddForce(new Vector3(0,startimpulse,0),ForceMode.Impulse);
    }

    
    void Update()
    {
        rb.linearVelocity += speed * Time.deltaTime * timealive * (target.position - transform.position).normalized;
        timealive += Time.deltaTime;
        if (timealive > 2 || (target.position - transform.position).sqrMagnitude < 0.1)
        {
            Instantiate(explosionvfx,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }            
    }
}
