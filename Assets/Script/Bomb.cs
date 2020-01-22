using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Rigidbody rigidBody;
    Vector3 direction;
    public GameObject player;
    [SerializeField]
    GameObject explosion;

    float aliveTimer;
    float aliveTimerEnd;
    // Start is called before the first frame update
    void Start()
    {
        aliveTimerEnd = 4;
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.Find("shuteye(Clone)");
        direction = (player.transform.forward * 2) + Vector3.up;
        transform.rotation = Random.rotation;
        rigidBody.AddForce(direction * 6, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        aliveTimer += Time.deltaTime;
        if (aliveTimer >= aliveTimerEnd)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            aliveTimer = aliveTimerEnd;
        }
    }
}
