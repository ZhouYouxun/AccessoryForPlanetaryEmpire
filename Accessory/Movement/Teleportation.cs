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
using AccessoryForPlanetaryEmpire.Players.Movement;

namespace AccessoryForPlanetaryEmpire.Accessory.Movement
{
    public class Teleportation : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Movement";

        //public override void SetStaticDefaults()
        //{
        //    DisplayName.SetDefault("折跃启动引擎");
        //    Tooltip.SetDefault("(这不是消耗品) " +
        //        "\n按下快捷键，到达鼠标所在的地方，传送的一瞬间提升30倍伤害，同时清除鼠标范围半径15格以内的所有敌方弹幕" +
        //        "\n困难模式初期" +
        //        "\n这很明显是帝国舰队所配备的舰载机动性装置，但它的尺寸能够让你很熟练的使用它" +
        //        "\n眨眼既达！");
        //}

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();
            player.GetModPlayer<TeleportationPlayer>().TeleportationEquipped = true;


            //// 检查技能冷却时间
            //if (modPlayer.TSkillCooldown > 0)
            //{
            //    modPlayer.TSkillCooldown--; // 冷却时间减少

            //    // 冷却时间结束后，重置技能状态
            //    if (modPlayer.TSkillCooldown == 0)
            //    {
            //        modPlayer.TSkillActive = false;
            //    }
            //}

            //// 检查技能是否激活
            //if (modPlayer.TSkillActive)
            //{
            //    // 检查技能持续时间
            //    if (modPlayer.TSkillTime > 0)
            //    {
            //        if (!modPlayer.TSkillEffectsApplied)
            //        {
            //            // 瞬移到鼠标位置
            //            Vector2 mousePosition = Main.MouseWorld;
            //            player.Teleport(mousePosition, -1);
            //            player.velocity = Vector2.Zero;

            //            // 清除以自身为半径15格以内的所有敌方弹幕
            //            for (int i = 0; i < Main.projectile.Length; i++)
            //            {
            //                Projectile projectile = Main.projectile[i];
            //                if (projectile.active && projectile.hostile)
            //                {
            //                    float distance = Vector2.Distance(player.Center, projectile.Center);
            //                    if (distance <= 15 * 16) // 15格半径，每格16像素
            //                    {
            //                        projectile.active = false;
            //                    }
            //                }
            //            }
            //        }
            //        modPlayer.TSkillEffectsApplied = true; // 标记效果已应用

            //        // 提升3000%伤害
            //        player.GetDamage(DamageClass.Generic) += 30f;

            //        // 持续时间减少
            //        modPlayer.TSkillTime--;
            //    }
            //    else
            //    {
            //        // 设置冷却时间
            //        modPlayer.TSkillCooldown = 30 * 60; // 30冷却时间

            //        player.GetDamage(DamageClass.Generic) -= 10f;

            //        // 清除标记
            //        modPlayer.TSkillActive = false;
            //        modPlayer.TSkillEffectsApplied = false;
            //    }
            //}
            //else
            //{
            //    // 检查技能是否可以激活
            //    if (KeybindSystem.TKeybind.JustPressed && modPlayer.TSkillCooldown == 0)
            //    {
            //        // 设置技能持续时间和标记
            //        modPlayer.TSkillTime = 5 * 1; // 5
            //        modPlayer.TSkillActive = true;

            //        // 设置冷却时间
            //        modPlayer.TSkillCooldown = 30 * 60; // 30冷却时间
            //    }
            //}
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.RodofDiscord, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.DarkShard, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}

