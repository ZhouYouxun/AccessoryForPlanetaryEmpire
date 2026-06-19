using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public static class DamageClassUtils
    {
        private static DamageClass _rogueDamageClass;

        // 获取 Calamity 的 RogueDamageClass 实例（只查找一次）
        private static DamageClass GetRogueDamageClass()
        {
            if (_rogueDamageClass != null)
                return _rogueDamageClass;

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");
                if (rogueType != null)
                {
                    FieldInfo rogueInstanceField = rogueType.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
                    _rogueDamageClass = rogueInstanceField?.GetValue(null) as DamageClass;
                }
            }
            return _rogueDamageClass;
        }

        // 判断一个弹幕是否属于可接受的职业类型
        public static bool IsEligibleProjectile(Projectile proj)
        {
            return proj.DamageType == DamageClass.Melee ||
                   proj.DamageType == DamageClass.Ranged ||
                   proj.DamageType == DamageClass.Magic ||
                   proj.DamageType == DamageClass.Throwing ||
                   proj.DamageType == GetRogueDamageClass();
        }

        // 判断一个 HeldItem 是否属于可接受的职业类型
        public static bool IsEligiblePlayerClass(Player player)
        {
            var type = player.HeldItem.DamageType;
            return type == DamageClass.Melee ||
                   type == DamageClass.Ranged ||
                   type == DamageClass.Magic ||
                   type == GetRogueDamageClass();
        }
    }
}
