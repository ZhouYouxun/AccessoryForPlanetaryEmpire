using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class LifeSubSpace : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        private const int Cooldown = 90 * 60; // 90秒冷却时间
        private int timer = 0;

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
            if (timer > 0)
            {
                timer--; // 每帧减少冷却时间
            }

            // 当玩家血量低于20%且冷却时间结束时触发效果
            if (timer <= 0 && player.statLife <= player.statLifeMax2 * 0.2f)
            {
                // 创建LifeSubSpacePROJ弹幕
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item), // 来源于此配饰
                    player.Center,                    // 在玩家中心生成
                    Vector2.Zero,                     // 没有速度，静止不动
                    ModContent.ProjectileType<LifeSubSpacePROJ>(), // 弹幕类型
                    0,                                 // 无伤害
                    0,                                 // 无击退
                    player.whoAmI                      // 这个弹幕属于当前玩家
                );
                timer = Cooldown; // 重置冷却时间
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.FleshCloningVaat, 1); // 血肉制造机
            recipe.AddIngredient(ItemID.CrystalBall, 1);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}