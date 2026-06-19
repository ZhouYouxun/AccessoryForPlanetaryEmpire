//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria;
//using AccessoryForPlanetaryEmpire.Global;
//using AccessoryForPlanetaryEmpire.Buffs;
//using AccessoryForPlanetaryEmpire.Accessory.Defense;
//using Microsoft.Xna.Framework;
//using Terraria.Audio;

//namespace AccessoryForPlanetaryEmpire.Players.Defense
//{
//    public class EMPPlayer : ModPlayer
//    {
//        // 专门存储跟电磁脉冲有关的装备的玩家代码，一共6个

//        private const int cooldown = 45; // （所有的电子脉冲共用）冷却时间为45秒

//        public bool hasBulletBomb; // 子弹雷管
//        public bool hasHighVoltageGrid; // 高压电网
//        public bool hasReflectArcPulse; // 电弧脉冲
//        public bool hasReflectArcPulseHAV; // 电弧脉冲手动版
//        public bool hasStunEMP; // 眩晕脉冲

//        public override void ResetEffects()
//        {
//            // 确保每帧重置
//            hasBulletBomb = false;
//            hasHighVoltageGrid = false;
//            hasReflectArcPulse = false;
//            hasReflectArcPulseHAV = false;
//            hasStunEMP = false;
//        }

//        // 子弹雷管
//        public int BBSkillCooldown = 0;
//        public bool BBSkillActive = false;
//        public int BBSkillTime = 0;
//        public bool BBSkillEffectsApplied = false;

//        // 高压电网

//        // 电弧脉冲
//        private int RAPTimer = 0; // 添加一个计时器用于跟踪何时应该触发清除
//        private const int RAPsearchRange = 40; // 搜索范围的半径，单位：格子

//        // 电弧脉冲手动版
//        public int RAPHSkillCooldown = 0;
//        public bool RAPHSkillActive = false;
//        public int RAPHSkillTime = 0;
//        public bool RAPHSkillEffectsApplied = false;

//        // 眩晕脉冲
//        public int SEMPSkillCooldown = 0;
//        public bool SEMPSkillActive = false;
//        public int SEMPSkillTime = 0;
//        public bool SEMPSkillEffectsApplied = false;
//        private const int searchRange = 60; // 搜索范围的半径，单位：格子

//        public override void PostUpdate()
//        {
//            // 子弹雷管
//            if (hasBulletBomb)
//            {
//                ShowParticleRing(searchRange);

//                if (BBSkillCooldown > 0)
//                {
//                    BBSkillCooldown--;
//                    if (BBSkillCooldown == 0)
//                    {
//                        BBSkillActive = false;
//                    }
//                }

//                if (BBSkillActive)
//                {
//                    if (BBSkillTime > 0)
//                    {
//                        if (!BBSkillEffectsApplied)
//                        {
//                            BBSkillEffectsApplied = true;
//                        }

//                        foreach (Projectile projectile in Main.projectile)
//                        {
//                            if (projectile.hostile && !projectile.friendly)
//                            {
//                                if (Vector2.Distance(projectile.Center, Player.Center) <= searchRange * 16f)
//                                {
//                                    projectile.Kill();

//                                    foreach (NPC npc in Main.npc)
//                                    {
//                                        if (npc.active && !npc.friendly)
//                                        {
//                                            if (Vector2.Distance(npc.Center, projectile.Center) <= 5 * 16f)
//                                            {
//                                                int damage = (int)(projectile.damage * 7f);
//                                                npc.SimpleStrikeNPC(damage, 0, true, 0f, DamageClass.Generic, false);
//                                                DrawParticleLine(Player.Center, npc.Center); // 绘制粒子线
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }

