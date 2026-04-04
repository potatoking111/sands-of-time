using UnityEngine;

public class DownedState : EnemyStateBase
{
    private EnemyController enemy;



    public float downedTime = 1f;
    private float timer = 0f;
    public float damageTaken = 0;
    public float damageThreshold = 10f;
    public string Label = "Downed"; // just for clarity in  editor
    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Downed State");
        timer = 0f;
        damageTaken = 0;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;

        if (timer == 0f)
        {
            variables.rigidBody.linearVelocity = Vector2.zero;

        }


 


        if (timer >= downedTime)
        {
            enemy.SwitchState(NextState(Random.Range(0, nextStates.Length)));
        }
        timer += Time.deltaTime;

    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        if (damageTaken >= damageThreshold)
        {
            return true;
        }
        return false;

    }
    public override void ExitState() { base.ExitState(); }
}