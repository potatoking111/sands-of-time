using UnityEngine;
using UnityEngine.Events;

public class LevelCompleteScript : MonoBehaviour
{
    public UnityEvent LevelCompleteEvent;
    public PlayerVariables overworldVariables;
    public GameObject overworld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            LevelCompleteEvent.Invoke();
        }
    }

    public void RewardMoney(int amount)
    {
        overworldVariables.money += amount;
    }
    public void GoToArea( )
    {
        AreaManagerScript.LoadAreaGlobal(overworld);
    }
}
