using UnityEngine;
using CarterGames.Assets.SaveManager;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine.SceneManagement;

public class SaveCreate : MonoBehaviour
{
    public string gameSceneName = "MainScene";
    public static int PendingSlotIndex = -1;

    public void EnterSave(int index)
    {
        Debug.Log($"[SaveCreate] Slot selected: {index}");

        // Create the slot if it doesn't exist yet
        if (!SaveSlotManager.AllSlots.ContainsKey(index))
        {
            Debug.Log($"[SaveCreate] Slot {index} does not exist, creating it.");
            SaveSlotManager.TryCreateSlotAtId(index, out _);
        }

        SaveSlotManager.LoadSlot(index);
        Debug.Log($"[SaveCreate] Active slot is now: {SaveSlotManager.ActiveSlotId}");

        PendingSlotIndex = index;
        SceneManager.LoadScene(gameSceneName);
    }
}