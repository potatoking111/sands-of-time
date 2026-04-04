using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public BoxCollider2D hitbox;
    public float senseRadius;
    public float senseAngleOffset;
    private Vector2 _facing;
    public Vector2 facing
    {
        get { return _facing; }
        set { _facing = value; this.spriteRenderer.flipX = value.x > 0; }
    }


    public float walkSpeed;
    public float runSpeed;
    public bool isCharging = false;
    public float chargeExtraDamage;
    public float enemyDamageCooldown;
    public float enemyContactDamageAmount;
    public float acceleration;
    public Vector2 detectedPlayerDistance;
    public EnemyChaseGround chaseScript;
    public EnemyContactDamage contactDamageScript;
    public GameObject player;
    public bool touchingSolidGround;
    public float health = 20f;

    public SpriteRenderer spriteRenderer;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
