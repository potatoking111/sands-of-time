using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyScript : MonoBehaviour
{
    public List<UnityEvent> requirements;
    public UnityEvent buyEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TryPurchase()
    {
        foreach (UnityEvent req in requirements)
        {
            req.Invoke();
        }
        buyEvent.Invoke();
    }
}
