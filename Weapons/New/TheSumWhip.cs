using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

// ★命名空间改成你的模组路径
namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    // ★类名从 ExampleWhipProjectile 改成 TheSumWhip
    public class TheSumWhip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // This makes the projectile use whip collision detection and allows flasks to be applied to it.
            // ★ 设置 Projectile 为“鞭子类型”，开启鞭子碰撞、药水附魔等系统
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            // This method quickly sets the whip's properties.
            // ★ 使用原版鞭子默认参数（非常关键）
            Projectile.DefaultToWhip();

            // use these to change from the vanilla defaults
            // Projectile.WhipSettings.Segments = 20;
            // Projectile.WhipSettings.RangeMultiplier = 1f;

            // ★ 以上两行属于“开发者可选改动”，原文保留不动
        }

        private float Timer
        {
            get => Projectile.ai[0];   // ★ 鞭子动画计时器，控制甩出过程
            set => Projectile.ai[0] = value;
        }

        private float ChargeTime
        {
            get => Projectile.ai[1];   // ★ 充能时间（蓄力状态）
            set => Projectile.ai[1] = value;
        }

        // This example uses PreAI to implement a charging mechanic.
        // If you remove this, also remove Item.channel = true from the item's SetDefaults.
        // ★ PreAI：控制“蓄力鞭子”的逻辑，如果删掉，需要 Item 不再使用 channel。
        public override bool PreAI()
        {
            Player owner = Main.player[Projectile.owner]; // ★ 发出鞭子的玩家实例

            // Like other whips, this whip updates twice per frame (Projectile.extraUpdates = 1), so 120 is equal to 1 second.
            // ★ 鞭子每帧更新 2 次，因此 120 tick = 实际 1 秒
            if (!owner.channel || ChargeTime >= 120)
            {
                return true; // Let the vanilla whip AI run.
                // ★ 当松开攻击键 / 达到满充时，回到原版鞭子 AI
            }

            if (++ChargeTime % 12 == 0) // 1 segment per 12 ticks of charge.
                Projectile.WhipSettings.Segments++;
            // ★ 每 12 tick 增加一段鞭子长度（越充越长）

            // Increase range up to 2x for full charge.
            // ★ 充能时，鞭子长度逐渐 ×2
            Projectile.WhipSettings.RangeMultiplier += 1 / 120f;

            // Reset the animation and item timer while charging.
            // ★ 蓄力期间保持挥舞动画不结束
            owner.itemAnimation = owner.itemAnimationMax;
            owner.itemTime = owner.itemTimeMax;

            {

                // ============================================
                // ★★（新增）飞行期间的科技蓝 Dust 轨迹特效 ★★
                // ============================================
                // 使用数学螺旋参数 + 局部坐标旋转模拟优雅科技光带

                float spiralAngle = Main.GameUpdateCount * 0.25f;
                Vector2 offset = new Vector2(6f, 0f).RotatedBy(spiralAngle);

                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    DustID.BlueCrystalShard,    // 科技蓝
                    offset.SafeNormalize(Vector2.Zero) * 1.25f,
                    150,
                    new Color(80, 160, 255),
                    1.1f
                );
                d.noGravity = true;

                // 对称双螺旋（数学美感：f(θ) + f(θ+π)）
                Dust d2 = Dust.NewDustPerfect(
                    Projectile.Center - offset,
                    DustID.BlueCrystalShard,
                    (-offset).SafeNormalize(Vector2.Zero) * 1.25f,
                    150,
                    new Color(100, 180, 255),
                    1.1f
                );
                d2.noGravity = true;
            }

            return false; // Prevent the vanilla whip AI from running.
            // ★ 在蓄力阶段禁用原版鞭子 AI
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // ★ 给敌人上 Example 自带的 Debuff（100%照抄）
            //target.AddBuff(ModContent.BuffType<ExampleWhipDebuff>(), 240);

            // ★ 让召唤物优先攻击被鞭子标记的敌人
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;

            // ★ 多段命中惩罚：每打一个敌人伤害 ×0.5
            //Projectile.damage = (int)(Projectile.damage * 0.5f);

            {
                // ======================================================
                // ★★（新增）命中时的科技蓝前喷式 Dust 释放（方向性！）★★
                // ======================================================

                Vector2 dir = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);

                for (int i = 0; i < 12; i++)
                {
                    float angle = MathHelper.ToRadians(Main.rand.NextFloat(-25f, 25f));
                    Vector2 shootVel = dir.RotatedBy(angle) * Main.rand.NextFloat(4f, 7f);

                    Dust d = Dust.NewDustPerfect(
                        target.Center,
                        DustID.BlueCrystalShard,
                        shootVel,
                        120,
                        new Color(50, 150, 255),
                        Main.rand.NextFloat(1.0f, 1.6f)
                    );
                    d.noGravity = true;
                }
            }
        }

        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        // ★ 在鞭子段之间画线，避免空隙；这是鞭子的关键部分
        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value; // ★ 默认“鱼线”贴图
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);
                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list); // ★ 自动生成鞭子的控制点（核心）

            DrawLine(list); // ★ 先画线（补间）

            //Main.DrawWhip_WhipBland(Projectile, list);
            // ★ 官方注释：如果不想自定义绘制，可以改用 vanilla 的绘制方法

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[Type].Value; // ★ 使用 Projectile 对应贴图

            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                // ★ 以下每段贴图的 frame 与 origin 都是 Example 标准
                Rectangle frame = new Rectangle(0, 0, 10, 26);
                Vector2 origin = new Vector2(5, 8);
                float scale = 1;

                if (i == list.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 18;

                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 5)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}
