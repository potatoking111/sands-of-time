using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private float lastDamageTime = 0f;
    public float damageCooldown;
    public float damageAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
    Debug.Log("Something entered trigger: " + other.name);
    if (other.gameObject.CompareTag("PlayerHitbox") && lastDamageTime + damageCooldown < Time.time)
    {
        Debug.Log("Player entered trigger");
        PlayerVariables playerVariables = other.gameObject.GetComponentInParent<PlayerVariables>();
        if (playerVariables != null)
        {
            Debug.Log("Player variables found, applying damage");
            playerVariables.timeManagerScript.TakeDamageAction?.Invoke(damageAmount,DamageType.Spike);
            lastDamageTime = Time.time;

        }
        else
        {
            Debug.Log("Player variables not found on the player object");
        }
    }
}
}
