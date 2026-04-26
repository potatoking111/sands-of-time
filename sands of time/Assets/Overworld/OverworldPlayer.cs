using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
public class OverworldPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 moveInput;
    private HashSet<Vector2> heldDirections = new HashSet<Vector2>();
    public PlayerVariables variables;
    public PlayerMovement playerMovementScript;
    private void Start()
    {

    }
    private void OnEnable()
    {
        playerMovementScript = gameObject.GetComponent<PlayerMovement>();
        variables = gameObject.GetComponent<PlayerVariables>();
        GameState.inputActions.Player.Enable();
        // MoveFacingSetup();
        GameState.inputActions.Player.Move.performed += OnMove;
        GameState.inputActions.Player.Move.canceled += OnMove;
        GameState.inputActions.UI.Inventory.performed += variables.playerInventoryScript.DisplayInventoryHelper;
        // inputActions.Player.Attack.performed += OnAttack;
        // inputActions.Player.Dash.performed += OnDash;


                // temp
        // inputActions.Player.Interact.performed += (context) => variables.meterManagerScript.FlipAction?.Invoke();
        //
    }



    private void OnDisable()
    {

        // inputActions.Player.Jump.performed -= (context)=>variables.playerMovementScript.JumpAction?.Invoke();
        // inputActions.Player.Attack.performed -= OnAttack;
        GameState.inputActions.Player.Move.performed -= OnMove;
        GameState.inputActions.Player.Move.canceled -= OnMove;
        // inputActions.Player.Dash.performed -= OnDash;

        GameState.inputActions.UI.Inventory.performed -= variables.playerInventoryScript.DisplayInventoryHelper;

        GameState.inputActions.Player.Disable();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        this.playerMovementScript.MoveAction?.Invoke(moveInput,-1,-1,-1);
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
        variables.playerFacing = moveInput.normalized;
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