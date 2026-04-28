using UnityEngine;

public class BrittleDamageScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float extraDamageTaken;
    public float extraDamageDone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableBrittleDamage(EquipmentScript equipScript)
    {
        PlayerTimeManager timeManager = equipScript.playerVariables.timeManagerScript;
        PlayerAttackScript attackManagerScript = equipScript.playerVariables.playerAttackScript;

        timeManager.TakeDamageAction += (incomingDamage,incomingDamageType) => {timeManager.TakeDamage(extraDamageTaken,DamageType.Equipment);}; // maybe rework to not be extra damage but literally replace the damage take aciton later
        foreach (SwordStateEntry swordPrefabEntry in attackManagerScript.swordStates)
        {
            SwordDamageScript damageScript = swordPrefabEntry.swordPrefab.GetComponent<SwordDamageScript>();
            damageScript.DealDamageAction += () => damageScript.hitEnemy.TakeDamageAction.Invoke(extraDamageDone);
        }
    }
    public void DisableBrittleDamage(EquipmentScript equipScript)
    {
        PlayerTimeManager timeManager = equipScript.playerVariables.timeManagerScript;
        PlayerAttackScript attackManagerScript = equipScript.playerVariables.playerAttackScript;

        timeManager.TakeDamageAction -= (incomingDamage,incomingDamageType) => {timeManager.TakeDamage(extraDamageTaken,DamageType.Equipment);}; // maybe rework to not be extra damage but literally replace the damage take aciton later
        foreach (SwordStateEntry swordPrefabEntry in attackManagerScript.swordStates)
        {
            SwordDamageScript damageScript = swordPrefabEntry.swordPrefab.GetComponent<SwordDamageScript>();
            damageScript.DealDamageAction -= () => damageScript.hitEnemy.TakeDamageAction.Invoke(extraDamageDone);
        }
    }

}
