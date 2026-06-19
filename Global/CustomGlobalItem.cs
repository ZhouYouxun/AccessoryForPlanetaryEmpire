using AccessoryForPlanetaryEmpire.Global.Configs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace AccessoryForPlanetaryEmpire.Global
{
    public class CustomGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            //// 珍珠木弓
            //if (ModContent.GetInstance<PLAConfigs>().EnablePearlwoodBowTweaks)
            //{                
            //    if (item.type == ItemID.PearlwoodBow)
            //    {
            //        item.damage = 40; // 提升基础伤害
            //        item.useTime = 20; // 攻击速度（值越低，速度越快）
            //        item.useAnimation = 20; // 攻击动画速度（值越低，速度越快）
            //        item.shootSpeed = 19f; // 弹幕飞行速度
            //    }
            //}

            ////// 珍珠木剑
            ////if (ModContent.GetInstance<WeaponTweaksConfig>().EnablePearlwoodSwordTweaks)
            ////{
            ////    if (item.type == ItemID.PearlwoodSword)
            ////    {
            ////        item.damage = 75; // 提升基础伤害
            ////        item.crit = 25; // 提升暴击率
            ////    }
            ////}


            //// 死神镰刀
            //if (ModContent.GetInstance<PLAConfigs>().EnableDeathSickleTweaks)
            //{
            //    if (item.type == ItemID.DeathSickle)
            //    {
            //        item.damage = 80; // 提升基础伤害
            //        item.useTime = 20; // 攻击速度（值越低，速度越快）
            //        item.useAnimation = 20; // 攻击动画速度（值越低，速度越快）
            //        item.shootSpeed = 16f; // 弹幕飞行速度
            //    }
            //}

            //// 脉冲弓
            //if (ModContent.GetInstance<PLAConfigs>().EnablePulseBowTweaks)
            //{
            //    if (item.type == ItemID.PulseBow)
            //    {
            //        item.damage = 60; // 提升基础伤害
            //        item.useTime = 25; // 攻击速度（值越低，速度越快）
            //        item.useAnimation = 25; // 攻击动画速度（值越低，速度越快）
            //        item.shootSpeed = 14f; // 弹幕飞行速度
            //    }
            //}

            //// 庆典大炮
            //if (ModContent.GetInstance<PLAConfigs>().EnableFireworksLauncherTweaks)
            //{
            //    if (item.type == ItemID.FireworksLauncher)
            //    {
            //        item.damage = item.damage; // 保持基础伤害不变，留空以便以后修改
            //        item.useTime = item.useTime; // 保持攻击速度不变，留空以便以后修改
            //        item.useAnimation = item.useAnimation; // 保持攻击动画速度不变，留空以便以后修改
            //        item.shootSpeed = item.shootSpeed; // 保持弹幕飞行速度不变，留空以便以后修改
            //    }
            //}













        }












        //public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        //{
        //    // 珍珠木弓
        //    if (ModContent.GetInstance<PLAConfigs>().EnablePearlwoodBowTweaks && item.type == ItemID.PearlwoodBow)
        //    {
        //        // 平行发射5只箭矢
        //        int numberProjectiles = 5; // 发射5只箭矢
        //        float spacing = 10f; // 箭矢之间的间隔

        //        for (int i = 0; i < numberProjectiles; i++)
        //        {
        //            Vector2 spawnPosition = position + new Vector2(spacing * (i - (numberProjectiles - 1) / 2f), 0);
        //            Projectile.NewProjectile(source, spawnPosition, velocity, type, damage, knockback, player.whoAmI);
        //        }

        //        // 从玩家身体里射出一个PurificationPowder到鼠标位置
        //        Vector2 target = Main.MouseWorld;
        //        Vector2 direction = (target - player.Center).SafeNormalize(Vector2.UnitX) * item.shootSpeed;
        //        Projectile.NewProjectile(source, player.Center, direction, ProjectileID.PurificationPowder, damage, knockback, player.whoAmI);

        //        return false; // 返回false，表示不使用原始的射击行为
        //    }



        //    // 死神镰刀
        //    if (ModContent.GetInstance<PLAConfigs>().EnableDeathSickleTweaks && item.type == ItemID.DeathSickle)
        //    {
        //        int numberProjectiles = 4; // 发射4只死神镰刀弹幕
        //        for (int i = 0; i < numberProjectiles; i++)
        //        {
        //            // 随机速度和角度
        //            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(45)) * (1f - Main.rand.NextFloat(0.3f));
        //            Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
        //        }
        //        return false; // 返回false，表示不使用原始的射击行为
        //    }


        //    // 脉冲弓
        //    if (ModContent.GetInstance<PLAConfigs>().EnablePulseBowTweaks && item.type == ItemID.PulseBow)
        //    {
        //        int numberProjectiles = 3; // 发射3只PulseBolt弹幕
        //        float rotation = MathHelper.ToRadians(2); // 散射角度

        //        for (int i = 0; i < numberProjectiles; i++)
        //        {
        //            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
        //            Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
        //        }

        //        return false; // 返回false，表示不使用原始的射击行为
        //    }


        //    // 庆典大炮
        //    if (ModContent.GetInstance<PLAConfigs>().EnableFireworksLauncherTweaks && item.type == ItemID.FireworksLauncher)
        //    {
        //        int[] rocketTypes = new int[] { ProjectileID.RocketFireworkRed, ProjectileID.RocketFireworkGreen, ProjectileID.RocketFireworkBlue, ProjectileID.RocketFireworkYellow };
        //        int numberProjectiles = 7; // 发射7只火箭弹
        //        float rotation = MathHelper.ToRadians(20); // 散射角度

        //        for (int i = 0; i < numberProjectiles; i++)
        //        {
        //            Vector2 perturbedSpeed = velocity.RotatedByRandom(rotation); // 随机旋转角度
        //            int randomRocketType = Main.rand.Next(rocketTypes); // 随机选择火箭弹类型
        //            Projectile.NewProjectile(source, position, perturbedSpeed, randomRocketType, damage, knockback, player.whoAmI);
        //        }

        //        return false; // 返回false，表示不使用原始的射击行为
        //    }












        //    return base.Shoot(item, player, source, position, velocity, type, damage, knockback); // 对于其他物品，保持原始行为
        //}


        //public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockback, bool crit)
        //{
        //    // 脉冲弓
        //    if (ModContent.GetInstance<WeaponTweaksConfig>().EnablePulseBowTweaks && item.type == ItemID.PulseBow)
        //    {
        //        Vector2 spawnPosition = target.position; // 弹幕生成位置
        //        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, Vector2.Zero, ProjectileID.Electrosphere, hit.Damage, hit.Knockback, player.whoAmI);
        //    }
        //    base.OnHitNPC(item, player, target, damage, knockback, crit);
        //}





        //public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        //{
        //    // 珍珠木剑
        //    if (ModContent.GetInstance<WeaponTweaksConfig>().EnablePearlwoodSwordTweaks && item.type == ItemID.PearlwoodSword)
        //    {
        //        // 从玩家身体里射出StarCannonStar弹幕到鼠标位置
        //        Vector2 target = Main.MouseWorld;
        //        Vector2 direction = (target - player.Center).SafeNormalize(Vector2.UnitX) * item.shootSpeed; // 确保弹幕有初始速度
        //        Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, direction, ProjectileID.StarCannonStar, item.damage, item.knockBack, player.whoAmI);
        //    }

        //    base.MeleeEffects(item, player, hitbox);
        //}












    }
}
