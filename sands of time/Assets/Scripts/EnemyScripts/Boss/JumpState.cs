using UnityEngine;

public class JumpState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;


    public float timeToJumpApex = 1f;
    private float timer = 0f;
    public float jumpHeight = 10f;
    public Collider2D areaOfAttackCollider;
    public string Label  = "Jump State"; // just for clarity in  editor

    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Jump State");
        timer = 0f;
    }

    public void UpdateState()
    {
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


    public bool CheckEntryConditions(EnemyController enemy)
    {
        return areaOfAttackCollider.IsTouching(enemy.variables.player.GetComponent<Collider2D>());;
    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Charging State"); }
}