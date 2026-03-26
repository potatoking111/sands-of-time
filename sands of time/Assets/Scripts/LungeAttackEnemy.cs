using System;
using NUnit.Framework;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

public class LungeAttackEnemy : MonoBehaviour
{
    private EnemyVariables variables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject attackObject;
    public float attackDuration;
    public Action startLungeAction;
    
    public float jumpForceMod;
    public float attackRange;

    public float attackWaitTime;
    public bool isLunging = false;
    public bool isBackingUp = false;
    [Range(0, 90)]
    public float lungeAngle;
    void Start()
    {
        variables = gameObject.GetComponent<EnemyVariables>();
        startLungeAction += StartLunge;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(variables.detectedPlayerDistance.magnitude) <= attackRange && !isLunging && !isBackingUp)
        {
            isBackingUp = true;
            variables.chaseScript.pauseMovement = true;
            Invoke("StartLunge", attackWaitTime);
        }
        if (isBackingUp)
        {
            variables.chaseScript.MoveAction.Invoke(-variables.walkSpeed);
        }
    }
    void StartLunge()
    {
        isBackingUp = false;
        isLunging = true;
        variables.isCharging = false;
        variables.chaseScript.pauseMovement = true;
        attackObject.SetActive(true);
        Vector2 jumpDirection = (variables.facing* Mathf.Cos(lungeAngle * Mathf.Deg2Rad) + Vector2.up * Mathf.Sin(lungeAngle * Mathf.Deg2Rad)).normalized;
        Vector2 newAttackPos = new Vector2(Math.Abs(attackObject.transform.localPosition.x)* variables.facing.x,attackObject.transform.localPosition.y);
        attackObject.transform.localPosition = newAttackPos;

        variables.rigidBody.AddForce(jumpDirection * variables.runSpeed * jumpForceMod, ForceMode2D.Impulse);
        Invoke("EndLunge", attackDuration);
    }
    void EndLunge()
    {
        isLunging = false;
        variables.chaseScript.pauseMovement = false;
        attackObject.SetActive(false);


        // set facing correctly
        // not workin lowkey
        variables.chaseScript.FacePlayerAction.Invoke();
    }
}
