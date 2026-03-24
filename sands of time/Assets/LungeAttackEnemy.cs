using UnityEngine;

public class LungeAttackEnemy : MonoBehaviour
{
    EnemyVariables variables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<EnemyVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
