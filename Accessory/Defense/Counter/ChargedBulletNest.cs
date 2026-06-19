using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;
using System.Runtime.InteropServices;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class ChargedBulletNest : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";
        public override void SetStaticDefaults()
        {


        }

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
            player.GetModPlayer<ChargedBulletNestPlayer>().hasChargedBulletNest = true;
        }

        
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Revolver, 1);
            recipe.AddIngredient(ItemID.HighVelocityBullet, 6);
            recipe.AddIngredient(ItemID.EmptyBucket, 6);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}