using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;
using System;


namespace AccessoryForPlanetaryEmpire.Accessory.Others
{
    public class LiquidAbsorbentShield2 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Others";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 教徒后：Red，价值55金
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 55, 0, 0);
            Item.value = Item.sellPrice(0, 55, 0, 0);

        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            int shield1Type = ModContent.ItemType<LiquidAbsorbentShield>();
            int shield2Type = ModContent.ItemType<LiquidAbsorbentShield2>();

            // 如果尝试装备的是其他相冲突的饰品，返回 false
            if ((equippedItem.type == shield1Type && incomingItem.type == shield2Type) ||
                (equippedItem.type == shield2Type && incomingItem.type == shield1Type))
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetAttackSpeed(DamageClass.Melee) += 0.9f; // 所有职业的攻击速度


            // 综合属性提升
            player.GetDamage(DamageClass.Generic) += 0.2f; // 所有职业的攻击力
            player.GetAttackSpeed(DamageClass.Generic) += 0.2f; // 所有职业的攻击速度

            player.statDefense += (int)(player.statDefense * 0.1f); // 防御力提升 
            player.lifeRegen += 10; // 生命回复
            player.moveSpeed += 0.25f; // 移动速度
            player.endurance += 0.15f; // 伤害减免

            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.15f); // 生命上限
            player.statManaMax2 += (int)(player.statManaMax2 * 0.35f); // 魔力上限
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.Others.LiquidAbsorbentShield>(), 1);
            recipe.AddIngredient(ItemID.VolatileGelatin, 1);
            recipe.AddIngredient(ItemID.MechanicalWheelPiece, 1); 
            recipe.AddIngredient(ItemID.MechanicalWagonPiece, 1);
            recipe.AddIngredient(ItemID.MechanicalBatteryPiece, 1);
            recipe.AddIngredient(ItemID.SporeSac, 1);
            recipe.AddIngredient(ItemID.ShinyStone, 1);
            recipe.AddIngredient(ItemID.EmpressFlightBooster, 1);
            recipe.AddIngredient(ItemID.ShrimpyTruffle, 1);
            recipe.AddIngredient(ItemID.GravityGlobe, 1);
            recipe.AddTile(TileID.LunarCraftingStation); 
            recipe.Register();
        }

    }
}