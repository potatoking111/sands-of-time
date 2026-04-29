
using UnityEngine;

public class PatrolState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    


    public string Label { get; } = "Patrol State"; // just for clarity in  editor

    public float senseAngleOffset;
    
    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        Debug.Log("Entering Patrol State");

    }

    public override void UpdateState()
    {
        Debug.Log("tryna patrol");
        base.UpdateState();
        EnemyVariables variables = enemy.variables;
        float targetSpeed = variables.walkSpeed;
        variables.isCharging = false;


        if (enemy.CheckIfAirUnder(LayerMask.GetMask(groundLayer)) && variables.touchingSolidGround)
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
        return !enemy.CheckInFront(rayStartOffset: this.senseAngleOffset);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}