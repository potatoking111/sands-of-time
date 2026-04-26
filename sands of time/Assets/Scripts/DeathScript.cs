using System;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public Action OnDeath;

    public GameObject overworld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AreaManagerScript.OnLoadAction += () => {AreaManagerScript.DeleteCurrentGlobal();};
        OnDeath += ()=> {AreaManagerScript.LoadAreaGlobal(overworld);};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
