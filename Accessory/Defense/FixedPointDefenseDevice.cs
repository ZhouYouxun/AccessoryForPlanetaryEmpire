using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Localization;
using System;
using AccessoryForPlanetaryEmpire.Players.Defense;


namespace AccessoryForPlanetaryEmpire.Accessory.Defense
{
    public class FixedPointDefenseDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense";
        //public override void SetStaticDefaults()
        //{
        //    DisplayName.SetDefault("定点防御装置");
        //    Tooltip.SetDefault("在玩家不移动的时候增加60%的防御力，15点生命回复，但是降低50%的攻击力" +
        //        "\n但是移动的时候降低99%的攻击力和防御力" +
        //        "\n骷髅王后" +
        //        "\n帝国重装防卫军所配备的制式装备，但由于在转移目标的时候要频繁取下，而遭到战士们的嫌弃");
        //}

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值35金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FixedPointDefenseDevicePlayer>().hasFixedPointDefenseDevice = true;




            //if (player.velocity == Vector2.Zero) // 如果玩家不移动
            //{
            //    player.GetDamage(DamageClass.Generic) -= 0.85f; // 减少85%的攻击力
            //    player.lifeRegen += 15; // 增加15点生命回复
            //    player.statDefense += (int)(player.statDefense * 0.6f); // 增加30%的防御力

            //    float radius = 50f; // 圆圈半径
            //    int dustCount = 20; // 粒子数量
            //    for (int i = 0; i < dustCount; i++)
            //    {
            //        float angle = 2f * (float)Math.PI / dustCount * i;
            //        float x = player.Center.X + radius * (float)Math.Cos(angle);
            //        float y = player.Center.Y + radius * (float)Math.Sin(angle);
            //        Vector2 dustPosition = new Vector2(x, y);
            //        Dust dust = Dust.NewDustPerfect(dustPosition, DustID.UltraBrightTorch, Vector2.Zero, 100, default(Color), 1.5f);
            //        dust.velocity *= 0.3f; // 调整粒子的速度
            //        dust.noGravity = true; // 不受重力影响
            //        dust.fadeIn = 1f;
            //        dust.scale = 1.5f;
            //    }
            //}
            //else // 如果玩家在移动
            //{
            //    player.GetDamage(DamageClass.Generic) -= 0.99f; // 降低99%的攻击力
            //    player.statDefense -= (int)(player.statDefense * 0.99f); // 降低99%的防御力
            //}
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.BeetleShell, 1); // 甲虫防御甲
            recipe.AddIngredient(ItemID.CobaltShield, 1);
            recipe.AddIngredient(ItemID.ObsidianShield, 1);
            recipe.AddIngredient(ItemID.ShieldStatue, 1);
            recipe.AddIngredient(ItemID.PaladinsShield, 1);
            recipe.AddIngredient(ItemID.EoCShield, 1);
            recipe.AddIngredient(ItemID.FrozenShield, 1);
            recipe.AddIngredient(ItemID.HeroShield, 1);
            recipe.AddIngredient(ItemID.BouncingShield, 1);
            recipe.AddTile(TileID.LihzahrdFurnace);
            recipe.Register();
        }

    }
}
