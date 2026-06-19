using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;


namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Summon
{
    public class Summon3 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Summon";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 世纪之花~教徒：Cyan，价值35金
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 35, 0, 0);
            Item.value = Item.sellPrice(0, 35, 0, 0);

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
            player.GetDamage(DamageClass.Summon) += 0.30f;
            player.maxMinions += 4;
            player.maxTurrets += 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Summon.Summon2>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
            recipe.AddIngredient(ItemID.DeadlySphereStaff, 1);
            recipe.AddIngredient(ItemID.RavenStaff, 1);
            recipe.AddIngredient(ItemID.PygmyStaff, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
