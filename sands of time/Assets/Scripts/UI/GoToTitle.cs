using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToTitle : MonoBehaviour
{
    public string titleScreenName = "Title Scene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadTitle()
    {
        FindFirstObjectByType<PlayerSaveHook>().ForceSave();

        SceneManager.LoadScene(titleScreenName);
        // then do your scene load / title screen transition
    }
}
