using System;
using UnityEngine;

public interface IEnemyState
{
    void EnterState(EnemyController enemy);
    void UpdateState();
    void ExitState();
    
    bool CheckEntryConditions(EnemyController enemy);
}

