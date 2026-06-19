using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class BrassArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }


        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1; // 击中敌人后消失
            Projectile.timeLeft = 600; // 10秒后消失
            Projectile.scale = 0.5f; // 通过调整缩放比例来变大子弹贴图

        }

        public override void AI()
        {
            // 创建sin和cos形状的粒子特效
            float sinValue = (float)Math.Sin(Projectile.timeLeft * 0.1f);
            float cosValue = (float)Math.Cos(Projectile.timeLeft * 0.1f);

            Dust.NewDustPerfect(Projectile.position, DustID.GoldCoin, new Vector2(sinValue, sinValue), 0, Color.Yellow);
            Dust.NewDustPerfect(Projectile.position, DustID.Smoke, new Vector2(cosValue, cosValue), 0, Color.Black);

            // 使弹幕始终面朝飞行方向
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // 追踪逻辑
            if (Projectile.ai[0] == 1)
            {
                float maxDetectRadius = 400f; // 最大检测半径
                float projSpeed = 10f; // 弹幕速度

                NPC closestNPC = null;
                float closestNPCDistance = maxDetectRadius;

                // 找到最近的NPC
                foreach (NPC npc in Main.npc)
                {
                    if (npc.CanBeChasedBy(this) && !npc.friendly)
                    {
                        float distance = Vector2.Distance(npc.Center, Projectile.Center);
                        if (distance < closestNPCDistance)
                        {
                            closestNPCDistance = distance;
                            closestNPC = npc;
                        }
                    }
                }

                // 如果找到最近的NPC，调整速度方向
                if (closestNPC != null)
                {
                    Vector2 direction = closestNPC.Center - Projectile.Center;
                    direction.Normalize();
                    direction *= projSpeed;
                    Projectile.velocity = (Projectile.velocity * 20f + direction) / 21f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 生成黄色粒子特效和爆炸特效
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(2f, 2f);
                Dust.NewDustPerfect(Projectile.position, DustID.GoldCoin, speed, 0, Color.Yellow);
            }

            // 20% 概率生成新的 BrassLight 弹幕
            if (Main.rand.NextFloat() < 0.2f)
            {
                Vector2 spawnPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(25 * 16)).RotatedByRandom(MathHelper.TwoPi);
                Vector2 direction = Projectile.Center - spawnPosition;
                direction.Normalize();
                direction *= 10f; // 调整激光速度

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, direction, ModContent.ProjectileType<BrassLight>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}
