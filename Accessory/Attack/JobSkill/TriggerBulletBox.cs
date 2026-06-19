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
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Projectiles;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.JobSkill
{
    public class TriggerBulletBox : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.JobSkill";


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
            player.GetDamage<RangedDamageClass>() += 0.1f;
            player.GetCritChance<RangedDamageClass>() += 5;
            player.GetModPlayer<TriggerBulletBoxPlayer>().hasTriggerBulletBox = true;

            if (hideVisual)
            {
                player.GetModPlayer<TriggerBulletBoxPlayer>().eyeopen = true;
            }
            else
            {
                player.GetModPlayer<TriggerBulletBoxPlayer>().eyeopen = false;
            }

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.AmmoBox, 1); // 弹药箱
            recipe.AddIngredient(ItemID.CursedBullet, 999); // 两种邪恶子弹999发
            recipe.AddIngredient(ItemID.IchorBullet, 999);
            recipe.AddIngredient(ItemID.CrystalBullet, 999); // 水晶子弹
            recipe.AddIngredient(ItemID.GoldenBullet, 999); // 黄金子弹
            recipe.AddIngredient(ItemID.ExplodingBullet, 999); // 爆破子弹
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }


    }
}
