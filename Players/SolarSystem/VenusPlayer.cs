using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class VenusPlayer : ModPlayer
    {






        public bool hasVenusAccessory = false;
        public int venusCooldown = 0;
        private int particleTimer = 5;
        private int brightParticleIndex = 0;

        public override void ResetEffects()
        {
            hasVenusAccessory = false;
            if (venusCooldown > 0)
            {
                venusCooldown--;
            }
        }

        public override void PostUpdate()
        {
            if (hasVenusAccessory)
            {
                ManageVenusProjectile();
                if (venusCooldown <= 0)
                {
                    GenerateSpiralEffect();
                    TriggerScreenShake(Player.Center, 20f, 800f); // 添加屏幕震动，强度为20
                    venusCooldown = 600; // 重置冷却时间
                }
            }
            // GenerateRotatingParticleRing(); // 每帧更新粒子效果 不要了，太花哨了
        }

        private void ManageVenusProjectile() // 负责制作和销毁弹幕
        {
            bool venusExists = false;
            Vector2 venusPosition = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<VenusProjectile>())
                {
                    venusExists = true;
                    venusPosition = proj.Center;
                    break;
                }
            }

            if (!venusExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                venusPosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 25 * 16), (float)(Math.Sin(angle) * 25 * 16)); // 公转半径25格
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), venusPosition, Vector2.Zero, ModContent.ProjectileType<VenusProjectile>(), 0, 0, Player.whoAmI);
            }
        }

        private Projectile FindVenusProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<VenusProjectile>())
                {
                    return proj;
                }
            }
            return null;
        }

        private void GenerateSpiralEffect()
        {
            Projectile venusProjectile = FindVenusProjectile();
            if (venusProjectile == null) return;

            int particleCount = 100; // 粒子数量
            float initialRadius = 5f * 16f; // 初始半径
            float expansionSpeed = 10f; // 扩展速度

            for (int i = 0; i < particleCount; i++)
            {
                float angle = i * MathHelper.TwoPi / particleCount;
                Vector2 initialPosition = venusProjectile.Center + angle.ToRotationVector2() * initialRadius;

                Dust dust = Dust.NewDustPerfect(initialPosition, DustID.GoldFlame);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(2.0f, 3.0f);
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity = angle.ToRotationVector2() * expansionSpeed; // 设置粒子的速度，使其向外扩散
                dust.fadeIn = 1f;
                dust.color = Color.Yellow;
            }

            // 为半径100格内的敌人施加debuff
            float debuffRadius = 100f * 16f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(venusProjectile.Center) <= debuffRadius)
                {
                    npc.AddBuff(ModContent.BuffType<VenusHighTempMeltDebuff>(), 420);
                }
            }
        }

        // 屏幕震动（带距离衰减，且只作用于本地玩家）
        private void TriggerScreenShake(Vector2 sourcePos, float intensity, float maxDistance)
        {
            Player local = Main.LocalPlayer;

            // 计算距离
            float dist = Vector2.Distance(local.Center, sourcePos);

            // 超出范围就不震动
            if (dist > maxDistance)
                return;

            // 距离越远震动越弱
            float distanceFactor = Utils.GetLerpValue(maxDistance, 0f, dist, true);

            // 套用你定义的震动标准
            float shakePower = intensity * distanceFactor;

            local.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower =
                Math.Max(local.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower, shakePower);
        }


        private void GenerateRotatingParticleRing()
        {
            Projectile venusProjectile = FindVenusProjectile();
            if (venusProjectile == null) return;

            int particleCount = 100; // 粒子数量
            float radius = 50f * 16f; // 半径
            brightParticleIndex = (brightParticleIndex + 1) % particleCount; // 更新亮粒子索引

            // 清除已有的粒子
            for (int i = 0; i < Main.maxDust; i++)
            {
                Dust dust = Main.dust[i];
                if (dust.active && dust.customData is Vector2 data)
                {
                    dust.active = false;
                }
            }

            // 创建亮粒子
            for (int i = 0; i < 5; i++) // 调整亮粒子的数量
            {
                float angle = (brightParticleIndex + i * (particleCount / 5)) * MathHelper.TwoPi / particleCount;
                Vector2 position = venusProjectile.Center + angle.ToRotationVector2() * radius;

                Dust dust = Dust.NewDustPerfect(position, DustID.GoldFlame);
                dust.noGravity = true;
                dust.scale = 2f;
                dust.rotation = angle;
                dust.color = Color.Yellow;
                dust.alpha = 0; // 亮粒子
            }
        }







    }
}
