using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class NeptuneProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Neptune");
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5f;

        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.myPlayer != Projectile.owner) return;

            for (int i = 0; i < 4; i++)
            {
                int tid = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<NeptuneProjectileTrident>(),
                    0, 0, Projectile.owner,
                    i * MathHelper.PiOver2,
                    i);

                if (tid < Main.maxProjectiles)
                {
                    var tri = Main.projectile[tid].ModProjectile as NeptuneProjectileTrident;
                    if (tri != null)
                    {
                        tri.HostID = Projectile.whoAmI;  // ⭐ 绑定海王星
                    }
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<NeptunePlayer>().hasNeptuneAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            // 设置公转初始速度
            float baseRotatingSpeed = MathHelper.TwoPi / 1200f; // 类木行星的默认公转速度（每20秒一圈）
            float rotatingSpeed = baseRotatingSpeed;

            // 初始化设置，第一次运行时触发
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[1] = 10f; // 初始速度倍率为10倍
            }

            // 每一帧减速，直到达到正常速度
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 1f, 0.02f); // 指数减速
            rotatingSpeed *= Math.Max(Projectile.localAI[1], 1f); // 确保旋转速度不低于正常速度

            Projectile.localAI[0]++; // 递增计时器

            // 根据减速后的旋转速度调整角度增量
            Projectile.ai[0] += (float)(rotatingSpeed / baseRotatingSpeed * 2f);
            double angle = Projectile.ai[0] * baseRotatingSpeed;
            Vector2 offset = new Vector2(1, 1) * 25 * 16; // 公转半径，海王星位于右下角
            offset = offset.RotatedBy(angle);
            Projectile.position = player.Center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2);

            // 添加海蓝色光效（海王星主题色）
            Lighting.AddLight(Projectile.Center, 0.3f, 0.5f, 1.0f); // 添加海蓝色光效

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;

        
        }



        public override bool PreDraw(ref Color lightColor) // 确保贴图的中心点为绘制的中心点
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }




    }
}
