using AccessoryForPlanetaryEmpire.Buffs;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System;


namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class MercuryPlayer : ModPlayer
    {
        public bool hasMercuryAccessory = false;
        public bool hasTSSAccessory = false;
        private int mercuryCooldown = 0;
        private int particleTimer = 0;

        public override void ResetEffects()
        {
            hasMercuryAccessory = false;
            hasTSSAccessory = false;
        }

        public override void UpdateDead()
        {
            hasMercuryAccessory = false;
            hasTSSAccessory = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasMercuryAccessory && mercuryCooldown <= 0)
            {
                Player.endurance += 0.75f; // 75% damage reduction
                ManageMercuryProjectile();
            }

            if (mercuryCooldown > 0)
            {
                mercuryCooldown--;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (hasMercuryAccessory)
            {
                if (!hasTSSAccessory)
                {
                    Player.AddBuff(ModContent.BuffType<MercuryCurseingBuff>(), 600); // 10秒
                }
                mercuryCooldown = 600; // 10 seconds cooldown
                GenerateMercuryParticleLine();
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (hasMercuryAccessory)
            {
                if (!hasTSSAccessory)
                {
                    Player.AddBuff(ModContent.BuffType<MercuryCurseingBuff>(), 600); // 10秒
                }
                mercuryCooldown = 600; // 10 seconds cooldown
                GenerateMercuryParticleLine();
            }
        }




        private void ManageMercuryProjectile()
        {
            if (!hasMercuryAccessory) return; // 如果没有启用水星饰品，直接返回

            bool mercuryExists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<MercuryProjectile>())
                {
                    mercuryExists = true;
                    break;
                }
            }

            // 生成弹幕
            if (!mercuryExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                Vector2 position = Player.Center + new Vector2((float)(Math.Cos(angle) * 50), (float)(Math.Sin(angle) * 50));
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), position, Vector2.Zero, ModContent.ProjectileType<MercuryProjectile>(), 0, 0, Player.whoAmI);
            }
        }

        private void GenerateMercuryParticleLine()
        {
            Projectile mercuryProjectile = FindMercuryProjectile();
            if (mercuryProjectile == null)
                return;

            Vector2 start = Player.Center;
            Vector2 end = mercuryProjectile.Center;
            Vector2 direction = (end - start);
            float length = direction.Length();
            direction.Normalize();

            // 华丽 ×5 版本：
            int points = (int)(length / 2f);              // 密度 ×3
            float waveStrength = 35f;                     // 振幅 ×3
            float waveFreq = 6f;                          // 频率 ×3
            float thickness = 1.8f;                       // 粒子大小提升
            float brightnessBoost = 0.65f;                // 亮度增强

            float t = (float)Main.GameUpdateCount / 20f;

            for (int i = 0; i <= points; i++)
            {
                float progress = i / (float)points;
                Vector2 pos = Vector2.Lerp(start, end, progress);

                // 更柔和、更优美的双向正弦波
                Vector2 perp = direction.RotatedBy(MathHelper.PiOver2);
                float wave =
                    (float)Math.Sin(progress * MathHelper.TwoPi * waveFreq + t) * waveStrength
                    * (0.5f + 0.5f * (float)Math.Sin(t * 1.2f)); // 动态柔和变化

                pos += perp * wave;

                Dust d = Dust.NewDustPerfect(pos, DustID.WaterCandle);
                d.noGravity = true;
                d.scale = thickness + 0.5f * (float)Math.Sin(progress * MathHelper.TwoPi * 3f + t);

                // 更华丽：亮蓝 + 白色 + 加亮
                Color baseColor = Color.Lerp(Color.Cyan, Color.White, progress);
                d.color = baseColor * (1f + brightnessBoost);

                d.fadeIn = 1.2f;
                d.velocity = Vector2.Zero;
            }
        }


        private Projectile FindMercuryProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<MercuryProjectile>())
                {
                    return proj;
                }
            }
            return null;
        }
    




    }


}
