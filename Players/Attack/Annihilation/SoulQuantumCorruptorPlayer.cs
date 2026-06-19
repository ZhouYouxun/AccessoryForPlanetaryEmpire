using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class SoulQuantumCorruptorPlayer : ModPlayer
    {
        public bool hasSoulQuantumCorruptor;
        private int timer;

        public override void ResetEffects()
        {
            hasSoulQuantumCorruptor = false;
        }

        public override void PostUpdate()
        {
            if (hasSoulQuantumCorruptor)
            {
                timer++;
                if (timer >= 280) // 每6秒（360帧）
                {
                    ApplyDeathNoteBuff();
                    timer = 0;
                }
            }
        }

        private void ApplyDeathNoteBuff()
        {
            int radius = 600; // Buff的影响半径
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Player.Center) <= radius)
                {
                    npc.AddBuff(ModContent.BuffType<Buffs.DeathNoteBuff>(), 360); // 给敌人添加DeathNoteBuff，每次持续6秒
                    GenerateBlackLineEffect(npc); // 为每个受影响的敌人生成黑色粒子特效线
                }
            }
        }

        private void GenerateBlackLineEffect(NPC target)
        {
            Vector2 startPosition = target.Center;
            Vector2 endPosition = Player.Center;

            int numParticles = 50; // 粒子数量
            for (int i = 0; i < numParticles; i++)
            {
                float lerpValue = (float)i / (numParticles - 1);
                Vector2 position = Vector2.Lerp(startPosition, endPosition, lerpValue);

                // 增加随机性
                position += new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));

                int dustIndex = Dust.NewDust(position, 0, 0, DustID.Shadowflame); // 使用黑色粒子效果
                Dust dust = Main.dust[dustIndex];
                dust.noGravity = true;
                dust.scale = 2.0f + Main.rand.NextFloat(0.5f); // 放大粒子倍率为两倍，并引入一定的随机性
                dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)); // 增加粒子的随机速度
            }
        }



        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (hasSoulQuantumCorruptor)
            {
                modifiers.FinalDamage *= 1.15f; // 增加25%伤害
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (hasSoulQuantumCorruptor)
            {
                modifiers.FinalDamage *= 1.15f; // 增加25%伤害
            }
        }

        //public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        //{
        //    if (hasDeathNote && timer == 0)
        //    {
        //        float damageThreshold = target.life * 0.7f; // 当前生命值的70%
        //        if (modifiers.SourceDamage.Base < damageThreshold)
        //        {
        //            modifiers.SourceDamage = new StatModifier(1f, damageThreshold / modifiers.SourceDamage.Base); // 将 SourceDamage 设置为规定的数值
        //            CombatText.NewText(target.getRect(), Color.Red, "死亡笔记!", dramatic: true);
        //            timer = 300; // 重新进入5秒的冷却
        //        }
        //    }
        //}

        //public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        //{
        //    if (hasDeathNote)
        //    {
        //        float damageThreshold = target.life * 0.7f; // 当前生命值的70%
        //        if (modifiers.SourceDamage.Base < damageThreshold)
        //        {
        //            modifiers.SourceDamage = new StatModifier(1f, damageThreshold / modifiers.SourceDamage.Base); // 将 SourceDamage 设置为规定的数值
        //            CombatText.NewText(target.getRect(), Color.Red, "死亡笔记!", dramatic: true);
        //            timer = 300; // 重新进入5秒的冷却
        //            FireDeathNoteProjectile(target);
        //        }
        //    }
        //}

        //private void FireDeathNoteProjectile(NPC target)
        //{
        //    Vector2 spawnPosition = new Vector2(
        //        Player.position.X + Player.width * 0.5f + Main.rand.Next(-200, 201),
        //        Player.position.Y - 600f
        //    );

        //    Vector2 direction = target.Center - spawnPosition;
        //    direction.Normalize();
        //    direction *= 20f; // 设置速度，使其极快

        //    int damage = (int)(target.life * 0.7f); // 将浮点数转换为整数

        //    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPosition, direction, ModContent.ProjectileType<DeathNoteProjectile>(), damage, 0f, Player.whoAmI);
        //}



    }
}
