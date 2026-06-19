using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs
{
    internal class EMPUse : ModProjectile
    {
        //public override string Texture => "Terraria/Images/Item_4923"; // 星光
        public override string Texture => "Terraria/Images/Extra_89";


        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = frame.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // 🌟脉动透明度
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f + Projectile.whoAmI) * 0.15f + 0.85f;
            Color pulseColor = Color.White * pulse;
            pulseColor.A = 0;

            // === 🌀 内嵌旋转拖尾效果（原 mode 2） ===
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float rot = Projectile.oldRot[i];
                SpriteEffects fx = (Projectile.oldSpriteDirection[i] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = pulseColor * fade;

                Main.EntitySpriteDraw(texture, trailPos, frame, color, rot, origin, Projectile.scale, fx, 0);
            }

            // ✴️ 绘制本体
            Main.EntitySpriteDraw(texture, drawPos, frame, pulseColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }




        public Vector2 target;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 40;
            Projectile.extraUpdates = 2;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); // 旋转弹幕方向

            // 如果未初始化目标，则从 ai 赋值一次
            if (Projectile.localAI[0] == 0f)
            {
                target = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Projectile.localAI[0] = 1f;
            }

            // 🧭 飞向目标
            Vector2 toTarget = target - Projectile.Center;
            float speed = 18f;
            Projectile.velocity = toTarget.SafeNormalize(Vector2.Zero) * speed;

            // 🎯 到达目标，自动消除
            //if (toTarget.LengthSquared() < speed * speed)
            //    Projectile.Kill();

            // ✨可选：中途加一点光效
            Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Vector2.Zero, 100, Color.Cyan, 0.2f).noGravity = true;
        }


    }
}
