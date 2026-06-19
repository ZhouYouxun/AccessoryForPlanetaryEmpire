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
using XPT.Core.Audio.MP3Sharp.Decoding;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;

namespace AccessoryForPlanetaryEmpire.Accessory.Defense.OnlyCYC
{
    public class BloodVesselAccelerator : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Defense.OnlyCYC";
        // 血管加速器:
        // 生命值低于50%时提升25生命再生速度，低于15%提升100，高于75%归零

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
            //PlanetaryPlayer modPlayer = player.GetModPlayer<PlanetaryPlayer>();
            player.GetModPlayer<BloodVesselAcceleratorPlayer>().hasBloodVesselAccelerator = true;
            //player.GetModPlayer<HandHeldMechanicalBucklerPlayer>().hasHandHeldMechanicalBuckler = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.BoneWelder, 1); // 骨头焊机
            recipe.AddIngredient(ItemID.InletPump, 1); // 进的水泵
            recipe.AddIngredient(ItemID.OutletPump, 1); // 出的水泵
            recipe.AddIngredient(ItemID.RegenerationPotion, 15); // 再生药水
            recipe.AddIngredient(ItemID.AquaScepter, 1); // 海蓝法杖
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}




