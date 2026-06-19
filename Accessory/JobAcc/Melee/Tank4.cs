using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;


namespace AccessoryForPlanetaryEmpire.Accessory.JobAcc.Melee
{
    public class Tank4 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.JobAcc.Melee";
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
            int tank1Type = ModContent.ItemType<Tank1>();
            int tank2Type = ModContent.ItemType<Tank2>();
            int tank3Type = ModContent.ItemType<Tank3>();
            int tank4Type = ModContent.ItemType<Tank4>();

            // 如果尝试装备的是其他相冲突的饰品，返回 false
            if ((equippedItem.type == tank1Type && incomingItem.type == tank2Type) ||
                (equippedItem.type == tank1Type && incomingItem.type == tank3Type) ||
                (equippedItem.type == tank1Type && incomingItem.type == tank4Type) ||
                (equippedItem.type == tank2Type && incomingItem.type == tank1Type) ||
                (equippedItem.type == tank2Type && incomingItem.type == tank3Type) ||
                (equippedItem.type == tank2Type && incomingItem.type == tank4Type) ||
                (equippedItem.type == tank3Type && incomingItem.type == tank1Type) ||
                (equippedItem.type == tank3Type && incomingItem.type == tank2Type) ||
                (equippedItem.type == tank3Type && incomingItem.type == tank4Type) ||
                (equippedItem.type == tank4Type && incomingItem.type == tank1Type) ||
                (equippedItem.type == tank4Type && incomingItem.type == tank2Type) ||
                (equippedItem.type == tank4Type && incomingItem.type == tank3Type))
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.35f;
            player.GetCritChance(DamageClass.Melee) += 30f;
            player.aggro += 5000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Accessory.JobAcc.Melee.Tank3>(), 1);
            recipe.AddIngredient(ItemID.FragmentStardust, 50);
            recipe.AddIngredient(ItemID.FragmentNebula, 50);
            recipe.AddIngredient(ItemID.FragmentVortex, 50);
            recipe.AddIngredient(ItemID.SolarEruption, 1);
            recipe.AddIngredient(ItemID.PaladinsHammer, 1);
            recipe.AddIngredient(ItemID.Terrarian, 1);
            recipe.AddIngredient(ItemID.StarWrath, 1);
            recipe.AddIngredient(ItemID.Meowmere, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
