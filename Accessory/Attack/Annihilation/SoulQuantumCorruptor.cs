using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    public class SoulQuantumCorruptor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";
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
            player.GetModPlayer<SoulQuantumCorruptorPlayer>().hasSoulQuantumCorruptor = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ShadowScale, 10); // 10个暗影鳞片
            recipe.AddIngredient(ItemID.RottenChunk, 5); // 5个腐肉
            recipe.AddIngredient(ItemID.FallenStar, 3); // 3个坠落之星
            recipe.AddIngredient(ItemID.Bone, 15); // 15个骨头
            recipe.AddIngredient(ItemID.BookofSkulls, 1); // 需要一个魔法书
            recipe.AddTile(TileID.TinkerersWorkbench); // 在工匠作坊制作
            recipe.Register();
        }
    }
}
