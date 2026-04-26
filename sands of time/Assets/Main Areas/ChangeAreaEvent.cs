using System;
using UnityEngine;

public class ChangeAreaEvent : MonoBehaviour
{
    public GameObject newArea;

    public void ChangeArea()
    {
        AreaManagerScript.LoadAreaGlobal(newArea);
    }
}