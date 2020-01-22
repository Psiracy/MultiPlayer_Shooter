using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitScanShoot : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float maxLength;
    PhotonView pv;

    private void Update()
    {
        pv = GetComponent<PhotonView>();
    }


    public void Shoot()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxLength);
        for (int hit = 0; hit < hits.Length; hit++)
        {
            if (hits[hit].transform.tag == "Player" && hits[hit].transform.GetComponent<PhotonView>().IsMine == false)
            {
                hits[hit].transform.GetComponent<PlayerManager>().TakeDamage(damage);
            }
        }
    }
}
