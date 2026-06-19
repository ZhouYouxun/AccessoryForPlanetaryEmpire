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
    public class EnergizedChitin : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Energized Chitin");
            //Tooltip.SetDefault("Minions have a chance to block enemy projectiles when near the player\n" +
            //                   "Minions have a chance to enhance friendly projectiles when far from the player");
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
            player.GetModPlayer<EnergizedChitinPlayer>().hasEnergizedChitin = true;
        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            //recipe.AddIngredient(ItemID.SummonerEmblem, 1); // 召唤师徽章
            //recipe.AddIngredient(ItemID.SoulofFright, 25); // 灵液
            //recipe.AddIngredient(ItemID.HallowedBar, 10); // 神圣锭
            recipe.AddIngredient(ItemID.Bottle, 20); // 玻璃瓶
            recipe.AddIngredient(ItemID.Wire, 150); // 电线
            recipe.AddIngredient(ItemID.WandofSparking, 1); // 火花魔棒
            //recipe.AddIngredient(ItemID.ChlorophyteBar, 15); // 叶绿锭
            recipe.AddIngredient(ItemID.BeetleHusk, 15); // 甲虫壳
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }
    }
}