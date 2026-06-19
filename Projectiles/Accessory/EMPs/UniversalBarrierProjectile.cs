using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs
{
    public class UniversalBarrierProjectile : ModProjectile
    {
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV"; // 使用透明弹幕贴图

        public override void SetDefaults()
        {
            Projectile.width = 2000; // 设置屏障的宽度
            Projectile.height = 2000; // 设置屏障的高度
            Projectile.friendly = true; // 弹幕对玩家s友好
            Projectile.penetrate = -1; // 弹幕可以无限穿透
            Projectile.timeLeft = 15 * 60; // 弹幕持续时间，这里设置为15秒
            Projectile.tileCollide = false; // 弹幕不与瓷砖碰撞
            Projectile.ignoreWater = true; // 弹幕忽略水的影响
            Projectile.alpha = 100; // 初始时弹幕是半透明的
        }

        public override void AI()
        {
            // 给进入屏障范围内的玩家提供无敌效果
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead && Vector2.Distance(player.Center, Projectile.Center) <= Projectile.width / 2)
                {
                    player.AddBuff(ModContent.BuffType<INV>(), 60); // 每秒刷新一次无敌 Buff
                }
            }

            // 增加生成粒子的频率来增加粒子数量
            for (int i = 0; i < 20; i++) // 一次创建20个粒子
            {
                Vector2 position = Projectile.Center + Main.rand.NextVector2CircularEdge(1000f, 1000f); // 修改圆圈的半径为1000
                Dust dust = Dust.NewDustPerfect(position, DustID.MagicMirror, Vector2.Zero, 150, Color.Purple, 5.5f);
                dust.noGravity = true; // 粒子不受重力影响

                dust.fadeIn = 1f; // 让粒子逐渐消失
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.velocity = new Vector2(0, 0); // 初始速度为零
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.1f, 0.5f); // 暗淡的光




            // 清除与屏障接触的敌方弹幕
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProj = Main.projectile[i];
                if (otherProj.active && otherProj.hostile && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                {
                    otherProj.Kill();
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false; // 屏障不会切割瓷砖
        }

        public override bool? CanDamage()
        {
            return false; // 屏障不会造成伤害
        }
    }
}