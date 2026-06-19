using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class ControllableHaloPROJ : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100; // The visual width is just for reference, not affecting the particle radius.
            Projectile.height = 100;
            Projectile.friendly = true; // Make sure it's set to friendly to not harm players or NPCs.
            Projectile.penetrate = -1; // Infinite penetrate, it doesn't disappear upon hitting an enemy.
            Projectile.tileCollide = false; // Allows the projectile to pass through tiles.
            Projectile.ignoreWater = true; // The projectile will not be slowed by water.
            Projectile.timeLeft = 2; // Keeps the projectile alive only very briefly, to ensure it constantly respawns and follows the cursor.

        }
        public override bool? CanDamage()
        {
            // 确保弹幕不会对NPC造成伤害
            return false;
        }

        public override void AI()
        {
            // This will make the projectile always follow the cursor.
            Vector2 mousePosition = Main.MouseWorld;
            Projectile.position = mousePosition - Projectile.Size / 2f;

            // 添加白色光源
            Lighting.AddLight(Projectile.Center, 0.8f, 0.8f, 0.8f);

            // Ensuring the projectile doesn't disappear by resetting its time left.
            Projectile.timeLeft = 2;

            // 遍历所有NPC，检查是否与弹幕重叠
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Hitbox.Intersects(Projectile.Hitbox))
                {
                    // 给重叠的敌人NPC施加Buff，持续5秒（300帧）
                    npc.AddBuff(ModContent.BuffType<ControllableHaloBuff>(), 300);
                }
            }

            // 添加粒子特效
            CreateParticles();
        }

        private void CreateParticles()
        {
            float time = Main.GameUpdateCount * 0.05f;
            int count = 2;

            for (int i = 0; i < count; i++)
            {
                float angle = time + i * MathHelper.Pi;
                float radius = 24f;
                Vector2 offset = angle.ToRotationVector2() * radius;

                Vector2 pos = Projectile.Center + offset;
                Dust dust = Dust.NewDustPerfect(pos, DustID.GemDiamond, Vector2.Zero, 120, Color.White * 0.6f, 1.1f);
                dust.velocity = Vector2.Zero;
                dust.fadeIn = 1f;
                dust.noGravity = true;
                dust.noLight = false; // 开启光照
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }

            // 增加亮度（光源）
            Lighting.AddLight(Projectile.Center, 1.0f, 1.0f, 1.0f); // RGB 白光
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() / 2f;

            Main.spriteBatch.Draw(tex, drawPos, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            return false; // 屏蔽默认绘制
        }


    }
}
