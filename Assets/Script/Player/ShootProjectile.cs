using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootProjectile : MonoBehaviourPun
{
    [SerializeField]
    Transform projectileSpawn, bombSpawn;
    [SerializeField]
    GameObject lazer;

    GameObject lazerGO;

    PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        lazer.SetActive(false);
    }

    void Shoot()
    {
        pv.RPC("RPC_SpawnBullet", RpcTarget.All);
    }

    void ThrowBomb()
    {
        pv.RPC("RPC_SpawnBomb", RpcTarget.All);
    }

    void StartLaser()
    {
        photonView.RPC("RPC_StartLaser", RpcTarget.All);
    }

    void StopLaser()
    {
        photonView.RPC("RPC_StopLaser", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SpawnBullet()
    {
        GameObject item = PhotonNetwork.Instantiate("ProjectileShuteye", projectileSpawn.position, Quaternion.identity);
        item.GetComponent<ShuteyeProjectile>().look = Camera.main.transform.forward;
        item.GetComponent<ShuteyeProjectile>().player = gameObject;
    }

    [PunRPC]
    void RPC_SpawnBomb()
    {
        PhotonNetwork.Instantiate("bomb", bombSpawn.position, Quaternion.identity);
    }

    [PunRPC]
    void RPC_StartLaser()
    {
        lazerGO = PhotonNetwork.Instantiate("Lazer", projectileSpawn.position, Quaternion.identity);
        lazerGO.GetComponent<Laser>().parent = projectileSpawn;
    }

    [PunRPC]
    void RPC_StopLaser()
    {
        PhotonNetwork.Destroy(lazerGO.GetComponent<PhotonView>());
    }
}
