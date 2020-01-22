using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvaterRotationSetter : MonoBehaviourPun
{
    GameObject cam;
    void Awake()
    {
        if (!photonView.IsMine && GetComponent<AvaterRotationSetter>() != null)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        cam = Camera.main.gameObject;
    }

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, cam.transform.eulerAngles.x * -1, 0);
    }
}
