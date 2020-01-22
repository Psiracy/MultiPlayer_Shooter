using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class TeamManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    [SerializeField]
    LayerMask team1layer, team2layer;
    [SerializeField]
    TMPro.TextMeshProUGUI team1text, team2text;
    PunTeams punTeams;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        team1text.text = team2text.text = string.Empty;
        punTeams = GetComponent<PunTeams>();
    }


    public void AddPlayersToTeam(Player player)
    {
        Debug.Log(PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count);
        Debug.Log(PunTeams.PlayersPerTeam[PunTeams.Team.red].Count);
        if (PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count < PunTeams.PlayersPerTeam[PunTeams.Team.red].Count)
        {
            Debug.Log("blue");
            TeamExtensions.SetTeam(player, PunTeams.Team.blue);
        }
        else if (PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count >= PunTeams.PlayersPerTeam[PunTeams.Team.red].Count)
        {
            Debug.Log("red");
            TeamExtensions.SetTeam(player, PunTeams.Team.red);
        }
        punTeams.UpdateTeams();
        // pv.RPC("RPC_AddPlayersToTeam", RpcTarget.All, playername);
    }

    public void UpdateBoard()
    {
        pv.RPC("RPC_UpdateBoard", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UpdateBoard()
    {
        team1text.text = team2text.text = string.Empty;

        for (int i = 0; i < PunTeams.PlayersPerTeam[PunTeams.Team.red].Count; i++)
        {
            team1text.text += PunTeams.PlayersPerTeam[PunTeams.Team.red][i].NickName + "\n";
        }

        for (int i = 0; i < PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count; i++)
        {
            team2text.text += PunTeams.PlayersPerTeam[PunTeams.Team.blue][i].NickName + "\n";
        }
    }

    //[PunRPC]
    //void RPC_AddPlayersToTeam(string playername)
    //{
    //    bool exists = false;
    //    teams[currentTeam].Add(playername);
    //    if (currentTeam == Team.team1)
    //    {
    //        for (int i = 0; i < team1names.Count; i++)
    //        {
    //            if (team1names[i] == playername)
    //            {
    //                exists = true;
    //            }
    //        }
    //        if (exists == false)
    //        {
    //            team1names.Add(playername);
    //            team1text.text += playername + "\n";
    //            currentTeam = Team.team2;
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < team2names.Count; i++)
    //        {
    //            if (team2names[i] == playername)
    //            {
    //                exists = true;
    //            }
    //        }
    //        if (exists == false)
    //        {
    //            team2names.Add(playername);
    //            team2text.text += playername + "\n";
    //            currentTeam = Team.team1;
    //        }
    //    }
    //}

    ////add to team1
    //public void AddPlayerToTeam1(string playerToAdd)
    //{
    //    pv.RPC("RPC_AddPlayerToTeam1", RpcTarget.All, playerToAdd);
    //}

    //[PunRPC]
    //void RPC_AddPlayerToTeam1(string playername)
    //{
    //    team1names.Add(playername);
    //    team1text.text += playername + "\n";
    //}

    ////add to team2
    //public void AddPlayerToTeam2(string playerToAdd)
    //{
    //    pv.RPC("RPC_AddPlayerToTeam2", RpcTarget.All, playerToAdd);
    //}

    //[PunRPC]
    //void RPC_AddPlayerToTeam2(string playername)
    //{
    //    team2names.Add(playername);
    //    team2text.text += playername + "\n";
    //}
}
