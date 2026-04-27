using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public float movementSpeed;
    public float accelertion;
    public float decelertion;
    public float airControlFactor; // how much control the player has over their movement while in the air, 0 means no control, 1 means full control
    private Vector2 _facing;
    public Vector2 playerFacing
    {
        get { return _facing; }
        set { _facing = value; if (value.x == 0){return;} this.spriteRenderer.flipX = value.x > 0; }
    }
    public Rigidbody2D rigidBody;
    public PlayerMovement playerMovementScript;
    public float money = 0;

    public PlayerAttackScript playerAttackScript;
    public SpriteRenderer spriteRenderer;
    public float footRaycastDistance;
    public BoxCollider2D hitbox;
    public float jumpStrength;
    public float atttackTimePeriod;
    public PlayerTimeManager timeManagerScript;
    public PlayerMeterManager meterManagerScript;

    public Vector2 lastSolidGroundPosition;
    public Vector2 lastSolidGroundHangDirection;
    public bool isOnGround;

    public float timeHealth;
    public float maxTimeHealth;
    public float initialMaxTimeHealth;
    //Dash variables start
    public float dashSpeed = 20f;         
    public float dashDuration = 0.15f;   
    public float dashCooldown = 1f;       
    public float dashTimeCost = 5f;      
    public float dashCooldownTimer = 0f;
    public bool hasAirDash = true;
    public bool isDashing = false;

    public float meterMaxAmount = 100f;
    public float meterAmount = 0f;
    public float meterFlipRequirement = 50f;
    public DeathScript deathScript;
    public PlayerInventoryScript playerInventoryScript;
    public float invincibilityTime = 50f;

    //Dash variables end

    void Start()
    {
        initialMaxTimeHealth = maxTimeHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
