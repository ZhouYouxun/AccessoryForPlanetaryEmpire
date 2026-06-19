//using AccessoryForPlanetaryEmpire.Buffs;
//using AccessoryForPlanetaryEmpire.Global;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.GameInput;
//using Terraria;
//using Terraria.ModLoader;
//using AccessoryForPlanetaryEmpire.Buffs;
//using Terraria.ID;
//using AccessoryForPlanetaryEmpire.Projectiles;
//using Microsoft.Xna.Framework;


//namespace AccessoryForPlanetaryEmpire.Players.Attack.JobSkill
//{
//    public class BattlefieldMedicalSuitcasePlayer : ModPlayer
//    {
//        public bool hasBattlefieldMedicalSuitcase = false;
//        private int healProjectileCooldown = 0; // 冷却计数器

//        public override void ResetEffects()
//        {
//            hasBattlefieldMedicalSuitcase = false;
//        }

//        public override void UpdateDead()
//        {
//            hasBattlefieldMedicalSuitcase = false;
//        }

//        public override void PostUpdate()
//        {
//            if (healProjectileCooldown > 0)
//            {
//                healProjectileCooldown--; // 更新冷却
//            }
//        }

//        public override void OnHitAnything(float x, float y, Entity victim)
//        {
//            // 检查装备和冷却
//            if (hasBattlefieldMedicalSuitcase && healProjectileCooldown <= 0)
//            {
//                // 获取玩家手持武器的面板伤害
//                int weaponDamage = Player.HeldItem.damage;
//                // 仅当手持的是魔法武器时才触发治疗效果
//                Item heldItem = Player.HeldItem;
//                if (heldItem.DamageType == DamageClass.Magic)
//                {
//                    // 遍历所有玩家
//                    foreach (Player p in Main.player)
//                    {
//                        // 检查玩家是否有效并且不是死亡状态
//                        if (p.active && !p.dead)
//                        {
//                            // 计算玩家与事件发生位置的距离
//                            float distance = Vector2.Distance(p.Center, new Vector2(x, y));

//                            // 如果玩家在一定范围内（比如500像素），给玩家加血
//                            if (distance < 500f)
//                            {
//                                int healAmount = (int)Math.Round(Main.rand.NextFloat(0.05f, 0.25f) * weaponDamage);
//                                // CombatText.NewText(Player.getRect(), Color.LightGreen, $"治疗 {healAmount} 点生命值!", dramatic: false, dot: false);
//                                p.statLife += healAmount;
//                                p.HealEffect(healAmount, broadcast: true); // 显示治疗效果
//                            }
//                        }
//                    }
//                    // 生成粒子特效
//                    CreateHealingEffect(Player.Center);
//                }

//                healProjectileCooldown = 60; // 设置冷却时间为1秒
//            }
//        }


//        // 创建治疗粒子特效的方法
//        private void CreateHealingEffect(Vector2 position)
//        {
//            int dustID = DustID.GreenTorch;
//            float duration = 1.5f; // 粒子的持续时间
//            float sizeMultiplier = 4f; // 粒子的大小倍数
//            float brightnessMultiplier = 6f; // 粒子的亮度倍数

//            // 创建十字架粒子特效
//            for (int i = -2; i <= 2; i++)
//            {
//                if (i == 0) continue; // 跳过中心点

//                // 水平线
//                Vector2 horizontalOffset = new Vector2(i * 16, 0);
//                Dust horizontalDust = Dust.NewDustPerfect(position + horizontalOffset, dustID, Vector2.Zero, 100, Color.Green * brightnessMultiplier, 1.5f * sizeMultiplier);
//                horizontalDust.noGravity = true;
//                horizontalDust.fadeIn = duration;
//                horizontalDust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
//                horizontalDust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
//                horizontalDust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度

//                // 垂直线
//                Vector2 verticalOffset = new Vector2(0, i * 16);
//                Dust verticalDust = Dust.NewDustPerfect(position + verticalOffset, dustID, Vector2.Zero, 100, Color.Green * brightnessMultiplier, 1.5f * sizeMultiplier);
//                verticalDust.noGravity = true;
//                verticalDust.fadeIn = duration;
//                verticalDust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
//                verticalDust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
//                verticalDust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
//            }


//        }

//    }
//}
