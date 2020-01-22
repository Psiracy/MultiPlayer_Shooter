using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minoin : MonoBehaviour
{
    public GameObject player;
    public GameObject target;
    bool attacking;

    float attackTimer;
    float attackTimeEnd;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        target = player;
        attackTimeEnd = 2;
        speed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 1.4f && attacking == false && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), speed * Time.deltaTime);
        }
        else
        {
            if (target != player && target != null)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackTimeEnd)
                {
                    target.GetComponent<PlayerManager>().TakeDamage(3);
                    attackTimer = 0;
                }
            }
        }
    }
}
