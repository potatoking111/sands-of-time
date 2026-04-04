using UnityEngine;

public class ShootState : EnemyStateBase
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};



    public float waitTime = 1f;
    private float timer = 0f;
    public Collider2D areaOfAttackCollider;
    public string Label = "Shoot State"; // just for clarity in  editor

    
    public GameObject projectile;
    public override void EnterState(EnemyController enemy)
    {
        base.EnterState(enemy);
        this.enemy = enemy;
        enemy.FacePlayer();
        Debug.Log("Entering Jump State");
        timer = 0f;
    }

    public override  void UpdateState()
    {
        base.UpdateState();
        EnemyVariables variables = enemy.variables;

        if (timer == 0f)
        {
            GameObject projectileShot = Instantiate(projectile, enemy.transform.position, Quaternion.identity);
            projectileShot.GetComponent<EnemyVariables>().facing = variables.facing;
            projectileShot.SetActive(true);
            
            GameObject projectileShot2 = Instantiate(projectile, enemy.transform.position, Quaternion.identity);

            projectileShot2.GetComponent<EnemyVariables>().facing = -variables.facing;
            projectileShot2.SetActive(true);
        }


 

        timer += Time.deltaTime;

        if (timer >= waitTime)
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