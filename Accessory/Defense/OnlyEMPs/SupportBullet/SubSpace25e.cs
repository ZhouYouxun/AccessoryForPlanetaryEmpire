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
using AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.ClearBullet;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs.SupportBullet
{
    public class SubSpace25e : ModItem, ILocalizedModType, IEMPExclusive
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            // EMP 饰品互斥：两个中任意一个是 EMPExclusive 都禁止共存
            bool equippedIsEMP = ModContent.GetModItem(equippedItem.type) is IEMPExclusive;
            bool incomingIsEMP = ModContent.GetModItem(incomingItem.type) is IEMPExclusive;

            return !(equippedIsEMP && incomingIsEMP);
        }
        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs.SupportBullet";
        // 25f型超时空场生成装置【3王后】：按下快捷键时在原地放置一个半径35的屏障，屏障可以为内部的所有玩家提供持续的无敌，离开这个范围不再提供，冷却时间为30秒
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
            player.GetModPlayer<EMPSupportBulletPlayer>().has25eSubSpace = true;           

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.MechanicalBatteryPiece, 1);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
