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
    public class Interceptor55VI : ModItem, ILocalizedModType, IEMPExclusive
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            // EMP 饰品互斥：两个中任意一个是 EMPExclusive 都禁止共存
            bool equippedIsEMP = ModContent.GetModItem(equippedItem.type) is IEMPExclusive;
            bool incomingIsEMP = ModContent.GetModItem(incomingItem.type) is IEMPExclusive;

            return !(equippedIsEMP && incomingIsEMP);
        }
        public new string LocalizationCategory => "Accessory.Defense.OnlyEMPs.ClearBullet";
        // 55VI型震荡弹射器【石巨人后】：按下快捷键时清除半径75格范围内的敌方弹幕，与此同时，眩晕范围内的所有敌人0.7秒
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
            player.GetModPlayer<EMPClearBulletPlayer>().has55VIInterceptor = true;           

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.MechanicalLens, 1); // 机械晶状体 x1
            recipe.AddIngredient(ItemID.MartianConduitPlating, 100); // 火星砖块 x100
            recipe.AddIngredient(ItemID.ChargedBlasterCannon, 1); // 充能爆能炮 x1
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }



    }
}
