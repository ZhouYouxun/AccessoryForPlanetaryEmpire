//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using Terraria.ID;
//using AccessoryForPlanetaryEmpire.Accessory.Defense;

//namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
//{
//    public class FlowerVinePlayer : ModPlayer
//    {
//        public bool hasFlowerVine = false;
//        private int hitCount = 0; // 记录受击次数

//        public override void ResetEffects()
//        {
//            hasFlowerVine = false;
//        }

//        public override void PostHurt(Player.HurtInfo info)
//        {
//            if (hasFlowerVine)
//            {
//                hitCount++;

//                // 检测玩家是否处于地表或丛林区域
//                if ((Player.ZoneOverworldHeight || Player.ZoneJungle) && hitCount >= 3)
//                {
//                    // 生成弹幕效果
//                    ReleaseProjectiles();

//                    // 生成视觉特效
//                    GenerateEffects(Player.Center);

//                    // 重置受击计数
//                    hitCount = 0;
//                }
//            }
//        }

//        private void ReleaseProjectiles()
//        {
//            // 在玩家周围生成 75 个 SporeTrap 和 75 个 SporeTrap2，速度是之前的7倍
//            for (int i = 0; i < 75; i++)
//            {
//                Vector2 spawnPosition = Player.Center;
//                Vector2 velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)).SafeNormalize(Vector2.Zero) * 7f; // 速度乘以7
//                Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPosition, velocity, 567, 30, 0, Player.whoAmI);
//                Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPosition, velocity, 568, 30, 0, Player.whoAmI);
//            }

//            // 在鼠标位置上方生成 3 个 ChlorophyteOrb，拥有10倍大小
//            Vector2 realPlayerPos = Player.RotatedRelativePoint(Player.MountedCenter, true);
//            for (int j = 0; j < 3; j++)
//            {
//                Vector2 spawnPosition = new Vector2(
//                    Player.position.X + Player.width * 0.5f + Main.rand.Next(201) * -Player.direction + Main.mouseX + Main.screenPosition.X - Player.position.X,
//                    Player.MountedCenter.Y - 600f
//                );
//                spawnPosition.X = (spawnPosition.X + Player.Center.X) / 2f + Main.rand.Next(-200, 201);
//                spawnPosition.Y -= 100 * j;

//                Vector2 direction = Main.MouseWorld - spawnPosition;
//                direction.Normalize();
//                float speedX = direction.X * 5f + Main.rand.Next(-120, 121) * 0.01f;
//                float speedY = direction.Y * 5f + Main.rand.Next(-120, 121) * 0.01f;

//                int damage = (int)(Player.HeldItem.damage * 0.33f);
//                // 大小修正为原来的10倍
//                Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), 229, damage, 0, Player.whoAmI);
//                proj.scale *= 5f; // 增加10倍大小
//            }
//        }

//        private void GenerateEffects(Vector2 position)
//        {
//            // 生成原有反射、爆炸、闪光效果
//            for (int i = 0; i < 20; i++)
//            {
//                Vector2 velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
//                Dust dust = Dust.NewDustPerfect(position, DustID.CursedTorch, velocity, 150, default, 2f);
//                dust.noGravity = true;
//                dust.fadeIn = 1.5f;
//            }

//            for (int i = 0; i < 30; i++)
//            {
//                Dust dust = Dust.NewDustDirect(position, Player.width, Player.height, DustID.CursedTorch, 0f, 0f, 100, default, 2.5f);
//                dust.velocity *= 2f;
//                dust.noGravity = true;
//                dust.fadeIn = 1.5f;
//            }

//            Gore.NewGore(Player.GetSource_FromThis(), position, Vector2.Zero, GoreID.Smoke1, 1f);
//        }
//    }
//}