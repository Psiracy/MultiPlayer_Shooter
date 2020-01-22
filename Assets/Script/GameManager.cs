using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float redTeamCapturePercentage, blueTeamCapturePercentage;
    [SerializeField]
    Image redTeamProgress, blueTeamProgress;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        redTeamProgress.fillAmount = Mathf.MoveTowards(redTeamProgress.fillAmount, redTeamCapturePercentage / 100, 1);
        blueTeamProgress.fillAmount = Mathf.MoveTowards(blueTeamProgress.fillAmount, blueTeamCapturePercentage / 100, 1);
    }

    public void AddProcentage(Team team, int players)
    {
        switch (team)
        {
            case Team.team1:
                redTeamCapturePercentage += 1 * players;
                break;
            case Team.team2:
                blueTeamCapturePercentage += 1 * players;
                break;
            default:
                break;
        }
    }
}
