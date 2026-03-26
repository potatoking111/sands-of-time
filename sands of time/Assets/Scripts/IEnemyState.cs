using System;
using UnityEngine;

public interface IEnemyState
{
    void EnterState(EnemyController enemy);
    void UpdateState();
    void ExitState();
    IEnemyState NextState (int index=0);

    bool CheckEntryConditions(EnemyController enemy);
}

