using UnityEngine;

public class LevelEntryScript : MonoBehaviour
{

    public PlayerVariables playerVariables;

    public PlayerVariables overworldPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {

    }
    public void LevelStartup()
    {
        Debug.Log(overworldPlayer);
        playerVariables.playerInventoryScript = overworldPlayer.playerInventoryScript;
        foreach (GameObject equip in playerVariables.playerInventoryScript.equipment)
        {
            EquipmentScript equipScript = equip.GetComponent<EquipmentScript>();
            equipScript.playerVariables = playerVariables;
            equipScript.EquipEvent?.Invoke();
        }

        GiveUp giveUp = FindAnyObjectByType<GiveUp>(FindObjectsInactive.Include);
        GoToTitle titleScreen = FindAnyObjectByType<GoToTitle>(FindObjectsInactive.Include);
        giveUp.gameObject.SetActive(true);
        Debug.Log(playerVariables);
        giveUp.playerVariables = playerVariables;
        Debug.Log(giveUp.playerVariables);
        titleScreen.gameObject.SetActive(false);

        AreaManagerScript.OnLoadAction += () =>
        {
            titleScreen.gameObject.SetActive(true);
            giveUp.gameObject.SetActive(false);
        };

    }
}
