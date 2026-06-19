using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Accessory.Attack;
using Microsoft.Xna.Framework;
using System.Reflection;


namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj
{
    public class QuickDrawHolsterPlayer : ModPlayer
    {
        public bool hasQuickDrawHolster;

        public override void ResetEffects()
        {
            hasQuickDrawHolster = false;
        }



        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasQuickDrawHolster && IsEligiblePlayerClass(Player))
            {
                // 将距离从像素单位转换为方块单位（1格 = 16像素）
                float distanceInTiles = Vector2.Distance(Player.Center, target.Center) / 16f;

                if (distanceInTiles <= 50f)
                {
                    // 线性衰减：距离越近加成越高，最多加成 40%
                    float factor = Utils.GetLerpValue(50f, 0f, distanceInTiles, true); // 0格时为1，50格时为0
                    float bonus = 0.4f * factor; // 最多 +40%
                    modifiers.FinalDamage *= 1f + bonus;
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
