using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShuteyeProjectile : MonoBehaviourPun
{
    float projectileSpeed;
    float projectileDamage;
    float timer;
    float aliveTime;
    public GameObject player;
    public Vector3 look;
    // Start is called before the first frame update
    void Start()
    {
        aliveTime = 2.5f;
        projectileSpeed = 30;
        projectileDamage = 20;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (look * projectileSpeed) * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer > aliveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject != player.gameObject)
        {
            other.GetComponentInParent<PlayerManager>().TakeDamage(projectileDamage);
        }

        PhotonNetwork.Destroy(photonView);
    }
}
