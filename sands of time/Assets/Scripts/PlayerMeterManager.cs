using System;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMeterManager : MonoBehaviour
{
    private PlayerVariables variables;
    public GameObject meterBar;
    private GameObject meterBarContainer;
    public Action FlipAction {get;set;}
    public Action<float> AddMeterAction {get;set;}
    private int timeDirection = 1; 
    private float initialMeterDisplaySize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
        FlipAction += Flip;
        AddMeterAction += AddMeter;
        meterBarContainer = meterBar.transform.parent.gameObject;

        initialMeterDisplaySize = meterBarContainer.GetComponent<RectTransform>().localScale.x;
        AddMeter(0);
    }
    public void Flip()
    {
        if (variables.meterAmount >= variables.meterFlipRequirement)
        {
            AddMeter(-variables.meterFlipRequirement);
            variables.timeHealth = variables.maxTimeHealth - variables.timeHealth;
            timeDirection *= -1;
        }



    }   
    public void AddMeter(float meterAddition)
    {
        variables.meterAmount += meterAddition;


        



         if (variables.meterAmount < 0)
        {
            variables.meterAmount = 0;
        }
        if (variables.meterAmount > variables.meterMaxAmount)
        {
            variables.meterAmount = variables.meterMaxAmount;
        }


        RectTransform meterBarContainerRect = meterBarContainer.GetComponent<RectTransform>();
        Vector3 newScale = meterBarContainerRect.localScale;
        newScale.x = initialMeterDisplaySize;
        meterBarContainerRect.localScale = newScale;


        meterBar.transform.localScale = new Vector3(variables.meterAmount/variables.meterMaxAmount,1,1);
        float size = meterBar.GetComponent<RectTransform>().sizeDelta.x;
        meterBar.transform.localPosition = new Vector3(-timeDirection*(size-variables.meterAmount/variables.meterMaxAmount*size)/2,meterBar.transform.localPosition.y,meterBar.transform.localPosition.z);

    }
    // Update is called once per frame
    void Update()
    {


    }
}
