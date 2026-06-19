using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
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


namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class SaturnRingProjectile : ModProjectile
    {
        private Vector2 startPosition;
        private bool initialized = false;
        private NPC target;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // 设置拖尾的长度
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // 设置拖尾模式
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.scale = 1.5f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
            }

            CheckScreenBounds();

            if (Main.rand.NextBool(3)) // 以1/3的概率创建粒子特效
            {
                int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Fireworks);
                Main.dust[dustId].velocity /= 2f;
            }

        }

        private void CheckScreenBounds()
        {
            Vector2 screenBoundsMin = Main.screenPosition;
            Vector2 screenBoundsMax = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight);

            if (Projectile.position.X <= screenBoundsMin.X || Projectile.position.X + Projectile.width >= screenBoundsMax.X)
            {
                Projectile.velocity.X = -Projectile.velocity.X;
                Projectile.damage = (int)(Projectile.damage * 1.15f); // 每次反弹增加15%的伤害
            }
            if (Projectile.position.Y <= screenBoundsMin.Y || Projectile.position.Y + Projectile.height >= screenBoundsMax.Y)
            {
                Projectile.velocity.Y = -Projectile.velocity.Y;
                Projectile.damage = (int)(Projectile.damage * 1.15f); // 每次反弹增加15%的伤害
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 2;
            target.AddBuff(BuffID.OnFire, 60); // 使敌人持续受到火焰伤害
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D lightTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SaturnRingProjectile").Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
                Color color = Color.Lerp(Color.Orange, Color.OrangeRed, colorInterpolation) * 0.4f;
                color.A = 0;

                Vector2 drawPosition = Projectile.oldPos[i] + lightTexture.Size() * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(-28f, -28f);

                Color outerColor = color;
                Color innerColor = color * 0.5f;

                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
                if (Projectile.timeLeft <= 60)
                {
                    intensity *= Projectile.timeLeft / 60f;
                }

                Vector2 outerScale = new Vector2(2f) * intensity;
                Vector2 innerScale = new Vector2(2f) * intensity * 0.7f;
                outerColor *= intensity;
                innerColor *= intensity;

                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, 0f, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, 0f, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
            }
            return false;
        }



    }
}
