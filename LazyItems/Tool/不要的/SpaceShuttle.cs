//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using System;
//using AccessoryForPlanetaryEmpire.Players;
//using Terraria.Audio;


//namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
//{
//    public class SpaceShuttle : ModItem
//    {

//        private Vector2 teleportPosition = Vector2.Zero;
//        private bool hasTeleportPosition = false;
//        private int rightClickHoldTime = 0;

//        public override void SetStaticDefaults()
//        {
//            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;

//            // DisplayName.SetDefault("Space Shuttle");
//            // Tooltip.SetDefault("Right-click to set a teleportation point, left-click to return to it.");
//        }

//        public override void SetDefaults()
//        {
//            Item.width = 32;
//            Item.height = 32;
//            Item.useTime = 60;
//            Item.useAnimation = 60;
//            Item.useStyle = ItemUseStyleID.HoldUp;
//            Item.rare = ItemRarityID.Cyan;
//            Item.value = Item.sellPrice(gold: 10);
//            Item.noMelee = true;
//            Item.autoReuse = false;
//            Item.useTurn = true;
//        }

//        public override bool AltFunctionUse(Player player)
//        {
//            return true; // 允许右键使用
//        }

//        public override bool CanUseItem(Player player)
//        {
//            if (player.altFunctionUse == 2) // 右键功能
//            {
//                if (rightClickHoldTime > 0)
//                {
//                    rightClickHoldTime++;

//                    // 持续释放粒子特效
//                    if (rightClickHoldTime % 10 == 0)
//                    {
//                        for (int i = 0; i < 5; i++)
//                        {
//                            Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror);
//                        }
//                    }

//                    if (rightClickHoldTime >= 180) // 3秒钟后
//                    {
//                        SetTeleportPoint(player);
//                        rightClickHoldTime = 0;
//                        return false;
//                    }
//                }
//            }
//            else if (hasTeleportPosition) // 左键传送
//            {
//                player.itemTime = Item.useTime; // 初始化传送计时器
//                if (player.itemTime == Item.useTime - 1) // 准备传送
//                {
//                    for (int i = 0; i < 5; i++)
//                    {
//                        Dust.NewDust(player.position, player.width, player.height, DustID.Teleporter);
//                    }

//                    player.Teleport(teleportPosition, 1, 0);
//                    SoundEngine.PlaySound(SoundID.Item6, player.position);
//                    for (int i = 0; i < 30; i++)
//                    {
//                        Dust.NewDust(teleportPosition, player.width, player.height, DustID.Teleporter);
//                    }

//                    player.itemTime = 0; // 重置计时器
//                    return true;
//                }
//            }
//            return base.CanUseItem(player);
//        }

//        public override void HoldItem(Player player)
//        {
//            if (player.altFunctionUse == 2) // 右键功能
//            {
//                if (rightClickHoldTime == 0)
//                {
//                    rightClickHoldTime = 1; // 开始计时
//                }
//            }
//            else
//            {
//                rightClickHoldTime = 0; // 重置右键计时
//            }
//        }

//        private void SetTeleportPoint(Player player)
//        {
//            teleportPosition = player.position;
//            hasTeleportPosition = true;

//            // 释放大量粒子特效
//            for (int i = 0; i < 30; i++)
//            {
//                Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror);
//            }

//            // 在左下角打印提示信息
//            Main.NewText($"{player.name} 的传送点已被设置在 ({(int)teleportPosition.X / 16}, {(int)teleportPosition.Y / 16})", 255, 255, 0);
//        }

//        public override void AddRecipes()
//        {
//            Recipe recipe = CreateRecipe();
//            recipe.AddIngredient(ItemID.MagicMirror, 1);
//            recipe.AddIngredient(ItemID.TeleportationPotion, 5);
//            recipe.AddTile(TileID.Anvils);
//            recipe.Register();
//        }
//    }
//}