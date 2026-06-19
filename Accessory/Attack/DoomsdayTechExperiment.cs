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
using AccessoryForPlanetaryEmpire.Players.Attack;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack
{
    public class DoomsdayTechExperiment : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("末日科技实验体");
            //Tooltip.SetDefault("当玩家不发动攻击的时候，每1.5秒增加手持武器一倍的攻击力，上限为15秒增加10倍，达到上限之后会在玩家身边扩散射出一圈蓝色dust来告诉玩家充能完毕（如果没有达到上限发动进攻也是可以享受加成），只有战士射手召唤可以有这个加成。当玩家发动攻击时，只有第一发可以有这个加成，再次射击不会有加成，需要重新蓄力");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值15金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DoomsdayTechExperimentPlayer>().hasDoomsdayTechExperiment = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10); // 圣锭
            recipe.AddIngredient(ItemID.Wire, 25); // 电线
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil); // 在MythrilAnvil制造
            recipe.Register();
        }
    }
}