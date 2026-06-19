using AccessoryForPlanetaryEmpire.Players.Defense;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Projectiles;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Players.Defense;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.ClearBullet
{
    // EMP 互斥标记接口
    public interface IEMPExclusive { }

    public class Absorber15 : ModItem, ILocalizedModType, IEMPExclusive
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            // EMP 饰品互斥：两个中任意一个是 EMPExclusive 都禁止共存
            bool equippedIsEMP = ModContent.GetModItem(equippedItem.type) is IEMPExclusive;
            bool incomingIsEMP = ModContent.GetModItem(incomingItem.type) is IEMPExclusive;

            return !(equippedIsEMP && incomingIsEMP);
        }
        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs.ClearBullet";
        //15型能量吸收装置【花后】：按下快捷键时清除半径15格范围内的敌方弹幕，但清除的每一发弹幕都能转化为3.5%的临时伤害提升，伤害提升持续5秒，冷却时间45秒

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
            player.GetModPlayer<EMPClearBulletPlayer>().has15Absorber = true;           

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ChlorophytePlateMail, 1);
            recipe.AddIngredient(ItemID.Timer1Second, 1);
            recipe.AddIngredient(ItemID.Nanites, 100);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
