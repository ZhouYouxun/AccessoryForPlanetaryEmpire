using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players;
using System.Reflection;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class GlitchCorePROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool hasMalfunctioned = false;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var player = Main.player[projectile.owner];

            if (player.GetModPlayer<GlitchCorePlayer>().hasGlitchCore && !hasMalfunctioned && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                TriggerMalfunction(projectile, target);
                hasMalfunctioned = true;
            }
        }

        private void TriggerMalfunction(Projectile projectile, NPC target)
        {
            int malfunctionType = Main.rand.Next(21); // 21种故障，每种故障概率相同
            //停下来，完全不移动 - 弹幕停止所有移动，静止在原地。
            //弹幕飞行速度×10 - 弹幕的飞行速度瞬间提高10倍。
            //弹幕飞行速度减50 %，同时受重力影响 - 弹幕速度减半，并开始受重力影响，逐渐下落。
            //弹幕飞行速度乘以0.1~0.5之间的随机数 - 弹幕速度被随机减慢至初始速度的10 % 到50 % 之间。
            //弹幕左转10到90度 - 弹幕在飞行中向左转一个随机角度，范围为10度到90度。
            //弹幕右转10到90度 - 弹幕在飞行中向右转一个随机角度，范围为10度到90度。
            //弹幕反向移动 - 弹幕受到反弹影响，朝与原方向相反的方向飞行。
            //弹幕伤害×3 - 弹幕的伤害被提升至原来的三倍。
            //弹幕击退×10 - 弹幕的击退效果被放大十倍。
            //弹幕传送到上方5到10格处 - 弹幕瞬间传送到其当前位置上方5到10格的位置。
            //弹幕传送到下方5到10格处 - 弹幕瞬间传送到其当前位置下方5到10格的位置。
            //弹幕传送到左方5到10格处 - 弹幕瞬间传送到其当前位置左方5到10格的位置。
            //弹幕传送到右方5到10格处 - 弹幕瞬间传送到其当前位置右方5到10格的位置。
            //对敌人造成最大生命值2 % 的伤害 - 弹幕对击中的敌人直接造成其最大生命值2 % 的伤害。
            //秒杀非Boss敌人 - 如果弹幕击中的是普通敌人，则直接将其秒杀（对Boss无效）。
            //弹幕分裂为两个方向相反的复制体 - 弹幕分裂为两个，分别向前和向后的方向继续飞行。
            //弹幕追踪最近的敌人 - 弹幕自动锁定并追踪最近的敌人。
            //弹幕获得无限穿透能力 - 弹幕可以穿透无限数量的敌人或障碍物。
            //弹幕分裂为8个低伤害复制体 - 弹幕击中敌人后，分裂出8个方向的复制体，伤害为原来的20 %。
            //弹幕变大5倍 - 弹幕的体积变大5倍，覆盖更大的区域。
            //弹幕缩小到原来的20 % -弹幕的体积缩小到原来的20 %，变得更加隐蔽。

            switch (malfunctionType)
            {
                case 0:
                    projectile.velocity = Vector2.Zero; // 停止移动
                    // CombatText.NewText(projectile.getRect(), Color.Red, "0", true);
                    break;
                case 1:
                    projectile.velocity *= 10f; // 速度×10
                    // CombatText.NewText(projectile.getRect(), Color.Red, "1", true);
                    break;
                case 2:
                    projectile.velocity *= 0.5f; // 速度减半，受重力影响
                    projectile.tileCollide = true;
                    // CombatText.NewText(projectile.getRect(), Color.Red, "2", true);
                    break;
                case 3:
                    projectile.velocity *= Main.rand.NextFloat(0.1f, 0.5f); // 速度乘以随机数
                    // CombatText.NewText(projectile.getRect(), Color.Red, "3", true);
                    break;
                case 4:
                    projectile.velocity *= -1f; // 反方向移动
                    // CombatText.NewText(projectile.getRect(), Color.Red, "4", true);
                    break;
                case 5:
                    // 随机左转10到90度
                    float leftTurnAngle = MathHelper.ToRadians(Main.rand.NextFloat(10f, 90f));
                    projectile.velocity = projectile.velocity.RotatedBy(leftTurnAngle);
                    // CombatText.NewText(projectile.getRect(), Color.Red, "5", true);
                    break;
                case 6:
                    // 随机右转10到90度
                    float rightTurnAngle = MathHelper.ToRadians(Main.rand.NextFloat(10f, 90f));
                    projectile.velocity = projectile.velocity.RotatedBy(-rightTurnAngle);
                    // CombatText.NewText(projectile.getRect(), Color.Red, "6", true);
                    break;

                case 7:
                    projectile.damage *= 3; // 伤害×3
                    // CombatText.NewText(projectile.getRect(), Color.Red, "7", true);
                    break;
                case 8:
                    projectile.knockBack *= 10; // 击退×10
                    // CombatText.NewText(projectile.getRect(), Color.Red, "8", true);
                    break;
                case 9:
                    projectile.position.Y -= Main.rand.Next(2, 11) * 16; // 传送到上方2-10格
                    // CombatText.NewText(projectile.getRect(), Color.Red, "9", true);
                    break;
                case 10:
                    projectile.position.Y += Main.rand.Next(2, 11) * 16; // 传送到下方2-10格
                    // CombatText.NewText(projectile.getRect(), Color.Red, "10", true);
                    break;
                case 11:
                    projectile.position.X -= Main.rand.Next(2, 11) * 16; // 传送到左方2-10格
                    // CombatText.NewText(projectile.getRect(), Color.Red, "11", true);
                    break;
                case 12:
                    projectile.position.X += Main.rand.Next(2, 11) * 16; // 传送到右方2-10格
                    // CombatText.NewText(projectile.getRect(), Color.Red, "12", true);
                    break;
                case 13:
                    target.life -= (int)(target.lifeMax * 0.02); // 对敌人造成2%最大生命值伤害
                    // CombatText.NewText(projectile.getRect(), Color.Red, "13", true);
                    break;
                case 14:
                    if (!target.boss)
                    {
                        target.life = 0; // 直接将非boss敌人的生命值设置为0，达到秒杀效果
                        target.HitEffect(0, 10.0); // 产生击中效果
                        target.checkDead(); // 检查是否已经死亡并触发死亡效果
                    }
                    // CombatText.NewText(projectile.getRect(), Color.Red, "14", true);
                    break;
                case 15:
                    Vector2 direction = projectile.velocity;
                    projectile.velocity = direction; // 分裂
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, -direction, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                    // CombatText.NewText(projectile.getRect(), Color.Red, "15", true);
                    break;
                case 16:
                    projectile.aiStyle = 0; // 追踪玩家
                    // CombatText.NewText(projectile.getRect(), Color.Red, "16", true);
                    break;
                case 17:
                    NPC closestNPC = null;
                    float closestDistance = float.MaxValue;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.CanBeChasedBy(projectile) && !npc.friendly)
                        {
                            float distance = Vector2.Distance(projectile.Center, npc.Center);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestNPC = npc;
                            }
                        }
                    }
                    if (closestNPC != null)
                    {
                        projectile.aiStyle = 1; // 更改弹幕AI为追踪模式
                        projectile.velocity = Vector2.Normalize(closestNPC.Center - projectile.Center) * projectile.velocity.Length();
                        projectile.extraUpdates = 1; // 提升追踪效果
                    }
                    // CombatText.NewText(projectile.getRect(), Color.Green, "17", true);
                    break;
                case 18:
                    projectile.penetrate = -1; // -1表示无限穿透
                    // CombatText.NewText(projectile.getRect(), Color.Green, "18", true);
                    break;
                case 19:
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 spawnDirection = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(45 * i)); // 45度间隔生成8个方向
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, spawnDirection * projectile.velocity.Length(), projectile.type, (int)(projectile.damage * 0.2f), projectile.knockBack, projectile.owner);
                    }
                    // CombatText.NewText(projectile.getRect(), Color.Green, "19", true);
                    break;
                //case 20:
                //    //int healAmount = (int)(damageDone * Main.rand.NextFloat(0.1f, 0.7f)); // 计算回复量
                //    player.statLife += (damageDone * Main.rand.NextFloat(0.1f, 0.7f)); // 恢复生命值
                //    player.HealEffect((damageDone * Main.rand.NextFloat(0.1f, 0.7f)), true); // 显示治疗效果
                //    CombatText.NewText(projectile.getRect(), Color.Green, "20", true);
                //    break;
                case 20:
                    projectile.scale *= 5f; // 放大5倍
                    // CombatText.NewText(projectile.getRect(), Color.Green, "20", true);
                    break;
                case 21:
                    projectile.scale *= 0.2f; // 缩小为20%
                    // CombatText.NewText(projectile.getRect(), Color.Green, "21", true);
                    break;







            }
        }

     

    }
}