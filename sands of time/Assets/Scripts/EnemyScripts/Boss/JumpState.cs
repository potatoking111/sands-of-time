using UnityEngine;

public class JumpState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};



    public float timeToJumpApex = 1f;
    private float timer = 0f;
    public float jumpHeight = 10f;
    public Collider2D areaOfAttackCollider;
    public string Label  = "Jump State"; // just for clarity in  editor

    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Jump State");
        timer = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;

        if (timer == 0f)
        {
            enemy.JumpToPoint(variables.player.transform.position+Vector3.up*jumpHeight, timeToJumpApex);
        }


 

        timer += Time.deltaTime;

        if (timer >= timeToJumpApex)
        {
            enemy.SwitchState(NextState(Random.Range(0, nextStates.Length)));
        }

    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return areaOfAttackCollider.IsTouching(enemy.variables.player.GetComponent<Collider2D>());;
    }
    public override void ExitState() { base.ExitState(); }
}