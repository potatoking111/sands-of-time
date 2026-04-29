using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public GameObject[] objs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteObjects()
    {
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
    }
}
