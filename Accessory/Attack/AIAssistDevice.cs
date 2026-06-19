using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Accessory.Attack
{
    public class AIAssistDevice : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Attack";

        private bool aiAssist;

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 困难模式前：Orange，价值5金
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.value = Item.sellPrice(0, 20, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AIAssistPlayer>().assistActive = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Y_TestAcc.TheItem>(), 1);
            recipe.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 15); // 黄金
            recipe.AddIngredient(ItemID.Wire, 10); // 电线
            recipe.AddIngredient(ItemID.LargeDiamond, 1); // 大钻石
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}



public class AIAssistGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    public bool forceNoTileCollide = false;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        // 如果这个弹幕来自拥有该饰品的玩家，就立刻设为穿墙
        if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].GetModPlayer<AIAssistPlayer>().assistActive)
        {
            projectile.tileCollide = false;
            forceNoTileCollide = true;
        }
    }

    public override void AI(Projectile projectile)
    {
        // 保证穿墙属性不会被其他 mod 重置掉
        if (forceNoTileCollide)
            projectile.tileCollide = false;
    }
}


public class AIAssistPlayer : ModPlayer
{
    public bool assistActive;

    public override void ResetEffects()
    {
        assistActive = false;
    }
}
