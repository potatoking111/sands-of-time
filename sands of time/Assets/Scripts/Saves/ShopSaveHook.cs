using System.Collections;
using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

public class ShopSaveHook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ShopManager shopManager;
    private Save.ShopSlotSaveObject save;
    private float saveInterval = 30f;
    private float saveTimer;
    private bool isReady = false;

    public void StartSaver()
    {


        
        if (!SaveManager.TryGetActiveSlotSaveObject<Save.ShopSlotSaveObject>(out save))
        {
            return;
        }

        SaveManager.GameSaveCompletedEvt.Add(OnSave);
        SaveManager.GameLoadedEvt.Add(OnLoad);

        OnLoad();

        isReady = true;
    }

    void Update()
    {
        if (!isReady) return;
        saveTimer += Time.deltaTime;
        if (saveTimer >= saveInterval)
        {
            saveTimer = 0f;
            ForceSave();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && isReady)
            ForceSave();
    }

    void OnApplicationQuit()
    {
        if (isReady)
            ForceSave();
    }

    void OnDestroy()
    {
        SaveManager.GameSaveCompletedEvt.Remove(OnSave);
        SaveManager.GameLoadedEvt.Remove(OnLoad);
    }

    public void ForceSave()
    {
        if (!isReady || save == null) return;
        OnSave();
        SaveManager.SaveGame();
        // Debug.Log($"[PlayerSaveHook] ForceSave: {playerVariables.playerInventoryScript.inventory.Count} inventory, {playerVariables.playerInventoryScript.equipment.Count} equipment");
    }

    void OnSave()
    {
        if (save == null || shopManager == null) return;


        SaveStock();
    }

    void OnLoad()
    {
        if (save == null || shopManager == null) return;



        LoadStock();


    }

    void SaveStock()
    {

        save.GetValue<List<int>>("stock").Value = shopManager.stock;
    }

    void LoadStock()
    {
        var stock = save.GetValue<List<int>>("stock").Value;
        Debug.Log("ASHASHDAHSDFH"+stock);

        if (stock == null) return;

        shopManager.stock = stock;
    }

}
