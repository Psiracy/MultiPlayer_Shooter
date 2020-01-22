using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameStartUp : MonoBehaviour
{
    [SerializeField]
    NetworkConectionManager network;
    [SerializeField]
    InputField inputUsername;
    [SerializeField]
    GameObject button;

    string username;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inputUsername.text != string.Empty)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
        username = inputUsername.text;

        if (network.isConnnectedToServer == true)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Connect()
    {
        network.ConnectToServer(username);
    }
}
