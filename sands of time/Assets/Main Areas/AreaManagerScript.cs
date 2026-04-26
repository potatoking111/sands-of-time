using System;
using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using CarterGames.Assets.SaveManager.Slots;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AreaManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> areas;
    public GameObject currentArea;
    public static Action OnLoadAction;
    void Start()
    {
        // SaveManager.LoadGame();

        currentArea = areas[0];
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void LoadAreaGlobal(GameObject area)
    {
        if (OnLoadAction != null)
        {
            OnLoadAction.Invoke();
  
        }
        OnLoadAction = null;
        AreaManagerScript script = FindFirstObjectByType<AreaManagerScript>();
        script.LoadArea(area);
    }
    public static void DeleteCurrentGlobal()
    {
        AreaManagerScript script = FindFirstObjectByType<AreaManagerScript>();
        script.DeleteCurrent();
    }
    private void DeleteCurrent()
    {
        areas.Remove(currentArea);

        currentArea.SetActive(false);
        Destroy(currentArea);
        currentArea = null;
    }
    public void LoadArea(GameObject areaPrefab)
    {
        if (currentArea != null)
            currentArea.SetActive(false);

        // Try to find an existing instance of this prefab
        foreach (GameObject existing in areas)
        {
            if (IsSamePrefab(existing, areaPrefab))
            {
                currentArea = existing;
                Debug.Log("Found existing area instance");
                Debug.Log(currentArea.name);
                currentArea.SetActive(true);
                existing.SetActive(true);
                Debug.Log("FINAL STATE:");
                Debug.Log("Name: " + currentArea.name);
                Debug.Log("Self: " + currentArea.activeSelf);
                Debug.Log("Hierarchy: " + currentArea.activeInHierarchy);
                return;
            }
        }

        // If not found, instantiate it
        GameObject newArea = Instantiate(areaPrefab);
        areas.Add(newArea);
        GameObject oldArea = currentArea;

        currentArea = newArea;

        LevelEntryScript entryScript = currentArea.GetComponent<LevelEntryScript>();
        if (entryScript != null)
        {
            Debug.Log("setting old overworld player");
            entryScript.overworldPlayer = oldArea.GetComponentInChildren<PlayerVariables>();
            entryScript.LevelStartup();
        }

        
        currentArea.SetActive(true);

    }

    bool IsSamePrefab(GameObject instance, GameObject prefab)
    {
        return PrefabUtility.GetCorrespondingObjectFromSource(instance) == prefab;
    }
    void OnApplicationQuit()
    {
        SaveManager.SaveGame();
    }

    public void DeleteArea(GameObject obj)
    {
        areas.Remove(obj);
        Destroy(obj);
    }


    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("scene unloaded");
        SaveManager.SaveGame();
    }
    }
