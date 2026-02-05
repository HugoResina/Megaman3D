using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public InputSystem_Actions InputActions { get; private set; }
    //public PlayerShoot playerShoot;
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public float JumpInput { get; private set; }
    public int InteractInput { get; private set; }

    private double MaxTimeToJump = 0.5;
    private Rigidbody rb;
    double timeHeld = 0;
    private float jumpForce = 10f;
    public bool isJumpHeld = false;
    public static event Action HasInteracted;

    private Shooter Shooter;
    //private bool isJumping = false;
    //private float jumpTimeCounter = 0;
    //private float extraJumpForce = 10f;


    private void OnEnable()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();
        Shooter = GetComponent<Shooter>();

        InputActions.Player.Enable();
        InputActions.Player.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();

        InputActions.Player.Jump.started += JumpPerformed;
        InputActions.Player.Jump.canceled += JumpCanceled;
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
        if (context.performed)
        {
            //Debug.Log("Attack Performed");
            // Debug.Log(context.duration);
        }
        if (context.canceled)
        {
            //Debug.Log(context.duration);
            //Debug.Log(context.time);
            //Debug.Log(context.startTime);

            //playerShoot.ChooseProj(context.time - context.startTime);
            Shooter.ChooseProj(context.time - context.startTime);
        }
    }


    public void JumpPerformed(InputAction.CallbackContext context)
    {
        //timeHeld = context.duration;
        //Debug.Log("performed");        

        //JumpInput = 0;


    }
    public void JumpCanceled(InputAction.CallbackContext context)
    {
        //Debug.Log("canceledS");
        //JumpInput = 0;



    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = 1f;
            isJumpHeld = true;
        }

        if (context.canceled)
        {
            JumpInput = 0f;
            isJumpHeld = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HasInteracted?.Invoke();
        }
    }
}