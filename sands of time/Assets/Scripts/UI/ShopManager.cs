using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public PlayerVariables playerVariables;
    public bool canBuy = true;

    public Action OnBuyAction;
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
        if (playerVariables.money >= cost)
        {
            OnBuyAction += () => playerVariables.money -= cost;
            return;
        }
        else
        {
            canBuy = false;
        }
    }
    public void Buy(GameObject boughtObject)
    {
        if (canBuy)
        {
            OnBuyAction.Invoke();
            Debug.Log(boughtObject);  
        }
        else
        {
            Debug.Log("couldnt buy");
        }
        OnBuyAction = null;
        canBuy = true;
    }
}
