using AccessoryForPlanetaryEmpire.Accessory.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Global;
using Terraria.GameInput;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.Audio;



namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class ChargedBulletNestPlayer : ModPlayer
    {
        public bool hasChargedBulletNest;
        private const int maxStoredProjectiles = 150;

        private List<ReflectedProjectileInfo> storedEnemyProjectiles = new();

        public override void ResetEffects()
        {
            hasChargedBulletNest = false;
        }


        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (!hasChargedBulletNest)
                return;

            // 播放弹药箱音效
            SoundEngine.PlaySound(SoundID.Item149 with { Volume = 1.5f, Pitch = 0.1f }, Player.Center);

            if (proj != null && proj.active && !proj.friendly && proj.hostile)
            {
                for (int i = 0; i < 6; i++) // 每次记录6发
                {
                    storedEnemyProjectiles.Add(new ReflectedProjectileInfo
                    {
                        projectileType = proj.type,
                        damage = proj.damage,
                        knockback = proj.knockBack,
                        originalVelocity = proj.velocity
                    });

                    if (storedEnemyProjectiles.Count >= maxStoredProjectiles)
                        break;
                }

                if (storedEnemyProjectiles.Count > maxStoredProjectiles)
                    storedEnemyProjectiles.RemoveRange(maxStoredProjectiles, storedEnemyProjectiles.Count - maxStoredProjectiles);

                // 左轮机械圆圈粒子特效
                float radius = 1.5f * 16f;
                for (int i = 0; i < 6; i++)
                {
                    float angle = MathHelper.TwoPi * i / 6f;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    Vector2 pos = Player.Center + offset;

                    int dustType = Main.rand.NextBool() ? DustID.Silver : DustID.Iron;
                    Dust dust = Dust.NewDustPerfect(pos, dustType, Vector2.Zero);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (storedEnemyProjectiles.Count > 0)
            {
                ReleaseStoredProjectiles();
                storedEnemyProjectiles.Clear();
            }
        }

        private void ReleaseStoredProjectiles()
        {
            List<NPC> nearbyTargets = Main.npc.Where(n => n.active && !n.friendly && !n.dontTakeDamage)
                                             .OrderBy(n => Vector2.Distance(n.Center, Player.Center))
                                             .Take(6)
                                             .ToList();

            int targetCount = nearbyTargets.Count;

            for (int i = 0; i < storedEnemyProjectiles.Count; i++)
            {
                var projInfo = storedEnemyProjectiles[i];

                Vector2 direction;

                if (i < targetCount)
                {
                    // 锁定前 N 个敌人
                    direction = (nearbyTargets[i].Center - Player.Center).SafeNormalize(Vector2.UnitY);
                }
                else
                {
                    // 剩下的随机方向发射
                    direction = Main.rand.NextVector2CircularEdge(1f, 1f);
                }

                Projectile.NewProjectile(
                    Player.GetSource_Death(),
                    Player.Center,
                    direction * projInfo.originalVelocity.Length(),
                    projInfo.projectileType,
                    projInfo.damage * 10, // ✅ 伤害乘以10倍
                    projInfo.knockback,
                    Player.whoAmI
                );
            }
        }

        private struct ReflectedProjectileInfo
        {
            public int projectileType;
            public int damage;
            public float knockback;
            public Vector2 originalVelocity;
        }
    }
}
