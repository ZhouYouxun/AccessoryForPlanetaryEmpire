using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj
{
    public class EchoBoundary : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Echo Boundary");
            //Tooltip.SetDefault("Projectiles bounce off the screen edges up to 10 times,\n" +
            //    "increasing damage by 15% with each bounce.");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 启用 EchoBoundaryPlayer 中的开关
            player.GetModPlayer<EchoBoundaryPlayer>().hasEchoBoundary = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.EchoBlock, 15); // 回森方块
            recipe.AddIngredient(ItemID.EchoMonolith, 1); // 回森家具
            recipe.AddIngredient(ItemID.Nanites, 100); // 纳米机器人
            recipe.AddTile(TileID.MythrilAnvil); // 在秘银砧处制作
            recipe.Register();
        }
    }
}