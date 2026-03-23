using System;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTimeManager : MonoBehaviour
{
    private PlayerVariables variables;
    public GameObject healthBar;
    private GameObject healthBarContainer;
    public Action FlipAction {get;set;}
    public Action<float> TakeDamageAction {get;set;}
    private int timeDirection = 1; 
    private float initialTimeDisplaySize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
        FlipAction += Flip;
        TakeDamageAction += TakeDamage;
        healthBarContainer = healthBar.transform.parent.gameObject;

        initialTimeDisplaySize = healthBarContainer.GetComponent<RectTransform>().localScale.x;
    }
    public void Flip()
    {
        variables.timeHealth = variables.maxTimeHealth - variables.timeHealth;
        timeDirection *= -1;
    }   
    public void TakeDamage(float damageAmount)
    {
        variables.maxTimeHealth -= damageAmount;
        if (variables.maxTimeHealth < 0)
        {
            variables.maxTimeHealth = 0;
        }
        if (variables.timeHealth > variables.maxTimeHealth)
        {
            variables.timeHealth = variables.maxTimeHealth;
        }
        RectTransform healthBarContainerRect = healthBarContainer.GetComponent<RectTransform>();
        Vector3 newScale = healthBarContainerRect.localScale;
        newScale.x = initialTimeDisplaySize * (variables.maxTimeHealth / variables.initialMaxTimeHealth);
        healthBarContainerRect.localScale = newScale;
    }
    // Update is called once per frame
    void Update()
    {
        variables.timeHealth -= Time.deltaTime;
        if (variables.timeHealth <= 0)
        {
            variables.timeHealth = 0;
        }
        healthBar.transform.localScale = new Vector3(variables.timeHealth/variables.maxTimeHealth,1,1);
        float size = healthBar.GetComponent<RectTransform>().sizeDelta.x;
        healthBar.transform.localPosition = new Vector3(-timeDirection*(size-variables.timeHealth/variables.maxTimeHealth*size)/2,healthBar.transform.localPosition.y,healthBar.transform.localPosition.z);
    }
}
