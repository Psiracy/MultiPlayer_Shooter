using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkConectionManager : MonoBehaviourPunCallbacks
{
    public bool isConnnectedToServer;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ConnectToServer(string playername)
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = playername;
        PhotonNetwork.GameVersion = "alpha 0.0v";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ConnecttToRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("connected");
        isConnnectedToServer = true;
    }
}
