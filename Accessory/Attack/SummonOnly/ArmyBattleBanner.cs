using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.IO.Pipelines;
using System.Collections;
using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly
{
    public class ArmyBattleBanner : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";

        public override void SetStaticDefaults()
        {


        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ArmyBattleBannerPlayer>().hasArmyBattleBanner = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.RedBanner, 1);
            recipe.AddIngredient(ItemID.GreenBanner, 1);
            recipe.AddIngredient(ItemID.BlueBanner, 1);
            recipe.AddIngredient(ItemID.YellowBanner, 1);
            //recipe.AddIngredient(ItemID.WarTableBanner, 1); // 战争桌旗
            //recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
}
