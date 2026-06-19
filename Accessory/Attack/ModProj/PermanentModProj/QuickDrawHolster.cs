using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using System;
using Microsoft.Build.Tasks;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj.PermanentModProj
{
    public class QuickDrawHolster : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj.PermanentModProj";

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
            player.GetModPlayer<QuickDrawHolsterPlayer>().hasQuickDrawHolster = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.TitanGlove, 1); // 泰坦手套
            recipe.AddIngredient(ItemID.PhoenixBlaster, 1); // 凤凰爆破枪
            recipe.AddIngredient(ItemID.Silk, 15); // 15个丝绸
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
