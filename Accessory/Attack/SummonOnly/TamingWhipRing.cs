using AccessoryForPlanetaryEmpire.Players;
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
using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly
{
    public class TamingWhipRing : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Taming Whip Ring");
            //Tooltip.SetDefault("Reduces minion count by 3\n" +
            //                   "Reduces minion damage by 30%\n" +
            //                   "Minions have a chance to buff whip speed and damage on hit");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值15金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions -= 3; // Reduce minion count by 3
            player.GetDamage<SummonDamageClass>() *= 0.7f; // Reduce minion damage by 30%
            player.GetModPlayer<TamingWhipRingPlayer>().hasTamingWhipRing = true;

        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Chain, 15); // 链条
            recipe.AddIngredient(ItemID.Leather, 3); // 皮革
            recipe.AddIngredient(ItemID.ThornWhip, 1); // 丛林鞭
            recipe.AddIngredient(ItemID.BlandWhip, 1); // 皮鞭
            recipe.AddTile(TileID.Anvils); // 铁砧
            recipe.Register();
        }


    }
}