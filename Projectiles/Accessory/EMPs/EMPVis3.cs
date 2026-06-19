using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.Defense;
using AccessoryForPlanetaryEmpire.Global;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs
{
    public class EMPVis65L : ModProjectile
    {
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV";

        private const float MinScale = 0.04f;
        private const float MaxScale = 2.6f;
        private const int fadeOutTime = 15;

        private float visualScale = MinScale;
        private float targetScale = MinScale;
        private float fadeOutProgress = 0f;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60 * 10;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.timeLeft = 60;
            Player player = Main.player[Projectile.owner];

            if (!player.active || !player.GetModPlayer<EMP65LPlayer>().has65LInterceptor)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.Center;
            var modPlayer = player.GetModPlayer<EMP65LPlayer>();

            // 🧮 清除次数范围（可以调整）
            const int minCharge = 1;
            const int maxCharge = 150;

            // 🔠 对应的视觉大小范围（可以调整）
            const float minScale = 0.02f;
            const float maxScale = 1.5f;

            // 获取当前次数（统一接口）
            int currentCharge = modPlayer.GetCurrentCharge();

            // 🧮 根据当前次数计算比例 t：0 → 1
            float t = Utils.GetLerpValue(minCharge, maxCharge, currentCharge, true);

            // 插值计算目标缩放值
            targetScale = MathHelper.Lerp(minScale, maxScale, t);

            // 🎯 平滑缩放（缓冲）
            visualScale = MathHelper.Lerp(visualScale, targetScale, 0.15f);

            // 💠 周期性粒子效果（可选）
            if (Main.myPlayer == Projectile.owner && Projectile.timeLeft % 15 == 0)
            {
                //EMPAll.CreateCooldownCircle(65);
            }
        }
















































        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float time = Main.GlobalTimeWrappedHourly;

            // 🟠 主色为橙红，透明度略调低
            Color baseColor = Color.OrangeRed * 0.35f;
            baseColor.A = 0;

            // 🌌 多层魔法阵绘制（增加表现力）
            string[] outerRings = {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_01",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_02",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_03"
    };

            for (int i = 0; i < outerRings.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(outerRings[i]).Value;
                float scaleFactor = visualScale * (1f + i * 0.1f);
                float rotation = time * (0.4f + i * 0.15f);
                Color layerColor = baseColor * (1f - i * 0.25f);

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    layerColor,
                    rotation,
                    tex.Size() / 2f,
                    scaleFactor,
                    SpriteEffects.None,
                    0
                );
            }

            // ✨ 星星层绘制
            string[] stars = {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_04",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_07",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_08"
    };

            for (int i = 0; i < stars.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(stars[i]).Value;
                float rotation = time * (i % 2 == 0 ? 1f : -1f);
                Color starLayerColor = baseColor * (0.9f - i * 0.25f);

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    starLayerColor,
                    rotation,
                    tex.Size() * 0.5f,
                    visualScale * 0.5f,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }












    }
}
