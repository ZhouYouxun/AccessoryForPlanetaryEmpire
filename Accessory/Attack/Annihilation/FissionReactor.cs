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
    internal class FissionReactor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";

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
            player.GetModPlayer<FissionReactorPlayer>().hasFissionReactor = true; // 激活效果
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 1个通用物品
            recipe.AddIngredient(ItemID.HallowedBar, 15); // 神圣定
            recipe.AddIngredient(ItemID.SoulofMight, 5); // 力量之魂
            recipe.AddIngredient(ItemID.HellstoneBar, 25); // 狱石锭
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 25);
            recipe.AddIngredient(ItemID.WaterBucket, 15); // 水桶
            recipe.AddIngredient(ItemID.Cog, 55); // 齿轮
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧
            recipe.Register();
        }
    }
}