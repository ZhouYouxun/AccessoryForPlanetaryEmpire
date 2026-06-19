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
using AccessoryForPlanetaryEmpire.Players.Defense;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class SedativeRefrigerator : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";

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
            player.GetModPlayer<AttackIndicatorPlayer>().hasDefenseIndicator = true;
        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.IceMachine, 1); // 冰雪机
            //recipe.AddIngredient(ItemID.Leather, 3);
            recipe.AddIngredient(ItemID.BlizzardinaBottle, 1); // 暴雪瓶
            recipe.AddIngredient(ItemID.SnowBlock, 500); // 冰雪块
            recipe.AddIngredient(ItemID.FrostburnArrow, 500); // 霜冻箭
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }



    }
}
