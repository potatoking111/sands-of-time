using System.Collections;
using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

public class PlayerSaveHook : MonoBehaviour
{
    public PlayerVariables playerVariables;
    public List<GameObject> itemPrefabs;

    private Save.PlayerSaveSaveObject save;
    private float saveInterval = 30f;
    private float saveTimer;
    private bool isReady = false;

    IEnumerator Start()
    {
        int slotToLoad = SaveCreate.PendingSlotIndex;
        SaveCreate.PendingSlotIndex = -1;

        yield return new WaitUntil(() => SaveManager.IsInitialized);

        if (!SaveSlotManager.AllSlots.ContainsKey(slotToLoad))
        {
            Debug.Log($"[PlayerSaveHook] Creating slot {slotToLoad}");
            SaveSlotManager.TryCreateSlotAtId(slotToLoad, out _);
        }

        SaveSlotManager.LoadSlot(slotToLoad);
        SaveManager.LoadGame();

        yield return new WaitUntil(() => !SaveManager.IsLoading);

        SaveSlotManager.LoadSlot(slotToLoad);

        if (!SaveManager.TryGetActiveSlotSaveObject<Save.PlayerSaveSaveObject>(out save))
        {
            Debug.LogError($"[PlayerSaveHook] PlayerSaveSaveObject not found in slot {slotToLoad}.");
            yield break;
        }

        SaveManager.GameSaveCompletedEvt.Add(OnSave);
        SaveManager.GameLoadedEvt.Add(OnLoad);

        OnLoad();

        isReady = true;
        Debug.Log($"[PlayerSaveHook] Save system ready on slot {slotToLoad}.");

        foreach (ShopSaveHook shopHook in FindObjectsByType<ShopSaveHook>(FindObjectsSortMode.None)) // TESTING
        {
            shopHook.StartSaver();
        }


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
        Debug.Log($"[PlayerSaveHook] ForceSave: {playerVariables.playerInventoryScript.inventory.Count} inventory, {playerVariables.playerInventoryScript.equipment.Count} equipment");
    }

    void OnSave()
    {
        if (save == null || playerVariables == null) return;

        save.GetValue<Vector3>("player_position").Value = transform.position;
        save.GetValue<float>("player_money").Value = playerVariables.money;

        SaveInventory();
        SaveEquipment();
    }

    void OnLoad()
    {
        if (save == null || playerVariables == null) return;

        transform.position = save.GetValue<Vector3>("player_position").Value;
        playerVariables.money = save.GetValue<float>("player_money").Value;

        LoadInventory();
        LoadEquipment();

        if (playerVariables.playerInventoryScript.isInventoryOpen)
            playerVariables.playerInventoryScript.DisplayInventory(false);
    }

    void SaveInventory()
    {
        var ids = new List<string>();
        foreach (var item in playerVariables.playerInventoryScript.inventory)
        {
            if (!item) continue;
            var s = item.GetComponent<ItemScript>();
            if (s) ids.Add(s.itemId);
        }
        save.GetValue<List<string>>("inventory_items").Value = ids;
    }

    void LoadInventory()
    {
        var ids = save.GetValue<List<string>>("inventory_items").Value;
        if (ids == null) return;

        foreach (var item in playerVariables.playerInventoryScript.inventory)
            if (item != null && item.scene.isLoaded) Destroy(item);
        playerVariables.playerInventoryScript.inventory.Clear();

        foreach (var id in ids)
        {
            var prefab = itemPrefabs.Find(p => p.GetComponent<ItemScript>()?.itemId == id);
            if (!prefab) continue;
            var obj = Instantiate(prefab);
            obj.GetComponent<ItemScript>().original = obj;
            playerVariables.playerInventoryScript.inventory.Add(obj);
        }
    }

    void SaveEquipment()
    {
        var ids = new List<string>();
        foreach (var item in playerVariables.playerInventoryScript.equipment)
        {
            if (!item) continue;
            var s = item.GetComponent<ItemScript>();
            if (s) ids.Add(s.itemId);
        }
        save.GetValue<List<string>>("equipment_items").Value = ids;
    }

    void LoadEquipment()
    {
        var ids = save.GetValue<List<string>>("equipment_items").Value;
        if (ids == null) return;

        foreach (var item in playerVariables.playerInventoryScript.equipment)
            if (item != null && item.scene.isLoaded) Destroy(item);
        playerVariables.playerInventoryScript.equipment.Clear();

        foreach (var id in ids)
        {
            var prefab = itemPrefabs.Find(p => p.GetComponent<ItemScript>()?.itemId == id);
            if (!prefab) continue;
            var obj = Instantiate(prefab);
            obj.GetComponent<ItemScript>().original = obj;
            playerVariables.playerInventoryScript.equipment.Add(obj);
        }
    }
}