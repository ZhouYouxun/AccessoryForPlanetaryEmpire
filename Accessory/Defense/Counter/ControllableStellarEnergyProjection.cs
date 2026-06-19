using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class ControllableStellarEnergyProjection : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_1"; // 不显示贴图

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.timeLeft = 2;
            Projectile.Center = player.Center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            SpriteBatch sb = Main.spriteBatch;

            Vector2 center = player.Center;

            float t = Main.GlobalTimeWrappedHourly;

            float yaw = t * 0.8f;
            float pitch = t * 0.6f;
            float roll = t * 0.4f;

            Matrix rot = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);

            float halfSize = 12f * 16f;
            float focal = 1200f;
            float zBias = 1200f;

            // ===== 正十二面体顶点（基于黄金比例 φ）=====
            float phi = (1f + (float)Math.Sqrt(5f)) * 0.5f;

            Vector3[] v =
            {
        // (±1, ±1, ±1)
        new(-1,-1,-1), new(-1,-1, 1), new(-1, 1,-1), new(-1, 1, 1),
        new( 1,-1,-1), new( 1,-1, 1), new( 1, 1,-1), new( 1, 1, 1),

        // (0, ±1/φ, ±φ)
        new(0, -1/phi, -phi), new(0, -1/phi,  phi),
        new(0,  1/phi, -phi), new(0,  1/phi,  phi),

        // (±1/φ, ±φ, 0)
        new(-1/phi, -phi, 0), new(-1/phi,  phi, 0),
        new( 1/phi, -phi, 0), new( 1/phi,  phi, 0),

        // (±φ, 0, ±1/φ)
        new(-phi, 0, -1/phi), new(-phi, 0,  1/phi),
        new( phi, 0, -1/phi), new( phi, 0,  1/phi),
    };

            // ===== 正十二面体 30 条边（固定拓扑）=====
            int[,] e =
            {
                {0,1},{0,2},{0,4},
                {1,3},{1,5},
                {2,3},{2,6},
                {3,7},
                {4,5},{4,6},
                {5,7},
                {6,7},

                {8,9},{8,10},{9,11},{10,11},
                {12,13},{12,14},{13,15},{14,15},
                {16,17},{16,18},{17,19},{18,19},

                {8,16},{9,17},{10,18},{11,19}
            };

            Vector2[] pv = new Vector2[v.Length];

            // ===== 3D → 2D 投影 =====
            for (int i = 0; i < v.Length; i++)
            {
                Vector3 p = Vector3.Transform(v[i] * halfSize, rot);
                float persp = focal / (focal + p.Z + zBias);
                pv[i] = center + new Vector2(p.X, p.Y) * persp;
            }

            // ===== 中心纠偏（保持绕玩家中心旋转）=====
            Vector2 avg = Vector2.Zero;
            for (int i = 0; i < pv.Length; i++)
                avg += pv[i];
            avg /= pv.Length;

            Vector2 fix = center - avg;
            for (int i = 0; i < pv.Length; i++)
                pv[i] += fix;

            // ===== 亮黄色科技感（完全保持原逻辑）=====
            Color lineColor = Color.Lerp(
                new Color(255, 220, 80),
                Color.White,
                0.5f + 0.5f * (float)Math.Sin(t * 3f)
            );

            Texture2D lineTex =
                ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/Line").Value;

            for (int i = 0; i < e.GetLength(0); i++)
            {
                DrawLine(
                    sb,
                    pv[e[i, 0]],
                    pv[e[i, 1]],
                    lineColor,
                    6f,
                    lineTex
                );
            }

            return false;
        }


        private void DrawLine(
            SpriteBatch sb,
            Vector2 start,
            Vector2 end,
            Color color,
            float width,
            Texture2D tex)
        {
            if (start == end)
                return;

            start -= Main.screenPosition;
            end -= Main.screenPosition;

            float rotation = (end - start).ToRotation();
            float length = Vector2.Distance(start, end);

            sb.Draw(
                tex,
                start,
                null,
                color,
                rotation,
                tex.Size() * Vector2.UnitY * 0.5f,
                new Vector2(length / tex.Width, width),
                SpriteEffects.None,
                0f
            );
        }
    }
}
