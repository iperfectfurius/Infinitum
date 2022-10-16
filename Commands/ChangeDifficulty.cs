using Infinitum.WorldChanges;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Infinitum.Commands
{
    public class ChangeDifficulty : ModCommand
    {
        public override string Command => "difficulty";
        public override CommandType Type => CommandType.World;
        public override string Description => "Selects Infinitum [Normal,Hard,T1 or disabled] difficulties";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length != 1)
            {
                Infinitum.instance.GameMessage($"Please, select any difficultys, /difficulty [Normal,Hard,T1 or Disabled].(Currently selected {Infinitum.instance.Difficulty.DifficultySetted})", Color.Red);
                return;
            }

            try
            {
                Difficulties difficulty = (Difficulties)Enum.Parse(typeof(Difficulties), args[0], true);
                Infinitum.instance.Difficulty.ChangeDifficulty(difficulty);
                Infinitum.instance.GameMessage($"Difficulty {Infinitum.instance.Difficulty.DifficultySetted} setted.", Color.Blue);

                if (Main.netMode != NetmodeID.Server) return;

                ModPacket myPacket = ModLoader.GetMod("Infinitum").GetPacket();

                myPacket.Write((byte)MessageType.ChangeDifficulty);
                myPacket.Write((byte)difficulty);
                myPacket.Write(Infinitum.instance.Difficulty.Hp);
                myPacket.Write(Infinitum.instance.Difficulty.Speed);
                myPacket.Write(Infinitum.instance.Difficulty.Defense);
                myPacket.Write(Infinitum.instance.Difficulty.Damage);
                myPacket.Send();



            }
            catch (ArgumentException)
            {
                Infinitum.instance.GameMessage($"Please, select any difficulty, /difficulty [Normal,Hard,T1 or Disabled]", Color.Red);
            }
        }
    }
}
