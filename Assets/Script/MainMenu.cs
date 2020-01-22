using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    NetworkConectionManager network;
    // Start is called before the first frame update
    void Start()
    {
        network = GameObject.Find("connect").GetComponent<NetworkConectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinRoom()
    {
        network.ConnecttToRoom();
        SceneManager.LoadScene(2);
    }
}
