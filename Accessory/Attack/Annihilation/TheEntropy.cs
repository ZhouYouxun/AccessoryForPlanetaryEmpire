using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    internal class TheEntropy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 教徒后：Red，价值35金
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TheEntropyPlayer>().hasTheEntropy = true; // 开启效果
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10); // 叶绿定
            recipe.AddIngredient(ItemID.Ectoplasm, 25); // 25个幽灵质
            recipe.AddIngredient(ItemID.InvisibilityPotion, 5); // 隐身药水
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
