using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;


namespace AccessoryForPlanetaryEmpire.Accessory.Others
{
    public class LiquidAbsorbentShield : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Others";
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

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            int shield1Type = ModContent.ItemType<LiquidAbsorbentShield>();
            int shield2Type = ModContent.ItemType<LiquidAbsorbentShield2>();

            // 尝试装备的是其他相冲突的饰品，返回 false
            if ((equippedItem.type == shield1Type && incomingItem.type == shield2Type) ||
                (equippedItem.type == shield2Type && incomingItem.type == shield1Type))
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }



        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 综合属性提升
            player.GetDamage(DamageClass.Generic) += 0.1f; // 所有职业的攻击力
            player.GetAttackSpeed(DamageClass.Generic) += 0.1f; // 所有职业的攻击速度

            player.statDefense += (int)(player.statDefense * 0.05f); // 防御力提升 
            player.lifeRegen += 5; // 生命回复
            player.moveSpeed += 0.2f; // 移动速度
            player.endurance += 0.1f; // 伤害减免

            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.1f); // 生命上限
            player.statManaMax2 += (int)(player.statManaMax2 * 0.25f); // 魔力上限
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Y_TestAcc.TheItem>(), 1);
            recipe.AddIngredient(ItemID.RoyalGel, 1);
            recipe.AddIngredient(ItemID.EoCShield, 1);
            recipe.AddIngredient(ItemID.WormScarf, 1);
            recipe.AddIngredient(ItemID.BrainOfConfusion, 1);
            recipe.AddIngredient(ItemID.HiveBackpack, 1);
            recipe.AddIngredient(ItemID.BoneGlove, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

    }
}