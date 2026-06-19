using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Reflection;


namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj
{
    public class MomentumChipPlayer : ModPlayer
    {
        public bool hasMomentumChip = false;

        public override void ResetEffects()
        {
            hasMomentumChip = false;
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (hasMomentumChip && IsEligiblePlayerClass(Player))
            {
                // 获取玩家当前速度与最大速度的比例
                float speedFactor = Player.velocity.Length() / Player.maxRunSpeed;
                // 将比例限制在0到1之间
                speedFactor = MathHelper.Clamp(speedFactor, 0f, 1f);
                // 按比例增加玩家伤害
                damage = (int)(damage * (1f + 0.3f * speedFactor));
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
