using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Weapons.New
{
    internal class TheBow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "NewWeapons";




        public override void SetDefaults()
        {
            // 基础属性（偏弱，新手用）
            Item.width = 26;
            Item.height = 48;

            Item.damage = 6;              // 低伤害，初期用
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 7;
            Item.useAnimation = 28;
            Item.useLimitPerAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.noMelee = true;          // 弓不能近战伤害
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.White;
            Item.value = Item.buyPrice(silver: 10);

            Item.UseSound = SoundID.Item5;
            Item.autoReuse = false;

            Item.shoot = ProjectileID.WoodenArrowFriendly;  // 默认木箭
            Item.shootSpeed = 7f;

            Item.useAmmo = AmmoID.Arrow;  // 使用箭矢
        }

        public override Vector2? HoldoutOffset()
        {
            // 可微调手持位置，也可以留空
            return new Vector2(-2f, 0f);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // 右键：超高速单发模式
                Item.useTime = 5;
                Item.useAnimation = 5;
                Item.useLimitPerAnimation = 1;
                Item.autoReuse = true;

                Item.shoot = ProjectileID.WoodenArrowFriendly;
            }
            else
            {
                // 左键：三平行箭（保持原状）
                Item.useTime = 7;
                Item.useAnimation = 28;
                Item.useLimitPerAnimation = 2;
                Item.autoReuse = false;

                Item.shoot = ProjectileID.WoodenArrowFriendly;
            }

            return base.CanUseItem(player);
        }


        public override bool Shoot(Player player,
        Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            // ============================
            // 右键：单发高速箭
            // ============================
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(
                    source,
                    position,
                    velocity,
                    type,
                    damage,
                    knockBack,
                    player.whoAmI
                );

                return false;
            }

            // ============================
            // 左键：三平行箭（保持原样）
            // ============================

            Vector2 dir = velocity.SafeNormalize(Vector2.UnitX);
            Vector2 horizontal = dir.RotatedBy(MathHelper.PiOver2);

            float offsetDist = 6f;

            // 中间箭
            Projectile.NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI);

            // 左箭
            Projectile.NewProjectile(
                source,
                position + horizontal * (-offsetDist),
                velocity,
                type, damage, knockBack,
                player.whoAmI
            );

            // 右箭
            Projectile.NewProjectile(
                source,
                position + horizontal * (offsetDist),
                velocity,
                type, damage, knockBack,
                player.whoAmI
            );

            return false;
        }


        // ----------------------------
        // 配方（随便写一个，后面你再改）
        // ----------------------------
        public override void AddRecipes()
        {
            //Recipe recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.Wood, 25);
            //recipe.AddIngredient(ItemID.StoneBlock, 10);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.Register();
        }




    }
}
