using UnityEngine;

public class ChaseState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public float chaseSpeed = 5f;
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;

    public float senseAngleOffset;
    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        EnemyVariables variables = enemy.variables;
        float targetSpeed = chaseSpeed;
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


    public bool CheckEntryConditions(EnemyController enemy)
    {
        return enemy.CheckInFront(rayStartOffset: this.senseAngleOffset);
    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Chase State"); }
}