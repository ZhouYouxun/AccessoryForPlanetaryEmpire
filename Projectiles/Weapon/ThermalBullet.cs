using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;



namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class ThermalBullet : ModProjectile
    {
        public bool hasPierced = false; // 标记是否已经穿透了一个敌人

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2; // 可以穿透一个敌人
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            // 添加橘黄色的光学尾迹
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3() * 0.6f);

            if (hasPierced)
            {
                // 查找最近的敌人并追踪
                NPC target = null;
                float maxDistance = 3000f;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, Projectile.Center);
                        if (distance < maxDistance)
                        {
                            maxDistance = distance;
                            target = npc;
                        }
                    }
                }

                if (target != null)
                {
                    // 追踪目标
                    Vector2 direction = target.Center - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity = (Projectile.velocity * 20f + direction * 10f) / 21f;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // 撞到物块时销毁弹幕
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasPierced)
            {
                hasPierced = true;
                Projectile.penetrate++; // 增加穿透次数
                // 生成第一次击中时的粒子效果
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, default, 1.5f);
                }
            }
            else
            {
                // 生成最终消失时的粒子效果
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, default, 1.5f);
                }
                Projectile.Kill(); // 第二次击中后销毁弹幕
            }
        }



        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D lightTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/Weapon/ThermalBullet").Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
                Color color = Color.Lerp(Color.Orange, Color.OrangeRed, colorInterpolation) * 0.4f;
                color.A = 0;
                Vector2 drawPosition = Projectile.oldPos[i] + lightTexture.Size() * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(-28f, -28f); // 最后的向量是为了将圆圈偏移显示在实际碰撞箱的位置，而不是右下角。
                Color outerColor = color;
                Color innerColor = color * 0.5f;
                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
                if (Projectile.timeLeft <= 60)
                {
                    intensity *= Projectile.timeLeft / 60f;
                }
                Vector2 outerScale = new Vector2(8f) * intensity; // 调整大小为原来的8倍
                Vector2 innerScale = new Vector2(8f) * intensity * 0.7f; // 调整大小为原来的8倍
                outerColor *= intensity;
                innerColor *= intensity;
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, 0f, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, 0f, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
            }
            return false;
        }








        //public override void OnSpawn(IEntitySource source)
        //{
        //    // 生成时的粒子效果
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, default, 1.5f);
        //    }
        //}




    }
}
