using UnityEngine;

public class PlayerAnimatorUpdater : MonoBehaviour
{
    public PlayerVariables variables;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       animator.SetFloat("Speed", Mathf.Abs(variables.rigidBody.linearVelocity.x));
        animator.SetBool("IsGrounded", variables.isOnGround);
    }
}
