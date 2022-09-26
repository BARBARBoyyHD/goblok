using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    public float gravityPower = -9.18f;
    public float jumpValue = 0;
    private bool cursorLoced;
    private Vector3 relativeVector;
    [HideInInspector]public float gravityForce = -9.18f;

    [Range(1,4)]public float movementSpeed = 2;
    [Range(0, .5f)] public float groundClearance;
    [Range(0, 1f)] public float groundDistance;

    [HideInInspector] public Vector3 motionVector,gravityVector;
    public GameObject focus;
    private float turnDirection;
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        movement();
        mouseLook();

    }
    void mouseLook()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) cursorLoced = cursorLoced ? false : true;
        if (!cursorLoced) return;

        Cursor.lockState = cursorLoced ? CursorLockMode.Locked : CursorLockMode.None;

        relativeVector = transform.InverseTransformPoint(focus.transform.position);
        relativeVector /= relativeVector.magnitude;
        turnDirection = (relativeVector.x / relativeVector.magnitude);

        // vertical
        focus.transform.eulerAngles = new Vector3(focus.transform.eulerAngles.x + Input.GetAxis("Mouse Y"), focus.transform.eulerAngles.y, 0);
        // horizontal
        focus.transform.parent.Rotate(transform.up * Input.GetAxis("Mouse X")*100*Time.deltaTime);

    }
    public float turnMultiplier;
    void movement()
    {
        animator.SetFloat("vertical", inputManager.vertical);
        animator.SetFloat("horizontal", inputManager.horizontal);
        animator.SetBool("grounded", isGrounded());
        animator.SetFloat("jump", inputManager.jump);

        if (isGrounded()&& gravityVector.y < 0)
        {
            gravityVector.y = -2;
        }

        gravityVector.y += gravityPower * Time.deltaTime;
        characterController.Move(gravityVector*Time.deltaTime);
        if (isGrounded())
        {
            motionVector = transform.right * inputManager.horizontal + transform.forward * inputManager.vertical;
            if(inputManager.vertical > 0)
            {
                characterController.Move(motionVector * movementSpeed * Time.deltaTime);
                transform.Rotate(transform.up * turnDirection * turnMultiplier *Time.deltaTime);
                focus.transform.parent.Rotate(transform.up * -turnDirection * turnMultiplier * Time.deltaTime);
            }
            
        }
        if(inputManager.jump != 0)
        {
            jump();
        }

        
    }
    void jump()
    {
        if (isGrounded())
        {
            characterController.Move(transform.up * (jumpValue * -2 * gravityForce) * Time.deltaTime);
        }
        
    }
    bool isGrounded()
    {
        return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y -groundDistance, transform.position.z),groundClearance);
    }
  void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z), groundClearance);
    }



}
