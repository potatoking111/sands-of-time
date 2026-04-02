using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField]
    public List<SwordStateEntry> swordStates;
    private Dictionary<Vector2,GameObject> swordsKey;
    private PlayerVariables variables;
    public Action attackAction;
    private float nextAttackTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
        attackAction += Attack;
        swordsKey = new Dictionary<Vector2, GameObject>();
        foreach (SwordStateEntry state in swordStates)
        {
            swordsKey.Add(state.direction,state.swordPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Attack()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }
        nextAttackTime = Time.time + variables.atttackTimePeriod;
        GameObject swordPrefab = swordsKey[variables.playerFacing];
        BoxCollider2D swordBox = swordPrefab.GetComponent<BoxCollider2D>();
        Vector3 offset = new Vector3(variables.playerFacing.x*variables.hitbox.size.x/2,variables.playerFacing.y*variables.hitbox.size.y/2,0);
        Vector3 swordSizeOffset = new Vector3(swordBox.size.x/2,swordBox.size.y/2,0) * variables.playerFacing;
        GameObject attackObject = Instantiate(swordsKey[variables.playerFacing],variables.rigidBody.transform.position+offset+swordSizeOffset,variables.rigidBody.transform.rotation,variables.rigidBody.transform);
        attackObject.GetComponent<SwordDamageScript>().variables = variables;
        Destroy(attackObject,variables.atttackTimePeriod);
    }
}

[System.Serializable]
public class SwordStateEntry
{
    public Vector2 direction;
    public GameObject swordPrefab;
}