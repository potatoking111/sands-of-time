using System;
using UnityEngine;

public class OverworldAnimatorUpdater : MonoBehaviour
{
    public PlayerVariables variables;
    public Animator animator;
    Vector2 prevFacing = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float speed = variables.rigidBody.linearVelocity.magnitude;

       animator.SetFloat("Speed", speed);

        animator.SetInteger("facingX", Mathf.RoundToInt(variables.playerFacing.x));
        animator.SetInteger("facingY", -Mathf.RoundToInt(variables.playerFacing.y));
        animator.SetBool("changedFace",prevFacing != variables.playerFacing);
        prevFacing = variables.playerFacing;

    }
}
