using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Accessory.Attack;
using System.Reflection;


namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    public class DoomsdayTechExperimentPlayer : ModPlayer
    {
        public bool hasDoomsdayTechExperiment = false;
        private int attackChargeTime = 0;
        private const int chargeInterval = 90; // 1.5秒
        private const int maxChargeTime = 900; // 15秒
        private bool hasCharged = false;

        public override void ResetEffects()
        {
            hasDoomsdayTechExperiment = false;
        }

        public override void PostUpdate()
        {
            if (hasDoomsdayTechExperiment && IsEligiblePlayerClass(Player))
            {
                if (Player.itemAnimation == 0) // 未攻击状态
                {
                    if (attackChargeTime < maxChargeTime)
                    {
                        attackChargeTime++;
                        if (attackChargeTime % chargeInterval == 0)
                        {
                            CombatText.NewText(Player.getRect(), Color.Blue, "蓄力增加！", dramatic: false, dot: false);
                        }
                    }
                    else if (!hasCharged)
                    {
                        hasCharged = true;
                        CombatText.NewText(Player.getRect(), Color.Blue, "充能完毕！", dramatic: false, dot: false);
                        for (int i = 0; i < 360; i += 10)
                        {
                            Vector2 dustPosition = Player.Center + new Vector2((float)Math.Cos(MathHelper.ToRadians(i)), (float)Math.Sin(MathHelper.ToRadians(i))) * 50;
                            Dust dust = Dust.NewDustPerfect(dustPosition, DustID.BlueTorch, null, 100, Color.Blue, 1.5f);
                            dust.noGravity = true;
                        }
                    }
                }
                else // 攻击状态
                {
                    attackChargeTime = 0;
                    hasCharged = false;
                }
            }

        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (hasDoomsdayTechExperiment && IsEligiblePlayerClass(Player))
            {
                if (attackChargeTime > 0)
                {
                    damage += damage * attackChargeTime / chargeInterval; // 增加100%也就是1倍的伤害
                    // damage += damage * 0.5f; // 增加50%的伤害
                    attackChargeTime = 0;
                    hasCharged = false;
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