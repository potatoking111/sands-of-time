using UnityEngine;

public class GiveUp : MonoBehaviour
{
    public PlayerVariables playerVariables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GiveUpGame()
    {
        playerVariables.timeHealth = -1;
        FindAnyObjectByType<MenuManager>().OpenMenu();
    }
}
