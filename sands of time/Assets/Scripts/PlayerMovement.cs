using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    public  Action<Vector2,float,float,float,bool> MoveAction {get;set;}
    public  Action JumpAction {get;set;}
    public List<Func<bool>> JumpPossibleRequirements = new List<Func<bool>>();
    private string[] defaultLayerNames = new string[]{"Ground"};
    private PlayerVariables variables;

    public Action GoToLastGroundPositionAction {get;set;}
    private float dashTimer = 0f;
    private float dashDirection = 1f;
    private float normalGravityScale;

    public Action TouchGroundAction {get;set;}
    
    
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
        GoToLastGroundPositionAction += GoToLastGroundPosition;
        normalGravityScale = variables.rigidBody.gravityScale;
        JumpPossibleRequirements.Add(()=>variables.isOnGround);
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
            if (variables.isOnGround == false) // if is switching from false to true
            {
                TouchGroundAction?.Invoke();
            }
            variables.isOnGround = true;
            variables.lastSolidGroundPosition = variables.rigidBody.position;
            variables.lastSolidGroundHangDirection = hangDirection;
            variables.hasAirDash = true;
        }
        if (variables.dashCooldownTimer > 0)
            variables.dashCooldownTimer -= Time.fixedDeltaTime;

        if (variables.isDashing)
        {
            variables.rigidBody.gravityScale = 0f;                                                   
            dashTimer -= Time.fixedDeltaTime;
            variables.rigidBody.linearVelocity = new Vector2(dashDirection * variables.dashSpeed, 0f);

            if (dashTimer <= 0)
            {
                variables.isDashing = false;
                variables.rigidBody.gravityScale = normalGravityScale;
            }

        }
    }
    public void Move(Vector2 dirVector,float speedOverride = -1,float accelOverride = -1,float decelOverride = -1, bool ignoreTargetY = false)
    {
    if (accelOverride == -1) accelOverride = variables.accelertion;
    if (speedOverride == -1) speedOverride = variables.movementSpeed;
    if (decelOverride == -1) decelOverride = variables.decelertion;
    if (variables.isDashing) return;

    Vector2 target = new Vector2(dirVector.x * speedOverride, dirVector.y * speedOverride);
    Vector2 current = variables.rigidBody.linearVelocity;
    float rate;
    bool isPressingMove = dirVector.x != 0;

    if (isPressingMove)
    {
        if (variables.isOnGround)
            rate = accelOverride;
        else
            rate = accelOverride * variables.airControlFactor;
    }
    else
    {
        if (variables.isOnGround)
            rate = decelOverride;
        else
            rate = decelOverride * variables.airControlFactor;
    }
    if (ignoreTargetY)
        {
            target.y = variables.rigidBody.linearVelocity.y;
        }
    Vector2 newVelocity = new Vector2(Mathf.MoveTowards(current.x, target.x, rate * speedOverride * Time.fixedDeltaTime), Mathf.MoveTowards(current.y, target.y, rate * speedOverride * Time.fixedDeltaTime));
    // if (dirVector.y == 0)
    //     {
    //         newVelocity.y = current.y;
    //     }
    variables.rigidBody.linearVelocity = newVelocity;
}
    public void Jump()
    {
        bool canJump = false;

        foreach (Func<bool> req in JumpPossibleRequirements)
        {
            if (req())
            {
                canJump = true;
            }
        }
        
        if (canJump)
        {
            variables.rigidBody.linearVelocity = new Vector2(variables.rigidBody.linearVelocity.x, 0);
            variables.rigidBody.AddForce(Vector2.up*variables.jumpStrength,ForceMode2D.Impulse);

        }
    }
    public void TryDash()
    {
        if (variables.isDashing) return;
        if (variables.dashCooldownTimer > 0) return;
        if (!variables.isOnGround && !variables.hasAirDash) return;
        if (variables.timeHealth <= variables.dashTimeCost) return; // can't afford it

        if (!variables.isOnGround)
            variables.hasAirDash = false;

        dashDirection = variables.playerFacing.x >= 0 ? 1f : -1f;

        variables.isDashing = true;
        variables.dashCooldownTimer = variables.dashCooldown;
        dashTimer = variables.dashDuration;

        variables.timeManagerScript.TakeDamageAction?.Invoke(variables.dashTimeCost, DamageType.None);
    }
    public GameObject GetObjectUnder(LayerMask layer, float rayLength = 1f)
    {
        BoxCollider2D box = variables.hitbox;

        Bounds bounds = box.bounds;

        Vector2 leftPos = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 rightPos = new Vector2(bounds.max.x, bounds.min.y);

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