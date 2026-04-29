using System;
using UnityEngine;

public class SwordDamageScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage;
    public bool doneDamage = false;
    public PlayerVariables variables;
    public float knockbackDuration = 0.2f;
    public float knockbackSpeed = 10f;
    public float knockbackAcceleration = 20f;
    public float knockbackDeceleration = 40f;
    public float knockbackTimer;
    public float meterGainOnHit = 10f;
    private Vector2 initialFacing;
    public Action DealDamageAction;

    public EnemyController hitEnemy;
    void Start()
    {
        this.gameObject.SetActive(true);
        DealDamageAction += DealDamage;
    }
    public void DealDamage()
    {
        hitEnemy.TakeDamageAction?.Invoke(damage);
                    variables.meterManagerScript.AddMeter(meterGainOnHit);
                    doneDamage = true;
                    Vector2 facing = variables.playerFacing;
                    variables.playerMovementScript.MoveAction?.Invoke(-facing,knockbackSpeed,knockbackAcceleration,knockbackDeceleration,true);
                    knockbackTimer = knockbackDuration;
                    initialFacing = variables.playerFacing;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doneDamage && knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
                           

            variables.playerMovementScript.MoveAction?.Invoke(-initialFacing,knockbackSpeed,knockbackAcceleration,knockbackDeceleration, true);
            
        }
        if (doneDamage)
        {
            return;
        }
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") || collider.gameObject.CompareTag("Damagable"))
            {
                EnemyController health = collider.gameObject.GetComponent<EnemyController>();
                hitEnemy = health;
                if (health != null)
                {
                    DealDamageAction.Invoke();
                    break;
                }
            }
    }
    }
}
