using UnityEngine;

public class SmashState : EnemyStateBase
{
    private EnemyController enemy;



    public float smashTime = 1f;
    private float timer = 0f;
    public float smashPower = 20f;
    public string Label  = "Smash State"; // just for clarity in  editor

    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Smash State");
        timer = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;

        if (timer == 0f)
        {
            variables.rigidBody.linearVelocity = Vector2.zero;

            variables.rigidBody.AddForce(Vector2.down * smashPower, ForceMode2D.Impulse);
        }


 


        if (timer >= smashTime)
        {
            enemy.SwitchState(NextState(Random.Range(0, nextStates.Length)));
        }
        timer += Time.deltaTime;

    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return true;
    }
    public override void ExitState() { base.ExitState(); }
}