using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Laser : MonoBehaviourPun
{
    LineRenderer line;
    public Transform parent;
    public Vector3 look;
    float lazerDamage;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = parent;
        line.SetPosition(0, transform.position);
        RaycastHit hit;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            line.SetPosition(1, hit.point);
            if (hit.collider.tag == "Player")
            {
                hit.transform.GetComponent<PlayerManager>().TakeDamage(1.5f);
            }
        }
        else
        {
            line.SetPosition(1, transform.forward * 1000);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("wree");
    }
}
