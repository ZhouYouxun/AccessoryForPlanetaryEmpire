using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;
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

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly
{
    public class MedicalDrones : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Medical Drones");
            //Tooltip.SetDefault("Reduces summon damage by 45%\n" +
            //                   "Each minion provides 25 max life and 7 life regeneration\n" +
            //                   "Up to 20 minions can increase these effects\n" +
            //                   "Minions have a 30% chance to lifesteal based on their damage");
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
            player.GetModPlayer<MedicalDronesPlayer>().hasMedicalDrones = true;
            player.GetDamage<SummonDamageClass>() -= 0.45f; // 降低45%召唤伤害
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            //recipe.AddIngredient(ItemID.SummonerEmblem, 1); // 召唤师徽章
            recipe.AddIngredient(ItemID.GlowingMushroom, 50); // 荧光蘑菇
            recipe.AddIngredient(ItemID.GreaterHealingPotion, 10); // 强效治疗药水
            recipe.AddIngredient(ItemID.Nanites, 50); // 纳米机器人
            recipe.AddIngredient(ItemID.JimsDrone, 1); // 四轴竞速无人机
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }








    }
}