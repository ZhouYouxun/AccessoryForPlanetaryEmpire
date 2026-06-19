using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Gambler
{
    public class LuckyCoin : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Gambler";
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
            player.GetModPlayer<LuckyCoinPlayer>().hasLuckyCoin = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.LargeEmerald, 1);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 20);
            recipe.AddIngredient(ItemID.CopperCoin, 99);
            recipe.AddIngredient(ItemID.SilverCoin, 99);
            recipe.AddIngredient(ItemID.GoldCoin, 99);
            //recipe.AddIngredient(ItemID.PlatinumCoin, 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
