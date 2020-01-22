using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piloteer : MonoBehaviourPun
{
    [SerializeField]
    GameObject leftMuzzleFlash, rightMuzzleFlash;
    [SerializeField]
    TMPro.TextMeshPro ammoTXT;
    [SerializeField]
    HitScanShoot shoot;
    [SerializeField]
    Image dashImage, jumpImage;
    [SerializeField]
    Animator animator;
    bool left;

    PlayerMovement movement;
    Rigidbody rigidBody;

    float shootTimer;
    float shootTimerEnd;
    bool timerStart;

    int currentAmmo;
    int maxAmmo;

    float timer;
    float reloadtime;

    float dashStrength;
    float jumpStrength;

    float cooldowndashTimer;
    float dashTimerEnd;
    bool canUseDash;

    float cooldownJumpTimer;
    float jumpTimerEnd;
    bool canUseJump;

    float dashMovementStopTimer;
    float dashMovementStopEnd;
    bool starTimer;

    bool once;
    void Awake()
    {
        if (!photonView.IsMine && GetComponent<Piloteer>() != null)
        {
            Destroy(GetComponent<Piloteer>());
        }
    }

    void Start()
    {
        //canvas image
        #region CanvasLogic
        GameObject dashImageGo = Instantiate(dashImage.gameObject);
        GameObject jumpImageGo = Instantiate(jumpImage.gameObject);

        dashImageGo.transform.parent = GameObject.Find("Canvas").transform;
        jumpImageGo.transform.parent = GameObject.Find("Canvas").transform;

        dashImageGo.transform.localPosition = new Vector3(0, dashImageGo.transform.localPosition.y, 0);
        jumpImageGo.transform.localPosition = new Vector3(0, jumpImageGo.transform.localPosition.y, 0);

        dashImage = dashImageGo.GetComponent<Image>();
        jumpImage = jumpImageGo.GetComponent<Image>();
        #endregion
        maxAmmo = 30;
        //values
        currentAmmo = maxAmmo;
        reloadtime = .8f;
        dashStrength = 30;
        jumpStrength = 30;

        jumpTimerEnd = 6;
        dashTimerEnd = 4;
        dashMovementStopEnd = .8f;
        canUseJump = true;
        canUseDash = true;

        ammoTXT.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        rigidBody = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //shooting
        if (Input.GetButton("Fire1") && currentAmmo > 0)
        {
            if (shootTimer == 0)
            {
                if (left == true)
                {
                    leftMuzzleFlash.SetActive(true);
                    rightMuzzleFlash.SetActive(false);
                    left = false;
                }
                else
                {
                    rightMuzzleFlash.SetActive(true);
                    leftMuzzleFlash.SetActive(false);
                    left = true;
                }

                currentAmmo--;
                shoot.Shoot();
                timerStart = true;
            }

            if (timerStart == true)
            {
                shootTimer += Time.deltaTime;

                if (shootTimer >= .1f)
                {
                    shootTimer = 0;
                    timerStart = false;
                }
            }
        }
        else
        {
            rightMuzzleFlash.SetActive(false);
            leftMuzzleFlash.SetActive(false);
        }
        //reloading
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = 0;
        }
        if (currentAmmo <= 0)
        {
            if (once == false)
            {
                animator.SetTrigger("Reload");
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
            ammoTXT.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        }
        else
        {
            ammoTXT.text = "reloading";
        }

        //dash
        if (Input.GetKeyDown(KeyCode.Q) && canUseDash == true)
        {
            animator.SetTrigger("Dash");
            rigidBody.AddForce(transform.forward * dashStrength, ForceMode.Impulse);
            movement.CanMove(false);
            starTimer = true;
            canUseDash = false;
        }

        if (starTimer == true)
        {
            dashMovementStopTimer += Time.deltaTime;
            if (dashMovementStopTimer >= dashMovementStopEnd)
            {
                movement.CanMove(true);
                starTimer = false;
            }
        }

        if (canUseDash == false)
        {
            movement.CanMove(true);
            dashImage.color = new Color(1, 1, 1, .6f);
            cooldowndashTimer += Time.deltaTime;
            if (cooldowndashTimer >= dashTimerEnd)
            {
                canUseDash = true;
                cooldowndashTimer = 0;
            }
        }
        else
        {
            dashImage.color = new Color(1, 1, 1, 1);
        }

        //super jump
        if (Input.GetKeyDown(KeyCode.E) && canUseJump == true)
        {
            animator.SetTrigger("Jump");
            rigidBody.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
            canUseJump = false;
        }

        if (canUseJump == false)
        {
            jumpImage.color = new Color(1, 1, 1, .6f);
            cooldownJumpTimer += Time.deltaTime;
            if (cooldownJumpTimer >= jumpTimerEnd)
            {
                canUseJump = true;
                cooldownJumpTimer = 0;
            }
        }
        else
        {
            jumpImage.color = new Color(1, 1, 1, 1);
        }

        //animation
        if (movement.IsMoveing() == true)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
