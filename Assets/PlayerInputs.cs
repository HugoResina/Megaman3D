// PlayerInputs.cs
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public InputSystem_Actions InputActions { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float JumpInput { get; private set; }
    public bool isJumpHeld = false;

    public static event Action HasInteracted;

    private Shooter Shooter;
    private Rigidbody rb;

    private Coroutine chargeCoroutine;
    private const float MAX_CHARGE_TIME = 3.27f;

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
        if (context.started)
        {
            AudioManager.instance.PlaySFX("Charging");
            chargeCoroutine = StartCoroutine(AutoFireRoutine());
        }

        if (context.canceled)
        {
            if (chargeCoroutine != null)
            {
                StopCoroutine(chargeCoroutine);
                chargeCoroutine = null;
                ExecuteShoot(context.time - context.startTime);
            }
        }
    }

    private IEnumerator AutoFireRoutine()
    {
        yield return new WaitForSeconds(MAX_CHARGE_TIME);
        chargeCoroutine = null;
        ExecuteShoot(MAX_CHARGE_TIME);
    }

    private void ExecuteShoot(double duration)
    {
        Shooter.ChooseProj(duration);
        AudioManager.instance.StopSFX("Charging");
    }

    public void JumpPerformed(InputAction.CallbackContext context) { }
    public void JumpCanceled(InputAction.CallbackContext context) { }

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
            HasInteracted?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (MenuManager.Instance.IsPaused)
                MenuManager.Instance.ResumeGame();
            else
                MenuManager.Instance.PauseGame();
        }
    }
}