using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSetUp : MonoBehaviour
{
    PhotonView pv;
    public GameObject avatar;
    public string characterValue;
    string prefabname;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
      //  prefabname = GameObject.Find("Player").GetComponent<CharacterSpawn>().selectedChar;
        if (pv.IsMine)
        {
            SetAvatar(prefabname);
            Destroy(avatar.transform.Find("holder").gameObject);
        }
        else
        {
            avatar = GameObject.Find(characterValue);
        }
    }

    //[PunRPC]
    void SetAvatar(string charname)
    {
        avatar = PhotonNetwork.Instantiate(charname, transform.position, transform.rotation);
        avatar.transform.parent = transform;
        avatar.transform.localRotation = Quaternion.Euler(0, 90, 0);
        avatar.transform.localScale = new Vector3(20, 20, 20);
        Debug.Log(avatar.transform.parent.name + "/" + avatar.name);
    }
}
