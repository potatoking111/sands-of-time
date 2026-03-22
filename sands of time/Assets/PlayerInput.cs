using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private InputSystem_Actions inputActions;
    private PlayerVariables variables;
    private Vector2 moveInput;

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
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove; // important!
        inputActions.Player.Jump.performed += (context)=>variables.playerMovementScript.JumpAction?.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= (context)=>variables.playerMovementScript.JumpAction?.Invoke();

        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        variables.playerMovementScript.MoveAction?.Invoke(moveInput);
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
