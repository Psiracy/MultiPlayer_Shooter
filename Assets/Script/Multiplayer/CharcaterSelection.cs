using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CharcaterSelection : MonoBehaviour
{
    [SerializeField]
    GameObject shuteye, piloteer, necromancer;
    [SerializeField]
    PhotonRoom room;
    [SerializeField]
    GameObject playButton;

    PhotonView pv;
    string selectedChar;
    NetworkConectionManager network;

    private void Start()
    {
        network = GameObject.Find("connect").GetComponent<NetworkConectionManager>();
        pv = GetComponent<PhotonView>();
    }

    public void ShutEye()
    {
        selectedChar = "ShuteyeAvatar";
        Spawn(shuteye, "shuteye");
    }

    public void Piloteer()
    {
        selectedChar = "Arthur";
        Spawn(piloteer, "piloteer");
    }

    public void Ramos()
    {
        selectedChar = "Ramos";
        Spawn(necromancer, "necromancer");
    }

    public void ReadyUp()
    {
        room.AddReadyPlayer();
        playButton.SetActive(false);
    }

    void Spawn(GameObject player, string character)
    {
        GameObject characterSpawn = new GameObject();
        characterSpawn.AddComponent<CharacterSpawn>();
     //   characterSpawn.GetComponent<CharacterSpawn>().selectedChar = selectedChar;
        characterSpawn.name = "Player";
        DontDestroyOnLoad(characterSpawn);
        room.thisPlayerFPS = character;
    }
}
