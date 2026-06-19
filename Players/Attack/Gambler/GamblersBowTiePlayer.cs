using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Reflection;




namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class GamblersBowTiePlayer : ModPlayer
    {
        public bool hasGamblersBowTie;

        public override void ResetEffects()
        {
            hasGamblersBowTie = false;
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!hasGamblersBowTie || !IsEligiblePlayerClass(Player))
                return;

            if (Main.rand.NextFloat() >= 0.25f)
                return;

            // 尝试修复弹药转化问题
            int ammoProjectile;
            float shootSpeed;
            int ammoDamage;
            float ammoKnockback;
            int ammoType;

            // ✅ 直接调用 PickAmmo 会自动判断是否为有效弹药
            if (Player.PickAmmo(item, out ammoProjectile, out shootSpeed, out ammoDamage, out ammoKnockback, out ammoType))
            {
                type = ammoProjectile;
            }


            float baseSpeed = velocity.Length();

            for (int i = 0; i < 3; i++)
            {
                Vector2 spawnPos = Main.MouseWorld + new Vector2(Main.rand.NextFloat(-200f, 200f), -600f);
                spawnPos += Main.rand.NextVector2Circular(50f, 30f);

                Vector2 direction = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitY);
                Vector2 finalVelocity = direction * baseSpeed + Main.rand.NextVector2Circular(1.2f, 1.2f);

                int finalDamage = (int)(damage * Main.rand.NextFloat(0.5f, 1.9f));
                Projectile.NewProjectile(Player.GetSource_ItemUse(item), spawnPos, finalVelocity, type, finalDamage, knockback, Player.whoAmI);
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
