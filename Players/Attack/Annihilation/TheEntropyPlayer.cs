using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class TheEntropyPlayer : ModPlayer
    {
        public bool hasTheEntropy;
        // private int globalCooldown = 0; // 全局冷却时间
        private Dictionary<int, bool> projectileTriggered = new Dictionary<int, bool>(); // 用于记录弹幕是否已经触发过复制体


        public override void ResetEffects()
        {
            hasTheEntropy = false;
        }
        public override void PostUpdate()
        {
            // if (globalCooldown > 0)
            {
                // globalCooldown--; // 减少冷却计时器
            }
        }

        public override void PreUpdate()
        {
            if (hasTheEntropy)
            {
                // 禁用治疗药水
                Player.AddBuff(BuffID.PotionSickness, 300, true);

                // 生命值损失计算
                float currentHealthPercentage = (float)Player.statLife / Player.statLifeMax2 * 100;

                // 使用对数函数计算损失，并将速度降低为原来的1/20
                if (currentHealthPercentage > 8)
                {
                    float logLoss = (float)Math.Log10(currentHealthPercentage) * 5 / 5f; // 对数损失函数乘以1/5
                    Player.statLife -= (int)logLoss; // 强制损失生命
                    if (Player.statLife < Player.statLifeMax2 * 0.25f) // 停止在25%
                    {
                        Player.statLife = (int)(Player.statLifeMax2 * 0.25f);
                    }
                }

                // 生命恢复归零
                Player.lifeRegen = 0;
            }
        }


        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            // 确保饰品开启且为合法的弹幕类型
            if (hasTheEntropy && IsEligiblePlayerClass(Player))
            {
                // 检查弹幕是否已经触发过复制体
                if (!projectileTriggered.ContainsKey(proj.whoAmI))
                {
                    projectileTriggered[proj.whoAmI] = false;
                }

                // 40% 的概率触发并确保每个弹幕仅触发一次
                if (!projectileTriggered[proj.whoAmI] && Main.rand.NextFloat() < 0.4f)
                {
                    float projectileSpeed = proj.velocity.Length();

                    for (int j = 0; j < 1; j++) // 这里只生成一个复制体
                    {
                        // 在敌人上方随机生成弹幕复制体
                        Vector2 spawnPosition = new Vector2(
                            target.position.X + target.width * 0.5f + Main.rand.Next(201) * -Player.direction + Main.mouseX + Main.screenPosition.X - Player.position.X,
                            target.Center.Y - 600f
                        );
                        spawnPosition.X = (spawnPosition.X + target.Center.X) / 2f + Main.rand.Next(-200, 201);
                        spawnPosition.Y -= 100 * j;

                        // 计算方向
                        Vector2 direction = target.Center - spawnPosition;
                        direction.Normalize();
                        float speedX = direction.X * projectileSpeed + Main.rand.Next(-120, 121) * 0.01f;
                        float speedY = direction.Y * projectileSpeed + Main.rand.Next(-120, 121) * 0.01f;

                        // 生成弹幕复制体
                        Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPosition, new Vector2(speedX, speedY), proj.type, (proj.damage) * 3, proj.knockBack, Player.whoAmI);
                    }

                    // 标记这个弹幕已经触发过复制体
                    projectileTriggered[proj.whoAmI] = true;

                    // 设置全局冷却时间为1帧
                    // globalCooldown = 1;
                }
            }
        }
        private bool IsEligiblePlayerClass(Player player)
        {
            // 默认的职业类型检测
            bool isEligible = player.HeldItem.DamageType == DamageClass.Melee ||
                              player.HeldItem.DamageType == DamageClass.Ranged ||
                              player.HeldItem.DamageType == DamageClass.Magic;

            // 尝试加载 CalamityMod 模组并检查 RogueDamageClass
            Mod calamityMod;
            if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
            {
                // 使用反射获取 RogueDamageClass 的实例
                var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");

                if (rogueType != null)
                {
                    FieldInfo rogueInstanceField = rogueType.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
                    var rogueInstance = rogueInstanceField?.GetValue(null) as DamageClass;
                    if (rogueInstance != null && player.HeldItem.DamageType == rogueInstance)
                    {
                        isEligible = true; // 允许 Rogue 类型职业也能触发技能
                    }
                }
            }

            return isEligible;
        }
    }
}