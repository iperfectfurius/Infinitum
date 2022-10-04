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

namespace Infinitum
{
	public class InfinitumNPCs : GlobalNPC
	{
		private static Mod myMod = Infinitum.myMod;
		public static InfinitumNPCs instance;
        //TODO Rework xp scalate
        private float GetDefense(int defense) => defense > 120 ? 120 : defense;
        private float GetXpFromNPC(NPC target) => ((GetDefense(target.defense) * 0.025f) + 0.5f) * (float)(target.lifeMax / 5.5);
		
		public override void OnKill(NPC npc)
		{			
			float xp = GetXpFromNPC(npc);

			if (Main.netMode == NetmodeID.Server)
			{
				//This helps in performance??
				Task.Run(() =>
                {
					ModPacket myPacket = myMod.GetPacket();
					myPacket.Write(xp);
					myPacket.Send();
				});
				
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				
				addXpToPlayer(xp);

			}
			base.OnKill(npc);
		}
        public static void ChatMessage(string text)
		{
			
			if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Red);
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(text);
			}
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{						
			base.ModifyNPCLoot(npc, npcLoot);
		}
		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			
			System.Collections.Generic.List<IItemDropRule> items = new System.Collections.Generic.List<IItemDropRule>();
			items.Add(new DropBasedOnExpertMode(
				new CommonDrop(ModContent.ItemType<ExpStar>(), ExpStar.NormalChanceFromNPCs, 1), 
				new CommonDrop(ModContent.ItemType<ExpStar>(), ExpStar.ExpertChanceFromNPCs, 1)));
			items.Add(new DropBasedOnExpertMode(
				new CommonDrop(ModContent.ItemType<MultiplierStar>(),MultiplierStar.NormalChanceFromNPCS, 1, 3),
				new CommonDrop(ModContent.ItemType<MultiplierStar>(), MultiplierStar.ExpertChanceFromNPCS, 1, 3)));
			items.Add(new ItemDropWithConditionRule(ModContent.ItemType<SuperiorMultiplierStar>(), SuperiorMultiplierStar.ChanceFromNPCS, 1, 1, new Conditions.IsHardmode()));

			items.ForEach(e => globalLoot.Add(e));
		}
        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            base.SetBestiary(npc, database, bestiaryEntry);
        }
        private void addXpToPlayer(float xp)
		{
			Infinitum.instance.AddXPToPlayer(xp);
		}
        public override void OnSpawn(NPC npc, IEntitySource source)
        {			
			//base.OnSpawn(npc, source);
        }
        public override void SetDefaults(NPC npc)
        {
			base.SetDefaults(npc);
			npc.life += (int)(npc.life * Infinitum.instance.Difficulty.Hp);
			npc.lifeMax += (int)(npc.lifeMax * Infinitum.instance.Difficulty.Hp);
			npc.damage += (int)(npc.damage * Infinitum.instance.Difficulty.Damage);
			npc.defense += (int)(npc.defense * Infinitum.instance.Difficulty.Defense);

			
			
        }
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {

            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<MultiplierStar>());
                shop.item[nextSlot].value = 0;
                shop.item[nextSlot].shopCustomPrice = Item.sellPrice(platinum: 2);

                nextSlot++;


            }
            //base.SetupShop(type, shop, ref nextSlot);
        }
		

    }
}