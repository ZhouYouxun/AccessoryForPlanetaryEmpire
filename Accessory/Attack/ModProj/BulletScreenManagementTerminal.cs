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
using System.IO.Pipelines;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj;
using AccessoryForPlanetaryEmpire.Players.Defense.Counter;
using AccessoryForPlanetaryEmpire.Players.Attack;
using AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj.PermanentModProj;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack.ModProj
{
    public class BulletScreenManagementTerminal : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack.ModProj";
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
            player.manaCost -= 1f; // 魔力消耗变为0（常驻）



            // 增加穿透能力和弹幕速度（常驻）
            // player.GetModPlayer<VortexSightPlayer>().hasVortexSight = true;

            // 弹幕必定暴击（常驻）
            player.GetCritChance(DamageClass.Generic) += 100f;

            player.GetDamage(DamageClass.Generic) += 0.3f; // 全职业伤害 +30%


            // 固定提升伤害（常驻）
            player.GetModPlayer<AttackIndicatorPlayer>().hasAttackIndicator = true;

            // 破甲效果（常驻）
            player.GetModPlayer<BreakingDrillPlayer>().hasBreakingDrill = true;

            // 随速度提升伤害（常驻）
            player.GetModPlayer<MomentumChipPlayer>().hasMomentumChip = true;

            // 增加近距离远程伤害（常驻）
            player.GetModPlayer<QuickDrawHolsterPlayer>().hasQuickDrawHolster = true;



            if (hideVisual) // 关闭可见性
            {
                //player.GetModPlayer<AITrackingDevicePlayer>().SuoyouZhuizong = true;
                player.GetModPlayer<PhaseProjectorPlayer>().hasPhaseProjector = false;
            }
            else // 开启可见性
            {
                //player.GetModPlayer<AITrackingDevicePlayer>().SuoyouZhuizong = false;
                player.GetModPlayer<PhaseProjectorPlayer>().hasPhaseProjector = true; // 相位投影仪：让不穿透的弹幕获得强力的效果（只有开启可见性才会启用）
            }






            BulletScreenManagementTerminalPlayer bsmPlayer = player.GetModPlayer<BulletScreenManagementTerminalPlayer>();
            bsmPlayer.hasBulletScreenManagementTerminal = true;

            if (KeybindSystem.BSMTKeybind.JustPressed)
            {
                // 切换效果索引，索引从0到6
                bsmPlayer.currentEffectIndex = (bsmPlayer.currentEffectIndex + 1) % 7;

                // 根据索引启用对应的效果
                switch (bsmPlayer.currentEffectIndex)
                {
                    case 0:
                        CombatText.NewText(player.getRect(), Color.Gray, "已切换至 无效果");
                        // 关闭所有效果
                        player.GetModPlayer<GlitchCorePlayer>().hasGlitchCore = false;
                        player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation = false;
                        player.GetModPlayer<ParticleAcceleratorPlayer>().hasParticleAccelerator = false;
                        player.GetModPlayer<AITrackingDevicePlayer>().hasAITrackingDevice = false;
                        player.GetModPlayer<EchoBoundaryPlayer>().hasEchoBoundary = false;
                        player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet = false;
                        break;
                    case 1:
                        CombatText.NewText(player.getRect(), Color.Red, "已切换至 故障装置 效果");
                        player.GetModPlayer<GlitchCorePlayer>().hasGlitchCore = true;
                        break;
                    case 2:
                        CombatText.NewText(player.getRect(), Color.Yellow, "已切换至 万有引力 效果");
                        player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation = true;
                        break;
                    case 3:
                        CombatText.NewText(player.getRect(), Color.White, "已切换至 粒子加速 效果");
                        player.GetModPlayer<ParticleAcceleratorPlayer>().hasParticleAccelerator = true;
                        break;
                    case 4:
                        CombatText.NewText(player.getRect(), Color.Cyan, "已切换至 定位装置 效果");
                        player.GetModPlayer<AITrackingDevicePlayer>().hasAITrackingDevice = true;
                        break;
                    case 5:
                        CombatText.NewText(player.getRect(), Color.Green, "已切换至 回音边界 效果");
                        player.GetModPlayer<EchoBoundaryPlayer>().hasEchoBoundary = true;
                        break;
                    case 6:
                        CombatText.NewText(player.getRect(), Color.Magenta, "已切换至 星云钓竿 效果");
                        player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet = true;
                        break;
                }
            }

            // 检查并保持启用当前选择的效果
            if (bsmPlayer.currentEffectIndex == 0)
            {
                player.GetModPlayer<GlitchCorePlayer>().hasGlitchCore = false;
                player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation = false;
                player.GetModPlayer<ParticleAcceleratorPlayer>().hasParticleAccelerator = false;
                player.GetModPlayer<AITrackingDevicePlayer>().hasAITrackingDevice = false;
                player.GetModPlayer<EchoBoundaryPlayer>().hasEchoBoundary = false;
                player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet = false;
            }
            else if (bsmPlayer.currentEffectIndex == 1)
            {
                player.GetModPlayer<GlitchCorePlayer>().hasGlitchCore = true;
            }
            else if (bsmPlayer.currentEffectIndex == 2)
            {
                player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation = true;
            }
            else if (bsmPlayer.currentEffectIndex == 3)
            {
                player.GetModPlayer<ParticleAcceleratorPlayer>().hasParticleAccelerator = true;
            }
            else if (bsmPlayer.currentEffectIndex == 4)
            {
                player.GetModPlayer<AITrackingDevicePlayer>().hasAITrackingDevice = true;
            }
            else if (bsmPlayer.currentEffectIndex == 5)
            {
                player.GetModPlayer<EchoBoundaryPlayer>().hasEchoBoundary = true;
            }
            else if (bsmPlayer.currentEffectIndex == 6)
            {
                player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet = true;
            }




        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.PermanentModProj.UltraHighCrit>(), 1); // 暴击戒指（常驻）
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.PermanentModProj.AttackIndicator>(), 1); // 进攻指示器（常驻）
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.PermanentModProj.BreakingDrill>(), 1); // 破阵冲击钻（常驻）
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.PermanentModProj.MomentumChip>(), 1); // 动能芯片（常驻）
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.PermanentModProj.QuickDrawHolster>(), 1); // 快速扳机（常驻）            

            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.GlitchCore>(), 1); // 故障核心-触发故障
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.Universalgravitation>(), 1); // 万有引力-自身旋转
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.ParticleAccelerator>(), 1); // 粒子加速-增加速度与伤害
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.AITrackingDevice> (), 1); // 定位装置-追杀敌人
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.EchoBoundary>(), 1); // 回声边界-屏幕边缘弹射
            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.NebulaBracelet>(), 1); // 星云钓竿-鼠标旋转

            recipe.AddIngredient(ModContent.ItemType<Attack.ModProj.PhaseProjector>(), 1); // 相位投影仪-加强非穿透弹幕

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("MysteriousCircuitry").Type, 10);
                recipe.AddIngredient(calamityMod.Find<ModItem>("DubiousPlating").Type, 10);
                //recipe.AddIngredient(calamityMod.Find<ModItem>("PrototypePlasmaDriveCore").Type, 1);
            }

            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
