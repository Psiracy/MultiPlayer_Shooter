using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNoBobing : MonoBehaviour
{
    [SerializeField]
    Transform sameCamPlace;
    [SerializeField]
    Animator animator;

    Vector3 startPos;
    private void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walking"))
        {
            Debug.Log("s");
            transform.position = sameCamPlace.position;
        }
        else
        {
            transform.localPosition = startPos;
        }
    }
}
