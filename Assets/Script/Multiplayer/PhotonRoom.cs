using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public enum Team
{
    team1,
    team2
}

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    static PhotonRoom room;

    PhotonView pv;

    int maxPlayers;

    public bool isGameLoaded;
    public int currentScene;

    [SerializeField]
    TeamManager teamManager;
    Player[] currentPlayers;
    public int numberOfPlayersInRoom;
    public int currentRoomPositionNumber;

    public int numberOfPlayesInGame;
    int numberofReadyPlayers;

    bool readyToStartGame;

    float notAtMaxPlayers;
    float atMaxPLayers;
    float timeToStart;
    public float startingTime;

    public string thisPlayerFPS;
    private void Awake()
    {
        //singelton
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += SceneFinishLoading;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= SceneFinishLoading;
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        readyToStartGame = false;
        isGameLoaded = false;
        notAtMaxPlayers = startingTime;
        maxPlayers = 12;
        atMaxPLayers = maxPlayers;
        timeToStart = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        //reset values if only player
        //if (numberOfPlayersInRoom == 1)
        //{
        //    ResetTimer();
        //}

        //start game if conditions are met
        if (!isGameLoaded)
        {
            if (readyToStartGame == true && numberofReadyPlayers == maxPlayers)
            {
                atMaxPLayers -= Time.deltaTime;
                notAtMaxPlayers = atMaxPLayers;
                timeToStart = atMaxPLayers;
            }
            Debug.Log("time to start " + timeToStart);

            if (timeToStart <= 0)
            {
                StartGame();
                Debug.Log("wree");
            }
        }

        Debug.Log("ready players:" + numberofReadyPlayers);
    }

    void ResetTimer()
    {
        notAtMaxPlayers = 0;
        timeToStart = startingTime;
        atMaxPLayers = 6;
        readyToStartGame = false;
    }

    //start game
    void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(3);
    }

    void SceneFinishLoading(Scene scene, LoadSceneMode loadMode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == 3)
        {
            isGameLoaded = true;
            pv.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
        }
    }
    [PunRPC]
    void RPC_LoadedGameScene()
    {
        numberOfPlayesInGame++;
        if (numberOfPlayesInGame == PhotonNetwork.PlayerList.Length)
        {
            pv.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_CreatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(thisPlayerFPS, GameObject.Find("SpawnPoint" + currentRoomPositionNumber).transform.position, Quaternion.identity);
    }

    //ready up player
    public void AddReadyPlayer()
    {
        teamManager.AddPlayersToTeam(PhotonNetwork.LocalPlayer);
        teamManager.UpdateBoard();
        pv.RPC("RPC_PlayerReady", RpcTarget.All);
    }

    [PunRPC]
    void RPC_PlayerReady()
    {
        numberofReadyPlayers++;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        teamManager.UpdateBoard();
        Debug.Log(string.Format("master: {0} - room name: {1}", PhotonNetwork.IsMasterClient, PhotonNetwork.CurrentRoom.Name));
        SetPlayers(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        SetPlayers(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    void SetPlayers(bool joined)
    {
        currentPlayers = PhotonNetwork.PlayerList;
        numberOfPlayersInRoom = currentPlayers.Length;
        if (joined == true)
        {
            currentRoomPositionNumber = numberOfPlayersInRoom;
        }
        Debug.Log(string.Format("there are {0} players out of {1} players in the current room", numberOfPlayersInRoom, maxPlayers));

        //close the room when full
        if (numberOfPlayersInRoom == maxPlayers)
        {
            readyToStartGame = true;
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public void ForceStartGame()
    {
        pv.RPC("RPC_ForceStartGame", RpcTarget.All);
    }

    [PunRPC]
    void RPC_ForceStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        readyToStartGame = true;
        StartGame();
    }
}
