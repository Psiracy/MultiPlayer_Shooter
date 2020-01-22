using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ramos : MonoBehaviourPun
{
    float maxLength = 50;
    [SerializeField]
    Shader norm, glow;
    [SerializeField]
    MinoinManager minoinManager;
    [SerializeField]
    Renderer hand;
    [SerializeField]
    LineRenderer handParticals;
    [SerializeField]
    PlayerMovement movement;
    [SerializeField]
    GameObject healAera;
    [SerializeField]
    Image minionimage, healimage;

    GameObject lockedPlayer = null;

    float attackTimer;
    float attackTimerEnd;

    float healtimer;
    float healDurration;
    bool isChanting;

    float minoinCooldownTimer;
    float minoinCooldownTimerEnd;
    bool canUseMinoins;

    float healCooldownTimer;
    float healCooldownTimerEnd;
    bool canuseHeal;

    float healPower;

    void Awake()
    {
        if (!photonView.IsMine && GetComponent<Ramos>() != null)
        {
            Destroy(GetComponent<Ramos>());
        }
    }

    void Start()
    {
        //canvas image
        GameObject minionImageGo = Instantiate(minionimage.gameObject);
        GameObject healImageGo = Instantiate(healimage.gameObject);

        minionImageGo.transform.parent = GameObject.Find("Canvas").transform;
        healImageGo.transform.parent = GameObject.Find("Canvas").transform;

        minionimage = minionImageGo.GetComponent<Image>();
        healimage = healImageGo.GetComponent<Image>();

        //values
        attackTimerEnd = .4f;
        norm = GetComponent<Renderer>().material.shader;
        healAera.SetActive(false);
        healDurration = .6f;
        healPower = 80;

        healCooldownTimerEnd = 10;
        minoinCooldownTimerEnd = 12;
        canUseMinoins = true;
        canuseHeal = true;
    }

    // Update is called once per frame
    void Update()
    {
        //damage beam
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength))
            {
                if (hit.transform.tag == "Player")
                {
                    lockedPlayer = hit.transform.gameObject;
                    hit.transform.GetComponent<Renderer>().material.shader = glow;
                    hand.material.shader = glow;
                    handParticals.enabled = true;
                }
            }
        }

        if (lockedPlayer != null)
        {
            if (!Input.GetButton("Fire1") || lockedPlayer.GetComponent<Renderer>().isVisible == false)
            {
                attackTimer = 0;
                lockedPlayer.GetComponent<Renderer>().material.shader = norm;
                hand.material.shader = norm;
                handParticals.enabled = false;
                lockedPlayer = null;
            }
            else
            {
                handParticals.SetPosition(0, handParticals.transform.position);
                handParticals.SetPosition(1, lockedPlayer.transform.position);
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackTimerEnd)
                {
                    lockedPlayer.transform.GetComponent<PlayerManager>().TakeDamage(5);
                    attackTimer = 0;
                }
            }
        }
        else
        {
            attackTimer = 0;
            hand.material.shader = norm;
            handParticals.enabled = false;
            lockedPlayer = null;
        }

        //minions
        if (Input.GetKeyDown(KeyCode.Q) && canUseMinoins == true)
        {
            minoinManager.MakeMinoins();
            canUseMinoins = false;
        }

        if (canUseMinoins == false)
        {
            minionimage.color = new Color(1, 1, 1, .6f);
            minoinCooldownTimer += Time.deltaTime;
            if (minoinCooldownTimer >= minoinCooldownTimerEnd)
            {
                minoinCooldownTimer = 0;
                canUseMinoins = true;
            }
        }
        else
        {
            minionimage.color = new Color(1, 1, 1, 1);
        }

        if (minoinManager.GetMinoinsCount() != 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength))
                {
                    for (int i = 0; i < minoinManager.GetMinoinsCount(); i++)
                    {
                        minoinManager.GetMinoins()[i].target = hit.transform.gameObject;
                    }
                }
            }
        }

        //heals
        if (Input.GetKeyDown(KeyCode.E) && canuseHeal == true)
        {
            isChanting = true;
            healAera.SetActive(true);
        }

        if (isChanting == true)
        {
            movement.CanMove(false);
            healtimer += Time.deltaTime;
            if (healtimer >= healDurration)
            {
                Collider[] allies = Physics.OverlapSphere(transform.position, 11);
                for (int i = 0; i < allies.Length; i++)
                {
                    if (allies[i].tag == "Player")
                    {
                        Debug.Log(allies[i].name);
                        allies[i].GetComponent<PlayerManager>().Heal(healPower);
                    }
                }
                healtimer = 0;
                healAera.SetActive(false);
                movement.CanMove(true);
                canuseHeal = false;
                isChanting = false;
            }
        }

        if (canuseHeal == false)
        {
            healimage.color = new Color(1, 1, 1, .6f);
            healCooldownTimer += Time.deltaTime;
            if (healCooldownTimer >= healCooldownTimerEnd)
            {
                healCooldownTimer = 0;
                canuseHeal = true;
            }
        }
        else
        {
            healimage.color = new Color(1, 1, 1, 1);
        }

    }
}
