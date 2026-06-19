using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Players.Movement;
using static AccessoryForPlanetaryEmpire.Global.PLGlobalItem;

namespace AccessoryForPlanetaryEmpire.Accessory.Dash
{
    public class ShallowOxygenBottle : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Dash";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shallow Oxygen Bottle");
            //Tooltip.SetDefault("Double-tap left or right to dash.\nCan also be activated with a custom keybind.");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值25金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // player.GetModPlayer<ShallowOxygenBottlePlayer>().hasShallowOxygenBottle = true;

            if (hideVisual)
            {
                player.GetModPlayer<ShallowOxygenBottlePlayer>().hasShallowOxygenBottleAD = true;
                player.GetModPlayer<ShallowOxygenBottlePlayer>().hasShallowOxygenBottle = false;
            }
            else
            {
                player.GetModPlayer<ShallowOxygenBottlePlayer>().hasShallowOxygenBottleAD = false;
                player.GetModPlayer<ShallowOxygenBottlePlayer>().hasShallowOxygenBottle = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ArcticDivingGear);
            recipe.AddIngredient(ItemID.FishronWings, 1);
            recipe.AddIngredient(ItemID.SharkFin, 15);
            recipe.AddIngredient(ItemID.Coral, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

    }
}
