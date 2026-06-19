using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AccessoryForPlanetaryEmpire.Global.PLGlobalItem;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj.PermanentModProj
{
    internal class BreakingDrill : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj.PermanentModProj";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Breaking Drill");
            //Tooltip.SetDefault("Increases damage based on enemy's defense and damage reduction.");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值15金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BreakingDrillPlayer>().hasBreakingDrill = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10); // 圣锭
            recipe.AddIngredient(ItemID.SoulofFright, 5); // 恐惧之魂
            recipe.AddIngredient(ItemID.CobaltDrill, 1); // 钴钻头
            recipe.AddIngredient(ItemID.PalladiumDrill, 1); // 钯金钻头
            recipe.AddTile(TileID.MythrilAnvil); // 在MythrilAnvil制造
            recipe.Register();
        }
    }
}