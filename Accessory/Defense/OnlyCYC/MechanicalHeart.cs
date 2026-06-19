using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class MechanicalHeart : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        // 机械心脏：受到致命伤害时复活一次，冷却时间三分钟

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值35金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();
            player.GetModPlayer<MechanicalHeartPlayer>().hasMechanicalHeart = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.SteampunkBoiler, 1); // 蒸汽朋克制作台
            recipe.AddIngredient(ItemID.LifeCrystal, 10); // 生命水晶
            recipe.AddIngredient(ItemID.EmptyDropper, 5); // 空的滴管
            recipe.AddIngredient(ItemID.MartianConduitPlating, 100); // 火星管道堵板
            recipe.AddIngredient(ItemID.ElectrosphereLauncher, 1); // 电圈发射器
            recipe.AddIngredient(ItemID.SoulofLight, 5); // 光明之魂
            //recipe.AddIngredient(ItemID.LightShard, 5); // 光明碎片
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}




