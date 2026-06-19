using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Range
{
    public class Ranger2 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Range";
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
            player.GetDamage(DamageClass.Ranged) += 0.20f;
            player.GetCritChance(DamageClass.Ranged) += 10f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Range.Ranger1>(), 1);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.OnyxBlaster, 1);
            recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
