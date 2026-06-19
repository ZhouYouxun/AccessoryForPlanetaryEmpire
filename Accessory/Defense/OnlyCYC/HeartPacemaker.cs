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
    public class HeartPacemaker : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        // 心脏起搏器
        // 生命值每损失10%，便会提供10点防御力和5%的伤害减免，上限为损失90%生命，提供90点防御和45%的伤害减免。

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
            //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();
            player.lifeRegen += 15;
            player.GetModPlayer<HeartPacemakerPlayer>().hasHeartPacemaker = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Solidifier, 1); // 固化机
            recipe.AddIngredient(ItemID.TurtleShell, 1); // 海龟壳
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5); // 叶绿锭
            //recipe.AddIngredient(ItemID.SoulofLight, 10); // 光明之魂
            //recipe.AddIngredient(ItemID.LightShard, 5); // 光明碎片
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}




