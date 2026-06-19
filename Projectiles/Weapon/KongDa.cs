using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class KongDa : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0"; // 透明贴图

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.scale *= 2f; // 放大敌人
            target.width = (int)(target.width * 2f); // 更新宽度
            target.height = (int)(target.height * 2f); // 更新高度
            // target.position = target.position - new Vector2(target.width / 4, target.height / 4); // 更新位置，使碰撞箱中心保持不变
            target.netUpdate = true; // 触发网络更新

            //target.life -= 50; // 减少50点生命值
            //target.defense -= 5; // 减少5点防御值
            //target.knockBackResist = 0f; // 设置击退抗性为0
            //target.velocity = new Vector2(-10, 0); // 将敌人向左击退
            //target.noTileCollide = true; // 使敌人不与地形碰撞
            //target.noGravity = true; // 使敌人忽略重力
            //target.buffImmune[BuffID.Poisoned] = false; // 使敌人不再对中毒免疫
            //target.AddBuff(BuffID.OnFire, 300); // 使敌人着火，持续5秒
            //target.scale *= 0.5f; // 缩小敌人
            //target.netUpdate = true; // 触发网络更新
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                for (int num134 = 0; num134 < 10; num134++)
                {
                    float x = Projectile.position.X - Projectile.velocity.X / 10f * (float)num134;
                    float y = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)num134;
                    int dust = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.RainbowTorch, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[dust].alpha = Projectile.alpha;
                    Main.dust[dust].position.X = x;
                    Main.dust[dust].position.Y = y;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }
    }
}
