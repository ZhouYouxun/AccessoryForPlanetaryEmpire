using AccessoryForPlanetaryEmpire.Players.Defense;
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

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyEMPs
{
    public class Interceptor65L : ModItem, ILocalizedModType, IEMPExclusive
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            // EMP 饰品互斥：两个中任意一个是 EMPExclusive 都禁止共存
            bool equippedIsEMP = ModContent.GetModItem(equippedItem.type) is IEMPExclusive;
            bool incomingIsEMP = ModContent.GetModItem(incomingItem.type) is IEMPExclusive;

            return !(equippedIsEMP && incomingIsEMP);
        }
        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs";
        // 65L型激光弹幕拦截器【花后】：摁住快捷键时，以每秒三发弹幕的速度，从距离玩家最近开始清除，总清除次数上限为100次，最大半径为65格。
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
            player.GetModPlayer<EMP65LPlayer>().has65LInterceptor = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.MechanicalLens, 1);
            recipe.AddIngredient(ItemID.Timer1Second, 1);
            recipe.AddIngredient(ItemID.Nanites, 100);
            recipe.AddIngredient(ItemID.ChainGun, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }


    }
}
