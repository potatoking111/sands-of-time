using UnityEngine;

public class BackupState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};

    public float backupSpeed;
    public float backupDuration;
    private float backupTimer;
    public float senseAngleOffset;
    public float senseDistance;
    public string Label { get; } = "Backup State"; // just for clarity in  editor

    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        backupTimer = 0f;
        Debug.Log("Entering Backup State");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;
        float targetSpeed = backupSpeed;
        variables.isCharging = false;


        if (!enemy.CheckIfAirUnder(LayerMask.GetMask(groundLayer)))
        {
            enemy.MoveForward(-targetSpeed);
        }
        else
        {
            enemy.variables.rigidBody.linearVelocityX = 0;
        }

        backupTimer += Time.deltaTime;
        if (backupTimer >= backupDuration)        {
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
        enemy.CheckInFront(rayStartOffset: this.senseAngleOffset);
        if (Mathf.Abs(enemy.variables.detectedPlayerDistance.x)< senseDistance)
        {
            return true;
        }
        return false;
    }


    public override void ExitState()
    {
        base.ExitState();
    }
}