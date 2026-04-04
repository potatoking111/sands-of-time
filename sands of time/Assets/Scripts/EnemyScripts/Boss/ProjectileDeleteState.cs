
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileDeleteState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public float timeToDelete = 0.5f;   

    public string Label { get; } = "Projectile Delete State"; // just for clarity in  editor


    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        Debug.Log("Entering Projectile Delete State");
        Destroy(this.gameObject,timeToDelete);
    }

    public override void UpdateState()
    {

    }


    public override bool CheckEntryConditions(EnemyController enemy)
    {
        return true;
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}