using System;
using UnityEngine;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    public  Action<Vector2> MoveAction {get;set;}
    private PlayerVariables variables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        variables = gameObject.GetComponent<PlayerVariables>();
    }
    void OnEnable()
    {
        MoveAction+=Move;
    }
    void OnDisable()
    {
        MoveAction-=Move;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector2 dirVector)
    {
        variables.playerFacing = dirVector;
        Vector2 moveVector = new Vector2(dirVector.x*variables.movementSpeed,variables.rigidBody.linearVelocity.y);
        variables.rigidBody.linearVelocity = moveVector;
        
    }
}
