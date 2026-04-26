using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

namespace Save
{
    public class PlayerSaveSaveObject : SlotSaveObject
    {
        [SerializeField] private SaveValue<Vector3> position =
            new SaveValue<Vector3>("player_position");

        [SerializeField] private SaveValue<float> money =
            new SaveValue<float>("player_money");

        public SaveValue<List<string>> inventory_items =
            new SaveValue<List<string>>("inventory_items", new List<string>());
        public SaveValue<List<string>> equipment_items =
                new SaveValue<List<string>>("equipment_items", new List<string>());
    }
}