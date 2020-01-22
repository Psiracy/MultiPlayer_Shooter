using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shuteye : MonoBehaviourPun
{
    [SerializeField]
    ObjectPool pool;
    [SerializeField]
    Transform projectileSpawn, bombSpawn;
    [SerializeField]
    TMPro.TextMeshPro ammo;
    [SerializeField]
    GameObject bomb, lazer;
    [SerializeField]
    Image bombimage, thatotherablitieimage, ult;
    [SerializeField]
    Sprite ulton, ultoff;
    [SerializeField]
    Animator animator;


    AvatarSetUp avatarSetUp;
    PhotonView pv;
    PlayerMovement movement;
    GameObject spawnedbomb;
    AvatarController avatarController;

    int maxAmmo;
    int currentAmmo;

    float timer;
    float reloadtime;

    float shootTimer;
    float shootTimerEnd;
    bool shot;

    float cooldownTimerBomb;
    float timerBombEnd;
    bool canUseBomb;

    float toughenupTimer;
    float toughenupEnd;

    float ultDuration;
    float ultEndingTime;
    bool isUlting;

    float cooldownToughenUp;
    float cooldownEndToughenUp;
    bool canUseThoughenUp;
    bool ToughenUpStart;
    bool startCooldown;
    bool once;

    void Awake()
    {
        if (!photonView.IsMine && GetComponent<Shuteye>() != null)
        {
            Destroy(GetComponent<Shuteye>());
        }
    }

    void Start()
    {
        //canvas image      
        #region Canvaslogic
        GameObject BombImageGo = Instantiate(bombimage.gameObject);
        GameObject ThougenImageGo = Instantiate(thatotherablitieimage.gameObject);
        GameObject UltImageGo = Instantiate(ult.gameObject);

        BombImageGo.transform.SetParent(GameObject.Find("Canvas").transform);
        ThougenImageGo.transform.SetParent(GameObject.Find("Canvas").transform);
        UltImageGo.transform.SetParent(GameObject.Find("Canvas").transform);

        BombImageGo.transform.localPosition = new Vector3(0, BombImageGo.transform.localPosition.y, 0);
        ThougenImageGo.transform.localPosition = new Vector3(0, BombImageGo.transform.localPosition.y, 0);
        UltImageGo.transform.localPosition = new Vector3(0, BombImageGo.transform.localPosition.y, 0);

        bombimage = BombImageGo.GetComponent<Image>();
        thatotherablitieimage = ThougenImageGo.GetComponent<Image>();
        ult = UltImageGo.GetComponent<Image>();
        #endregion
        //values
        maxAmmo = 6;
        currentAmmo = maxAmmo;
        reloadtime = 1.45f;
        shootTimerEnd = .55f;
        timerBombEnd = 8;
        toughenupEnd = 3;
        ultEndingTime = 3.5f;
        cooldownEndToughenUp = 6;
        canUseBomb = true;
        canUseThoughenUp = true;
        pv = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();
        avatarSetUp = GetComponent<AvatarSetUp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && avatarController == null)
        {
            avatarController = gameObject.transform.GetComponentInChildren<AvatarController>();
        }

        //shooting
        if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && shot == false)
        {
            animator.SetTrigger("Shoot");
            avatarController.Shoot();
            currentAmmo--;
            shot = true;
        }

        if (shot == true)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTimerEnd)
            {
                shot = false;
                shootTimer = 0;
            }
        }

        //reloading
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = 0;
        }
        if (currentAmmo <= 0 && shot == false)
        {
            if (once == false)
            {
                animator.SetTrigger("Reload");
                avatarController.Reload();
                once = true;
            }
            timer += Time.deltaTime;
            if (timer >= reloadtime)
            {
                once = false;
                currentAmmo = maxAmmo;
                timer = 0;
            }
        }

        if (currentAmmo != 0)
        {
            ammo.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        }
        else
        {
            ammo.text = "reloading";
        }

        //pulse bomb
        if (Input.GetKeyDown(KeyCode.Q) && canUseBomb == true)
        {
            if (pv.IsMine)
            {
                animator.SetTrigger("Grenade");
                avatarController.Ablity("Grenade");
            }
            canUseBomb = false;
        }

        if (canUseBomb == false)
        {
            bombimage.color = new Color(1, 1, 1, .6f);
            cooldownTimerBomb += Time.deltaTime;
            if (cooldownTimerBomb >= timerBombEnd)
            {
                cooldownTimerBomb = 0;
                canUseBomb = true;
            }
        }
        else
        {
            bombimage.color = new Color(1, 1, 1, 1);
        }

        //toughenup
        if (Input.GetKeyDown(KeyCode.E) && canUseThoughenUp == true)
        {
            if (ToughenUpStart == false)
            {
                animator.SetTrigger("Tough");
                avatarController.Ablity("Tough");
            }
            GetComponent<PlayerManager>().deffence = .5f;
            ToughenUpStart = true;
        }

        if (ToughenUpStart == true)
        {
            toughenupTimer += Time.deltaTime;
            if (toughenupTimer >= toughenupEnd)
            {
                GetComponent<PlayerManager>().deffence = 1;
                toughenupTimer = 0;
                ToughenUpStart = false;
                canUseThoughenUp = false;
                startCooldown = true;
            }
        }

        if (startCooldown == true)
        {
            thatotherablitieimage.color = new Color(1, 1, 1, .6f);
            cooldownToughenUp += Time.deltaTime;
            if (cooldownToughenUp >= cooldownEndToughenUp)
            {
                cooldownToughenUp = 0;
                canUseThoughenUp = true;
                startCooldown = false;
            }
        }
        else
        {
            thatotherablitieimage.color = new Color(1, 1, 1, 1);
        }

        //ult 
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("Ult", true);
            isUlting = true;
        }

        if (isUlting == true)
        {
            ultDuration += Time.deltaTime;
            if (ultDuration >= ultEndingTime)
            {
                animator.SetBool("Ult", false);
                isUlting = false;
                ultDuration = 0;
            }
        }

        //animations
        if (movement.IsMoveing() == true)
        {
            animator.SetBool("Walking", true);
            avatarController.Walking(true);
        }
        else
        {
            animator.SetBool("Walking", false);
            avatarController.Walking(false);
        }
    }
}
