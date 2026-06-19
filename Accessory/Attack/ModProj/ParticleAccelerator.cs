using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj
{
    public class ParticleAccelerator : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Particle Accelerator");
            //Tooltip.SetDefault("Increases projectile speed over time up to 300% after 3 seconds.");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ParticleAcceleratorPlayer>().hasParticleAccelerator = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.AdamantiteBar, 10); // 精金锭
            recipe.AddIngredient(ItemID.CrystalShard, 5); // 水晶碎块
            recipe.AddTile(TileID.MythrilAnvil); // 秘银砧或山铜砧
            recipe.Register();
        }
    }
}