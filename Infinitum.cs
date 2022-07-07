using Terraria.ModLoader;
using Terraria;
using System.Threading.Tasks;
using System.IO;
using Infinitum.UI;
using Terraria.UI;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infinitum
{
	public class Infinitum : Mod
	{
		public UserInterface customUI;
		internal InfinitumUI infinitumUI;
		public Infinitum()
		{

		}
		public override void Load()
		{
			base.Load();		

			if (!Main.dedServ)
			{
				customUI = new UserInterface();
				infinitumUI = new InfinitumUI();
				infinitumUI.Initialize();
				//infinitumUI.Visible = true;//static??
				customUI.SetState(infinitumUI);

			}

		}
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            base.HandlePacket(reader, whoAmI);
			AddXPToPlayer(reader.ReadSingle());

		}
		private void AddXPToPlayer(float xp)
        {
			Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(xp);
        }

		/*public override void UpdateUI(GameTime gameTime)
		{
			// it will only draw if the player is not on the main menu
			if (!Main.gameMenu
				&& infinitumUI.Visible)
			{
				infinitumUI?.Update(gameTime);
			}
		}*/
	}
}