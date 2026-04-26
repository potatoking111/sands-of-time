using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerVariables variables;
    private Vector2 moveInput;
    private HashSet<Vector2> heldDirections = new HashSet<Vector2>();
    private void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();

    }
    private void OnEnable()
    {

        GameState.inputActions.Player.Enable();
        MoveFacingSetup();
        GameState.inputActions.Player.Move.performed += OnMove;
        GameState.inputActions.Player.Move.canceled += OnMove;

        GameState.inputActions.Player.Jump.performed += (context)=>variables.playerMovementScript.JumpAction?.Invoke();
        GameState.inputActions.Player.Attack.performed += OnAttack;
        GameState.inputActions.Player.Dash.performed += OnDash;


                // temp
        GameState.inputActions.Player.Interact.performed += (context) => variables.meterManagerScript.FlipAction?.Invoke();
        //
    }

private void OnMoveUp(InputAction.CallbackContext _) => OnMoveEditFace(Vector2.up);
private void OnMoveDown(InputAction.CallbackContext _) => OnMoveEditFace(Vector2.down);
private void OnMoveLeft(InputAction.CallbackContext _) => OnMoveEditFace(Vector2.left);
private void OnMoveRight(InputAction.CallbackContext _) => OnMoveEditFace(Vector2.right);

private void OnMoveUpCanceled(InputAction.CallbackContext _) => OnFaceKeyRelease(Vector2.up);
private void OnMoveDownCanceled(InputAction.CallbackContext _) => OnFaceKeyRelease(Vector2.down);
private void OnMoveLeftCanceled(InputAction.CallbackContext _) => OnFaceKeyRelease(Vector2.left);
private void OnMoveRightCanceled(InputAction.CallbackContext _) => OnFaceKeyRelease(Vector2.right);

private void MoveFacingSetup()
{
    GameState.inputActions.Player.MoveUp.performed += OnMoveUp;
    GameState.inputActions.Player.MoveDown.performed += OnMoveDown;
    GameState.inputActions.Player.MoveLeft.performed += OnMoveLeft;
    GameState.inputActions.Player.MoveRight.performed += OnMoveRight;

    GameState.inputActions.Player.MoveUp.canceled += OnMoveUpCanceled;
    GameState.inputActions.Player.MoveDown.canceled += OnMoveDownCanceled;
    GameState.inputActions.Player.MoveLeft.canceled += OnMoveLeftCanceled;
    GameState.inputActions.Player.MoveRight.canceled += OnMoveRightCanceled;
}

private void MoveFacingDeSetup()
{
    GameState.inputActions.Player.MoveUp.performed -= OnMoveUp;
    GameState.inputActions.Player.MoveDown.performed -= OnMoveDown;
    GameState.inputActions.Player.MoveLeft.performed -= OnMoveLeft;
    GameState.inputActions.Player.MoveRight.performed -= OnMoveRight;

    GameState.inputActions.Player.MoveUp.canceled -= OnMoveUpCanceled;
    GameState.inputActions.Player.MoveDown.canceled -= OnMoveDownCanceled;
    GameState.inputActions.Player.MoveLeft.canceled -= OnMoveLeftCanceled;
    GameState.inputActions.Player.MoveRight.canceled -= OnMoveRightCanceled;
}

    private void OnDisable()
    {

        GameState.inputActions.Player.Jump.performed -= (context)=>variables.playerMovementScript.JumpAction?.Invoke();
        GameState.inputActions.Player.Attack.performed -= OnAttack;
        GameState.inputActions.Player.Move.performed -= OnMove;
        GameState.inputActions.Player.Move.canceled -= OnMove;
        GameState.inputActions.Player.Dash.performed -= OnDash;

        MoveFacingDeSetup();

        // GameState.inputActions.Disable();
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("gameState" + GameState.GameOn);
        moveInput.y = 0;
        variables.playerMovementScript.MoveAction?.Invoke(moveInput,-1,-1,-1);
    }
    private void OnMoveEditFace(Vector2 dir)
    {
        variables.playerFacing = dir;
        heldDirections.Add(dir);
    }
    private void OnFaceKeyRelease(Vector2 dir)
    {
        heldDirections.Remove(dir);

        if (heldDirections.Count > 0)
        {
            // Revert to the most recently pressed key still held
            variables.playerFacing = heldDirections.Last<Vector2>();
        }
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        variables.playerAttackScript.attackAction.Invoke();
    }
     private void OnDash(InputAction.CallbackContext context)
    {
        variables.playerMovementScript.TryDash();
    }
}
