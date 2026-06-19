using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class NeptunePlayer : ModPlayer
    {
        public bool hasNeptuneAccessory = false;
        public bool hasTSSAccessory = false; // 添加hasTSSAccessory
        private Projectile activeNeptuneWaterProjectile;
        private int particleTimer = 0; // 添加 particleTimer 变量

        public override void ResetEffects()
        {
            hasNeptuneAccessory = false;
            hasTSSAccessory = false; // 重置hasTSSAccessory
        }

        public override void PostUpdate()
        {
            if (hasNeptuneAccessory || hasTSSAccessory)
            {
                ManageNeptuneProjectile();

                if (Player.controlUseItem && activeNeptuneWaterProjectile == null)
                {
                    Vector2 neptunePosition = FindNeptuneProjectile();
                    if (neptunePosition != Vector2.Zero)
                    {
                        int damage = (int)(Player.HeldItem.damage * (hasTSSAccessory ? 1.0f : 0.5f)); // 增强后1.0，未增强0.5
                        //activeNeptuneWaterProjectile = Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), neptunePosition, Vector2.Zero, ModContent.ProjectileType<NeptuneWaterProjectile>(), damage, 0, Player.whoAmI)];
                        //SoundEngine.PlaySound(SoundID.Item92, Player.position);
                    }
                }

                // 检查并回收水弹幕
                if (!Player.controlUseItem && activeNeptuneWaterProjectile != null)
                {
                    activeNeptuneWaterProjectile.Kill();
                    activeNeptuneWaterProjectile = null;
                }

                GenerateNeptuneParticleLine();
            }
        }

        private void ManageNeptuneProjectile()
        {
            bool neptuneExists = false;
            Vector2 neptunePosition = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<NeptuneProjectile>())
                {
                    neptuneExists = true;
                    break;
                }
            }

            if (!neptuneExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                neptunePosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 30 * 16), (float)(Math.Sin(angle) * 30 * 16)); // 公转半径30格
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), neptunePosition, Vector2.Zero, ModContent.ProjectileType<NeptuneProjectile>(), 0, 0, Player.whoAmI);
            }
        }

        private Vector2 FindNeptuneProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<NeptuneProjectile>())
                {
                    return proj.Center;
                }
            }
            return Vector2.Zero;
        }

        //private void GenerateNeptuneParticleLine()
        //{
        //    Vector2 neptunePosition = FindNeptuneProjectile();
        //    if (neptunePosition == Vector2.Zero) return;

        //    if (activeNeptuneWaterProjectile == null) return;

        //    particleTimer++;
        //    if (particleTimer < 120 / Main.rand.Next(3, 5)) // 每2秒生成3到4次特效
        //    {
        //        return;
        //    }
        //    particleTimer = 0;

        //    Vector2 start = neptunePosition;
        //    Vector2 end = activeNeptuneWaterProjectile.Center;
        //    float distance = Vector2.Distance(start, end);
        //    int segmentCount = (int)(distance / 10); // 每10个像素一个段
        //    Vector2 direction = (end - start) / distance;

        //    Vector2[] points = new Vector2[segmentCount];
        //    points[0] = start;

        //    for (int i = 1; i < segmentCount; i++)
        //    {
        //        float offsetX = Main.rand.NextFloat(-15f, 15f); // 更大的随机偏移
        //        float offsetY = Main.rand.NextFloat(-15f, 15f);
        //        points[i] = start + direction * i * 10 + new Vector2(offsetX, offsetY);
        //    }
        //    points[segmentCount - 1] = end;

        //    for (int i = 0; i < segmentCount - 1; i++)
        //    {
        //        Vector2 startSegment = points[i];
        //        Vector2 endSegment = points[i + 1];
        //        float segmentDistance = Vector2.Distance(startSegment, endSegment);
        //        int segmentParticleCount = (int)(segmentDistance / 4); // 每4个像素一个粒子

        //        Vector2 segmentDirection = (endSegment - startSegment) / segmentDistance;

        //        for (int j = 0; j < segmentParticleCount; j++)
        //        {
        //            Vector2 position = startSegment + segmentDirection * j * 4;

        //            Dust dust = Dust.NewDustPerfect(position, DustID.WaterCandle);
        //            dust.noGravity = true;
        //            dust.scale = Main.rand.NextFloat(1.0f, 1.5f);
        //            dust.fadeIn = 1f;
        //            dust.velocity = Vector2.Zero;
        //            dust.color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat());

        //            if (Main.rand.NextFloat() < 0.1f) // 10% 的粒子变大以模拟脉动效果
        //            {
        //                dust.scale *= 1.5f;
        //                dust.color = Color.White;
        //            }
        //        }
        //    }
        //}


        private void GenerateNeptuneParticleLine()
        {
            //Vector2 neptunePosition = FindNeptuneProjectile();
            //if (neptunePosition == Vector2.Zero) return;

            //if (activeNeptuneWaterProjectile == null) return;

            //particleTimer++;
            //if (particleTimer < 120 / Main.rand.Next(3, 5)) // 每2秒生成3到4次特效
            //{
            //    return;
            //}
            //particleTimer = 0;

            //Vector2 start = neptunePosition;
            //Vector2 end = activeNeptuneWaterProjectile.Center;
            //float distance = Vector2.Distance(start, end);
            //int segmentCount = (int)(distance / 10); // 每10个像素一个段
            //Vector2 direction = (end - start) / distance;

            //Vector2[] points = new Vector2[segmentCount];
            //points[0] = start;

            //for (int i = 1; i < segmentCount; i++)
            //{
            //    float t = (float)i / segmentCount;
            //    float sineOffset = (float)Math.Sin(t * MathHelper.TwoPi * 14) * 10f; // 增加频率至14倍
            //    float cosineOffset = (float)Math.Cos(t * MathHelper.TwoPi * 14) * 10f; // 增加频率至14倍

            //    // 一正一反形成DNA链
            //    points[i] = start + direction * i * 10 + new Vector2(sineOffset, -cosineOffset);
            //}
            //points[segmentCount - 1] = end;

            //for (int i = 0; i < segmentCount - 1; i++)
            //{
            //    Vector2 startSegment = points[i];
            //    Vector2 endSegment = points[i + 1];
            //    float segmentDistance = Vector2.Distance(startSegment, endSegment);
            //    int segmentParticleCount = (int)(segmentDistance / 4); // 每4个像素一个粒子

            //    Vector2 segmentDirection = (endSegment - startSegment) / segmentDistance;

            //    for (int j = 0; j < segmentParticleCount; j++)
            //    {
            //        Vector2 position = startSegment + segmentDirection * j * 4;

            //        // 上下链条的粒子
            //        Dust dustUpper = Dust.NewDustPerfect(position + new Vector2(5, 5), DustID.WaterCandle);
            //        Dust dustLower = Dust.NewDustPerfect(position + new Vector2(-5, -5), DustID.WaterCandle);

            //        dustUpper.noGravity = true;
            //        dustUpper.scale = Main.rand.NextFloat(1.0f, 1.5f);
            //        dustUpper.fadeIn = 1f;
            //        dustUpper.velocity = Vector2.Zero;
            //        dustUpper.color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat());

            //        dustLower.noGravity = true;
            //        dustLower.scale = Main.rand.NextFloat(1.0f, 1.5f);
            //        dustLower.fadeIn = 1f;
            //        dustLower.velocity = Vector2.Zero;
            //        dustLower.color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat());

            //        if (Main.rand.NextFloat() < 0.1f) // 10% 的粒子变大以模拟脉动效果
            //        {
            //            dustUpper.scale *= 1.5f;
            //            dustUpper.color = Color.White;

            //            dustLower.scale *= 1.5f;
            //            dustLower.color = Color.White;
            //        }
            //    }
            //}
        }


    }
}