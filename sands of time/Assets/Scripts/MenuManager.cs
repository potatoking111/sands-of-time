using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject Menu;
    private bool isMenuActive = false;

    public static Action OnMenuOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        GameState.inputActions.UI.Enable();
        GameState.inputActions.UI.Menu.performed += OpenMenu;
    }
    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (OnMenuOpen != null)
        {
            OnMenuOpen.Invoke();
            OnMenuOpen = null;
        }
 

        isMenuActive = !isMenuActive;
        Menu.SetActive(isMenuActive);
        GameState.GameOn = !isMenuActive;
        if (GameState.GameOn)
        {
            GameState.inputActions.Player.Enable();
            GameState.inputActions.UI.Inventory.Enable();
        }
        else
        {
            GameState.inputActions.Player.Disable();
            GameState.inputActions.UI.Inventory.Disable();

        }
        Debug.Log("bazinga");
        Debug.Log(GameState.inputActions.Player.enabled);
    }
    public void OpenMenu()
    {
        OpenMenu(new InputAction.CallbackContext());
    }
    void OnDisable()
    {
        GameState.inputActions.UI.Disable();
        GameState.inputActions.UI.Menu.performed -= OpenMenu;
    }
    
}
