using Unity.VisualScripting;
using UnityEngine;

public class ChargingState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public Collider2D areaOfAttackCollider;
    public float timesFlippedDirection = 0;

    public float chargeSpeed = 10f;

    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        timesFlippedDirection = 0;
        enemy.FacePlayer();
        Debug.Log("Entering Charging State");
        enemy.variables.touchingSolidGround = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;
        float targetSpeed = chargeSpeed;
        variables.isCharging = false;


        if (enemy.CheckIfAirUnder(LayerMask.GetMask(groundLayer)) && variables.touchingSolidGround)
        {
            variables.facing = new Vector2(-variables.facing.x, variables.facing.y);
            variables.touchingSolidGround = false;
            timesFlippedDirection++;
        }


        if (timesFlippedDirection >= 2)
        {
                
        enemy.SwitchState(NextState(Random.Range(0, nextStates.Length)));

        }



        
        enemy.MoveForward(targetSpeed);
    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return areaOfAttackCollider.IsTouching(enemy.variables.player.GetComponent<Collider2D>());
    }
    public override void ExitState() { base.ExitState(); }
}

