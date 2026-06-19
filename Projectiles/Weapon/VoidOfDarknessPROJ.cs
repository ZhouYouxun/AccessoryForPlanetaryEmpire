using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class VoidOfDarknessPROJ : ModProjectile
    {
        // 定义长枪弹幕的范围。这些是可覆盖的属性，以防你想从这个类继承并修改这些属性。
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 96f;

        public override void SetDefaults()
        {
            // 克隆一个原版长枪弹幕的默认值。长枪特定的值设置了宽度、高度、aiStyle、友好、穿透、碰撞、缩放、隐藏、持有者命中检测和近战属性。
            Projectile.CloneDefaults(ProjectileID.Spear);
        }

        public override bool PreAI()
        {
            // 由于我们频繁访问拥有者玩家实例，所以创建一个辅助局部变量是有用的
            Player player = Main.player[Projectile.owner];
            // 定义弹幕存在的持续时间（帧数）
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI; // 更新玩家持有的弹幕ID

            // 如有必要，重置弹幕的剩余时间
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity); // 速度在这个长枪实现中并没有使用，但我们使用该字段来存储长枪的攻击方向。

            float halfDuration = duration * 0.5f;
            float progress;

            // 在物品使用动画期间，将“progress”设置为从0.0到1.0再回到0.0的值。
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }

            // 使用SmoothStep来平滑移动，将弹幕从HoldoutRangeMin移动到HoldoutRangeMax再回到HoldoutRangeMin
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            // 对精灵应用适当的旋转。
            if (Projectile.spriteDirection == -1)
            {
                // 如果精灵面朝左，旋转45度
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                // 如果精灵面朝右，旋转135度
                Projectile.rotation += MathHelper.ToRadians(135f);
            }

            // 避免在专用服务器上生成尘埃
            if (!Main.dedServ)
            {
                // 这些尘埃是在稍后添加的，用于‘ExampleMod’效果
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);
                }

                if (Main.rand.NextBool(4))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, Alpha: 128, Scale: 0.3f);
                }
            }

            // 在长枪的挥动过程中生成虚空精华弹幕
            if (Projectile.timeLeft == (int)halfDuration)
            {
                Vector2 spawnPosition = Projectile.Center;
                Vector2 direction = Projectile.velocity;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, direction * 10f, ModContent.ProjectileType<AbyssEssence>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

                // 在长枪的顶部生成暗紫色粒子特效组成的五角星
                CreatePentagramEffect(Projectile.Center, player);
            }

            return false; // 不执行原版AI。
        }

        private void CreatePentagramEffect(Vector2 position, Player player)
        {
            int dustType = DustID.PurpleCrystalShard;
            float radius = 25f; // 五角星的半径
            int points = 5; // 五角星的点数
            int segmentsPerEdge = 10; // 每条边的粒子数

            Vector2 playerDirection = player.Center - position;
            playerDirection.Normalize();
            float playerAngle = playerDirection.ToRotation();

            for (int i = 0; i < points; i++)
            {
                float startAngle = playerAngle + MathHelper.ToRadians(72 * i - 18);
                float endAngle = playerAngle + MathHelper.ToRadians(72 * (i + 1) - 18);

                Vector2 startPoint = position + new Vector2((float)Math.Cos(startAngle), (float)Math.Sin(startAngle)) * radius;
                Vector2 endPoint = position + new Vector2((float)Math.Cos(endAngle), (float)Math.Sin(endAngle)) * radius;

                for (int j = 0; j < segmentsPerEdge; j++)
                {
                    float t = (float)j / (segmentsPerEdge - 1);
                    Vector2 pointPosition = Vector2.Lerp(startPoint, endPoint, t);
                    Dust.NewDustPerfect(pointPosition, dustType, Vector2.Zero, 150, default(Color), 1.5f).noGravity = true;
                }
            }
        }
    }
}
