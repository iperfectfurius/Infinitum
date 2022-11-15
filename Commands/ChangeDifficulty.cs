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
            if (args.Length == 0 || args.Length > 2)
            {
                Infinitum.instance.GameMessage($"Please, select any difficulties, /difficulty [Normal,Hard,T1 or Disabled] " +
                    $"[(optionally) PreHardMode,HardMode,PostPlantera,PostGolem].(Currently selected {Infinitum.instance.Difficulty.DifficultySetted}, {Infinitum.instance.Difficulty.BestBossTypeBeated})" +
                    $"\nCurrent Monsters Stats: +{(int)(Infinitum.instance.Difficulty.Hp * 100)}% HP, +{(int)(Infinitum.instance.Difficulty.Speed)}% Speed," +
                    $" +{(int)(Infinitum.instance.Difficulty.Defense * 100)}% Defense, +{(int)(Infinitum.instance.Difficulty.Damage * 100)}% Damage" +
                    $"\nXP Multiplier +{(Infinitum.instance.Difficulty.GetXPFromDifficulty - 1.0f) * 100:n2}%", Color.Red);
                return;
            }
            try
            {
                Difficulties difficulty = (Difficulties)Enum.Parse(typeof(Difficulties), args[0], true);
                Boss.BossType progress;
                //temp
                if ((int)difficulty > 2 && difficulty != Difficulties.Disabled) throw new Exception("Error: Invalid difficulty ID");

                if (args.Length > 1)
                {
                    progress = (Boss.BossType)Enum.Parse(typeof(Boss.BossType), args[1], true);
                    if ((int)progress > Enum.GetNames(typeof(Boss.BossType)).Length) throw new Exception("Error: Invalid progress/step ID");

                    Infinitum.instance.Difficulty.ChangeStepAndDifficulty(difficulty, progress);
                }
                else
                    Infinitum.instance.Difficulty.ChangeDifficulty(difficulty);

                Infinitum.instance.GameMessage($"Difficulty {Infinitum.instance.Difficulty.DifficultySetted} setted, Progress: {Infinitum.instance.Difficulty.BestBossTypeBeated}." +
                    $"\nCurrents Monsters Stats: +{(int)(Infinitum.instance.Difficulty.Hp * 100)}% HP, +{(int)(Infinitum.instance.Difficulty.Speed)}% Speed," +
                    $" +{(int)(Infinitum.instance.Difficulty.Defense * 100)}% Defense, +{(int)(Infinitum.instance.Difficulty.Damage * 100)}% Damage" +
                    $"\nXP Multiplier +{(Infinitum.instance.Difficulty.GetXPFromDifficulty - 1.0f) * 100:n2}%", Color.Blue);
            }
            catch (Exception)
            {
                Infinitum.instance.GameMessage($"Please, select any difficulty, /difficulty [Normal,Hard,T1 or Disabled]", Color.Red);
            }
        }
    }
}
