//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.ID;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;

//namespace AccessoryForPlanetaryEmpire.Weapons
//{
//    internal class ChangeWeapon : GlobalItem
//    {

//        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
//        {
//            if (item.type == ItemID.Meowmere)
//            {
//                Projectile.NewProjectile(
//                    source,
//                    position,
//                    velocity,
//                    ProjectileID.Meowmere, // 502
//                    damage,
//                    knockback,
//                    player.whoAmI
//                );

//                // 返回 false，取消原本的发射逻辑
//                return false;
//            }

//            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
//        }



//    }
//}
