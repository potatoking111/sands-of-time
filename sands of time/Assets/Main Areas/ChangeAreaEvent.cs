using System;
using UnityEngine;

public class ChangeAreaEvent : MonoBehaviour
{
    public GameObject newArea;
    public bool check = false;
    public PlayerVariables variables;
    public void ChangeArea()
    {
        if (check && !variables.levelTwoUnlocked)
        {
            return;
        }
        AreaManagerScript.LoadAreaGlobal(newArea);
    }
}