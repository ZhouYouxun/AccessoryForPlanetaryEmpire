using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccessoryForPlanetaryEmpire.Global.PLGlobalItem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Attack;
using Microsoft.Xna.Framework;



namespace AccessoryForPlanetaryEmpire.Accessory.Attack
{
    public class AntimatterNuclearMissile : ModItem, ILocalizedModType
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
            // 教徒后：Red，价值35金
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AntimatterNuclearMissilePlayer>().hasAntimatterNuclearMissile = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Detonator, 1); // 引爆器
            recipe.AddIngredient(ItemID.Timer5Second, 120); // 5秒计时器
            recipe.AddIngredient(ItemID.Dynamite, 3996); // 炸药
            recipe.AddIngredient(ItemID.Dynamite, 3996); // 炸药
            recipe.AddIngredient(ItemID.Dynamite, 3996); // 炸药
            recipe.AddIngredient(ItemID.Wire, 3996); // 电线
            recipe.AddIngredient(ItemID.Wire, 3996); // 电线
            recipe.AddIngredient(ItemID.Wire, 3996); // 电线
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
