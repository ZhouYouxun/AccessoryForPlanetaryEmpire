using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using System.Net.Sockets;

namespace AccessoryForPlanetaryEmpire.Accessory.Movement
{
    public class ControllableHalo : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值5金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.value = Item.sellPrice(0, 20, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 增加25%的跳跃速度
            player.jumpSpeedBoost += player.jumpSpeedBoost * 0.25f;

            // 增加15%的移速
            player.moveSpeed += player.moveSpeed * 0.15f;

            // 增加30%的飞行时间
            player.wingTimeMax = (int)(player.wingTimeMax * 1.3f);


            player.GetModPlayer<ControllableHaloPlayer>().hasControllableHalo = true;


            //// 判断是否已经有一个活跃的"ControllableHaloProjectile"弹幕
            bool haloExists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<ControllableHaloPROJ>())
                {
                    haloExists = true;
                    break;
                }
            }

            // 如果没有找到活跃的光环弹幕，则在鼠标位置生成一个新的
            if (!haloExists)
            {
                Vector2 mousePosition = Main.MouseWorld;
                Projectile.NewProjectile(player.GetSource_Accessory(Item), mousePosition, Vector2.Zero, ModContent.ProjectileType<ControllableHaloPROJ>(), 0, 0, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Binoculars, 1); // 望远镜
            recipe.AddIngredient(ItemID.Lens, 5); // 晶状体
            recipe.AddIngredient(ItemID.BlackLens, 1); // 黑色晶状体
            //recipe.AddIngredient(ItemID.LavaBucket, 3); // 熔岩桶
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }


    }
}
