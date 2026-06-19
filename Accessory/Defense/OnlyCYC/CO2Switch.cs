using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class CO2Switch : ModItem, ILocalizedModType
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

            // 检查玩家是否浸没在液体中
            if (player.wet)
            {
                // 增加20%的最大生命值
                player.statLifeMax2 += (int)(player.statLifeMax2 * 0.2f);

                // 增加15点生命回复
                player.lifeRegen += 15;

                // 增加25点防御力
                player.statDefense += 25;

                // 增加20%的移动速度
                player.moveSpeed += 0.2f;

                // 增加20%的飞行时间
                player.wingTimeMax = (int)(player.wingTimeMax * 1.2f);
            }
            else
            {
                // 当玩家不在液体中时，没有任何惩罚
            }
        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.SkyMill, 1);
            recipe.AddIngredient(ItemID.Bottle, 15);
            recipe.AddIngredient(ItemID.GillsPotion, 5);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddIngredient(ItemID.Seashell, 3);
            recipe.AddIngredient(ItemID.Starfish, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}
