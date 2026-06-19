using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Utilities; // 确保引用 Utils 类
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class VoidOfDarknessROT : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 408;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float spinCycleTime = 50f;

            // 如果玩家死亡或取消通道，则销毁弹幕
            if (player.dead || !player.channel)
            {
                Projectile.Kill();
                player.reuseDelay = 2;
                return;
            }

            int direction = Math.Sign(Projectile.velocity.X);
            Projectile.velocity = new Vector2(direction, 0f);

            // 初始旋转角度
            if (Projectile.ai[0] == 0f)
            {
                Projectile.rotation = new Vector2(direction, -player.gravDir).ToRotation() + MathHelper.ToRadians(135f);
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.rotation -= MathHelper.PiOver2;
                }
            }

            Projectile.ai[0] += 1f;
            Projectile.rotation += MathHelper.TwoPi * 2f / spinCycleTime * direction;
            int expectedDirection = (player.DirectionTo(Main.MouseWorld).X > 0f).ToDirectionInt();
            if (Projectile.ai[0] % spinCycleTime > spinCycleTime * 0.5f && expectedDirection != Projectile.velocity.X)
            {
                player.ChangeDir(expectedDirection);
                Projectile.velocity = Vector2.UnitX * expectedDirection;
                Projectile.rotation -= MathHelper.Pi;
                Projectile.netUpdate = true;
            }
            SpawnDust(player, direction);
            PositionAndRotation(player);
            VisibilityAndLight();
        }

        private void SpawnDust(Player player, int direction)
        {
            float rotateDirection = Projectile.rotation - MathHelper.PiOver4 * direction;
            Vector2 dustSpawn = Projectile.Center + (rotateDirection + ((direction == -1) ? MathHelper.Pi : 0f)).ToRotationVector2() * 30f;
            Vector2 staffTipDirection = rotateDirection.ToRotationVector2();
            Vector2 tipDustDirection = staffTipDirection.RotatedBy(MathHelper.PiOver2 * Projectile.spriteDirection);

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(dustSpawn - new Vector2(5f), 10, 10, DustID.PurpleTorch, player.velocity.X, player.velocity.Y, 150, default, 1f);
                dust.velocity = Projectile.DirectionTo(dust.position) * 0.1f + dust.velocity * 0.1f;
            }
            for (int j = 0; j < 4; j++)
            {
                float dustVelMult = 1f;
                float dustVelMult2 = 1f;
                switch (j)
                {
                    case 1:
                        dustVelMult2 = -1f;
                        break;
                    case 2:
                        dustVelMult2 = 1.25f;
                        dustVelMult = 0.5f;
                        break;
                    case 3:
                        dustVelMult2 = -1.25f;
                        dustVelMult = 0.5f;
                        break;
                }
                if (!Main.rand.NextBool(6))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.PurpleTorch, 0f, 0f, 100, default, 1f);
                    dust.position = Projectile.Center + staffTipDirection * (60f + Main.rand.NextFloat() * 20f) * dustVelMult2;
                    dust.velocity = tipDustDirection * (4f + 4f * Main.rand.NextFloat()) * dustVelMult2 * dustVelMult;
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.scale = 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        dust.noGravity = false;
                    }
                }
            }
        }

        private void PositionAndRotation(Player player)
        {
            Vector2 plrCtr = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 offset = Vector2.Zero;
            Projectile.Center = plrCtr + offset;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
        }

        private void VisibilityAndLight()
        {
            Lighting.AddLight(Projectile.Center, 1.45f, 1.22f, 0.58f);
            Projectile.alpha -= 128;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitEffects(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffects(target.Center);
        }

        private void OnHitEffects(Vector2 position)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                // 生成黑色粒子特效
                for (int i = 0; i < 40; i++)
                {
                    Vector2 dustPosition = position;
                    Dust dust = Dust.NewDustDirect(dustPosition, 1, 1, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                    dust.noGravity = true;
                    dust.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 5f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle rectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 origin = tex.Size() / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // 调整绘制时的比例，使图片变大50%
            float scale = Projectile.scale * 1.75f;

            // 水平和垂直平移量（可以根据需要调整）
            // Vector2 offset = new Vector2(-500f, -500f); // 水平平移10个像素，垂直平移-10个像素

            Main.EntitySpriteDraw(tex, drawPos, new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation, origin, scale, spriteEffects, 0);
            return false;
        }

    }
}
