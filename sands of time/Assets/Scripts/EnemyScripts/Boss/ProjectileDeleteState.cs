
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileDeleteState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    public MonoBehaviour[] nextStates;
    public float timeToDelete = 0.5f;   
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;

    public string Label { get; } = "Projectile Delete State"; // just for clarity in  editor


    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        Debug.Log("Entering Projectile Delete State");
        Destroy(this.gameObject,timeToDelete);
    }

    public void UpdateState()
    {

    }


    public bool CheckEntryConditions(EnemyController enemy)
    {
        return true;
    }
    public void ExitState() { UnityEngine.Debug.Log("Exiting Charging State"); }
}