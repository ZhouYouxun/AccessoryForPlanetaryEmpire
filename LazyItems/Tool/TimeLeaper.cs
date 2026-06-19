using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events; // 引入沙尘暴相关的命名空间

namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
{
    public class TimeLeaper : ModItem
    {
        private int timeState = 0; // 用于循环时间的状态变量
        private bool isRaining = false; // 用于控制雨/暴雪状态
        private bool isSandstorm = false; // 用于控制沙尘暴状态

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Time Leaper");
            // Tooltip.SetDefault("Left-click to control the weather, right-click to control the time.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true; // 允许右键使用
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // 如果是右键使用
            {
                CycleTime(); // 循环调整时间
            }
            else // 左键使用
            {
                ToggleWeather(player); // 切换天气
            }

            return true;
        }

        private void CycleTime()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            timeState = (timeState + 1) % 4;

            switch (timeState)
            {
                case 0:
                    Main.SkipToTime(0, true); // 早上4:30 - 早上9点
                    Main.NewText("4:30", 255, 255, 0); // 显示调整后的时间信息
                    break;
                case 1:
                    Main.SkipToTime(27000, true); // 中午12:00
                    Main.NewText("12:00", 255, 255, 0); // 显示调整后的时间信息
                    break;
                case 2:
                    Main.SkipToTime(0, false); // 晚上7:30 早上4:30
                    Main.NewText("19:30", 255, 255, 0); // 显示调整后的时间信息
                    break;
                case 3:
                    Main.SkipToTime(16200, false); // 晚上12:00 早上4:30
                    Main.NewText("00:00", 255, 255, 0); // 显示调整后的时间信息
                    break;
            }
            
        }

        private void ToggleWeather(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (player.ZoneOverworldHeight) // 在地表区域
            {
                if (player.ZoneSnow || player.ZoneForest) // 森林或雪地
                {
                    isRaining = !isRaining;
                    if (isRaining)
                    {
                        Main.StartRain();
                        Main.NewText("下雨!", 0, 255, 0);
                    }
                    else
                    {
                        Main.StopRain();
                        Main.NewText("结束下雨!", 0, 255, 0);
                    }
                }
                else if (player.ZoneDesert) // 沙漠
                {
                    isSandstorm = !isSandstorm;
                    if (isSandstorm)
                    {
                        Sandstorm.StartSandstorm();
                        Main.NewText("沙尘暴!", 0, 255, 0);
                    }
                    else
                    {
                        Sandstorm.StopSandstorm();
                        Main.NewText("结束沙尘暴!", 0, 255, 0);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Sundial, 1);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }


    }
}