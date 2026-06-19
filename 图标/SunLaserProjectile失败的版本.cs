//using System;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria.ID;
//using ReLogic.Content;
//using Terraria.Audio;
//using AccessoryForPlanetaryEmpire.Players;

//namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
//{
//    public class SunLaserProjectile失败的版本 : ModProjectile
//    {
//        public Vector2 InitialCenter;
//        public Vector2 Destination;
//        public Vector2 NPCDestination;
//        public const float MaxRadius = 90f;
//        public ref float YDirection => ref Projectile.ai[0];
//        public ref float Time => ref Projectile.ai[1];
//        public Color sparkColor;

//        public override void SetStaticDefaults()
//        {
//            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
//            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 36;
//        }

//        public override void SetDefaults()
//        {
//            Projectile.width = 84; // 70% 尺寸
//            Projectile.height = 84; // 70% 尺寸
//            Projectile.friendly = true;
//            Projectile.penetrate = -1;
//            Projectile.tileCollide = false;
//            Projectile.ignoreWater = true;
//            Projectile.timeLeft = 300; // 激光持续时间，5秒
//            Projectile.scale = 7f; // 激光放大倍率（70%）
//            Projectile.damage = 850; // 基础伤害
//        }

//        public override void AI()
//        {


//            // 随机选择火花颜色
//            sparkColor = Main.rand.Next(4) switch
//            {
//                0 => Color.Red,
//                1 => Color.MediumTurquoise,
//                2 => Color.Orange,
//                _ => Color.LawnGreen,
//            };

//            // 添加光效
//            Lighting.AddLight(Projectile.Center, Color.DarkSlateGray.ToVector3());

//            // 调整激光的比例
//            Projectile.scale = MathHelper.Lerp(0.001f, 1f, Utils.GetLerpValue(0f, 25f, Time, true));

//            // 保持激光跟随鼠标
//            if (Projectile.localAI[0] == 0f)
//            {
//                InitialCenter = Projectile.Center;
//                Projectile.localAI[0] = 1f;
//            }

//            // 计算激光的运动轨迹（垂直于屏幕）
//            Projectile.velocity = Vector2.UnitY.RotatedBy(Projectile.ai[1]) * 10f;
//            Projectile.ai[1] = (Main.MouseWorld - Projectile.Center).ToRotation();

//            Time++;
//        }

//        public override void OnKill(int timeLeft)
//        {
//            float scaleBonus = Time >= 120f ? Main.rand.NextFloat(3.4f, 4.2f) : Main.rand.NextFloat(0.8f, 1.6f);
//            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center); // 替换为合适的音效
//            float numberOfDusts = Time >= 120f ? 30 : 20;
//            float rotFactor = 360f / numberOfDusts;
//            for (int i = 0; i < numberOfDusts; i++)
//            {
//                sparkColor = Main.rand.Next(4) switch
//                {
//                    0 => Color.Red,
//                    1 => Color.MediumTurquoise,
//                    2 => Color.Orange,
//                    _ => Color.LawnGreen,
//                };

//                float rot = MathHelper.ToRadians(i * rotFactor);
//                Vector2 offset = (Vector2.UnitX * Main.rand.NextFloat(0.2f, 3.1f)).RotatedBy(rot * Main.rand.NextFloat(1.1f, 9.1f));
//                Vector2 velOffset = (Vector2.UnitX * Main.rand.NextFloat(0.2f, 3.1f)).RotatedBy(rot * Main.rand.NextFloat(1.1f, 9.1f));

//                Dust.NewDustPerfect(Projectile.Center + offset, DustID.Lava, velOffset, 100, sparkColor, 1.5f);
//            }
//        }

//        //public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Projectile.velocity * 20f, Projectile.Center + Projectile.velocity * 20f, 22, ref _);

//        //public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//        //{
//        //    target.AddBuff(BuffID.OnFire, 300); // 替换为合适的debuff
//        //}

//        //public override void OnHitPlayer(Player target, Player.HurtInfo info)
//        //{
//        //    target.AddBuff(BuffID.OnFire, 300); // 替换为合适的debuff
//        //}





//    }
//}
