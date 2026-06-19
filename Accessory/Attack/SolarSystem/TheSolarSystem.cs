using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem;
using Terraria.Audio;
using AccessoryForPlanetaryEmpire.Players.Attack.Gambler;
using AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem
{
    public class TheSolarSystem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.SolarSystem";

        //private int MARSfireCooldown = 180; // 180帧即3秒
        //private int SATfireCooldown = 180; // 180帧即3秒
        //private int NEPfireCooldown = 0;
        //private const int XHXTCooldown = 45 * 60; // 45秒冷却时间
        //private int XHXTtimer = 0;

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
            // 基础数值提升
            player.GetDamage(DamageClass.Generic) += 0.25f; // 全职业伤害 +25%
            player.GetCritChance(DamageClass.Generic) += 15f; // 全职业暴击 +15%
            player.moveSpeed += 0.15f; // 移速 +15%
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.25f); // 最大生命值 +25%

            player.GetAttackSpeed(DamageClass.Melee) += 0.25f; // 近战攻速 +25%
            player.ammoCost75 = true; // 远程弹速 +30%
            player.manaCost -= 0.4f; // 魔力消耗 -40%
            player.maxMinions += 4; // 召唤栏 +4
            player.maxTurrets += 4; // 哨兵栏 +4
                        
            player.jumpSpeedBoost += player.jumpSpeedBoost * 0.35f; // 增加35%的跳跃速度           
            player.wingTimeMax = (int)(player.wingTimeMax * 2.1f); // 增加110%的飞行时间

            player.GetModPlayer<MercuryPlayer>().hasMercuryAccessory = true;
            player.GetModPlayer<MercuryPlayer>().hasTSSAccessory = true;
            player.GetModPlayer<VenusPlayer>().hasVenusAccessory = true;
            player.GetModPlayer<EarthPlayer>().hasEarthAccessory = true;
            player.GetModPlayer<EarthPlayer>().hasTSSAccessory = true;
            player.GetModPlayer<MarsPlayer>().hasMarsAccessory = true;
            player.GetModPlayer<MarsPlayer>().hasTSSAccessory = true;

            player.GetModPlayer<JupiterPlayer>().hasJupiterAccessory =true;
            player.GetModPlayer<JupiterPlayer>().hasTSSAccessory = true;
            player.GetModPlayer<SaturnPlayer>().hasSaturnAccessory = true;
            player.GetModPlayer<SaturnPlayer>().hasTSSAccessory = true;
            player.GetModPlayer<UranusPlayer>().hasUranusAccessory = true;
            player.GetModPlayer<UranusPlayer>().hasTSSAccessory = true;
            player.GetModPlayer<NeptunePlayer>().hasNeptuneAccessory = true;
            player.GetModPlayer<NeptunePlayer>().hasTSSAccessory = true;

            player.GetModPlayer<PlutoPlayer>().hasPlutoAccessory = true;
            player.GetModPlayer<GravitationalFieldPlayer>().hasGravitationalField = true;

            player.GetModPlayer<SunPlayer>().hasSunAccessory = true; 


            if (hideVisual) // 关闭可见性
            {
                player.GetModPlayer<UranusPlayer>().GuangHuan = false;
                player.GetModPlayer<SunPlayer>().GuangHuan = false;
            }
            else // 开启可见性
            {
                player.GetModPlayer<UranusPlayer>().GuangHuan = true;
                player.GetModPlayer<SunPlayer>().GuangHuan = true;
            }

            player.GetModPlayer<GravitationalFieldPlayer>().hasGravitationalField = true;


        }







        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();            
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.MercuryAccessory>(), 1); // 水星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.VenusAccessory>(), 1); // 金星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.EarthAccessory>(), 1); // 地球
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.MarsAccessory>(), 1); // 火星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.JupiterAccessory>(), 1); // 木星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.SaturnAccessory>(), 1); // 土星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.UranusAccessory>(), 1); // 天王星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.NeptuneAccessory>(), 1); // 海王星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.PlutoAccessory>(), 1); // 冥王星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.GravitationalField>(), 1); // 哈雷彗星
            recipe.AddIngredient(ModContent.ItemType<Accessory.Attack.SolarSystem.SunAccessory>(), 1); // 太阳

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("DarkPlasma").Type, 1); // 暗离子体
            }
            recipe.AddIngredient(ItemID.LunarBar, 5); // 夜明锭

            recipe.AddTile(TileID.LunarCraftingStation); // 远古合成站
            recipe.Register();
        }

    }
}




