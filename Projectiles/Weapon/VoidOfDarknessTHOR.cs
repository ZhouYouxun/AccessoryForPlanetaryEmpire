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
using AccessoryForPlanetaryEmpire.Projectiles.Weapon;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class VoidOfDarknessTHOR : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 144; // 弹幕的宽度
            Projectile.height = 144; // 弹幕的高度
            Projectile.friendly = true; // 设置为友好弹幕
            Projectile.hostile = false; // 设置为非敌对弹幕
            Projectile.DamageType = DamageClass.Melee; // 设置弹幕的伤害类型
            Projectile.penetrate = -1; // 穿透次数（-1表示无限次）
            Projectile.timeLeft = 300; // 弹幕的存在时间（帧数）
            Projectile.tileCollide = false; // 设置为不与地形碰撞
            Projectile.ignoreWater = true; // 设置为不受水影响
            Projectile.extraUpdates = 2; // 增加飞行速度

        }

        public override void AI()
        {
            // 确保弹幕持续旋转
            // Projectile.rotation += MathHelper.ToRadians(10f);

            // 设置弹幕的旋转方向，使其朝向飞行方向
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(405f); // 调整角度

            // 飞行过程中添加暗色调的光和粒子特效
            Lighting.AddLight(Projectile.Center, 0.2f, 0f, 0.2f);
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
            }

            // 确保每15帧生成一对虚空精华弹幕
            if (Projectile.ai[0] % 15 == 0)
            {
                Vector2 leftSpawnPosition = Projectile.Center + new Vector2(-10, 0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90f));
                Vector2 rightSpawnPosition = Projectile.Center + new Vector2(10, 0).RotatedBy(Projectile.rotation + MathHelper.ToRadians(90f));

                Vector2 leftVelocity = new Vector2(-Projectile.velocity.X, -Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(15));
                Vector2 rightVelocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(15));

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), leftSpawnPosition, leftVelocity, ModContent.ProjectileType<AbyssEssence>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), rightSpawnPosition, rightVelocity, ModContent.ProjectileType<AbyssEssence>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            // 确保弹幕的伤害为武器伤害的20%
            // Projectile.damage = (int)(Main.player[Projectile.owner].HeldItem.damage * 0.2f);

            Projectile.ai[0]++;
        }
    }
}
