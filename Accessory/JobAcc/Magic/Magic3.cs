using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Magic
{
    public class Magic3 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Magic";
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
            int magic1Type = ModContent.ItemType<Magic1>();
            int magic2Type = ModContent.ItemType<Magic2>();
            int magic3Type = ModContent.ItemType<Magic3>();
            int magic4Type = ModContent.ItemType<Magic4>();

            // 如果尝试装备的是其他相冲突的饰品，返回 false
            if ((equippedItem.type == magic1Type && incomingItem.type == magic2Type) ||
                (equippedItem.type == magic1Type && incomingItem.type == magic3Type) ||
                (equippedItem.type == magic1Type && incomingItem.type == magic4Type) ||
                (equippedItem.type == magic2Type && incomingItem.type == magic1Type) ||
                (equippedItem.type == magic2Type && incomingItem.type == magic3Type) ||
                (equippedItem.type == magic2Type && incomingItem.type == magic4Type) ||
                (equippedItem.type == magic3Type && incomingItem.type == magic1Type) ||
                (equippedItem.type == magic3Type && incomingItem.type == magic2Type) ||
                (equippedItem.type == magic3Type && incomingItem.type == magic4Type) ||
                (equippedItem.type == magic4Type && incomingItem.type == magic1Type) ||
                (equippedItem.type == magic4Type && incomingItem.type == magic2Type) ||
                (equippedItem.type == magic4Type && incomingItem.type == magic3Type))
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.3f;
            player.GetCritChance(DamageClass.Magic) += 20f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Magic.Magic2>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
            recipe.AddIngredient(ItemID.LeafBlower, 1);
            recipe.AddIngredient(ItemID.ShadowbeamStaff, 1);
            recipe.AddIngredient(ItemID.LaserMachinegun, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
