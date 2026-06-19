using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System.IO.Pipelines;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj
{
    public class Universalgravitation : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj";

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
            player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10); // 圣锭
            recipe.AddIngredient(ItemID.SoulofSight, 5); // 视觉之魂
            recipe.AddIngredient(ItemID.GravitationPotion, 5);
            recipe.AddIngredient(ItemID.FeatherfallPotion, 5);
            recipe.AddTile(TileID.MythrilAnvil); // 在MythrilAnvil制造
            recipe.Register();
        }
    }
}
