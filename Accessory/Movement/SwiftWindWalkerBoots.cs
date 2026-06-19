using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Movement;


namespace AccessoryForPlanetaryEmpire.Accessory.Movement
{
    public class SwiftWindWalkerBoots : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Movement";
        // 疾风行者靴：开启技能后提升125%奔跑速度并且有无敌，开启技能到结束技能之后的10秒内无法造成伤害，持续时间5秒，冷却45秒
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
            player.GetModPlayer<WindrunnerBootsPlayer>().hasSwiftWindWalkerBoots = true;
            player.moveSpeed += 0.1f; 
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.TerrasparkBoots, 1);
            recipe.AddIngredient(ItemID.AmphibianBoots, 1);
            recipe.AddIngredient(ItemID.SandBoots, 1);
            recipe.AddIngredient(ItemID.FairyBoots, 1);
            recipe.AddIngredient(ItemID.LightShard, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

    }
}
