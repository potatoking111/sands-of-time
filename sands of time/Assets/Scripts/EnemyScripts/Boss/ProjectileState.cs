using UnityEngine;

public class ProjectileState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};


    private float timer = 0f;
    public Collider2D areaOfAttackCollider;

    public float distanceToTravel;
    public float peakHeight;
    public float timeToTravel;
    public string Label { get; } = "Projectile State"; // just for clarity in  editor

    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        Debug.Log("Entering Projectile State");
        timer = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;

        if (timer == 0f)
        {
            enemy.JumpWithPeak(enemy.transform.position + (Vector3)(variables.facing.normalized * distanceToTravel), peakHeight, timeToTravel);
        }


 

        timer += Time.deltaTime;

        if (timer >= timeToTravel)
        {
            enemy.SwitchState(NextState(Random.Range(0, nextStates.Length)));
        }

    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return areaOfAttackCollider.IsTouching(enemy.variables.player.GetComponent<Collider2D>());;
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}