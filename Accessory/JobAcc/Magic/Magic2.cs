using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Magic
{
    public class Magic2 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Magic";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式~世纪之花：Pink，价值25金
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.value = Item.sellPrice(0, 25, 0, 0);

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
            player.GetDamage(DamageClass.Magic) += 0.20f;
            player.GetCritChance(DamageClass.Magic) += 10f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Magic.Magic1>(), 1);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SkyFracture, 1);
            recipe.AddIngredient(ItemID.CrystalVileShard, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
