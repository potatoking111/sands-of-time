using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public float movementSpeed;
    public float accelertion;
    public float decelertion;
    public float airControlFactor; // how much control the player has over their movement while in the air, 0 means no control, 1 means full control
    public Vector2 playerFacing;
    public Rigidbody2D rigidBody;
    public PlayerMovement playerMovementScript;
    public PlayerAttackScript playerAttackScript;
    public float footRaycastDistance;
    public BoxCollider2D hitbox;
    public float jumpStrength;
    public float atttackTimePeriod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
