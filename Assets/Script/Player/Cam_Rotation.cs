using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cam_Rotation : MonoBehaviourPun
{
    float verticalCamRotation;
    float xRotation;
    [SerializeField]
    float turningSpeed;

    void Awake()
    {
        if (!photonView.IsMine && GetComponent<Cam_Rotation>() != null)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //cam rotation
        verticalCamRotation = Input.GetAxis("Mouse Y") * (turningSpeed / 2);
        verticalCamRotation *= -1;
        xRotation += verticalCamRotation;
        Clamp(ref xRotation, -70, 70);
        Vector3 rot = new Vector3(xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(rot);
    }

    void Clamp(ref float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
    }
}
