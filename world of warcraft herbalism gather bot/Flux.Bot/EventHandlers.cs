using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flux.WoW;

namespace Flux.Bot
{
    public static class EventHandlers
    {
        [WoWEvent("LOOT_OPENED")]
        public static void HandleLoot(WoWEventArgs e)
        {
            LootWindow.LootAll();
        }

        [WoWEvent("LOOT_BIND_CONFIRM")]
        [WoWEvent("EQUIP_BIND_CONFIRM")]
        [WoWEvent("AUTOEQUIP_BIND_CONFIRM")]
        public static void HandleEquipBindConfirm(WoWEventArgs e)
        {
            // This *should* handle both cases just fine
            Lua.DoString("ConfirmBindOnUse()");
        }
    }
}
