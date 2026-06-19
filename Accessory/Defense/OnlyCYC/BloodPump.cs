using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using static System.Collections.Specialized.BitVector32;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class BloodPump : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        // 活血泵:
        // 替换掉治疗药水，用更强大的治疗效果替代
        // （立刻恢复已损失生命值的一半，同时给予50点生命再生，持续10秒，在这10秒钟内拥有一个70%的伤害减免。当这10秒结束，一切恢复正常，重新进入冷却，冷却时间为60秒）

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();
            player.GetModPlayer<BloodPumpPlayer>().hasBloodPump = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.ImbuingStation, 1); // 灌注站
            recipe.AddIngredient(ItemID.InletPump, 1); // 进的水泵
            recipe.AddIngredient(ItemID.OutletPump, 1); // 出的水泵
            recipe.AddIngredient(ItemID.Switch, 1); // 开关
            recipe.AddIngredient(ItemID.LifeforcePotion, 15);
            recipe.AddIngredient(ItemID.SoulofLight, 5); // 光明之魂
            recipe.AddIngredient(ItemID.LightShard, 2); // 光明碎片
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}




