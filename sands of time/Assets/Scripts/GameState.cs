using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool GameOn = true;
    public static InputSystem_Actions inputActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
}
