using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= .6f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerManager>().TakeDamage(50);
        }
    }
}
