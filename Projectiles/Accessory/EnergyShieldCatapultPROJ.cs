using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;


namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class EnergyShieldCatapultPROJ : ModProjectile
    {
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV"; // 使用透明弹幕贴图

        public override void SetDefaults()
        {
            Projectile.width = 100; // 直径为10格，半径为5格
            Projectile.height = 100;
            Projectile.friendly = true; // 设置弹幕对玩家友好，意味着这个弹幕可以对敌方NPC造成伤害，而不会伤害玩家或玩家的队友。
            Projectile.hostile = false; // 设置弹幕不是敌对的，意味着这个弹幕不会对玩家造成伤害。
            Projectile.penetrate = -1; // 设置弹幕的穿透次数为无限。弹幕在击中敌人后不会消失，可以继续飞行并击中更多的敌人。
            Projectile.tileCollide = false; // 设置弹幕不会与地形碰撞，意味着弹幕可以穿过墙壁和其他障碍物。
            Projectile.ignoreWater = true; // 设置弹幕忽略水的存在，弹幕在水中的行为就像在空气中一样，不会减速或受到阻碍。
            Projectile.timeLeft = 300; // 设置弹幕持续时间为5秒（60帧*5秒）

        }

        public override void AI()
        {
            // 修改粒子效果的生成半径
            for (int i = 0; i < 20; i++)
            {
                Vector2 position = Projectile.Center + Main.rand.NextVector2CircularEdge(100f, 100f); // 半径为5格
                Vector2 velocity = Main.rand.NextVector2CircularEdge(3f, 3f); // 随机旋转速度
                Dust dust = Dust.NewDustPerfect(position, DustID.MagicMirror, velocity, 150, Color.Red, Main.rand.NextFloat(1f, 2f));
                dust.noGravity = true;
                dust.fadeIn = 1f; // 开始时淡入
                dust.scale = Main.rand.NextFloat(1f, 1.5f); // 随机大小
            }

            // 检查并清除与此弹幕碰撞的敌方弹幕
            if (Projectile.owner == Main.myPlayer) // 确保只有本地玩家的弹幕执行这段逻辑
            {
                foreach (Projectile p in Main.projectile)
                {
                    // 检查弹幕是否活跃，确保不是当前弹幕本身，且是敌方弹幕
                    if (p.active && p.type != Projectile.type && !p.friendly && p.hostile)
                    {
                        // 计算距离，如果敌方弹幕在圆环范围内，则摧毁
                        if (Vector2.Distance(Projectile.Center, p.Center) <= 100f)
                        {
                            p.Kill(); // 销毁敌方弹幕
                            p.active = false;
                            p.netUpdate = true;
                        }
                    }
                }
            }

            // 使粒子逐渐消失
            for (int i = 0; i < Main.maxDust; i++)
            {
                Dust dust = Main.dust[i];
                if (dust.active && dust.type == DustID.MagicMirror)
                {
                    float distance = Vector2.Distance(Projectile.Center, dust.position);
                    if (distance <= 100f)
                    {
                        dust.scale *= 0.98f; // 逐渐缩小
                        dust.alpha += 2; // 逐渐变透明
                        if (dust.alpha > 255) dust.active = false; // 当完全透明时消失
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // 🌟 全局缩放系数
            float globalScale = 0.5f;

            // === 🔵 中心圆环（circle_05，深蓝脉动） ===
            Texture2D centerTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/circle_05").Value;
            float centerPulse = 1f + 0.05f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f);
            Color centerColor = Color.DarkBlue * 0.8f;
            centerColor.A = 0;

            Main.EntitySpriteDraw(
                centerTex,
                drawPos,
                null,
                centerColor,
                0f,
                centerTex.Size() * 0.5f,
                centerPulse * globalScale,
                SpriteEffects.None,
                0
            );

            // === 💠 中层双向 window_04（浅蓝反转+脉动） ===
            Texture2D windowTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/window_04").Value;

            for (int i = 0; i < 2; i++)
            {
                float pulse = 1f + 0.06f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f + i);
                float rotation = Main.GlobalTimeWrappedHourly * (i == 0 ? 1f : -1f);

                Color color = Color.LightBlue * 0.55f;
                color.A = 0;

                Main.EntitySpriteDraw(
                    windowTex,
                    drawPos,
                    null,
                    color,
                    rotation,
                    windowTex.Size() * 0.5f,
                    pulse * globalScale,
                    SpriteEffects.None,
                    0
                );
            }

            // === 🌀 外层旋转弧线 twirl_01~03 + 多次绘制形成圆环 ===
            string[] twirls = new[]
            {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/twirl_01",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/twirl_02",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/twirl_03"
    };

            for (int j = 0; j < 5; j++) // 绕圈绘制5次
            {
                for (int i = 0; i < twirls.Length; i++)
                {
                    Texture2D tex = ModContent.Request<Texture2D>(twirls[i]).Value;

                    float pulse = 1f + 0.05f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f + i);
                    float rotation = Main.GlobalTimeWrappedHourly * 0.5f * (i % 2 == 0 ? 1 : -1) + MathHelper.TwoPi * j / 5f;

                    Color color = Color.Cyan * 0.45f;
                    color.A = 0;

                    Main.EntitySpriteDraw(
                        tex,
                        drawPos,
                        null,
                        color,
                        rotation,
                        tex.Size() * 0.5f,
                        pulse * globalScale,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            return false;
        }





    }


}




