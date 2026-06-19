using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Magic
{
    public class Magic1 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Magic";

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
        //public override bool CanEquipAccessory(Player player, int slot, bool modded)
        //{
        //    return base.CanEquipAccessory(player, slot, modded);
        //}

        //public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        //{
        //    return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        //}


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
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.GetCritChance(DamageClass.Magic) += 5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FallenStar, 15);
            recipe.AddIngredient(ItemID.Mushroom, 30);
            recipe.AddIngredient(ItemID.Cloud, 50);
            recipe.AddIngredient(ItemID.WandofSparking, 1);
            recipe.AddIngredient(ItemID.Bone, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
