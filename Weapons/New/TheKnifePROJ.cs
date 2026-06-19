using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using AccessoryForPlanetaryEmpire; // 引用光点系统

namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    public class TheKnifePROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "NewWeapons";

        //public override string Texture => "Terraria/Images/Extra_89";
        //public override string Texture => "AccessoryForPlanetaryEmpire/Weapons/New/TheKnifePROJ";

        private static readonly string TrailTex = "AccessoryForPlanetaryEmpire/Texture/STP/fire_006";
        private static readonly string EyeTex = "AccessoryForPlanetaryEmpire/Texture/STP/flameeye_005";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.scale = 0.5f;
            Projectile.timeLeft = 250;
            Projectile.extraUpdates = 1;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;

            Projectile.DamageType = DamageClass.Melee;
        }

        // ========================================================================
        // AI：减速 + 旋转 + 甩出手里剑 + 甩出白色光点
        // ========================================================================
        public override void AI()
        {
            // 1. 每帧减速（唯一飞行逻辑）
            Projectile.velocity *= 0.99f;

            // 2. 持续旋转
            Projectile.rotation += 0.25f;


            // ========== 动态频率发射 Shuriken（3～10 帧之间） ==========
            Projectile.localAI[0]++; // 计时器

            // 当前速度越慢 → 发射越慢；越快 → 发射越快
            // 将速度映射为 3～10 帧区间
            float spd = Projectile.velocity.Length();
            int fireRate = (int)MathHelper.Clamp(10f - spd * 0.5f, 3f, 10f);

            // 到点就发射一个
            if (Projectile.localAI[0] >= fireRate)
            {
                Projectile.localAI[0] = 0;

                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 dir = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Vector2 vel = dir * 12f;

                    //Projectile.NewProjectile(
                    //    Projectile.GetSource_FromThis(),
                    //    Projectile.Center,
                    //    vel,
                    //    ProjectileID.Shuriken,
                    //    Projectile.damage / 2,
                    //    0f,
                    //    Projectile.owner
                    //);
                }
            }

            // 每 X 帧触发一次粒子
            Projectile.localAI[1]++;

            if (Projectile.localAI[1] >= 2)
            {
                Projectile.localAI[1] = 0;

                spiralAngle += 0.25f;
                PLALightingBoltsSystem.PLA_Spawn_DualBlueSpiral(Projectile.Center, spiralAngle);
            }

        }
        private float spiralAngle = 0f;

        // ========================================================================
        // 命中特效（保留原来的银白放射爆裂）
        // ========================================================================
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 银白爆裂阵
            for (int i = 0; i < 16; i++)
            {
                float angle = MathHelper.TwoPi * i / 16f;
                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(3f, 6f);
                Dust d = Dust.NewDustPerfect(
                    target.Center,
                    DustID.Silver,
                    vel,
                    50,
                    Color.White,
                    1.5f
                );
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            // 默认贴图（你的弹幕自身的贴图）
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() * 0.5f;

            float rotation = Projectile.rotation;
            float scale = 1.0f;

            // ===============================
            // 1. 主体绘制（正常旋转）
            // ===============================
            // 赛博朋克脉动颜色
            float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 6f) * 0.5 + 0.5);
            Color cyberColor = Color.Lerp(new Color(180, 200, 255), new Color(130, 160, 255), pulse);

            // 主体（脉动染色）
            sb.Draw(
                tex,
                pos,
                null,
                cyberColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );


            // ===============================
            // 2. 第二层（镜像反转）
            // ===============================
            sb.Draw(
                tex,
                pos,
                null,
                cyberColor * 0.75f,
                -rotation * 1.1f,
                origin,
                scale * 0.95f,
                SpriteEffects.FlipHorizontally,
                0f
            );

            // ===============================
            // 3. 荧光包边（Glow效果）
            // ===============================

            // 包边颜色（银白科技风）
            Color glow = new Color(240, 240, 255) * 0.85f;

            // 外扩量
            float glowDist = 4f;

            // 往四个方向扩一圈
            Vector2[] glowOffsets =
            {
        new Vector2( glowDist, 0),
        new Vector2(-glowDist, 0),
        new Vector2(0,  glowDist),
        new Vector2(0, -glowDist),
    };

            foreach (var off in glowOffsets)
            {
                sb.Draw(
                    tex,
                    pos + off,
                    null,
                    glow,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }

            return false;
        }









    }
}
