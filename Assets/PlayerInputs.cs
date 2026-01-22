using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public InputSystem_Actions InputActions { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public float JumpInput { get; private set; }

    private double MaxTimeToJump = 0.5;
    private Rigidbody rb;
    double timeHeld = 0;
    private float jumpForce = 10f;
    //private bool isJumping = false;
    //private float jumpTimeCounter = 0;
    //private float extraJumpForce = 10f;


    private void OnEnable()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();

        InputActions.Player.Enable();
        InputActions.Player.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();

        InputActions.Player.Jump.performed += JumpPerformed;
        InputActions.Player.Jump.performed += JumpCanceled;
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        InputActions.Player.RemoveCallbacks(this);
    }
    private void Start()
    {
        
    }



    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
      
        MoveInput = context.ReadValue<Vector2>();
    }

  


    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        Debug.Log("piupiu");
    }


    public void JumpPerformed(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector3.up * 15f, ForceMode.Impulse);
        Debug.Log("salto");
    }
    public void JumpCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("release space");

    }
    public void OnJump(InputAction.CallbackContext context)
    {


        
        JumpInput = context.ReadValue<float>();

        /*
         // press
       if (context.started)
       {
           rb.AddForce(new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z));
           isJumping = true;
           jumpTimeCounter = (float)MaxTimeToJump;
       }

       // hold
       if (context.performed && isJumping)
       {
           if (jumpTimeCounter > 0)
           {
               rb.AddForce(Vector3.up * extraJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
               jumpTimeCounter -= Time.fixedDeltaTime;
           }
       }

       //release
       if (context.canceled)
       {
           isJumping = false;
       }
        */

    }
}