using UnityEngine;

public class ProjectileState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;


    private float timer = 0f;
    public Collider2D areaOfAttackCollider;

    public float distanceToTravel;
    public float peakHeight;
    public float timeToTravel;
    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        Debug.Log("Entering Projectile State");
        timer = 0f;
    }

    public void UpdateState()
    {
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


    public bool CheckEntryConditions(EnemyController enemy)
    {
        return areaOfAttackCollider.IsTouching(enemy.variables.player.GetComponent<Collider2D>());;
    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Charging State"); }
}