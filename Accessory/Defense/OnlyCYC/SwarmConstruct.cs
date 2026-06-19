using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;


namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    internal class SwarmConstruct : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";


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
            player.GetModPlayer<SwarmConstructPlayer>().hasSwarmConstruct = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HoneyDispenser, 1); // 蜂蜜分配机
            recipe.AddIngredient(ItemID.HoneyBlock, 70); // 蜂蜜块
            recipe.AddIngredient(ItemID.BottledHoney, 35); // 瓶装蜂蜜
            recipe.AddIngredient(ItemID.HoneyComb, 1); // 蜂窝
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }


}
