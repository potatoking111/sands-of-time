using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractZone : MonoBehaviour
{
    public UnityEvent<GameObject> PlayerInteractTriggered;
    public bool PlayerInZone { get; private set; }

    public float senseRadius = 4f;
    private InputSystem_Actions inputActions;

    private CircleCollider2D interactCollider;
    private List<Func<PlayerVariables, bool>> conditions = new();
    public PlayerVariables playerVariables;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conditions.Add((playerVariables) => PlayerInZone);
    
    }
    void OnEnable()
    {
        if (inputActions == null)
        {
              inputActions = new InputSystem_Actions();
        }
        inputActions.Enable();
        if (interactCollider == null)
        {
            interactCollider = gameObject.AddComponent<CircleCollider2D>();
            interactCollider.isTrigger = true;

            interactCollider.radius = senseRadius;
         
        }


        inputActions.Player.Interact.performed += (context) =>
        {
            Debug.Log("checking!");
            foreach (var condition in conditions)
            {
                if (!condition(playerVariables))
                {
                    return;
                }
            }
            Debug.Log("triggering interact event");
            PlayerInteractTriggered.Invoke(gameObject);

        };

    }

    void OnDisable()
    {
        inputActions.Player.Interact.performed -= (context) =>
        {
            foreach (var condition in conditions)
            {
                if (!condition(playerVariables))
                {
                    return;
                }
            }
            PlayerInteractTriggered.Invoke(gameObject);

        };
    
    
        
    }
    
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("in zone!");
            PlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInZone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
