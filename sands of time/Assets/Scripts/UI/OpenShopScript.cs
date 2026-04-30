using UnityEngine;

public class OpenShopScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isShopOpen;
    public GameObject shopMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayShopHelper()
    {
        DisplayShop();
    }
        public void DisplayShop(bool switchMenus = true)
    {


        Debug.Log("tryna open inventory");
        if (switchMenus)
        {
            isShopOpen = !isShopOpen;
            shopMenu.SetActive(isShopOpen);
        }



        if (isShopOpen)
        {
            MenuManager.OnMenuOpen += DisplayShopHelper;


            GameState.GameOn = false;
            GameState.inputActions.Player.Disable();
            GameState.inputActions.UI.Inventory.Disable();

        }
        else
        {
            MenuManager.OnMenuOpen -= DisplayShopHelper;
            GameState.GameOn = true;
            GameState.inputActions.Player.Enable();
            GameState.inputActions.UI.Inventory.Enable();


        }}
}
