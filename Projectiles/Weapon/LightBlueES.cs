using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using AccessoryForPlanetaryEmpire.Players;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class LightBlueES : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1.0f; // 通过调整缩放比例来变大子弹贴图

        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); // 旋转弹幕方向

            // 增加亮光效果
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 1f); // 调整光的颜色和强度

            // 大量白色粒子效果
            for (int i = 0; i < 10; i++)
            {
                Vector2 dustPos = Projectile.position - Projectile.velocity * ((float)i / 10f);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.WhiteTorch, null, 100, Color.White, 5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.fadeIn = 1f; // 使粒子迅速消失
            }

            // 强化追踪能力
            float maxDetectRadius = 500f; // 增加检测范围
            float projSpeed = 10f; // 增加追踪速度
            NPC target = null;

            // 检测范围内的敌人
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(this) && Vector2.Distance(npc.Center, Projectile.Center) < maxDetectRadius)
                {
                    target = npc;
                    break;
                }
            }

            // 追踪目标
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                direction *= projSpeed;
                Projectile.velocity = (Projectile.velocity * 10f + direction) / 11f; // 增强追踪效果
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 增加击中时的效果
            Player player = Main.player[Projectile.owner];
            player.statLife += 7; // 恢复7点生命值
            player.HealEffect(7); // 显示治疗效果
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/Weapon/LightBlueES").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
