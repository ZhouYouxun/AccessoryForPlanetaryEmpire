using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    public class AntimatterSynthesizer : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";
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
            player.GetModPlayer<AntimatterSynthesizerPlayer>().hasAntimatterSynthesizer = true; // 激活效果
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 1个通用物品
            recipe.AddIngredient(ItemID.ChlorophyteBar, 15); // 15个叶绿锭
            recipe.AddIngredient(ItemID.LihzahrdBrick, 50); // 15个蜥蜴砖
            recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1); // 1个蜥蜴能量电池
            recipe.AddTile(TileID.LihzahrdFurnace); // 在丛林蜥蜴熔炉处制作
            recipe.Register();
        }
    }
}
