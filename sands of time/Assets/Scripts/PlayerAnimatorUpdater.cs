using System;
using UnityEngine;

public class PlayerAnimatorUpdater : MonoBehaviour
{
    public PlayerVariables variables;
    public Animator animator;
    public bool ignoreGround = false;
    public bool incorporateY = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 0;
        if (incorporateY)
        {
            speed = variables.rigidBody.linearVelocity.magnitude;
        }
        else
        {
            speed = Math.Abs(variables.rigidBody.linearVelocity.x);
        }
       animator.SetFloat("Speed", speed);
       bool onGround = variables.isOnGround;
       if (ignoreGround)
        {
            onGround = true;
        }
        animator.SetBool("IsGrounded", onGround);
    }
}
