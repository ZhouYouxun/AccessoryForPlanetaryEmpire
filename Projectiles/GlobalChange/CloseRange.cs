using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;
using System.Reflection;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class CloseRange : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        // 用于记录弹幕生成的帧数
        private uint spawnFrame;
        // 用于标记伤害是否已经增加
        private bool damageIncreased = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 初始化生成帧数为当前帧数
            spawnFrame = Main.GameUpdateCount;
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<QuickDrawHolsterPlayer>().hasQuickDrawHolster && IsEligiblePlayerClass(player))
            {
                // if (projectile.DamageType == DamageClass.Ranged)
                {
                    // 如果弹幕已经存在超过18帧，则不再增加伤害
                    if (damageIncreased || Main.GameUpdateCount - spawnFrame > 18)
                        return;

                    // 增加伤害，倍率改为1.75倍
                    projectile.damage = (int)(projectile.damage * 1.75f);
                    damageIncreased = true;
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            // 这里可以进行额外的修改，如果需要的话
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            // 这里可以进行额外的修改，如果需要的话
        }

        // 高级职业判断逻辑
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