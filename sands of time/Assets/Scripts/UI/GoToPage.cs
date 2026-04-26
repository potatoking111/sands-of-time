using UnityEngine;

public class GoToPage : MonoBehaviour
{   
    public GameObject page;
    public GameObject oldPage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchPage()
    {
        Debug.Log("tryna switch pages");
        page.SetActive(true);
        oldPage.SetActive(false);
    }
}
