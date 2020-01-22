using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    norm,
    tank
}

public class PlayerManager : MonoBehaviour
{
    public float health;
    float maxHealth;
    public float deffence;

    float deathTimer;
    float deathTimerEnd;
    bool dead;

    public string playerName;

    PhotonView pv;
    [SerializeField]
    Type type;
    [SerializeField]
    Image icon, healthbar, healthbarBG, healthBarStripes;

    TeamManager teamManager;

    PhotonRoom room;
    Vector3 SpawnPos;

    private void Awake()
    {
        teamManager = GameObject.FindObjectOfType<TeamManager>();
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();
        //canvas
        if (pv.IsMine)
        {
            GameObject HealthbarGo = Instantiate(healthbar.gameObject);
            GameObject IconGo = Instantiate(icon.gameObject);
            GameObject HealthbarBGGo = Instantiate(healthbarBG.gameObject);
            GameObject HealthbarStripesGo = Instantiate(healthBarStripes.gameObject);

            HealthbarBGGo.transform.SetParent(GameObject.Find("Canvas").transform);
            HealthbarGo.transform.SetParent(GameObject.Find("Canvas").transform);
            HealthbarStripesGo.transform.SetParent(GameObject.Find("Canvas").transform);
            IconGo.transform.SetParent(GameObject.Find("Canvas").transform);

            healthbar = HealthbarGo.GetComponent<Image>();
        }
        //values
        deffence = 1;
        if (type == Type.norm)
        {
            maxHealth = 150;
        }
        else
        {
            maxHealth = 300;
        }
        health = maxHealth;
        SpawnPos = transform.position;
        deathTimerEnd = 5;

        playerName = PhotonNetwork.NickName;
    }

    void Update()
    {
        //death and respawn
        //death
        if (health <= 0)
        {
            //disable compontets expect for this component and the adiolisten, camera, animator photonview
            foreach (Behaviour component in GetComponentsInChildren<Behaviour>())
            {
                if (component.GetType() != typeof(PlayerManager))
                {
                    if (component.GetType() != typeof(Camera))
                    {
                        if (component.GetType() != typeof(AudioListener))
                        {
                            if (component.GetType() != typeof(PhotonView))
                            {
                                if (component.GetType() != typeof(Animator))
                                {
                                    component.enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            //disable the player
            if (pv.IsMine)
            {
                GetComponentInChildren<Camera>().transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            health = maxHealth;
            dead = true;
        }
        //respawn
        if (dead == true)
        {
            //timer for how long you need to stay dead
            deathTimer += Time.deltaTime;
            if (deathTimer >= deathTimerEnd)
            {
                //enable the player
                if (pv.IsMine)
                {
                    GetComponentInChildren<Camera>().transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
                //enable the components again
                foreach (Behaviour component in GetComponentsInChildren<Behaviour>())
                {
                    component.enabled = true;
                }
                //set back to spawn
                transform.position = SpawnPos;
                dead = false;
            }
        }

        //healthbar
        if (pv.IsMine)
        {
            healthbar.fillAmount = health / maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        /// health -= damage * deffence;
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        health -= damage * deffence;
    }

    public void Heal(float heals)
    {
        pv.RPC("RPC_Heal", RpcTarget.All, heals);
    }

    [PunRPC]
    void RPC_Heal(float heals)
    {
        if (heals + heals < maxHealth)
        {
            health += heals;
        }
        else
        {
            health = maxHealth;
        }
    }

    public void SetTeam(Team team)
    {
        pv.RPC("RPC_SetTeam", RpcTarget.All, team);
    }

    void RPC_SetTeam(Team team)
    {
        switch (team)
        {
            case Team.team1:
                gameObject.layer = LayerMask.GetMask("Team1");
                break;
            case Team.team2:
                gameObject.layer = LayerMask.GetMask("Team2");
                break;
            default:
                break;
        }
    }
}
