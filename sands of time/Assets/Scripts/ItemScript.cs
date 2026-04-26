using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public bool isEquipped = false;

    public GameObject original;
    public string itemId;
    public PlayerInventoryScript playerInventoryScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SelectItem);
        
    }
    public void SelectItem()
    {
        playerInventoryScript.ItemSelectEvent(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Display(PlayerInventoryScript playerInventory,GameObject container)
    {
        playerInventoryScript = playerInventory;
        GameObject itemObject = Instantiate(this.original, container.transform);
        if (original == null)
        {
            itemObject.GetComponent<ItemScript>().original = this.gameObject;
        }
        else
        {
            itemObject.GetComponent<ItemScript>().original = this.original;

        }

    }
}
