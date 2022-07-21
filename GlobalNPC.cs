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
		private static Mod myMod = ModLoader.GetMod("Infinitum");
		private float GetXpFromNPC(NPC target) => (float)target.defense + 0.5f * (float)(target.lifeMax / 4.5);

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
			items.Add(new DropBasedOnExpertMode(new CommonDrop(ModContent.ItemType<ExpStar>(), 1500, 1), new CommonDrop(ModContent.ItemType<ExpStar>(), 1250, 1)));
			items.Add(new DropBasedOnExpertMode(new CommonDrop(ModContent.ItemType<MultiplierStar>(), 125, 1, 3), new CommonDrop(ModContent.ItemType<MultiplierStar>(), 100, 1, 3)));
			items.Add(new ItemDropWithConditionRule(ModContent.ItemType<SuperiorMultiplierStar>(), 500, 1, 1, new Conditions.IsHardmode()));

			items.ForEach(e => globalLoot.Add(e));
		}
        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            base.SetBestiary(npc, database, bestiaryEntry);
        }
        private void addXpToPlayer(float xp)
		{
			Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp);
		}
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
			
			
			//base.OnSpawn(npc, source);
        }
        public override void SetDefaults(NPC npc)
        {
			base.SetDefaults(npc);
			npc.life += (int)(npc.life * 0.25f);
			npc.lifeMax += (int)(npc.lifeMax * 0.25f);
			npc.damage += (int)(npc.damage * 0.10f);
			npc.defense += (int)(npc.defense * 0.10f);
			
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