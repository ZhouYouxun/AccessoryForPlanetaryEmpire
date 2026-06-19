using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj.PermanentModProj
{
    public class MomentumChip : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj.PermanentModProj";
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
            player.GetModPlayer<MomentumChipPlayer>().hasMomentumChip = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10); // 圣锭
            recipe.AddIngredient(ItemID.SoulofMight, 5); // 力量之魂
            recipe.AddIngredient(ItemID.SandBlock, 100); // 沙子[做芯片]           LavaLamp？   
            recipe.AddIngredient(ItemID.Wire, 50); // 电线
            recipe.AddIngredient(ItemID.Cog, 50);  // 齿轮
            recipe.AddTile(TileID.MythrilAnvil); // 在MythrilAnvil制造
            recipe.Register();
        }
    }
}