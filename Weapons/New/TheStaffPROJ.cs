using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    public class TheStaffPROJ : ModProjectile
    {

        // Trail 开启
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 500;
            Projectile.extraUpdates = 1;

            Projectile.DamageType = DamageClass.Magic;
        }

        float pulse = 0f;

        public override void AI()
        {
            pulse += 0.18f;

            // ========= 直线加速 =========
            Projectile.velocity *= 1.01f;

            // 摆正
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // ========= 紫色赛博朋克 Dust =========
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.PurpleTorch,
                    Projectile.velocity * Main.rand.NextFloat(-0.1f, 0.15f),
                    80,
                    Color.Lerp(Color.MediumPurple, Color.Violet, Main.rand.NextFloat()),
                    Main.rand.NextFloat(1.0f, 1.6f)
                );
                d.noGravity = true;
            }

            // ========= 赛博朋克“光学错位”小碎片 =========
            if (Main.rand.NextBool(4))
            {
                Dust d2 = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(4f, 4f),
                    DustID.PurpleCrystalShard,
                    Projectile.velocity.RotatedByRandom(0.4f) * 0.2f,
                    120,
                    Color.White,
                    0.8f
                );
                d2.noGravity = true;
            }
        }

        // ========== 数学美感 紫色爆裂 ==========
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damage)
        {
            // 放射
            for (int i = 0; i < 20; i++)
            {
                float angle = MathHelper.TwoPi * i / 20f;
                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(2f, 6f);

                Dust d = Dust.NewDustPerfect(
                    target.Center,
                    DustID.PurpleTorch,
                    vel,
                    90,
                    Color.Lerp(Color.White, Color.MediumPurple, 0.7f),
                    Main.rand.NextFloat(1.1f, 1.8f)
                );
                d.noGravity = true;
            }

            // 椭圆轨迹（数学美感）
            for (int i = 0; i < 24; i++)
            {
                float t = MathHelper.TwoPi * i / 24f;
                Vector2 pos = target.Center + new Vector2(
                    (float)Math.Cos(t) * 18f,
                    (float)Math.Sin(t) * 9f
                );

                Dust d = Dust.NewDustPerfect(
                    pos,
                    DustID.PurpleTorch,
                    Vector2.Zero,
                    110,
                    Color.MediumPurple,
                    1.0f
                );
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            // 原本的主体贴图（紫色拖尾 + 错乱）
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = tex.Size() / 2f;

            // 赛博朋克紫光错乱颜色
            Color[] glitch =
            {
        Color.MediumPurple,
        Color.Violet,
        Color.Magenta
    };

            float t = (Main.GlobalTimeWrappedHourly * 2f) % 3f;
            int idx = (int)t;
            Color mix = Color.Lerp(glitch[idx], glitch[(idx + 1) % 3], t % 1f);

            // ========== 拖尾渲染 ==========
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 old = Projectile.oldPos[i];
                if (old == Vector2.Zero) continue;

                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                float distort = (float)Math.Sin(i * 0.4f + pulse) * 2f;

                Vector2 drawPos = old + Projectile.Size / 2f - Main.screenPosition;
                Color c = mix * fade * 0.75f;

                sb.Draw(
                    tex,
                    drawPos + new Vector2(distort, 0),
                    null,
                    c,
                    Projectile.rotation,
                    origin,
                    0.55f + fade * 0.1f,
                    SpriteEffects.None,
                    0f
                );
            }

            // ========== 主体贴图 ==========
            Vector2 pos = Projectile.Center - Main.screenPosition;

            sb.Draw(
                tex,
                pos,
                null,
                Color.Lerp(Color.White, mix, 0.6f),
                Projectile.rotation,
                origin,
                0.75f,
                SpriteEffects.None,
                0f
            );

            // ========== ★ 升级版太极双图层（flamelance_009 / flamelance_010） ==========

            // 载入贴图
            Texture2D flameA = ModContent.Request<Texture2D>(
                "AccessoryForPlanetaryEmpire/Texture/STP/flamelance_009"
            ).Value;

            Texture2D flameB = ModContent.Request<Texture2D>(
                "AccessoryForPlanetaryEmpire/Texture/STP/flamelance_010"
            ).Value;

            Vector2 flameOriginA = flameA.Size() / 2f;
            Vector2 flameOriginB = flameB.Size() / 2f;

            // 缩放改成 0.07（更精致）
            float flameScale = 0.07f;

            // 旋转速度大幅提升
            float fastRot = Projectile.timeLeft * 0.35f;

            // 透光紫脉动
            float pulseGlow = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 6f) * 0.5 + 0.5);
            Color purpleGlow = Color.Lerp(Color.MediumPurple, Color.Magenta, pulseGlow) * 0.85f;

            // 第一层：正常旋转
            sb.Draw(
                flameA,
                pos,
                null,
                purpleGlow,
                fastRot,
                flameOriginA,
                flameScale,
                SpriteEffects.None,
                0f
            );

            // 第二层：旋转 + 180°（太极）
            sb.Draw(
                flameB,
                pos,
                null,
                purpleGlow * 0.9f,  // 再亮一点
                fastRot + MathHelper.Pi,
                flameOriginB,
                flameScale,
                SpriteEffects.None,
                0f
            );


            return false;
        }




    }
}
