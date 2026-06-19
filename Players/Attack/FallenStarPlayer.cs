using AccessoryForPlanetaryEmpire.Projectiles.GlobalChange;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    internal class FallenStarPlayer : ModPlayer
    {
        public bool hasFallenStar;

        // 自己的计时器（不能用 localAI）
        private int starTimer = 0;

        public override void ResetEffects()
        {
            hasFallenStar = false;
        }

        public override void PostUpdate()
        {
            if (!hasFallenStar)
                return;

            // ⭐ 白天不触发
            if (Main.dayTime)
                return;

            // ⭐ 只能在地表/天空层触发，地下与洞穴不触发
            if (!Player.ZoneOverworldHeight && !Player.ZoneSkyHeight)
                return;

            // 计时器累加
            starTimer++;

            // 每 60~120 帧射一次（随机）
            int interval = 60 + Main.rand.Next(60);
            if (starTimer >= interval)
            {
                starTimer = 0;
                ShootFallingStar();
            }
        }

        private void ShootFallingStar()
        {
            // ⭐ 生成位置：玩家左右 ±1200 像素、上方 600 像素
            Vector2 spawnPos = new Vector2(
                Player.Center.X + Main.rand.NextFloat(-1200f, 1200f),
                Player.Center.Y - 600f
            );

            // ⭐ 所有星星固定往右偏斜的方向落下（基础方向是正下）
            Vector2 direction = Vector2.UnitY;
            float fixedAngle = MathHelper.ToRadians(10f);
            direction = direction.RotatedBy(fixedAngle);

            // ⭐ 固定速度
            Vector2 velocity = direction * 14f;

            // ⭐ 伤害随困难模式变化
            int damage = Main.hardMode ? 1500 : 1000;

            int projID = Projectile.NewProjectile(
                Player.GetSource_FromThis(),
                spawnPos,
                velocity,
                ProjectileID.FallingStar,
                damage,
                0f,
                Player.whoAmI
            );

            // ⭐ 设置友方属性
            Projectile proj = Main.projectile[projID];
            proj.friendly = true;
            proj.hostile = false;
            proj.penetrate = 1;
            proj.usesLocalNPCImmunity = true;
            proj.localNPCHitCooldown = 10;
        }



    }
}