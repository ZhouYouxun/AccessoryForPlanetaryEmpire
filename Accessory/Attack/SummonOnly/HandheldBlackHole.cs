using System;
using System.Collections.Generic;
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
    // 哨兵栏+2，将哨兵吸附在玩家身边，跟随玩家移动
    public class HandheldBlackHole : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
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
            player.GetModPlayer<HandheldBlackHolePlayer>().hasHandheldBlackHole = true;
            player.maxTurrets += 2;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.SummonerEmblem, 1); // 召唤师徽章
            recipe.AddIngredient(ItemID.WarTable, 1); // 战争桌
            recipe.AddIngredient(ItemID.VoidVault, 1); // 虚空保险库
            recipe.AddIngredient(ItemID.ShadowPaint, 50); // 暗影漆
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }




    }
}
