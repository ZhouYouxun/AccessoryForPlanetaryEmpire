using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class AorticThruster : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
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
            player.GetModPlayer<AorticThrusterPlayer>().hasAorticThruster = true; // 激活效果
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Autohammer, 1); // 自动锤炼机
            recipe.AddIngredient(ItemID.ShroomiteBar, 5); // 蘑菇金属锭
            recipe.AddIngredient(ItemID.MySon, 1); // 蘑菇人的“我的儿子”这幅画
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
