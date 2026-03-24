using Unity.VisualScripting;
using UnityEngine;
public class EnemyChaseGround : MonoBehaviour
{
    private EnemyVariables variables;
    private string[] groundLayer = new string[]{"Ground"};
    private bool touchingSolidGround = true;
    public bool pauseMovement = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<EnemyVariables>();
    }
    void Enable()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(CheckInFront());
        if (!pauseMovement)
        {
            PatrollBehavior();
        }

    }

public void PatrollBehavior()
{
    float targetSpeed = variables.walkSpeed;
    variables.isCharging = false;

    if (CheckInFront(rayStartOffset: variables.senseAngleOffset))
    {
        variables.isCharging = true;
        targetSpeed = variables.runSpeed;
    }

    if (CheckIfAirUnder(LayerMask.GetMask(groundLayer)) && touchingSolidGround)
    {
        variables.facing = new Vector2(-variables.facing.x, variables.facing.y);
        touchingSolidGround = false;
    }

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
                return true;
            }


        }
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
        touchingSolidGround = true;
        return false;
    }
}
