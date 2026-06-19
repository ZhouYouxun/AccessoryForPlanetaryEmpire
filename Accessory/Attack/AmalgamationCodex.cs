using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Players.Attack;


namespace AccessoryForPlanetaryEmpire.Accessory.Attack
{
    public class AmalgamationCodex : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";

        public override void SetStaticDefaults()
        {


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
            var modPlayer = player.GetModPlayer<AmalgamationCodexPlayer>();
            // 设置三个游戏进度检测开关
            modPlayer.downedBoss2 = NPC.downedBoss2; // 第一个邪恶boss被打败
            modPlayer.hardMode = Main.hardMode;       // 困难模式开启
            modPlayer.downedGolemBoss = NPC.downedGolemBoss; // 石巨人被打败
            modPlayer.hasAmalgamationCodex = true;

            player.fireWalk = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.WindPushed] = true;
            player.buffImmune[BuffID.Stoned] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.Book, 15); // 15本书
            recipe.AddTile(TileID.Bookcases); // 在书架制作
            recipe.Register();
        }
    }
}
