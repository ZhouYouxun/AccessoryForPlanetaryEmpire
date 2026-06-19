using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using System;
using Terraria.Localization;
using Humanizer;

namespace AccessoryForPlanetaryEmpire.Players
{
    public class PLPlayer : ModPlayer
    {
        public bool HasHeroDamageDevice = false;
        public bool HasGravityBooster = false;
        public bool HasGravityReducer = false;

        public bool HasMagicSeeker = false;
        private int MagicSeekerCooldown;

        public bool HasCounterGrenadeLauncher = false;
        private int CounterGrenadeLauncherCooldown;

        public bool HasReactShieldGenerator = false;
        private int ReactShieldGeneratorCooldown;
        
        public bool HasStressResponseAccelerationEngine = false;
        private int StressResponseAccelerationEngineCooldown;

        public bool HasGiftFromTesla = false;

        public int respawnInvincibilityTimer = 0;

        public bool HasPainEditor; // 疼痛编辑器: 限制每次受到伤害在最大生命值的20%


        public override void ResetEffects()
        {
            HasHeroDamageDevice = false;
            HasGravityBooster = false;
            HasGravityReducer = false;
            HasMagicSeeker = false;
            HasCounterGrenadeLauncher = false;
            HasReactShieldGenerator = false;
            HasStressResponseAccelerationEngine = false;
            HasPainEditor = false;
        }



        public override void OnHurt(Player.HurtInfo info) // 这个是处理玩家受到伤害之后会发生什么
        {




        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) // 处理玩家在受到弹幕伤害之后会干什么
        {
            // 由于这些无法修改玩家受到的伤害，因此被删除



        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) // 玩家在受到接触伤害之后会干什么
        {
            //// 如果玩家装备了PainEditor装备，那么检查
            //if (HasPainEditor)
            //{
            //    // Main.NewText($"OnHitByNPC-HasPainEditor == {HasPainEditor}");
            //    int maxLifeThreshold = (int)(Player.statLifeMax2 * 0.20); // 计算最大生命值的20%
            //    // Main.NewText($"hurtInfo.Damage == {hurtInfo.Damage}");
            //    // Main.NewText($"maxLifeThreshold == {maxLifeThreshold}");
            //    if (hurtInfo.Damage > maxLifeThreshold)
            //    {
            //        // Main.NewText($"if (hurtInfo.Damage > maxLifeThreshold)");
            //        hurtInfo.Damage = maxLifeThreshold; // *********这里无法直接修改玩家这次受到多少伤害，因此这段代码仅仅作为复制参考
            //        // 举个例子，当玩家受到伤害时，回血/增加伤害/释放弹幕/积攒某个条的代码可以写在这里
            //    }
            //}
        }





        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (HasPainEditor)
            {
                // 假设疼痛编辑器始终将伤害（碰撞伤害）限制在最大生命值的20%
                float painEditorValue = 0.2f;
                modifiers.SetMaxDamage((int)(Player.statLifeMax2 * painEditorValue));
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) // 用于更改来自敌人的弹幕伤害
        {
            if (HasPainEditor)
            {
                // 假设疼痛编辑器始终将伤害（弹幕伤害）限制在最大生命值的20%
                float painEditorValue = 0.2f;
                modifiers.SetMaxDamage((int)(Player.statLifeMax2 * painEditorValue));
            }
        }






        private IEntitySource GetSource_FromAccessory()
        {
            throw new NotImplementedException();
        }        
    }
}

