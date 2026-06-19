using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Players.Attack;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack
{
    public class SonicRing : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";
        public override void SetStaticDefaults()
        {

        }

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
            player.GetModPlayer<SonicRingPlayer>().hasSonicRing = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.DrumSet, 1); // 鼓组
            recipe.AddIngredient(ItemID.DrumStick, 1); // 鼓槌
            recipe.AddIngredient(ItemID.IvyGuitar, 1); // 常春藤小吉他
            recipe.AddIngredient(ItemID.CarbonGuitar, 1); // 雨歌小吉他
            recipe.AddIngredient(ItemID.MagicalHarp, 1); // 魔法竖琴
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}