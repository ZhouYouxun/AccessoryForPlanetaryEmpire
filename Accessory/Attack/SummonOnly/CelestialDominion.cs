using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SummonOnly
{
    public class CelestialDominion : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SummonOnly";
        public override void SetStaticDefaults()
        {
        
        
        }

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
            player.GetModPlayer<EnergizedChitinPlayer>().hasEnergizedChitin = true;
            player.GetModPlayer<GiftofLcarusPlayer>().hasGiftofLcarus = true;
            player.GetModPlayer<HandheldBlackHolePlayer>().hasHandheldBlackHole = true;
            player.GetModPlayer<MedicalDronesPlayer>().hasMedicalDrones = true;
            player.GetModPlayer<SynergisticCatalystPlayer>().hasSynergisticCatalyst = true;
            player.GetModPlayer<TamingWhipRingPlayer>().hasTamingWhipRing = true;



            if (hideVisual) // 关闭可见性
            {
                player.GetModPlayer<ArmyBattleBannerPlayer>().hasArmyBattleBanner = false;
            }
            else // 开启可见性
            {
                player.GetModPlayer<ArmyBattleBannerPlayer>().hasArmyBattleBanner = true;
            }

            if (hideVisual)
            {
                player.GetModPlayer<SynergisticCatalystPlayer>().eyeopen = false;
            }
            else
            {
                player.GetModPlayer<SynergisticCatalystPlayer>().eyeopen = true;
            }

            player.GetDamage(DamageClass.Summon) += 0.8f; // 增加80伤害
            player.maxTurrets += 5; // 增加5个哨兵栏
            player.maxMinions += 10; // 增加10个仆从栏


        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.ArmyBattleBanner>(), 1); // 军队战旗：减少召唤栏的数量并提供伤害
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.EnergizedChitin>(), 1); // 充能几丁质：让召唤物阻挡敌方弹幕或增强我方弹幕
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.GiftofLcarus>(), 1); // 伊卡鲁斯的礼物：让召唤物射出其他手持武器的弹幕
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.HandheldBlackHole>(), 1); // 可控黑洞：让哨兵跟随玩家移动
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.MedicalDrones>(), 1); // 医疗无人机：增加血量，生命回复和吸血
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.SynergisticCatalyst>(), 1); // 协同催化剂：其他职业的武器手持也能增加伤害
            recipe.AddIngredient(ModContent.ItemType<Attack.SummonOnly.TamingWhipRing>(), 1); // 驯兽鞭还：召唤物能够提供可堆叠的鞭子buff
            
            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("UnholyEssence").Type, 10);
                recipe.AddIngredient(calamityMod.Find<ModItem>("Polterplasm").Type, 10);
            }

            recipe.AddIngredient(ItemID.FragmentStardust, 100); // 星尘碎片
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }


    }
}