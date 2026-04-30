using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public PlayerVariables playerVariables;
    public bool canBuy = true;

    public Action OnBuyAction;
    public List<int> stock;

    public List<GameObject> items;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopOpen()
    {
        OnBuyAction = null;
    }
    public void CheckEnoughMoney(float cost)
    {
        Debug.Log("player has money + to "+ playerVariables.money);
        if (playerVariables.money >= cost)
        {
            
            OnBuyAction += () => playerVariables.money -= cost;
            return;
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY");
            canBuy = false;
        }
    }
    public void CheckIfInStock(GameObject item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (item == items[i])
            {
                if (stock[i] > 0)
                {
                    OnBuyAction += () => stock[i] -= 1;
                    return;
                }
                else
                {
                    Debug.Log("NOT ENOUGH STOCK");
                    canBuy = false; 
                    return;
                }


            }
        }

    }
    public void Buy(GameObject boughtObject)
    {
        if (canBuy)
        {
            OnBuyAction.Invoke();
            Debug.Log(boughtObject);  
            playerVariables.playerInventoryScript.AddItemAction.Invoke(boughtObject);
        }
        else
        {
            Debug.Log("couldnt buy");
        }
        OnBuyAction = null;
        canBuy = true;
    }
}