//                        BBSkillTime--;
//                    }
//                    else
//                    {
//                        BBSkillCooldown = 90 * 60; // 45秒冷却时间
//                        BBSkillActive = false;
//                        BBSkillEffectsApplied = false;
//                    }
//                }
//                else
//                {
//                    if (KeybindSystem.BBKeybind.JustPressed && BBSkillCooldown == 0)
//                    {
//                        CombatText.NewText(Player.getRect(), Color.Brown, "电磁脉冲-子弹雷管 已开启！", dramatic: false, dot: false);
//                        BBSkillTime = 5;
//                        BBSkillActive = true;
//                        BBSkillCooldown = 45 * 60; // 45秒冷却时间
//                    }
//                }
//            }

//            // 高压电网
//            if (hasHighVoltageGrid)
//            {
//                ShowParticleRing((int)(searchRange * 1.5));

//                if (RAPTimer % 90 == 0)
//                {
//                    bool foundProjectile = false;

//                    for (int i = 0; i < Main.projectile.Length; i++)
//                    {
//                        Projectile projectile = Main.projectile[i];
//                        if (projectile.hostile && !projectile.friendly)
//                        {
//                            if (Vector2.Distance(projectile.Center, Player.Center) <= searchRange * 1.5 * 16f)
//                            {
//                                CombatText.NewText(Player.getRect(), Color.Brown, "高压电网已清除一个弹幕！", dramatic: false, dot: false);
//                                projectile.active = false;
//                                projectile.netUpdate = true;
//                                SoundEngine.PlaySound(SoundID.Item84, projectile.position);
//                                foundProjectile = true;
//                                break;
//                            }
//                        }
//                    }

//                    if (!foundProjectile)
//                    {
//                        RAPTimer = 0;
//                    }
//                }
//                RAPTimer++;
//            }

//            // 电弧脉冲自动版
//            if (hasReflectArcPulse)
//            {
//                ShowParticleRing(RAPsearchRange);

//                if (RAPTimer % 2700 == 0)
//                {
//                    CombatText.NewText(Player.getRect(), Color.Brown, "电弧脉冲装置（自动版）已开启！", dramatic: false, dot: false);
//                    for (int i = 0; i < Main.projectile.Length; i++)
//                    {
//                        Projectile projectile = Main.projectile[i];
//                        if (projectile.hostile && !projectile.friendly)
//                        {
//                            if (Vector2.Distance(projectile.Center, Player.Center) <= RAPsearchRange * 16f)
//                            {
//                                projectile.active = false;
//                                projectile.netUpdate = true;
//                                SoundEngine.PlaySound(SoundID.Item29, Player.position);
//                            }
//                        }
//                    }
//                }
//                RAPTimer++;
//            }

//            // 电弧脉冲手动版
//            if (hasReflectArcPulseHAV)
//            {
//                ShowParticleRing(searchRange);

//                if (RAPHSkillCooldown > 0)
//                {
//                    RAPHSkillCooldown--;
//                    if (RAPHSkillCooldown == 0)
//                    {
//                        RAPHSkillActive = false;
//                    }
//                }

//                if (RAPHSkillActive)
//                {
//                    if (RAPHSkillTime > 0)
//                    {
//                        for (int i = 0; i < Main.projectile.Length; i++)
//                        {
//                            Projectile projectile = Main.projectile[i];
//                            if (projectile.hostile && !projectile.friendly)
//                            {
//                                if (Vector2.Distance(projectile.Center, Player.Center) <= searchRange * 16f)
//                                {
//                                    projectile.active = false;
//                                    projectile.netUpdate = true;
//                                    SoundEngine.PlaySound(SoundID.Item29, Player.position);
//                                    DrawParticleLine(Player.Center, projectile.Center); // 绘制粒子线
//                                }
//                            }
//                        }
//                        RAPHSkillTime--;
//                    }
//                    else
//                    {
//                        RAPHSkillCooldown = 60 * 60; // 1分钟冷却时间
//                        RAPHSkillTime = 1;
//                        RAPHSkillActive = false;
//                        RAPHSkillEffectsApplied = false;
//                    }
//                }
//                else
//                {
//                    if (KeybindSystem.RAPHKeybind.JustPressed && RAPHSkillCooldown == 0)
//                    {
//                        CombatText.NewText(Player.getRect(), Color.BlueViolet, "电弧脉冲装置（手动版）已开启！", dramatic: false, dot: false);
//                        RAPHSkillTime = 60;
//                        RAPHSkillActive = true;
//                        RAPHSkillCooldown = 60 * 60; // 1分钟冷却时间
//                    }
//                    else if (KeybindSystem.RAPHKeybind.JustPressed && RAPHSkillCooldown != 0)
//                    {
//                        CombatText.NewText(Player.getRect(), Color.BlueViolet, "电弧脉冲装置（手动版）还处在冷却状态中！", dramatic: false, dot: false);
//                    }
//                }
//            }

