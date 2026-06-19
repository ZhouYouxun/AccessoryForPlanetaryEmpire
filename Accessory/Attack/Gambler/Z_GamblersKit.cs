using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Gambler
{
    public class Z_GamblersKit : ModItem, ILocalizedModType
    {
        // 赌徒套件：
        // 幸运骰子，幸运金币，赌徒礼帽，赌徒手套，赛博筹码，闪亮扑克牌6合1，拥有他们6个的所有效果（一股far味）

        public new string LocalizationCategory => "Accessory.Attack.Gambler";

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 最终毕业装：Master，价值95金
            Item.rare = ItemRarityID.Master;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.value = Item.sellPrice(0, 95, 0, 0);

        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CyberChipPlayer>().hasCyberChip = true;
            player.GetModPlayer<GamblersGlovesPlayer>().hasGamblersGloves = true;
            player.GetModPlayer<GamblersTopHatPlayer>().hasGamblersTopHat = true;
            player.GetModPlayer<SuperDicePlayer>().hasSuperDice = true;
            player.GetModPlayer<LuckyCoinPlayer>().hasLuckyCoin = true;
            player.GetModPlayer<GamblersBowTiePlayer>().hasGamblersBowTie = true;
            if (hideVisual)
            {
                player.GetModPlayer<ShinyPlayingCardsPlayer>().hasShinyPlayingCards = true;
                player.GetModPlayer<SuperDicePlayer>().hideVisual = true;
            }
            else
            {
                player.GetModPlayer<ShinyPlayingCardsPlayer>().hasShinyPlayingCards = false;
                player.GetModPlayer<SuperDicePlayer>().hideVisual = false;
            }
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.CyberChip>(), 1); // 他们7个
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.GamblersGloves>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.GamblersTopHat>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.ShinyPlayingCards>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.SuperDice>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.LuckyCoin>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Attack.Gambler.GamblersBowTie>(), 1);
            recipe.AddIngredient(ItemID.LunarBar, 5);

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("FantasyTalisman").Type, 51);
            }

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}




