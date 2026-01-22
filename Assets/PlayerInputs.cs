using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public InputSystem_Actions InputActions { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public float JumpInput { get; private set; }

    private double MaxTimeToJump = 0.5;
    double timeHeld = 0;


    private void OnEnable()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();

        InputActions.Player.Enable();
        InputActions.Player.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        InputActions.Player.RemoveCallbacks(this);
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
       
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
        if (context.canceled)
        {
           

        }
     
    }
}