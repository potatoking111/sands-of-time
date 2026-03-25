using System;
using UnityEngine;

public class LungeAttackEnemy : MonoBehaviour
{
    private EnemyVariables variables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject attackObject;
    public float attackDuration;
    public Action startLungeAction;
    
    public float jumpForceMod;
    public float attackRange;
    void Start()
    {
        variables = gameObject.GetComponent<EnemyVariables>();
        startLungeAction += StartLunge;
    }

    // Update is called once per frame
    void Update()
    {
        if (variables.detectedPlayerDistance <= attackRange && variables.isCharging)
        {
            variables.isCharging = false;
            startLungeAction.Invoke();
        }
    }
    void StartLunge()
    {
        variables.chaseScript.pauseMovement = true;
        attackObject.SetActive(true);
        Vector2 jumpDirection = (variables.facing+Vector2.up).normalized;
        Vector2 newAttackPos = new Vector2(Math.Abs(attackObject.transform.localPosition.x)* variables.facing.x,attackObject.transform.localPosition.y);
        attackObject.transform.localPosition = newAttackPos;

        variables.rigidBody.AddForce(jumpDirection * variables.runSpeed * jumpForceMod, ForceMode2D.Impulse);
        Invoke("EndLunge", attackDuration);
    }
    void EndLunge()
    {
        variables.chaseScript.pauseMovement = false;
        attackObject.SetActive(false);
    }
}
