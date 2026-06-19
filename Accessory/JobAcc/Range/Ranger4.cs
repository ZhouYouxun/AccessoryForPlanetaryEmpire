using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Range
{
    public class Ranger4 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Range";
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
            player.GetDamage(DamageClass.Ranged) += 0.35f;
            player.GetCritChance(DamageClass.Ranged) += 30f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Range.Ranger3>(), 1);
            recipe.AddIngredient(ItemID.FragmentSolar, 50);
            recipe.AddIngredient(ItemID.FragmentNebula, 50);
            recipe.AddIngredient(ItemID.FragmentStardust, 50);
            recipe.AddIngredient(ItemID.VortexBeater, 1);
            recipe.AddIngredient(ItemID.Phantasm, 1);
            recipe.AddIngredient(ItemID.Celeb2, 2);
            recipe.AddIngredient(ItemID.SDMG, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
