using Infinitum.WorldChanges;
using Microsoft.Xna.Framework;

namespace Infinitum.Commands
{
    internal class Reset : ModCommand
    {
        bool firstTime = true;
        public override string Command => "reset-progress";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "Reset ALL INFINITUM CHARACTER PROGRESS";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (firstTime)
            {
                firstTime = false;
                Infinitum.instance.GameMessage($"Please, write another time to RESET ALL YOUR PROGRESS", Color.Blue);
                return;
            }
            firstTime = true;
            Main.player[Main.myPlayer].GetModPlayer<Character_Data>().ResetAllCharacterData();
        }
    }
}
