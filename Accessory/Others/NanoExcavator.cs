using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;


namespace AccessoryForPlanetaryEmpire.Accessory.Others
{
    public class NanoExcavator : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Others";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 教徒后：Red，价值55金
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 55, 0, 0);
            Item.value = Item.sellPrice(0, 55, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NanoExcavatorPlayer>().hasNanoExcavator = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.DrillContainmentUnit, 1);
            //recipe.AddIngredient(ItemID.MeteoriteBar, 1);
            //recipe.AddIngredient(ItemID.HellstoneBar, 1);
            //recipe.AddIngredient(ItemID.HallowedBar, 1);
            //recipe.AddIngredient(ItemID.ChlorophyteBar, 1);
            //recipe.AddIngredient(ItemID.ShroomiteBar, 1);
            //recipe.AddIngredient(ItemID.SpectreBar, 1); 
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
