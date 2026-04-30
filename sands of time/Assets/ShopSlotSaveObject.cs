using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

namespace Save
{
    public class ShopSlotSaveObject : SlotSaveObject
    {


        public SaveValue<List<int>> stock =
            new SaveValue<List<int>>("stock", new List<int>());

    }
}