using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Potion
{
    public class ElectronicPsychedelicDrugs : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
                new Color(240, 240, 240),
                new Color(200, 200, 200),
                new Color(140, 140, 140)
            };
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 1);
            Item.buffType = ModContent.BuffType<Buffs.ElectronicPsychedelicDrugsBuff>();
            Item.buffTime = 160000;
        }


        // 添加配方
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Moonglow, 1)  // 月光草，反映迷幻的主题
                .AddIngredient(ItemID.Deathweed, 1) // 死亡草，象征着药水的强大效果
                .AddIngredient(ItemID.Waterleaf, 1) // 水叶草，代表液体药剂的基底
                .AddIngredient(ItemID.Waterleaf, 1) // 水叶草，代表液体药剂的基底
                .AddIngredient(ItemID.SoulofLight, 1) 
                .AddIngredient(ItemID.BottledWater, 1) // 瓶装水，药水的基础
                .AddTile(TileID.Bottles) // 配方需要在瓶子旁制作
                .Register();
        }


    }
}