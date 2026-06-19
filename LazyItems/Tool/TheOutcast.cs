using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using static AccessoryForPlanetaryEmpire.Global.PLGlobalItem;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
{
    public class TheOutcast : ModItem
    {
        private int currentSolutionIndex = 0; // 当前溶液的索引
        private readonly int[] solutionTypes = {
            ProjectileID.PureSpray,         // 145: 纯净（绿色）
            ProjectileID.HallowSpray,       // 146: 神圣（浅蓝色）
            ProjectileID.CorruptSpray,      // 147: 腐化（紫色）
            ProjectileID.MushroomSpray,     // 148: 蘑菇（深蓝色）
            ProjectileID.CrimsonSpray,      // 149: 猩红（猩红色）
            1015,                           // 亮黄色，传播沙漠地形
            1016,                           // 白色，传播雪原地形
            1017                            // 棕色，传播森林地形
        };

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Outcast");
            //Tooltip.SetDefault("A versatile tool that shoots various solutions to alter terrain.\nRight-click to switch between different solutions.");
        }

        public override void SetDefaults()
        {
            Item.damage = 0; // 工具的伤害为 0
            Item.width = 121;
            Item.height = 29;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = solutionTypes[currentSolutionIndex]; // 初始射击的溶液
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.useAmmo = AmmoID.None;
            Item.autoReuse = true; // 自动连续使用
        }

        public override bool AltFunctionUse(Player player)
        {
            return true; // 允许右键使用
        }

        public override Vector2? HoldoutOffset() => new Vector2(-28, 0);

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // 如果是右键点击
            {
                // 播放右键切换音效
                SoundEngine.PlaySound(SoundID.Item149);

                // 切换溶液模式
                currentSolutionIndex = (currentSolutionIndex + 1) % solutionTypes.Length;
                Item.shoot = solutionTypes[currentSolutionIndex];

                // 根据溶液类型显示不同颜色的文字
                Color textColor = GetSolutionColor(currentSolutionIndex);
                CombatText.NewText(player.getRect(), textColor, "已切换至 " + GetSolutionName(currentSolutionIndex) + " 溶液");

                return false; // 右键点击不会触发射击
            }
            else
            {
                // 播放左键射击音效
                SoundEngine.PlaySound(SoundID.Item34);
            }

            return true;
        }

        private string GetSolutionName(int index)
        {
            // 根据索引返回对应的溶液名称
            switch (solutionTypes[index])
            {
                case ProjectileID.PureSpray: return "绿色溶液（纯净）";
                case ProjectileID.HallowSpray: return "浅蓝色溶液（神圣）";
                case ProjectileID.CorruptSpray: return "紫色溶液（腐化）";
                case ProjectileID.MushroomSpray: return "深蓝色溶液（蘑菇）";
                case ProjectileID.CrimsonSpray: return "猩红色溶液（猩红）";
                case 1015: return "亮黄色溶液（沙漠）";
                case 1016: return "白色溶液（雪原）";
                case 1017: return "棕色溶液（森林）";
                default: return "未知溶液";
            }
        }

        private Color GetSolutionColor(int index)
        {
            // 根据索引返回对应的颜色
            switch (solutionTypes[index])
            {
                case ProjectileID.PureSpray: return Color.Green;
                case ProjectileID.HallowSpray: return new Color(173, 216, 230); // 浅蓝色
                case ProjectileID.CorruptSpray: return Color.Purple;
                case ProjectileID.MushroomSpray: return new Color(0, 0, 139); // 深蓝色
                case ProjectileID.CrimsonSpray: return Color.Red;
                case 1015: return Color.Yellow;
                case 1016: return Color.White;
                case 1017: return new Color(139, 69, 19); // 棕色
                default: return Color.Gray; // 未知溶液，使用灰色
            }
        }

        public override void AddRecipes() // 添加合成配方
        {
            Recipe recipe = CreateRecipe(); // 创建一个合成配方
            recipe.AddIngredient(ItemID.Clentaminator); // 环境改造枪
            recipe.AddTile(TileID.Anvils); // 设置合成站类型
            recipe.Register(); // 注册合成配方
        }


    }
}