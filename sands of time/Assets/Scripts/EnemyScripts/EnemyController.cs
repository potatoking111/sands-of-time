using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyVariables variables;
    public List<MonoBehaviour> stateScripts; // assign in inspector

    private List<IEnemyState> states = new List<IEnemyState>();
    private int currentStateIndex = 0;

    void Start()
    {
        // Convert scripts to IEnemyState
        foreach (var script in stateScripts)
        {
            if (script is IEnemyState state)
            {
                states.Add(state);
                state.EnterState(this);
            }
        }
    }

    void Update()
    {
        if (states.Count > 0)
            states[currentStateIndex].UpdateState();
    }

    public void SwitchState(int newIndex)
    {
        if (newIndex < 0 || newIndex >= states.Count) return;

        states[currentStateIndex].ExitState();
        currentStateIndex = newIndex;
        states[currentStateIndex].EnterState(this);
    }
    public void SwitchState(IEnemyState newState)
    {
        int newIndex = states.IndexOf(newState);
        if (newIndex == -1) return;

        states[currentStateIndex].ExitState();
        currentStateIndex = newIndex;
        states[currentStateIndex].EnterState(this);
    }


        public void MoveForward(float targetSpeed)
    {

        Vector2 targetVelocity = variables.facing * targetSpeed;

        // smooth acceleration
        variables.rigidBody.linearVelocity = Vector2.Lerp(
            variables.rigidBody.linearVelocity,
            targetVelocity,
            variables.acceleration * Time.deltaTime
        );
    }
    
    public bool CheckInFront(int amountOfRays = 10,float rayStartOffset=30) // start from bottom go to top angle
    {
        

        LayerMask layerMask = Physics2D.AllLayers;
        int[] indexLayersToRemove = { LayerMask.NameToLayer("Enemy") ,LayerMask.NameToLayer("PlayerHitbox")};
        foreach (int indexEnemyLayer in indexLayersToRemove)
        {
        int layerToRemoveBit = 1 << indexEnemyLayer;
        layerMask &= ~layerToRemoveBit;
        }


        float angleSpacing = (180.0f - rayStartOffset*2) / amountOfRays;
        UnityEngine.Vector2 start = variables.hitbox.transform.position;
        
        for (int rayNumber = 0; rayNumber < amountOfRays; rayNumber++)
        {
            UnityEngine.Vector2 direction = UnityEngine.Quaternion.Euler(0, 0, (angleSpacing*rayNumber - 90 + rayStartOffset)) * variables.facing;
            RaycastHit2D hit  = Physics2D.Raycast(start,direction,variables.senseRadius,layerMask);
            Debug.DrawRay(start, direction * variables.senseRadius, Color.red); // visualize
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
               
                variables.detectedPlayerDistance = (Vector2)variables.hitbox.transform.position - (Vector2)hit.collider.transform.position;
                return true;
            }


        }

        variables.detectedPlayerDistance = new Vector2(Mathf.Infinity, Mathf.Infinity);
        return false;
    }


        public bool CheckIfAirUnder(LayerMask layer, float rayLength = 1f)
    {
        BoxCollider2D box = variables.hitbox;
        Vector2 leftPos = new Vector2(box.transform.position.x-box.size.x/2,box.transform.position.y-box.size.y/2);
        Vector2 rightPos = new Vector2(box.transform.position.x+box.size.x/2,box.transform.position.y-box.size.y/2);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftPos, Vector2.down, rayLength, layer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightPos, Vector2.down, rayLength, layer);
        Debug.DrawRay(leftPos, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(rightPos, Vector2.down * rayLength, Color.green);

        
        if (hitLeft.collider == null || hitRight.collider == null)
        {
                return true;

            
        }
        variables.touchingSolidGround = true;
        return false;
    }

    public void FacePlayer()
    {
        CheckInFront(rayStartOffset: -90,amountOfRays:20);

        Vector2 direction = new Vector2(MathF.Sign(variables.player.transform.position.x - transform.position.x), 0);

        variables.facing = direction;
    }



}