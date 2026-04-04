using Unity.VisualScripting;
using UnityEngine;

public class ChargingState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;

    public Collider2D areaOfAttackCollider;
    public float timesFlippedDirection = 0;

    public float chargeSpeed = 10f;
    public string Label = "Charging State"; // just for clarity in  editor

    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        timesFlippedDirection = 0;
        enemy.FacePlayer();
        Debug.Log("Entering Charging State");
        enemy.variables.touchingSolidGround = false;
    }

    public void UpdateState()
    {
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


    public bool CheckEntryConditions(EnemyController enemy)
    {
        return areaOfAttackCollider.IsTouching(enemy.variables.player.GetComponent<Collider2D>());
    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Charging State"); }
}