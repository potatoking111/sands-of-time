using UnityEngine;

public class DoubleJumpScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int JumpAmount {get;set;}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableDoubleJump(EquipmentScript equipScript)
    {
        PlayerMovement movementScript = equipScript.playerVariables.playerMovementScript;
        movementScript.JumpPossibleRequirements.Add(()=>JumpAmount < 1);
        movementScript.TouchGroundAction += ()=>JumpAmount = 0;
        movementScript.JumpAction += ()=> JumpAmount +=1;
    }
    public void DisableDoubleJump(EquipmentScript equipScript)
    {
        PlayerMovement movementScript = equipScript.playerVariables.playerMovementScript;
        movementScript.JumpPossibleRequirements.Remove(()=>JumpAmount < 1);
        movementScript.TouchGroundAction -= ()=>JumpAmount = 0;
        movementScript.JumpAction -= ()=> JumpAmount +=1;
    }
}
