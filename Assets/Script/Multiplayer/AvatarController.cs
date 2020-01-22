using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarController : MonoBehaviourPun
{
    Animator animator;
    public PhotonView pv;

    private void Awake()
    {
        if (photonView.IsMine == true)
        {
            Destroy(gameObject.transform.Find("holder").gameObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    public void Walking(bool isWalking)
    {
        animator.SetBool("Walking", isWalking);
    }

    public void Ablity(string abiltyname)
    {
        animator.SetTrigger(abiltyname);
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
}
