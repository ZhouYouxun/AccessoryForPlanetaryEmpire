using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Magic
{
    public class Magic4 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Magic";
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
            player.GetDamage(DamageClass.Magic) += 0.35f;
            player.GetCritChance(DamageClass.Magic) += 30f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Magic.Magic3>(), 1);
            recipe.AddIngredient(ItemID.FragmentSolar, 50);
            recipe.AddIngredient(ItemID.FragmentStardust, 50);
            recipe.AddIngredient(ItemID.FragmentVortex, 50);
            recipe.AddIngredient(ItemID.NebulaBlaze, 1);
            recipe.AddIngredient(ItemID.NebulaArcanum, 1);
            recipe.AddIngredient(ItemID.LastPrism, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
