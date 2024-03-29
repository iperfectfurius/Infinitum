using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;
using Infinitum.Items;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using System.Threading.Tasks;
using System.Collections.Generic;
using Infinitum.Buffs;
using Infinitum.Items.Ores;

namespace Infinitum
{
    public class InfinitumNPCs : GlobalNPC
    {
        private static Mod myMod = Infinitum.myMod;
        private static int LastSpawnRate = 0;
        //TODO Rework xp scalate
        private float GetDefense(int defense) => defense > 120 ? 120 : defense;
        private float GetXpFromNPC(NPC target) => ((GetDefense(target.defense) * 0.025f) + 0.5f) * (float)(target.lifeMax / 5.5);

        public override void OnKill(NPC npc)
        {
            if(npc.friendly) return;
            Infinitum.instance.Difficulty.CheckBossPlaythrough(npc);

            float xp = GetXpFromNPC(npc);

            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket myPacket = myMod.GetPacket();
                myPacket.Write((byte)MessageType.XPFromNPCs);
                myPacket.Write(xp);
                myPacket.Send();
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                addXpToPlayer(xp);
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npc, npcLoot);
        }
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            List<IItemDropRule> items = new List<IItemDropRule>();
            items.Add(new DropBasedOnExpertMode(
                new CommonDrop(ModContent.ItemType<ExpStar>(), ExpStar.NormalChanceFromNPCs, 1),
                new CommonDrop(ModContent.ItemType<ExpStar>(), ExpStar.ExpertChanceFromNPCs, 1)));
            items.Add(new DropBasedOnExpertMode(
                new CommonDrop(ModContent.ItemType<MultiplierStar>(), MultiplierStar.NormalChanceFromNPCS, 1, 3),
                new CommonDrop(ModContent.ItemType<MultiplierStar>(), MultiplierStar.ExpertChanceFromNPCS, 1, 3)));
            items.Add(new ItemDropWithConditionRule(ModContent.ItemType<SuperiorMultiplierStar>(), SuperiorMultiplierStar.ChanceFromNPCS, 1, 1, new Conditions.IsHardmode()));
            items.Add(new ItemDropWithConditionRule(ModContent.ItemType<SanjacobosOre>(), SanjacobosOre.ChanceFromNPCs, 1, 5, new Conditions.DownedPlantera()));
            items.ForEach(e => globalLoot.Add(e));
        }
        private void addXpToPlayer(float xp)
        {
            Infinitum.instance.AddXPToPlayer(xp,MessageType.XPFromNPCs);
        }
        public override void SetDefaults(NPC npc)
        {
            if (npc.friendly || npc.CountsAsACritter) return;
            npc.life += (int)(npc.life * Infinitum.instance.Difficulty.Hp);
            npc.lifeMax += (int)(npc.lifeMax * Infinitum.instance.Difficulty.Hp);
            npc.damage += (int)(npc.damage * Infinitum.instance.Difficulty.Damage);
            npc.defense += (int)(npc.defense * Infinitum.instance.Difficulty.Defense);            
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(new Item(ModContent.ItemType<MultiplierStar>()) { shopCustomPrice = Item.buyPrice(platinum:2)});
                
                //shop.item[nextSlot].SetDefaults(ModContent.ItemType<MultiplierStar>());
                //shop.item[nextSlot].value = 0;
                //shop.item[nextSlot].shopCustomPrice = Item.sellPrice(platinum: 2);

                //nextSlot++;
            }
            //base.SetupShop(type, shop, ref nextSlot);
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (!player.HasBuff<InfinitumBuff>()) return;

                spawnRate = (int)(spawnRate * 0.65);
                maxSpawns = (int)((float)maxSpawns * 10.0f);
        }
    }
}