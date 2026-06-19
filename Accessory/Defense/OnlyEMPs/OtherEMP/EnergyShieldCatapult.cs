using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Players;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.OtherEMP
{
    public class EnergyShieldCatapult : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs.OtherEMP";

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

        private int energyShieldCatapultTimer; // 声明计时器变量

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.15f; // 增加15%的伤害减免
            player.statDefense += 15; // 增加15点防御力
            player.jumpSpeedBoost += 0.15f; // 增加15%的跳跃速度

            // 计时器逻辑
            if (energyShieldCatapultTimer > 0)
            {
                energyShieldCatapultTimer--; // 每帧减少计时器
            }
            else
            {
                // 获取鼠标位置
                Vector2 mousePosition = Main.MouseWorld;
                // 获取发射方向
                Vector2 direction = mousePosition - player.Center;
                direction.Normalize();
                direction *= 4f; // 设置弹幕速度

                // 发射弹幕
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item),
                    player.Center,
                    direction,
                    ModContent.ProjectileType<EnergyShieldCatapultPROJ>(),
                    0,
                    0,
                    player.whoAmI
                );

                // 生成粒子特效
                CreateExplosionEffect(player.Center);

                // 重置计时器为5秒（300帧）
                energyShieldCatapultTimer = 300;
            }
        }



        // 创建爆炸粒子特效的方法
        private void CreateExplosionEffect(Vector2 position)
        {
            int particleCount = 100;
            for (int i = 0; i < particleCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(5f, 10f);
                Dust dust = Dust.NewDustPerfect(position, DustID.BlueTorch, velocity);
                dust.noGravity = true;
                dust.scale = 7.2f;
                dust.fadeIn = 1f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Actuator, 50);
            recipe.AddIngredient(ItemID.Wire, 50);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.DarkShard, 1);
            recipe.AddIngredient(ItemID.LightShard, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }



    }


}




