using UnityEngine;

public  class EnemyStateBase : MonoBehaviour, IEnemyState
{
    [Header("Visual")]
    public Sprite stateSprite; // 👈 shows in inspector


    public MonoBehaviour[] nextStates;


    public string stateLabel;
    public EnemyStateBase NextState(int i)  => nextStates[i] as EnemyStateBase;

    public virtual void EnterState(EnemyController enemy)
    {
        Debug.Log("Entering " + stateLabel);
        enemy.variables.spriteRenderer.sprite = stateSprite;
    }
    public virtual void UpdateState()
    {
        
    }
    public virtual void ExitState()
    {
        Debug.Log("Exiting " + stateLabel);
    }

    public virtual bool CheckEntryConditions(EnemyController enemy)
    {
        return false;
    }
}