using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.LazyItems.Tool;
using Terraria.ModLoader.Default;
using AccessoryForPlanetaryEmpire.Accessory.Y_TestAcc;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Accessory;

namespace AccessoryForPlanetaryEmpire.Players
{
    public class GiveItemPlayer : ModPlayer
    {
        public bool hasReceivedStartingItem = false;

        public override void OnEnterWorld()
        {
            if (!hasReceivedStartingItem)
            {
                Player.QuickSpawnItem(Player.GetSource_Misc("AccessoryForPlanetaryEmpire"), ModContent.ItemType<StellarCleaver>());
                Player.QuickSpawnItem(Player.GetSource_Misc("AccessoryForPlanetaryEmpire"), ModContent.ItemType<Dreamer>());
                Player.QuickSpawnItem(Player.GetSource_Misc("AccessoryForPlanetaryEmpire"), ModContent.ItemType<StarPack>()); 
                hasReceivedStartingItem = true;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["hasReceivedStartingItem"] = hasReceivedStartingItem;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("hasReceivedStartingItem"))
            {
                hasReceivedStartingItem = tag.GetBool("hasReceivedStartingItem");
            }
        }



    }
}
