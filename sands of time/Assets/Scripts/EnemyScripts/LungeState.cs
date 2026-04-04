using UnityEngine;

public class LungeState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};

    private float attackTimer;
    public string Label { get; } = "Lunge State"; // just for clarity in  editor


    public GameObject attackObject;
    public float lungeAngle;
    public float jumpForceMod;
    public float attackDuration;
    
    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        attackTimer = 0f;
        Debug.Log("Entering Lunge State");

    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;
        float targetSpeed = variables.walkSpeed;
        variables.isCharging = false;


        if (enemy.CheckIfAirUnder(LayerMask.GetMask(groundLayer)) && variables.touchingSolidGround)
        {
            variables.facing = Vector2.zero;
        }
        if (attackTimer == 0f)
        {
            Lunge();

        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDuration)        {
            
            for (int i = 0; i < nextStates.Length; i++)
            {
                if (NextState(i) != null && NextState(i).CheckEntryConditions(enemy))
                {
                    enemy.SwitchState(NextState(i));
                    return;
                }
            }
        }
    }
    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return true;
    }
    private void Lunge()
    {
        EnemyVariables variables = enemy.variables;

        attackObject.SetActive(true);



        Vector2 horizontal = new Vector2(Mathf.Sign(variables.facing.x), 0f);

        float angleRad = lungeAngle * Mathf.Deg2Rad;

        Vector2 jumpDirection = new Vector2(
            horizontal.x * Mathf.Cos(angleRad),
            Mathf.Sin(angleRad)
        ).normalized;


        Vector2 newAttackPos = new Vector2(Mathf.Abs(attackObject.transform.localPosition.x)* variables.facing.x,attackObject.transform.localPosition.y);
        attackObject.transform.localPosition = newAttackPos;

        variables.rigidBody.AddForce(jumpDirection * variables.runSpeed * jumpForceMod, ForceMode2D.Impulse);


    }

    public override void ExitState() {base.ExitState(); attackObject.SetActive(false);        enemy.FacePlayer();
}
}