using System;
using System.Runtime.InteropServices;

using Flux.Utilities;
using Flux.WoW.Patchables;

namespace Flux.WoW
{
    public static class LootWindow
    {
        public static int LootItems
        {
            get
            {
                if (FluxWoW.Me.IsValid)
                {
                    return Lua.GetReturnVal<int>("GetNumLootItems()", 0);
                }

                return 0;
            }
        }

        public static uint GetItemId(int slot)
        {
            var field = (IntPtr) (GlobalOffsets.CGLootInfo__ItemArray + (uint) (32 * slot));
            return (uint) Marshal.ReadInt32(field);
        }

        public static WoWCache.ItemInfo GetItem(int slot)
        {
            return FluxWoW.Cache[CacheDb.Item].GetInfoBlockById(GetItemId(slot)).Item;
        }

        public static void Loot(int slot)
        {
            Lua.DoString("LootSlot(" + (slot + 1) + ")"); //so everything is a 0 based index
        }

        public static void LootAll()
        {
            for (int i = 0; i < LootItems; i++)
            {
                Logging.WriteDebug("Trying to loot " + GetItem(i).Name);
                Loot(i);
            }

            Close();
        }

        public static void Close()
        {
            Logging.WriteDebug("Closing the loot frame...");
            Lua.DoString("LootFrame:Hide()");
        }

        public static bool IsOpen()
        {
            return Lua.GetReturnVal<bool>("LootFrame:IsShown()", 0);
        }
    }
}