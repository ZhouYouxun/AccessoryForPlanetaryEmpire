using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class MoonShieldProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moon Shield");
        }
        public override string Texture => "Terraria/Images/Extra_89"; // 使用原版光点

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1; // 增加更新频率，使拖尾更平滑
        }

        public override void AI()
        {

            if (Main.rand.NextBool(1)) // 每帧50%概率生成，避免过密
            {
                PLALightingBoltsSystem.SpawnMoonShieldSparkle(Projectile.Center);
            }

            Projectile.velocity *= 0.98f; // 速度逐渐降低
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.hostile && other.Hitbox.Intersects(Projectile.Hitbox))
                {
                    other.Kill();
                }
            }

            // 保存旧位置
            if (Projectile.timeLeft % 2 == 0) // 减少存储频率
            {
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
                {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                }
                Projectile.oldPos[0] = Projectile.position;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float sizeMultiplier = 0.25f;

            Texture2D gearTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/STP/diamondgear_016").Value;
            Texture2D energyTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/STP/energy_001").Value;
            Texture2D starTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/star_04").Value;

            float time = Main.GlobalTimeWrappedHourly;

            // 🌀 外圈旋转齿轮（缓慢逆时针）
            spriteBatch.Draw(gearTex, drawPos, null, Color.White * 0.4f, -time * 0.4f, gearTex.Size() / 2f, sizeMultiplier * 1.5f, SpriteEffects.None, 0f);

            // 🔆 中圈能量缓动（脉动发光）
            float pulse = 1f + (float)Math.Sin(time * 3f) * 0.08f;
            spriteBatch.Draw(energyTex, drawPos, null, Color.White * 0.6f, time * 0.3f, energyTex.Size() / 2f, sizeMultiplier * 1.15f * pulse, SpriteEffects.None, 0f);

            // 🌟 中心星星叠加两层、反向旋转、缓慢脉动
            float centerPulse = 1f + (float)Math.Sin(time * 2f + 0.5f) * 0.05f;
            float rot1 = time * 0.6f;
            float rot2 = -time * 0.6f;

            spriteBatch.Draw(starTex, drawPos, null, Color.White * 0.9f, rot1, starTex.Size() / 2f, sizeMultiplier * 0.9f * centerPulse, SpriteEffects.None, 0f);
            spriteBatch.Draw(starTex, drawPos, null, Color.White * 0.9f, rot2, starTex.Size() / 2f, sizeMultiplier * 0.9f * centerPulse, SpriteEffects.None, 0f);

            return false;
        }





    }
}
