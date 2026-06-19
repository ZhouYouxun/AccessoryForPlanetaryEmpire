using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;


namespace AccessoryForPlanetaryEmpire.Projectiles
{
    public class WhiteHolePROJ2 : ModProjectile
    {
        private float initialSpeed = 10f; // 初始速度倍率
        private float finalSpeed = 1f; // 最终速度倍率
        private float speedDecay = 0.98f; // 每帧速度衰减
        private int rotationAngle = 720; // 旋转覆盖720度
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // 设置拖尾的长度
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // 设置拖尾模式
        }
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV"; // 使用透明弹幕贴图

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 36000;
        }

        public override bool? CanDamage()
        {
            return false; // 弹幕不会对NPC造成伤害
        }

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Player player = Main.player[Projectile.owner];
                if (!player.active || player.dead || !player.GetModPlayer<GravitationalFieldPlayer>().hasGravitationalField)
                {
                    Projectile.Kill();
                    return;
                }

                // 逐渐降低的旋转速度逻辑
                if (initialSpeed > finalSpeed)
                {
                    initialSpeed *= speedDecay; // 速度逐渐减慢
                }
                else
                {
                    initialSpeed = finalSpeed; // 最终速度稳定在1倍
                }

                // 使用调整后的速度进行旋转
                double rotatingSpeed = MathHelper.ToRadians(rotationAngle) / 500;
                int radius = 650; // 公转半径
                Projectile.ai[0] += initialSpeed; // 控制旋转速度
                double angle = Projectile.ai[0] * rotatingSpeed + Projectile.ai[1] * Math.PI;
                Vector2 offset = -Vector2.UnitX * radius; // 修改这里，使其向左
                offset = offset.RotatedBy(angle);
                Projectile.position = player.Center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2);

                // 生成粒子特效拖尾
                // CreateWhiteTrailEffect(Projectile);

                // 检测其他弹幕并增强伤害逻辑
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile otherProj = Main.projectile[i];
                    if (otherProj.active && otherProj.friendly && !otherProj.hostile && otherProj.owner == Projectile.owner)
                    {
                        if (otherProj.type != Projectile.type && otherProj.Hitbox.Intersects(Projectile.Hitbox))
                        {
                            if (IsEligibleProjectile(otherProj) && otherProj.ai[0] == 0)
                            {
                                otherProj.damage = (int)(otherProj.damage * 2.25); // 增加125%伤害
                                otherProj.CritChance = 100; // 必定暴击
                                otherProj.ai[0] = 1; // 标记这个弹幕已经增加了伤害，避免重复增加
                                SoundEngine.PlaySound(SoundID.Item84, Projectile.position); // 播放声音
                            }
                        }
                    }
                }
            }
        }

        private bool IsEligibleProjectile(Projectile projectile)
        {
            // 默认的职业类型检测
            bool isEligible = projectile.DamageType == DamageClass.Melee ||
                              projectile.DamageType == DamageClass.Ranged ||
                              projectile.DamageType == DamageClass.Magic ||
                              projectile.DamageType == DamageClass.Throwing;

            // 尝试加载 CalamityMod 模组并检查 RogueDamageClass
            Mod calamityMod;
            if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
            {
                // 使用反射获取 RogueDamageClass 的实例
                var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");

                if (rogueType != null)
                {
                    FieldInfo rogueInstanceField = rogueType.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
                    var rogueInstance = rogueInstanceField?.GetValue(null) as DamageClass;
                    if (rogueInstance != null && projectile.DamageType == rogueInstance)
                    {
                        isEligible = true; // 允许 Rogue 类型弹幕也能享受反弹效果
                    }
                }
            }

            return isEligible;
        }

        // 创建白色粒子特效拖尾的方法
        private void CreateWhiteTrailEffect(Projectile projectile)
        {
            int dustID = DustID.WhiteTorch;
            float sizeMultiplier = 4f;
            float brightnessMultiplier = 7f;
            int particleCount = 1;

            for (int i = 0; i < particleCount; i++)
            {
                Vector2 position = projectile.Center + new Vector2(Main.rand.NextFloat(-projectile.width / 2, projectile.width / 2), Main.rand.NextFloat(-projectile.height / 2, projectile.height / 2));
                Dust dust = Dust.NewDustPerfect(position, dustID, Vector2.Zero, 100, Color.White * brightnessMultiplier, sizeMultiplier);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity = Vector2.Zero;
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // 📏 全局缩放因子（整体缩小到原来的 50%）
            float globalScale = 0.5f;

            // === 🌌 外围魔法圈（蓝色星空） ===
            string[] outerRings = new[]
            {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_01",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_02",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_03"
    };

            for (int i = 0; i < outerRings.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(outerRings[i]).Value;

                float rotation = Main.GlobalTimeWrappedHourly * (0.4f + 0.1f * i);
                float pulse = 1f + 0.08f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f + i);
                float scale = 1.2f * pulse * globalScale;

                Color ringColor = Color.Lerp(Color.LightBlue, Color.Cyan, 0.4f) * 0.5f;
                ringColor.A = 0;

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    ringColor,
                    rotation,
                    tex.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            // === ⭐核心星星组（叠加+反转旋转+脉动） ===
            string[] stars = new[]
            {
                "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_04",
                "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_07",
                "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_08"
            };

            for (int i = 0; i < stars.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(stars[i]).Value;

                float pulse = 0.85f + 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f + i);
                float rotation = Main.GlobalTimeWrappedHourly * (i % 2 == 0 ? 1f : -1f); // 交替方向

                Color color = Color.Lerp(Color.SkyBlue, Color.White, 0.3f) * 0.7f;
                color.A = 0;

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    color,
                    rotation,
                    tex.Size() * 0.5f,
                    pulse * 0.5f * globalScale,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }





    }
}