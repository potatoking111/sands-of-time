using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public EnemyVariables variables;
    private List<MonoBehaviour> stateScripts; // assign in inspector
    public MonoBehaviour initialState;

    private List<EnemyStateBase> states = new List<EnemyStateBase>();
    private int currentStateIndex = 0;
    public Action<float> TakeDamageAction {get;set;}    

    public UnityEvent EnemyDeathEvent;

    void Start()
    {
        // Convert scripts to IEnemyState


        stateScripts = new List<MonoBehaviour>(gameObject.GetComponents<MonoBehaviour>());
        foreach (var script in stateScripts)
        {
            if (script is EnemyStateBase state)
            {   
                Debug.Log("Adding state: " + state.stateLabel);
                states.Add(state);
            }
        }
        if (initialState is EnemyStateBase initState)
        {
            currentStateIndex = states.IndexOf(initState);
            states[currentStateIndex].EnterState(this);

        }
        // Debug.Log("Entering initial state: " + states[currentStateIndex].stateLabel);
        TakeDamageAction += TakeDamage;

    }

    void Update()
    {
        if (!GameState.GameOn)
        {
            return;
        }
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
    public void SwitchState(EnemyStateBase newState)
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

        Bounds bounds = box.bounds;

        Vector2 leftPos = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 rightPos = new Vector2(bounds.max.x, bounds.min.y);
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

    public bool CheckIfInFront(LayerMask layer, float rayLength = -1f)
    {

        BoxCollider2D box = variables.hitbox;

        Bounds bounds = box.bounds;
        if (rayLength < 0)
        {
            rayLength = box.size.x / 2;
        }
        Vector2 midPos = new Vector2(bounds.max.x, bounds.max.y-(bounds.size.y/2));
        RaycastHit2D hit = Physics2D.Raycast(midPos, Vector2.right*variables.facing, rayLength, layer);
        Debug.DrawRay(midPos, Vector2.right * rayLength, Color.red);

        
        if (hit.collider)
        {
                return true;

            
        }
        return false;
    }

    // direction of side thats on ground
            public Vector2 BetterCheckIfAirUnder(LayerMask layer, float rayLength = 1f)
    {
        BoxCollider2D box = variables.hitbox;

        Bounds bounds = box.bounds;

        Vector2 leftPos = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 rightPos = new Vector2(bounds.max.x, bounds.min.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftPos, Vector2.down, rayLength, layer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightPos, Vector2.down, rayLength, layer);
        Debug.DrawRay(leftPos, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(rightPos, Vector2.down * rayLength, Color.green);

        
        if (hitLeft.collider == null || hitRight.collider == null)
        {
                if (hitLeft.collider == null && hitRight.collider == null) return Vector2.zero;
                if (hitLeft.collider == null) return Vector2.right;
                return Vector2.left;
            
        }
        variables.touchingSolidGround = true;
        return Vector2.zero;
    }

    public void FacePlayer()
    {
        CheckInFront(rayStartOffset: -90,amountOfRays:20);

        Vector2 direction = new Vector2(MathF.Sign(variables.player.transform.position.x - transform.position.x), 0);

        variables.facing = direction;
    }

    // test chatted code
    public void JumpToPoint(Vector2 target, float time)
    {
        Rigidbody2D rb = variables.rigidBody;

        Vector2 start = transform.position;

        float gravity = Physics2D.gravity.y * rb.gravityScale;

        // Calculate required velocity
        Vector2 velocity = new Vector2(
            (target.x - start.x) / time,
            (target.y - start.y - 0.5f * gravity * time * time) / time
        );

        rb.linearVelocity = velocity;
    }
    public void JumpWithPeak(Vector2 target, float peakHeight, float totalTime)
{
    Rigidbody2D rb = variables.rigidBody;

    Vector2 start = transform.position;

    float gravity = Physics2D.gravity.y * rb.gravityScale;

    float timeToPeak = totalTime / 2f;

    // Calculate vertical velocity needed to reach peak
    float vy = (peakHeight - start.y - 0.5f * gravity * timeToPeak * timeToPeak) / timeToPeak;

    // Calculate horizontal velocity
    float vx = (target.x - start.x) / totalTime;

    rb.linearVelocity = new Vector2(vx, vy);
}

    private void TakeDamage(float damage)
    {
        variables.health -= damage;
        if (variables.health <= 0)
        {
            EnemyDeathEvent.Invoke();
            Destroy(gameObject);
        }
    }


}