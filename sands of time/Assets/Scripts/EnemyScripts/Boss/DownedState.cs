using UnityEngine;

public class DownedState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;


    public float downedTime = 1f;
    private float timer = 0f;
    public float damageTaken = 0;
    public float damageThreshold = 10f;
    public string Label = "Downed"; // just for clarity in  editor
    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Downed State");
        timer = 0f;
        damageTaken = 0;
    }

    public void UpdateState()
    {
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


    public bool CheckEntryConditions(EnemyController enemy)
    {
        if (damageTaken >= damageThreshold)
        {
            return true;
        }
        return false;

    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Downed State"); }
}