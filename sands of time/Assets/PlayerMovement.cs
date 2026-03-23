using System;
using UnityEngine;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    public  Action<Vector2> MoveAction {get;set;}
    public  Action JumpAction {get;set;}
    private string[] defaultLayerNames = new string[]{"Ground"};
    private PlayerVariables variables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
    }
    void OnEnable()
    {
        MoveAction+=Move;
        JumpAction+=Jump;
    }
    void OnDisable()
    {
        MoveAction-=Move;
        JumpAction-=Jump;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector2 dirVector)
    {

        Vector2 moveVector = new Vector2(dirVector.x*variables.movementSpeed,variables.rigidBody.linearVelocity.y);
        variables.rigidBody.linearVelocity = moveVector;
        
    }
    public void Jump()
    {
        
        if (GetObjectUnder(LayerMask.GetMask(defaultLayerNames),variables.footRaycastDistance) != null)
        {
            variables.rigidBody.AddForce(Vector2.up*variables.jumpStrength,ForceMode2D.Impulse);

        }
    }
    public GameObject GetObjectUnder(LayerMask layer, float rayLength = 1f)
    {
        BoxCollider2D box = variables.hitbox;
        Vector2 leftPos = new Vector2(box.transform.position.x-box.size.x/2,box.transform.position.y-box.size.y/2);
        Vector2 rightPos = new Vector2(box.transform.position.x+box.size.x/2,box.transform.position.y-box.size.y/2);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftPos, Vector2.down, rayLength, layer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightPos, Vector2.down, rayLength, layer);
        Debug.DrawRay(leftPos, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(rightPos, Vector2.down * rayLength, Color.green);


        if (hitLeft.collider == null && hitRight.collider == null)
        {
            return null;
        }
        if (hitLeft.collider != null)
        {
            return hitLeft.collider.gameObject;
        }
        return hitRight.collider.gameObject;
    }
}
