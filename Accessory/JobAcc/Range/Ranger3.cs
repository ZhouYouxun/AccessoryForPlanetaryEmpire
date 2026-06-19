using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Range
{
    public class Ranger3 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Range";
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
            int ranger1Type = ModContent.ItemType<Ranger1>();
            int ranger2Type = ModContent.ItemType<Ranger2>();
            int ranger3Type = ModContent.ItemType<Ranger3>();
            int ranger4Type = ModContent.ItemType<Ranger4>();

            // 如果尝试装备的是其他相冲突的饰品，返回 false
            if ((equippedItem.type == ranger1Type && incomingItem.type == ranger2Type) ||
                (equippedItem.type == ranger1Type && incomingItem.type == ranger3Type) ||
                (equippedItem.type == ranger1Type && incomingItem.type == ranger4Type) ||
                (equippedItem.type == ranger2Type && incomingItem.type == ranger1Type) ||
                (equippedItem.type == ranger2Type && incomingItem.type == ranger3Type) ||
                (equippedItem.type == ranger2Type && incomingItem.type == ranger4Type) ||
                (equippedItem.type == ranger3Type && incomingItem.type == ranger1Type) ||
                (equippedItem.type == ranger3Type && incomingItem.type == ranger2Type) ||
                (equippedItem.type == ranger3Type && incomingItem.type == ranger4Type) ||
                (equippedItem.type == ranger4Type && incomingItem.type == ranger1Type) ||
                (equippedItem.type == ranger4Type && incomingItem.type == ranger2Type) ||
                (equippedItem.type == ranger4Type && incomingItem.type == ranger3Type))
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.30f;
            player.GetCritChance(DamageClass.Ranged) += 20f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Range.Ranger2>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
            recipe.AddIngredient(ItemID.ChainGun, 1);
            recipe.AddIngredient(ItemID.CandyCornRifle, 1);
            recipe.AddIngredient(ItemID.SniperRifle, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
