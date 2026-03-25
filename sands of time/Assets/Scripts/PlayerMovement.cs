using System;
using UnityEngine;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    public  Action<Vector2> MoveAction {get;set;}
    public  Action JumpAction {get;set;}
    private string[] defaultLayerNames = new string[]{"Ground"};
    private PlayerVariables variables;

    public Action GoToLastGroundPositionAction {get;set;}
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
        GoToLastGroundPositionAction += GoToLastGroundPosition;
    }
    void GoToLastGroundPosition()
    {
        variables.rigidBody.position = variables.lastSolidGroundPosition;
        if (variables.lastSolidGroundHangDirection== Vector2.right)
        {
            variables.rigidBody.position = variables.rigidBody.position + Vector2.left*variables.hitbox.size.x;
        }
        else if (variables.lastSolidGroundHangDirection == Vector2.left)
        {
            variables.rigidBody.position = variables.rigidBody.position + Vector2.right*variables.hitbox.size.x;
        }
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
    void FixedUpdate()
    {
        variables.isOnGround = false;
        Vector2 hangDirection = GetDirectionOfSideHanging(LayerMask.GetMask(defaultLayerNames),variables.footRaycastDistance);
        if (hangDirection != Vector2.zero)
        {
            variables.isOnGround = true;
            variables.lastSolidGroundPosition = variables.rigidBody.position;
            variables.lastSolidGroundHangDirection = hangDirection;
        }
    }
    public void Move(Vector2 dirVector)
{
    float targetX = dirVector.x * variables.movementSpeed;
    float currentX = variables.rigidBody.linearVelocity.x;
    float rate;
    bool isPressingMove = dirVector.x != 0;

    if (isPressingMove)
    {
        if (variables.isOnGround)
            rate = variables.accelertion;
        else
            rate = variables.accelertion * variables.airControlFactor;
    }
    else
    {
        if (variables.isOnGround)
            rate = variables.decelertion;
        else
            rate = variables.decelertion * variables.airControlFactor;
    }

    float newX = Mathf.MoveTowards(currentX, targetX, rate * variables.movementSpeed * Time.fixedDeltaTime);
    variables.rigidBody.linearVelocity = new Vector2(newX, variables.rigidBody.linearVelocity.y);
}
    public void Jump()
    {
        
        if (variables.isOnGround)
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

    public Vector2 GetDirectionOfSideHanging(LayerMask layer, float rayLength = 1f)
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
            return Vector2.zero;
        }
        if (hitLeft.collider != null)
        {
            return Vector2.right;
        }
        return Vector2.left;
    }
}