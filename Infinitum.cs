using Terraria.ModLoader;
using Terraria;
using System.Threading.Tasks;
using System.Drawing;

namespace Infinitum
{
	public class Infinitum : Mod
	{
		public Infinitum()
		{

		}
		public override void Load()
		{
			base.Load();
			
		}
		public static void ChatMessage(string text, Color color)
		{
			
			// if (Main.netMode == NetmodeID.Server)
			// {
			// 	ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
			// }
			// else if (Main.netMode == NetmodeID.SinglePlayer)
			// {
			// 	Main.NewText(text, color);
			// }
		}
	}
}