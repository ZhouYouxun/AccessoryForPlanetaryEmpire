//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
//using AccessoryForPlanetaryEmpire.Buffs;

//namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
//{
//    public class BrainInJarPlayer : ModPlayer
//    {
//        public bool hasBrainInJar = false;
//        private int fireRateTimer = 0; // 用于小吞噬者的发射频率
//        private int speedBoostCooldown = 0; // 速度提升冷却计时器
//        private const int speedBoostTime = 300; // 移动速度提升的持续时间，5秒（300帧）
//        private const int cooldownDuration = 1200; // 20秒的冷却时间（1200帧）

//        public override void ResetEffects()
//        {
//            hasBrainInJar = false;
//        }

//        public override void PostUpdate()
//        {
//            if (hasBrainInJar)
//            {
//                // 每15帧发射一次小吞噬者
//                fireRateTimer++;
//                if (fireRateTimer >= 15)
//                {
//                    if (Player.itemAnimation > 0 && Player.HeldItem.damage > 0 && Player.whoAmI == Main.myPlayer)
//                    {
//                        FireTinyEater();
//                    }
//                    fireRateTimer = 0;
//                }
//            }

//            // 冷却计时器更新
//            if (speedBoostCooldown > 0)
//            {
//                speedBoostCooldown--;
//            }
//        }

//        private void FireTinyEater()
//        {
//            Vector2 position = Player.Center;
//            Vector2 targetPosition = Main.MouseWorld;
//            Vector2 direction = (targetPosition - position).SafeNormalize(Vector2.UnitX);
//            float speed = 10f;

//            Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), position, direction * speed, ProjectileID.TinyEater, (int)(Player.HeldItem.damage * 0.35f), 0, Player.whoAmI);
//        }

//        // 检测玩家受伤时的处理逻辑
//        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
//        {
//            ApplySpeedBoostOnHit();
//        }

//        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
//        {
//            ApplySpeedBoostOnHit();
//        }

//        // 受伤时提供速度提升
//        private void ApplySpeedBoostOnHit()
//        {
//            if (hasBrainInJar && speedBoostCooldown <= 0)
//            {
//                // 提升玩家的移动速度75%
//                // Player.velocity *= 1.75f;

//                // 设置5秒的速度提升持续时间
//                Player.AddBuff(BuffID.Panic, speedBoostTime);

//                // 设置20秒的冷却时间
//                speedBoostCooldown = cooldownDuration;
//            }
//        }
//    }
//}