using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System.IO.Pipelines;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;


namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class FirepowerExoskeletonDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";
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
            player.GetModPlayer<FirepowerExoskeletonDevicePlayer>().firepowerExoskeleton = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 15); // 神圣定
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(ItemID.MechanicalLens, 1); // 机械晶状体
            recipe.AddIngredient(ItemID.SoulofFright, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
