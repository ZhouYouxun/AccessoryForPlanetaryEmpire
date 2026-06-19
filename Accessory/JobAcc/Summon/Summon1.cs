using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;


namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Summon
{
    public class Summon1 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Summon";
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
            int summon1Type = ModContent.ItemType<Summon1>();
            int summon2Type = ModContent.ItemType<Summon2>();
            int summon3Type = ModContent.ItemType<Summon3>();
            int summon4Type = ModContent.ItemType<Summon4>();

            // 如果尝试装备的是其他相冲突的饰品，返回 false
            if ((equippedItem.type == summon1Type && incomingItem.type == summon2Type) ||
                (equippedItem.type == summon1Type && incomingItem.type == summon3Type) ||
                (equippedItem.type == summon1Type && incomingItem.type == summon4Type) ||
                (equippedItem.type == summon2Type && incomingItem.type == summon1Type) ||
                (equippedItem.type == summon2Type && incomingItem.type == summon3Type) ||
                (equippedItem.type == summon2Type && incomingItem.type == summon4Type) ||
                (equippedItem.type == summon3Type && incomingItem.type == summon1Type) ||
                (equippedItem.type == summon3Type && incomingItem.type == summon2Type) ||
                (equippedItem.type == summon3Type && incomingItem.type == summon4Type) ||
                (equippedItem.type == summon4Type && incomingItem.type == summon1Type) ||
                (equippedItem.type == summon4Type && incomingItem.type == summon2Type) ||
                (equippedItem.type == summon4Type && incomingItem.type == summon3Type))
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.maxMinions += 2;
            player.maxTurrets += 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
            recipe.AddIngredient(ItemID.FallenStar, 30);
            recipe.AddIngredient(ItemID.AshBlock, 200);
            recipe.AddIngredient(ItemID.SlimeStaff, 1);
            recipe.AddIngredient(ItemID.Bone, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
