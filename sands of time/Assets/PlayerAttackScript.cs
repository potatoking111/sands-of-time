using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public Dictionary<Vector2,GameObject> swordStates;
    private PlayerVariables variables;
    public Action attackAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
        attackAction += Attack;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Attack()
    {
        Instantiate(swordStates[variables.playerFacing],variables.rigidBody.transform,true);

    }
}
