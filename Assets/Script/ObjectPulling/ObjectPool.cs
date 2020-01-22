using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject PooledItem;
    public int PoolSize;

    List<Poolitem> PooledItems;
    PhotonView pv;

    void Start()
    {
        PooledItems = new List<Poolitem>();
        pv = GetComponent<PhotonView>();
        pv.RPC("RPC_StartPool", RpcTarget.All);
        //for (int i = 0; i < PoolSize; i++)
        //{
        //    GameObject item = Instantiate(PooledItem);
        //    AddItem(item.GetComponent<Poolitem>());
        //    PooledItems[i].ObjectPool = this;
        //}
    }

    public void AddItem(Poolitem item)
    {
        //PooledItems.Add(item);
        //item.transform.parent = transform;
        //item.gameObject.SetActive(false);
        pv.RPC("RPC_AddItem", RpcTarget.All, item);
    }

    public GameObject InitItem(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (PooledItems.Count <= 0)
        {
            Debug.LogError("No items in pool");
            return null;
        }

        PooledItems[0].Spawn(position, rotation, parent);
        GameObject item = PooledItems[0].gameObject;
        PooledItems.RemoveAt(0);
        return item;
    }

    public GameObject InitItem(Vector3 position, Quaternion rotation, Vector3 look,GameObject player, Transform parent = null)
    {
        if (PooledItems.Count <= 0)
        {
            Debug.LogError("No items in pool");
            return null;
        }

        PooledItems[0].Spawn(position, rotation, parent);
        GameObject item = PooledItems[0].gameObject;
        item.GetComponent<ShuteyeProjectile>().look = look;
        item.GetComponent<ShuteyeProjectile>().player = player;
        PooledItems.RemoveAt(0);
        return item;
    }

    [PunRPC]
    void RPC_StartPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject item = PhotonNetwork.Instantiate("ProjectileShuteye", Vector3.zero , Quaternion.identity);
            AddItem(item.GetComponent<Poolitem>());
            PooledItems[i].ObjectPool = this;
        }
    }

    [PunRPC]
    void RPC_AddItem(Poolitem item)
    {
        PooledItems.Add(item);
        item.transform.parent = transform;
        item.gameObject.SetActive(false);
    }
}
