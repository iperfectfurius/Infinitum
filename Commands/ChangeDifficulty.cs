using Infinitum.WorldChanges;
using Microsoft.Xna.Framework;

namespace Infinitum.Commands
{
    public class ChangeDifficulty : ModCommand
    {
        public override string Command => "difficulty";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "Selects Infinitum [Normal,Hard,T1 or disabled] difficulties";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if(args.Length != 1)
            {
                Main.NewText($"Please, select any difficultys, /difficulty [Normal,Hard,T1 or Disabled].(Currently selected {Infinitum.instance.Difficulty.Difficulty})", Color.Red);
                return;
            }

            string inputDiff = args[0][0].ToString().ToUpper() + args[0].Substring(1).ToLower();

            try{
                Infinitum.instance.Difficulty.ChangeDifficulty((Difficulties)Enum.Parse(typeof(Difficulties), inputDiff));
                Infinitum.instance.GameMessage($"Difficulty {Infinitum.instance.Difficulty.Difficulty} setted.",Color.Blue);
            }
            catch
            {
                Infinitum.instance.GameMessage($"Please, select any difficulty, /difficulty [Normal,Hard,T1 or Disabled]", Color.Red);
            }
        }
    }
}
