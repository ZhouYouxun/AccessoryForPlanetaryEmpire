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
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class JupiterAccessory : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jupiter Accessory");
            // Tooltip.SetDefault("Summons Jupiter that deals collision damage and explodes.");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值35金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<JupiterPlayer>().hasJupiterAccessory = true;
        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 自定义物品
            recipe.AddIngredient(ItemID.ChainGuillotines, 1); // 血滴铁链子【腐化宝箱怪】
            //recipe.AddIngredient(ItemID.ChainKnife, 1); // 链刀【小丑和蝙蝠】
            recipe.AddIngredient(ItemID.TheMeatball, 1); // 血肉之球【猩红矿锭】
            recipe.AddIngredient(ItemID.BlueMoon, 1); // 蓝月
            recipe.AddIngredient(ItemID.Sunfury, 1); // 阳炎之怒
            //recipe.AddIngredient(ItemID.DripplerFlail, 1); // 滴滴怪致残者
            recipe.AddIngredient(ItemID.DaoofPow, 1); // 太极连枷
            recipe.AddIngredient(ItemID.FlowerPow, 1); // 花之力
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }

    }
}
