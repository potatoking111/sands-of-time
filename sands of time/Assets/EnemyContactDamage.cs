using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    private EnemyVariables variables;
    private float lastDamageTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<EnemyVariables>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
    Debug.Log("Something entered trigger: " + other.name);
    if (other.gameObject.CompareTag("PlayerHitbox") && lastDamageTime + variables.enemyDamageCooldown < Time.time)
    {
        Debug.Log("Player entered trigger");
        PlayerVariables playerVariables = other.gameObject.GetComponentInParent<PlayerVariables>();
        if (playerVariables != null)
        {
            Debug.Log("Player variables found, applying damage");
            playerVariables.timeManagerScript.TakeDamageAction?.Invoke(variables.enemyContactDamageAmount);
            lastDamageTime = Time.time;

        }
        else
        {
            Debug.Log("Player variables not found on the player object");
        }
    }
}

}
