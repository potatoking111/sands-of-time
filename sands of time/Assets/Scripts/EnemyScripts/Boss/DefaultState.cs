
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultState : MonoBehaviour, IEnemyState
{
    private EnemyController enemy;
    private string[] groundLayer = new string[]{"Ground"};
    
    public MonoBehaviour[] nextStates;
    public IEnemyState NextState(int i)  => nextStates[i] as IEnemyState;
    public float walkSpeed;

    public float senseAngleOffset;
    
    public float attackCooldown = 2f;
    private float attackTimer = 0f;

    public Dictionary<IEnemyState,float> stateWeights = new Dictionary<IEnemyState, float>();

    public void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        Debug.Log("Entering Boss Default State");
        EnemyVariables variables = enemy.variables;

        int randomDirection = Random.Range(0, 2) * 2 - 1; // Randomly -1 or 1
        variables.facing = new Vector2(randomDirection, 0);
        attackTimer = 0;    
        variables.touchingSolidGround = true;
        if (stateWeights.Count == 0) // Initialize weights for next states
        {
        foreach (MonoBehaviour mb in nextStates)
        {
            if (mb is IEnemyState state)
            {
                stateWeights[state] = 1f; // Default weight, can be adjusted in the inspector
            }
        }
        }
    }

    public void UpdateState()
    {
        EnemyVariables variables = enemy.variables;
        float targetSpeed = walkSpeed;
        // randomize direction of movement  




        // make sure doesnt go off edge 
        Vector2 result = enemy.BetterCheckIfAirUnder(LayerMask.GetMask(groundLayer));
        if (result != Vector2.zero && variables.touchingSolidGround)
        {
            variables.facing = result;
            variables.touchingSolidGround = false;
        }

        



        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;   
            int chosenIndex = ChooseIndex(stateWeights.Values.ToArray());

            var state = stateWeights.Keys.ElementAt(chosenIndex);

            if (state != null && state.CheckEntryConditions(enemy))
            {
                enemy.SwitchState(NextState(Random.Range(0, nextStates.Length)));
                Debug.Log("chosen weight: " + stateWeights[state]);
                for (int i = 0; i < stateWeights.Count; i++)
                {
                    
                    if (i == chosenIndex)
                    {
                        stateWeights[state] *= 0.5f; // Decrease weight of chosen state // tweak values later
                    }
                    else
                    {
                        stateWeights[stateWeights.Keys.ElementAt(i)] *= 1.1f; // Increase weight of other states
                    }
                }

            }
            // might wanna weight it in future
        }
        enemy.MoveForward(targetSpeed);

        attackTimer += Time.deltaTime;
    }



    public bool CheckEntryConditions(EnemyController enemy)
    {
        return enemy.CheckInFront(rayStartOffset: this.senseAngleOffset);
    }


    public int ChooseIndex(float[] weights)
    {
        float total = 0f;

        // Step 1: sum weights
        foreach (float w in weights)
            total += w;

        // Step 2: random value
        float rand = Random.Range(0f, total);

        // Step 3: find bucket
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand < weights[i])
                return i;

            rand -= weights[i];
        }

        return weights.Length - 1; // fallback (just in case)
    }
    public void ExitState() {UnityEngine.Debug.Log("Exiting Patrol State"); }
}