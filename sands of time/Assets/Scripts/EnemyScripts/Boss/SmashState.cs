using UnityEngine;

public class SmashState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;


    public float smashTime = 1f;
    private float timer = 0f;
    public float smashPower = 20f;
    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Smash State");
        timer = 0f;
    }

    public void UpdateState()
    {
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


    public bool CheckEntryConditions(EnemyController enemy)
    {
        return true;
    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Smash State"); }
}