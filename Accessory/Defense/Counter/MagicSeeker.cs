using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.Counter
{
    public class MagicSeeker : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.Counter";
        //public override void SetStaticDefaults()
        //{
        //    DisplayName.SetDefault("魔力引导头");
        //    Tooltip.SetDefault("受到伤害时将伤害转化为魔力受损，每受5次伤害将会触发一次" +
        //        "\n任意boss前" +
        //        "\n帝国开拓者在某一颗行星上发现的一个神秘装置，但正规军似乎不知道这个装置的用途，并将其永久放在了帝国母舰中" +
        //        "\n通过外观和仔细阅读上面的图像说明，你似乎了解到他可以跟你身上的魔力值进行互动");
        //}

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值15金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagicSeekerPlayer>().HasMagicSeeker = true; // 在global文件里面进行详细的解释
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.BandofStarpower, 1);
            recipe.AddIngredient(ItemID.ManaFlower, 1);
            recipe.AddIngredient(ItemID.WormScarf, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }


        // 以前写的，现在感觉一点用都没有，但还是留了下来：

        //public override void UpdateAccessory(Player player, bool hideVisual)
        //{
        //    PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();

        //    // 检查技能冷却时间
        //    if (modPlayer.MSSkillCooldown > 0)
        //    {
        //        modPlayer.MSSkillCooldown--; // 冷却时间减少

        //        // 冷却时间结束后，重置技能状态
        //        if (modPlayer.MSSkillCooldown == 0)
        //        {
        //            modPlayer.MSSkillActive = false;
        //        }
        //    }

        //    // 检查技能是否激活
        //    if (modPlayer.MSSkillActive)
        //    {
        //        // 检查技能持续时间
        //        if (modPlayer.MSSkillTime > 0)
        //        {
        //            if (!modPlayer.MSSkillEffectsApplied)
        //            {
        //                // 消耗生命和魔力
        //                int healthCost = (int)(player.statLifeMax2 * 0.7f);
        //                int manaCost = player.statManaMax2;
        //                player.statLife -= healthCost;
        //                player.statMana -= manaCost;

        //                modPlayer.MSSkillEffectsApplied = true; // 标记效果已应用
        //            }

        //            // 降低防御力和移动速度
        //            player.statDefense /= 4;
        //            player.moveSpeed /= 2f;

        //            // 提升仇恨值
        //            player.aggro += 3000;

        //            // 提升伤害
        //            player.GetDamage(DamageClass.Generic) += 9f;

        //            // 持续时间减少
        //            modPlayer.MSSkillTime--;
        //        }
        //        else
        //        {
        //            // 还原生命和魔力
        //            int healthGain = (int)(player.statLifeMax2 * 0.7f);
        //            int manaGain = player.statManaMax2;
        //            player.statLife += healthGain;
        //            player.statMana += manaGain;

        //            // 还原防御力和移动速度
        //            player.statDefense *= 4;
        //            player.moveSpeed *= 2f;

        //            // 还原伤害
        //            player.GetDamage(DamageClass.Generic) -= 9f;

        //            // 设置冷却时间
        //            modPlayer.MSSkillCooldown = 60 * 60; // 1分钟冷却时间

        //            // 清除标记
        //            modPlayer.MSSkillActive = false;
        //            modPlayer.MSSkillEffectsApplied = false;
        //        }
        //    }
        //    else
        //    {
        //        // 检查技能是否可以激活
        //        if (KeybindSystem.MSKeybind.JustPressed && modPlayer.MSSkillCooldown == 0)
        //        {
        //            // 设置技能持续时间和标记
        //            modPlayer.MSSkillTime = 15 * 60; // 15秒
        //            modPlayer.MSSkillActive = true;

        //            // 设置冷却时间
        //            modPlayer.MSSkillCooldown = 60 * 60; // 1分钟冷却时间
        //        }
        //    }
        //}



        //public override void UpdateAccessory(Player player, bool hideVisual)
        //{
        //    // 在UpdateAccessory函数中进行装备效果的逻辑处理

        //    int MSSkillCooldown = player.GetModPlayer<PLPlayer>().MSSkillCooldown;
        //    bool MSSkillActive = player.GetModPlayer<PLPlayer>().MSSkillActive;
        //    int MSSkillTime = player.GetModPlayer<PLPlayer>().MSSkillTime;

        //    // 检查技能是否冷却完毕
        //    if (MSSkillCooldown <= 0)
        //    {
        //        // 当技能冷却完毕并且技能处于激活状态时
        //        if (MSSkillActive)
        //        {
        //            // 开始技能的持续时间倒计时
        //            if (MSSkillTime > 0)
        //            {
        //                MSSkillTime--; // 减少剩余持续时间
        //                // 进行技能效果的处理
        //                // 检查玩家是否受到伤害
        //                if (player.statLifeLost > 0)
        //                {
        //                    int damageTaken = player.statLifeLost; // 受到的伤害量
        //                    player.statLife += damageTaken; // 补回受到的伤害
        //                    player.HealEffect(damageTaken); // 显示回复生命的特效

        //                    float manaCost = damageTaken * 1.5f; // 需要扣除的魔力值
        //                    // 检查剩余魔力是否足够扣除
        //                    if (player.statMana >= manaCost)
        //                    {
        //                        player.statMana -= (int)manaCost; // 扣除魔力值
        //                        player.ManaEffect((int)manaCost); // 显示扣除魔力的特效
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // 技能持续时间结束，重置技能状态
        //                MSSkillActive = false;
        //                MSSkillCooldown = 1800; // 设置技能的冷却时间
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // 技能还在冷却中，减少冷却时间
        //        MSSkillCooldown--;
        //    }

        //    player.GetModPlayer<PLPlayer>().MSSkillCooldown = MSSkillCooldown;
        //    player.GetModPlayer<PLPlayer>().MSSkillActive = MSSkillActive;
        //    player.GetModPlayer<PLPlayer>().MSSkillTime = MSSkillTime;
        //}


        //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();

        //if (!triggered)
        //{
        //    if (cooldownTime > 0)
        //    {
        //        cooldownTime--;
        //    }
        //    else
        //    {
        //        triggered = true;
        //        cooldownTime = 900; // 30秒的冷却时间，900帧

        //        Main.NewText("Magic Seeker activated!", 175, 75, 255); // 触发时在聊天框中显示信息
        //    }
        //}
        //else
        //{
        //    if (player.hurtCooldowns[0] == 0)
        //    {
        //        int damageTaken = player.statLifeMax2 - player.statLife; // 计算受到的伤害值
        //        int manaUsed = Math.Min(damageTaken, player.statMana); // 取较小值，避免超过当前魔力值
        //        player.statMana -= manaUsed;

        //        triggered = false; // 重新触发需要等待冷却时间
        //        Main.NewText("Damage converted to mana!", 175, 75, 255); // 在聊天框中显示信息
        //        SoundEngine.PlaySound(SoundID.Item71, player.position); // 播放Item71音效
        //    }
        //}


    }
}