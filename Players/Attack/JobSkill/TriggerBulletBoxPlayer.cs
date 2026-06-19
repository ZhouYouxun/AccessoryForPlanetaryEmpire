using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Attack.JobSkill
{
    public class TriggerBulletBoxPlayer : ModPlayer
    {

        private const int TriggerTime = 300; // 5秒，以帧计算
        private int shootingTimer = 0;

        public bool hasTriggerBulletBox = false;
        public bool eyeopen;

        public override void ResetEffects()
        {
            hasTriggerBulletBox = false;
        }

        public override void PostUpdate()
        {
            if(hasTriggerBulletBox)
            {
                // 无条件计时
                shootingTimer++;
                if (shootingTimer >= TriggerTime)
                {
                    TriggerBullets();
                    shootingTimer = 0; // 重置计时器
                }

                if(!eyeopen)
                {
                    // 在鼠标位置生成粒子特效
                    CreateMouseParticleEffect(Main.MouseWorld);
                }

            }

        }

        private void TriggerBullets()
        {
            // 获取当前手持武器的伤害
            int damage = Player.HeldItem.damage;
            Vector2 targetPosition = Main.MouseWorld;

            for (int i = 0; i < 7; i++)
            {
                // 生成随机角度和半径
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                float radius = Main.rand.NextFloat(10 * 16, 30 * 16); // 10格到30格，假设每格是16个像素

                // 计算生成位置
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                Vector2 spawnPosition = targetPosition + offset;

                // 计算弹幕速度
                Vector2 velocity = Vector2.Normalize(targetPosition - spawnPosition) * 16;

                // 发射弹幕
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), spawnPosition, velocity, 104, damage, Player.HeldItem.knockBack, Player.whoAmI); // 诅咒
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), spawnPosition, velocity, 279, damage, Player.HeldItem.knockBack, Player.whoAmI); // 灵液
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), spawnPosition, velocity, 285, damage, Player.HeldItem.knockBack, Player.whoAmI); // 纳米
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), spawnPosition, velocity, 286, damage, Player.HeldItem.knockBack, Player.whoAmI); // 爆破
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), spawnPosition, velocity, 287, damage, Player.HeldItem.knockBack, Player.whoAmI); // 黄金
                Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), spawnPosition, velocity, 89, damage, Player.HeldItem.knockBack, Player.whoAmI); // 水晶
            }
        }


        // 在鼠标位置生成粒子特效的方法
        private void CreateMouseParticleEffect(Vector2 mousePosition)
        {
            int dustID = DustID.UltraBrightTorch;
            float radius = 16f; // 粒子生成半径
            float sizeMultiplier = 1.5f; // 粒子的大小倍数
            float brightnessMultiplier = 3f; // 粒子的亮度倍数
            int particleCount = 5; // 每帧生成的粒子数量

            for (int i = 0; i < particleCount; i++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-radius, radius), Main.rand.NextFloat(-radius, radius));
                Vector2 particlePosition = mousePosition + offset;
                Dust dust = Dust.NewDustPerfect(particlePosition, dustID, Vector2.Zero, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }
        }



    }
}
