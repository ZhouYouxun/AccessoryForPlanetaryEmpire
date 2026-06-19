using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Global;
using static System.Collections.Specialized.BitVector32;
using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    public class UltimateFunctionOverloadDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值15金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<UltimateFunctionOverloadDevicePlayer>().hasUltimateFunctionOverloadDevice = true;
            player.manaCost -= 1f; // Reduce mana cost by 100%

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 5); // 神圣定
            recipe.AddIngredient(ItemID.WaterBucket, 6); // 三种液体
            recipe.AddIngredient(ItemID.LavaBucket, 6);
            recipe.AddIngredient(ItemID.HoneyBucket, 6);
            recipe.AddIngredient(ItemID.PutridScent, 1); // 腐香囊
            recipe.AddIngredient(ItemID.SoulofFright, 1); // 三王魂
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}



























//public override void UpdateAccessory(Player player, bool hideVisual)
//{
//    PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();

//    // 检查技能冷却时间
//    if (modPlayer.UFODSkillCooldown > 0)
//    {
//        modPlayer.UFODSkillCooldown--; // 冷却时间减少

//        // 冷却时间结束后，重置技能状态
//        if (modPlayer.UFODSkillCooldown == 0)
//        {
//            modPlayer.UFODSkillActive = false;
//        }
//    }

//    // 检查技能是否激活
//    if (!modPlayer.UFODSkillActive && KeybindSystem.UFODKeybind.JustPressed && modPlayer.UFODSkillCooldown == 0)
//    {
//        // 消耗生命和魔力
//        int healthCost = (int)(player.statLifeMax2 * 0.5f);
//        int manaCost = player.statManaMax2;
//        player.statLife -= healthCost;
//        player.statMana -= manaCost;

//        // 降低防御力和移动速度
//        player.statDefense /= 2;
//        player.moveSpeed /= 2f;

//        // 提升仇恨值
//        player.aggro += 3000;

//        // 提升伤害
//        player.GetDamage(DamageClass.Generic) += 9f;

//        // 设置技能持续时间和标记
//        modPlayer.ExampleSkillTime = 15 * 60; // 15秒
//        modPlayer.UFODSkillActive = true;

//        // 设置冷却时间
//        modPlayer.UFODSkillCooldown = 60 * 60; // 1分钟冷却时间
//    }
//}


//public override void UpdateAccessory(Player player, bool hideVisual)
//{
//    PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();

//    // 检查技能冷却时间
//    if (modPlayer.UFODSkillCooldown > 0)
//    {
//        modPlayer.UFODSkillCooldown--; // 冷却时间减少

//        // 冷却时间结束后，重置技能状态
//        if (modPlayer.UFODSkillCooldown == 0)
//        {
//            modPlayer.UFODSkillActive = false;
//        }
//    }

//    // 检查技能是否激活
//    if (modPlayer.UFODSkillActive)
//    {
//        // 检查技能持续时间
//        if (modPlayer.ExampleSkillTime > 0)
//        {

//            // 消耗生命和魔力
//            int healthCost = (int)(player.statLifeMax2 * 0.7f);
//            int manaCost = player.statManaMax2;
//            player.statLife -= healthCost;
//            player.statMana -= manaCost;

//            modPlayer.UFODSkillEffectsApplied = true; // 标记效果已应用
//        }


//        // 消耗生命和魔力
//        //int healthCost = (int)(player.statLifeMax2 * 0.5f);
//        //int manaCost = player.statManaMax2;
//        //player.statLife -= healthCost;
//        //player.statMana -= manaCost;



//        // 降低防御力和移动速度
//        player.statDefense /= 4;
//        player.moveSpeed /= 2f;

//        // 提升仇恨值
//        player.aggro += 3000;

//        // 提升伤害
//        player.GetDamage(DamageClass.Generic) += 9f;

//        // 持续时间减少
//        modPlayer.ExampleSkillTime--;
//        }
//        else
//        {
//        // 还原生命和魔力
//        int healthGain = (int)(player.statLifeMax2 * 0.5f);
//        int manaGain = player.statManaMax2;
//        player.statLife += healthGain;
//        player.statMana += manaGain;

//        // 还原防御力和移动速度
//        player.statDefense *= 4;
//        player.moveSpeed *= 2f;

//        // 还原伤害
//        player.GetDamage(DamageClass.Generic) += 9f;

//        // 设置冷却时间
//        modPlayer.UFODSkillCooldown = 60 * 60; // 1分钟冷却时间

//        // 清除标记
//        modPlayer.UFODSkillActive = false;
//        modPlayer.UFODSkillEffectsApplied = false;
//        }
//    }
//    else
//    {
//        // 检查技能是否可以激活
//        if (KeybindSystem.UFODKeybind.JustPressed && modPlayer.UFODSkillCooldown == 0)
//        {
//            // 设置技能持续时间和标记
//            modPlayer.ExampleSkillTime = 15 * 60; // 15秒
//            modPlayer.UFODSkillActive = true;

//            // 设置冷却时间
//            modPlayer.UFODSkillCooldown = 60 * 60; // 1分钟冷却时间
//        }
//    }
//}





//public override void UpdateAccessory(Player player, bool hideVisual)
//{
//    PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();

//    // 检查技能冷却时间
//    if (modPlayer.UFODSkillCooldown > 0)
//    {
//        modPlayer.UFODSkillCooldown--; // 冷却时间减少

//        // 冷却时间结束后，重置技能状态
//        if (modPlayer.UFODSkillCooldown == 0)
//        {
//            modPlayer.UFODSkillActive = false;

//            // 解除血量和魔力限制
//            player.GetModPlayer<PlanetaryPlayer>().RemoveHealthAndManaLimit();
//        }
//    }

//    // 检查技能是否激活
//    if (modPlayer.UFODSkillActive)
//    {
//        // 检查技能持续时间
//        if (modPlayer.ExampleSkillTime > 0)
//        {
//            // 检查血量是否超过限制
//            if (player.statLife > player.statLifeMax2 * 0.3f)
//            {
//                player.statLife = (int)(player.statLifeMax2 * 0.3f);
//            }

//            // 固定魔力为零
//            player.statMana = 0;

//            // 持续时间减少
//            modPlayer.ExampleSkillTime--;
//        }
//        else
//        {
//            // 还原防御力和移动速度
//            player.statDefense *= 2;
//            player.moveSpeed *= 2f;

//            // 还原伤害
//            player.GetDamage(DamageClass.Generic) -= 0.09f;

//            // 设置冷却时间
//            modPlayer.UFODSkillCooldown = 60 * 60; // 1分钟冷却时间

//            // 清除标记
//            modPlayer.UFODSkillActive = false;

//            // 解除血量和魔力限制
//            player.GetModPlayer<PlanetaryPlayer>().RemoveHealthAndManaLimit();
//        }
//    }
//    else
//    {
//        // 检查技能是否可以激活
//        if (KeybindSystem.UFODKeybind.JustPressed && modPlayer.UFODSkillCooldown == 0)
//        {
//            // 设置技能持续时间和标记
//            modPlayer.ExampleSkillTime = 15 * 60; // 15秒
//            modPlayer.UFODSkillActive = true;

//            // 设置冷却时间
//            modPlayer.UFODSkillCooldown = 60 * 60; // 1分钟冷却时间

//            // 限制血量和魔力
//            player.GetModPlayer<PlanetaryPlayer>().ApplyHealthAndManaLimit();
//        }
//    }
//}