//            // 眩晕电磁脉冲
//            if (hasStunEMP)
//            {
//                ShowParticleRing(searchRange);

//                if (SEMPSkillCooldown > 0)
//                {
//                    SEMPSkillCooldown--;
//                    if (SEMPSkillCooldown == 0)
//                    {
//                        SEMPSkillActive = false;
//                    }
//                }

//                if (SEMPSkillActive)
//                {
//                    if (SEMPSkillTime > 0)
//                    {
//                        if (!SEMPSkillEffectsApplied)
//                        {
//                            SEMPSkillEffectsApplied = true;
//                        }

//                        SoundEngine.PlaySound(SoundID.Item29, Player.position);

//                        foreach (NPC npc in Main.npc)
//                        {
//                            if (npc.active && !npc.friendly)
//                            {
//                                if (Vector2.Distance(npc.Center, Player.Center) <= searchRange * 16f)
//                                {
//                                    npc.velocity *= 0.5f;
//                                }
//                            }
//                        }

//                        foreach (Projectile projectile in Main.projectile)
//                        {
//                            if (projectile.hostile && !projectile.friendly)
//                            {
//                                if (Vector2.Distance(projectile.Center, Player.Center) <= searchRange * 16f)
//                                {
//                                    projectile.velocity *= 0.2f;
//                                }
//                            }
//                        }
//                    }
//                    SEMPSkillTime--;
//                }
//                else
//                {
//                    foreach (NPC npc in Main.npc)
//                    {
//                        if (npc.active && !npc.friendly)
//                        {
//                            npc.velocity *= 2f;
//                        }
//                    }

//                    foreach (Projectile projectile in Main.projectile)
//                    {
//                        if (projectile.hostile && !projectile.friendly)
//                        {
//                            projectile.velocity *= 5f;
//                        }
//                    }
//                    SEMPSkillCooldown = 45 * 60; // 45秒冷却时间
//                    SEMPSkillActive = false;
//                    SEMPSkillEffectsApplied = false;
//                }
//            }
//            else
//            {
//                if (KeybindSystem.SEMPKeybind.JustPressed && SEMPSkillCooldown == 0)
//                {
//                    SEMPSkillTime = 5 * 60; // 5秒
//                    SEMPSkillActive = true;
//                    SEMPSkillCooldown = 45 * 60; // 45秒冷却时间
//                    CombatText.NewText(Player.getRect(), Color.BlueViolet, "眩晕电磁脉冲已开启！", dramatic: false, dot: false);
//                }
//            }
//        }

//        private void ShowParticleRing(int range)
//        {
//            int particleCount = 100;
//            for (int i = 0; i < particleCount; i++)
//            {
//                float angle = MathHelper.TwoPi / particleCount * i;
//                Vector2 position = Player.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * range * 16f;
//                Dust dust = Dust.NewDustPerfect(position, DustID.UltraBrightTorch);
//                dust.noGravity = true;
//                dust.scale = 0.8f;
//            }
//        }

//        private void DrawParticleLine(Vector2 start, Vector2 end)
//        {
//            Vector2 direction = end - start;
//            int length = (int)direction.Length();
//            direction.Normalize();

//            for (int i = 0; i < length; i += 5)
//            {
//                Vector2 position = start + direction * i;
//                Dust dust = Dust.NewDustPerfect(position, DustID.UltraBrightTorch);
//                dust.noGravity = true;
//                dust.scale = 0.8f;
//            }
//        }
//    }
//}
