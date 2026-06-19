using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    internal class TungstenCore : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";


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
            player.statDefense += 15; // 提供15点防御力
            player.GetModPlayer<TungstenCorePlayer>().hasTungstenCore = true;

            if (hideVisual)
            {
                player.GetModPlayer<TungstenCorePlayer>().hideVisual = true;
            }
            else
            {
                player.GetModPlayer<TungstenCorePlayer>().hideVisual = false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.HeavyWorkBench, 1); // 重型工作台
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnySilverBar", 50); // 钨锭
            recipe.AddIngredient(ItemID.Bottle, 75); // 瓶
            recipe.AddIngredient(ItemID.LavaBucket, 5); // 岩浆桶
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }


}
