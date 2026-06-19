using AccessoryForPlanetaryEmpire.Players.Attack.Annihilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players.Attack;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.Annihilation
{
    public class CrownofOblivion : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.Annihilation";
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
            player.GetDamage(DamageClass.Melee) += 0.45f; // 提升45%的近战攻击力
            player.GetAttackSpeed(DamageClass.Melee) += 0.45f; // 提升45%的近战攻击速度
            player.GetModPlayer<AntimatterSynthesizerPlayer>().hasAntimatterSynthesizer = true;
            player.GetModPlayer<DamageProtectionPlayer>().hasDamageProtection = true;
            player.GetModPlayer<FissionReactorPlayer>().hasFissionReactor = true;
            player.GetModPlayer<HeroDamageDevicePlayer>().HasHeroDamageDevice = true;
            player.GetModPlayer<LimitResponseModulePlayer>().hasLimitResponseModule = true;
            player.GetModPlayer<SolarBannerPlayer>().hasSolarBanner = true;
            player.GetModPlayer<SoulQuantumCorruptorPlayer>().hasSoulQuantumCorruptor = true;
            player.GetModPlayer<UltimateFunctionOverloadDevicePlayer>().hasUltimateFunctionOverloadDevice = true;


            if (hideVisual)
            {
                player.GetModPlayer<TheEntropyPlayer>().hasTheEntropy = true;
            }
            else
            {
                player.GetModPlayer<TheEntropyPlayer>().hasTheEntropy = false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.AntimatterSynthesizer>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.DamageProtectionDevice>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.FissionReactor>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.HeroDamageDevice>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.LimitResponseModule>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.SolarBanner>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.SoulQuantumCorruptor>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.TheEntropy>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.Annihilation.UltimateFunctionOverloadDevice>(), 1);

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("PlanetaryAnnihilation").Type, 1);
            }

            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
