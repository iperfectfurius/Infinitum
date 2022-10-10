using Infinitum.WorldChanges;
using Microsoft.Xna.Framework;

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
                Infinitum.instance.GameMessage($"Please, select any difficultys, /difficulty [Normal,Hard,T1 or Disabled].(Currently selected {Infinitum.instance.Difficulty.Difficulty})", Color.Red);
                return;
            }

            try
            {
                Difficulties difficulty = (Difficulties)Enum.Parse(typeof(Difficulties),args[0], true);

                Infinitum.instance.Difficulty.ChangeDifficulty(difficulty);
                Infinitum.instance.GameMessage($"Difficulty {Infinitum.instance.Difficulty.Difficulty} setted.", Color.Blue);
            }
            catch (ArgumentException)
            {
                Infinitum.instance.GameMessage($"Please, select any difficulty, /difficulty [Normal,Hard,T1 or Disabled]", Color.Red);
            }
        }
    }
}
