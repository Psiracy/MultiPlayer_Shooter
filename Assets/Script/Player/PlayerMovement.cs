using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    norm,
    floaty
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviourPun
{
    Rigidbody rigidBody;
    float horizontalCamRotation;
    [SerializeField]
    float turningSpeed, movementSpeed, jumpPower;
    bool grounded;
    [SerializeField]
    MovementType movementType;
    [SerializeField]
    Transform feet;
    bool canMove;
    bool moveing;
    float groundRayLength;
    int layermask;
    void Awake()
    {
        if (!photonView.IsMine && GetComponent<PlayerMovement>() != null)
        {
            Destroy(GetComponent<PlayerMovement>());
        }
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canMove = true;
        layermask = 1 << 9;
        layermask = ~layermask;
        groundRayLength = .07f;

    }

    // Update is called once per frame
    void Update()
    {
        //player rotation
        horizontalCamRotation = Input.GetAxis("Mouse X") * turningSpeed;
        transform.rotation *= Quaternion.Euler(0, horizontalCamRotation, 0);
        //check for ground
        RaycastHit hit;
        if (Physics.Raycast(feet.position, (transform.up * -1), out hit, groundRayLength, layermask))
        {
            if (hit.collider)
            {
                Debug.Log(hit.collider.name);
            }
            if (hit.collider.tag == "ground")
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
        Debug.DrawRay(feet.position, (transform.up * -1) * groundRayLength);
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            grounded = false;
        }
    }


    void FixedUpdate()
    {
        if (canMove == false)
        {
            return;
        }
        //movement
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        if (movement != Vector3.zero)
        {
            moveing = true;
        }
        else
        {
            moveing = false;
        }
        Vector3 desiredMovement = (transform.TransformDirection(movement) * movementSpeed);
        Vector3 oldVelocity = rigidBody.velocity;
        Vector3 newVelocity = desiredMovement - oldVelocity;
        if (movementType == MovementType.norm)
        {
            rigidBody.AddForce(new Vector3(newVelocity.x, 0, newVelocity.z), ForceMode.Impulse);
        }
        else
        {
            rigidBody.AddForce(new Vector3(newVelocity.x, 0, newVelocity.z));
        }
    }

    public void CanMove(bool setMovement)
    {
        canMove = setMovement;
    }

    public bool IsMoveing()
    {
        return moveing;
    }
}
