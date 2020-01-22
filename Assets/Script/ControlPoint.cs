using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ControlPoint : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI controlpointText;
    public List<string> PLayersOnPoint;
    string currentPlayeronpoint;
    TeamManager teamManager;
    [SerializeField]
    GameManager gameManager;
    PhotonView pv;
    float timer;
    PunTeams punTeams;
    // Start is called before the first frame update
    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        PLayersOnPoint = new List<string>();
        pv = GetComponent<PhotonView>();
        controlpointText.text = "noone on point";
        punTeams = FindObjectOfType<PunTeams>();
    }

    private void Update()
    {
        //timer += Time.deltaTime;
        //if (timer >= 1)
        //{
        //    if (controlpointText.text == "red team on point")
        //    {
        //        gameManager.AddProcentage(Team.team1, PLayersOnPoint.Count);
        //    }
        //    else if (controlpointText.text == "blue team on point")
        //    {
        //        gameManager.AddProcentage(Team.team2, PLayersOnPoint.Count);
        //    }
        //    timer = 0;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.root.GetComponent<PhotonView>().player
        CheckForControllPointFight();
    }

    void CheckForControllPointFight()
    {
        pv.RPC("RPC_ControlPointFight", RpcTarget.All);
    }

    [PunRPC]
    void RPC_ControlPointFight()
    {
        PLayersOnPoint.Add();
        //check for 0 players on point
        if (PLayersOnPoint.Count == 0)
        {
            PLayersOnPoint.Clear();
            controlpointText.text = "noone on point";
            return;
        }

        //check what team is on point
        bool redTeamOnPoint = false;
        bool blueTeamOnPoint = false;

        if (teamManager.team1names.Count != 0)
        {
            for (int i = 0; i < teamManager.team1names.Count; i++)
            {
                for (int players = 0; players < PLayersOnPoint.Count; players++)
                {
                    if (teamManager.team1names[i] == PLayersOnPoint[players])
                    {
                        redTeamOnPoint = true;
                    }
                }
            }
        }
        if (teamManager.team2names.Count != 0)
        {
            for (int i = 0; i < teamManager.team2names.Count; i++)
            {
                for (int players = 0; players < PLayersOnPoint.Count; players++)
                {
                    if (teamManager.team2names[i] == PLayersOnPoint[players])
                    {
                        blueTeamOnPoint = true;
                    }
                }
            }
        }

        //set the controlpoint text
        if (redTeamOnPoint == true && blueTeamOnPoint == true)
        {
            controlpointText.text = "contesting";
        }
        else if (redTeamOnPoint == true && blueTeamOnPoint == false)
        {
            controlpointText.text = "red team on point";
        }
        else if (redTeamOnPoint == false && blueTeamOnPoint == true)
        {
            controlpointText.text = "blue team on point";
        }
        else
        {
            Debug.LogError("invalid team on point");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PLayersOnPoint.Remove(other.transform.root.gameObject.GetComponent<PlayerManager>().playerName);
        CheckForControllPointFight();
    }
}
