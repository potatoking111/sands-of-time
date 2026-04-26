using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryScript : MonoBehaviour
{

    public List<GameObject> inventory;

    public List<GameObject> equipment;
    public GameObject inventoryMenu;

    public GameObject itemsContainer;
    public RectTransform size;
    public float xCellSize = 15;
    public int cellsPerRow = 10;

    public bool isInventoryOpen = false;


    public GameObject selectedItem;

    public UnityEvent EquipEvent;

    public UnityEvent UnEquipEvent;

    public GameObject equipmentContainer;

    public GameObject ItemToEquip;
    public GameObject ItemToUnEquip;
    public GameObject lastClicked;
    public float lastClickTime = 0;
    public float doubleClickDelay = 100;

    

    public void ItemSelectEvent(GameObject clicked)
{
    // detect double click
        bool isDoubleClick =
            clicked == lastClicked &&
            (Time.time - lastClickTime) <= doubleClickDelay;

        lastClicked = clicked;
        lastClickTime = Time.time;

        // first click does nothing (optional: you could highlight here)
        if (!isDoubleClick)
            return;

        selectedItem = clicked;

        ItemScript itemScript = selectedItem.GetComponent<ItemScript>();
        if (itemScript == null || itemScript.original == null)
            return;

        ItemScript originalItemScript = itemScript.original.GetComponent<ItemScript>();
        if (originalItemScript == null)
            return;

        // EQUIP
        if (!originalItemScript.isEquipped)
        {
            ItemToEquip = selectedItem;
            EquipEvent?.Invoke();

            originalItemScript.isEquipped = true;
        }
        // UNEQUIP
        else
        {
            ItemToUnEquip = selectedItem;
            UnEquipEvent?.Invoke();

            originalItemScript.isEquipped = false;
        }

        // cleanup
        selectedItem = null;
        ItemToEquip = null;
        ItemToUnEquip = null;
        DisplayInventory(false);
}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (size != null)
        {
            xCellSize = size.rect.width;
  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInEquipmentSlots()
    {
        GameObject originalItem = selectedItem.GetComponent<ItemScript>().original;

        inventory.Remove(originalItem);
        equipment.Add(originalItem);

        // EquipEvent.Invoke();

        selectedItem = null;
    }
    public void DisplayInInventorySlots()
    {
        GameObject originalItem = selectedItem.GetComponent<ItemScript>().original;

        equipment.Remove(originalItem);
        inventory.Add(originalItem);

        // UnEquipEvent.Invoke();

        selectedItem = null;
    }



    public void DisplayInventoryHelper(InputAction.CallbackContext context)
    {
        DisplayInventory();
    }
    public static void DisplayInventoryHelper()
    {
    }
    public void DisplayInventory(bool switchMenus = true)
    {


        Debug.Log("tryna open inventory");
        if (switchMenus)
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryMenu.SetActive(isInventoryOpen);
        }


        foreach (Transform child in itemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in equipmentContainer.transform)
        {
            Destroy(child.gameObject);
        }

        if (isInventoryOpen)
        {
            MenuManager.OnMenuOpen += DisplayInventoryHelper;


            GameState.GameOn = false;
            GameState.inputActions.Player.Disable();

        }
        else
        {
            MenuManager.OnMenuOpen -= DisplayInventoryHelper;
            GameState.GameOn = true;
            GameState.inputActions.Player.Enable();


        }

        foreach (GameObject inventoryObject in inventory)
        {
            ItemScript itemScript = inventoryObject.GetComponent<ItemScript>();
            if (itemScript != null)
            {
                itemScript.isEquipped = false;

                itemScript.Display(this,itemsContainer);
            }
        }
        foreach (GameObject inventoryObject in equipment)
        {
            ItemScript itemScript = inventoryObject.GetComponent<ItemScript>();
            if (itemScript != null)
            {
                itemScript.isEquipped = true;
                itemScript.Display(this,equipmentContainer);
            
            }
        }
    }


}
