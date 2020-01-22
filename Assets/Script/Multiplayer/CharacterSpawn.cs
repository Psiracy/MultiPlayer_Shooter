using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CharacterSpawn : MonoBehaviour
{
    static CharacterSpawn spawn;
    public GameObject player;
    //public string selectedChar;
    bool game = true;
    private void Start()
    {
        //singelton
        if (CharacterSpawn.spawn == null)
        {
            CharacterSpawn.spawn = this;
        }
        else
        {
            if (CharacterSpawn.spawn != this)
            {
                Destroy(CharacterSpawn.spawn.gameObject);
                CharacterSpawn.spawn = this;
            }
        }
    }
    private void Update()
    {
        if (game == false)
        {
            return;
        }
    }
}
