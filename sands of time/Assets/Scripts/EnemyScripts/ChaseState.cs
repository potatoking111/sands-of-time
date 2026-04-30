using UnityEngine;

public class ChaseState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public float chaseSpeed = 5f;


    public float senseAngleOffset;
    public string Label { get; } = "Chase State"; // just for clarity in  editor

    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
    }

    public override void UpdateState()
    {
        EnemyVariables variables = enemy.variables;
        float targetSpeed = chaseSpeed;
        variables.isCharging = false;


        if ((enemy.CheckIfAirUnder(LayerMask.GetMask(groundLayer)) && variables.touchingSolidGround) || enemy.CheckIfInFront(LayerMask.GetMask(groundLayer)))
        {
            variables.facing = new Vector2(-variables.facing.x, variables.facing.y);
            variables.touchingSolidGround = false;
        }



        for (int i = 0; i < nextStates.Length; i++)
        {
            if (NextState(i) != null && NextState(i).CheckEntryConditions(enemy))
            {
                enemy.SwitchState(NextState(i));
                return;
            }
        }


        
        enemy.MoveForward(targetSpeed);
    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return enemy.CheckInFront(rayStartOffset: this.senseAngleOffset);
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}