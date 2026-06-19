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
    public class LimitResponseModule : ModItem, ILocalizedModType
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
            player.GetModPlayer<LimitResponseModulePlayer>().hasLimitResponseModule = true; // 激活效果
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1); // 1个通用物品
            recipe.AddIngredient(ItemID.HallowedBar, 15); // 神圣定
            recipe.AddIngredient(ItemID.SoulofMight, 5); // 视域之魂
            recipe.AddIngredient(ItemID.Wire, 50); // 50个电线
            recipe.AddIngredient(ItemID.Timer3Second, 1); // 1个3秒计时器
            recipe.AddIngredient(ItemID.LifeFruit, 3); // 3个生命果
            recipe.AddIngredient(ItemID.SharkFin, 2); // 2个鲨鱼鳍
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}