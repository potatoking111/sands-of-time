using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public BoxCollider2D hitbox;
    public float senseRadius;
    public float senseAngleOffset;
    public Vector2 facing;
    public float walkSpeed;
    public float runSpeed;
    public bool isCharging = false;
    public float chargeExtraDamage;
    public float enemyDamageCooldown;
    public float enemyContactDamageAmount;
    public float acceleration;
    public float detectedPlayerDistance;
    public EnemyChaseGround chaseScript;
    public EnemyContactDamage contactDamageScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
