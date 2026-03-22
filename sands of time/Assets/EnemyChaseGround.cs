using Unity.VisualScripting;
using UnityEngine;
public class EnemyChaseGround : MonoBehaviour
{
    private EnemyVariables variables;
    private string[] groundLayer = new string[]{"Ground"};
    private bool touchingSolidGround = true;
    
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
        PatrollBehavior();
    }
    public void PatrollBehavior()
    {
        float tempWalkSpeed = variables.walkSpeed;
        if (CheckInFront())
        {
            tempWalkSpeed = variables.runSpeed;
        }
        if (CheckIfAirUnder(LayerMask.GetMask(groundLayer)) && touchingSolidGround){
            variables.facing = new Vector2(variables.facing.x*-1,variables.facing.y);
            touchingSolidGround = false;
        }
        variables.rigidBody.linearVelocity = variables.facing * tempWalkSpeed ;

    }
    
    public bool CheckInFront(int amountOfRays = 10,float rayStartOffset=30) // start from bottom go to top angle
    {
        LayerMask layerMask = Physics.AllLayers;
        int indexEnemyLayer = LayerMask.NameToLayer("Enemy");
        int layerToRemoveBit = 1 << indexEnemyLayer;

        layerMask &= ~layerToRemoveBit;

        float angleSpacing = (180.0f - rayStartOffset*2) / amountOfRays;
        UnityEngine.Vector2 start = variables.hitbox.transform.position;
        
        for (int rayNumber = 0; rayNumber < amountOfRays; rayNumber++)
        {
            UnityEngine.Vector2 direction = UnityEngine.Quaternion.Euler(0, 0, (angleSpacing*rayNumber - 90 + rayStartOffset)) * variables.facing;
            RaycastHit2D hit  = Physics2D.Raycast(start,direction,variables.senseRadius,layerMask);
            Debug.DrawRay(start, direction * variables.senseRadius, Color.red); // visualize
            Debug.Log(hit.collider);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }


        }
        return false;
    }


        public bool CheckIfAirUnder(LayerMask layer, float rayLength = 1f)
    {
        Debug.Log(layer);
        BoxCollider2D box = variables.hitbox;
        Vector2 leftPos = new Vector2(box.transform.position.x-box.size.x/2,box.transform.position.y-box.size.y/2);
        Vector2 rightPos = new Vector2(box.transform.position.x+box.size.x/2,box.transform.position.y-box.size.y/2);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftPos, Vector2.down, rayLength, layer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightPos, Vector2.down, rayLength, layer);
        Debug.DrawRay(leftPos, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(rightPos, Vector2.down * rayLength, Color.green);

        Debug.Log(hitLeft.collider+ " enemy "+hitRight.collider);
        
        if (hitLeft.collider == null || hitRight.collider == null)
        {
                Debug.Log("AIuhafsuhf");
                return true;

            
        }
        touchingSolidGround = true;
        return false;
    }
}
