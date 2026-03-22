using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputSystem_Actions inputActions;
    private PlayerVariables variables;
    private Vector2 moveInput;
    private HashSet<Vector2> heldDirections = new HashSet<Vector2>();
    private void Start()
    {
        inputActions = new InputSystem_Actions();
        variables = gameObject.GetComponent<PlayerVariables>();

    }
    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new InputSystem_Actions();
        }
        inputActions.Enable();
        MoveFacingSetup();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += (context)=>variables.playerMovementScript.JumpAction?.Invoke();
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void MoveFacingSetup()
    {
        inputActions.Player.MoveUp.performed += _ =>OnMoveEditFace(Vector2.up);
        inputActions.Player.MoveDown.performed +=_ => OnMoveEditFace(Vector2.down);
        inputActions.Player.MoveLeft.performed += _ =>OnMoveEditFace(Vector2.left);
        inputActions.Player.MoveRight.performed += _ => OnMoveEditFace(Vector2.right);


        inputActions.Player.MoveUp.canceled += _ => OnFaceKeyRelease(Vector2.up);
        inputActions.Player.MoveDown.canceled += _ => OnFaceKeyRelease(Vector2.down);
        inputActions.Player.MoveLeft.canceled += _ => OnFaceKeyRelease(Vector2.left);
        inputActions.Player.MoveRight.canceled += _ => OnFaceKeyRelease(Vector2.right);
    }

    private void OnDisable()
    {

        inputActions.Player.Jump.performed -= (context)=>variables.playerMovementScript.JumpAction?.Invoke();
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Disable();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        variables.playerMovementScript.MoveAction?.Invoke(moveInput);
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
}
