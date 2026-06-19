using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AccessoryForPlanetaryEmpire.Global.PLGlobalItem;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly
{
    public class SynergisticCatalyst : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Synergistic Catalyst");
            //Tooltip.SetDefault("When using melee, ranged, or magic weapons, has a 35% chance to increase minion damage by 5% to 30%");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值15金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.value = Item.sellPrice(0, 15, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SynergisticCatalystPlayer>().hasSynergisticCatalyst = true;
            if (hideVisual)
            {
                player.GetModPlayer<SynergisticCatalystPlayer>().eyeopen = false;
            }
            else
            {
                player.GetModPlayer<SynergisticCatalystPlayer>().eyeopen = true;
            }
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Bottle, 10); // 玻璃瓶
            recipe.AddIngredient(ItemID.Ruby, 1); // 红宝石
            recipe.AddIngredient(ItemID.Feather, 1); // 羽毛
            recipe.AddIngredient(ItemID.SharkFin, 1); // 鲨鱼鳍
            recipe.AddIngredient(ItemID.Silk, 1); // 丝绸
            recipe.AddIngredient(ItemID.LavaBucket, 1); // 岩浆桶
            recipe.AddTile(TileID.CookingPots); // 烹饪锅
            recipe.Register();
        }


    }
}