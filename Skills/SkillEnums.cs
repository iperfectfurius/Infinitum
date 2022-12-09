using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    public class SkillEnums
    {
        public static int GetNumberOfSkills = Enum.GetNames(typeof(SkillOrder)).Length;
        public static string[] FullSkillNames = new string[GetNumberOfSkills];
        public enum Actions
        {
            LevelUp,
            LevelDown,
            LevelUpAll
        }
        public enum SkillOrder : ushort
        {
            Defense,
            MovementSpeed,
            LifeRegen,
            LifeSteal,
            GlobalCriticalChance,
            ArmorPenetration,
            MeleeDamage,
            MeleeAttackSpeed,
            MagicDamage,
            ManaConsumption,
            RangedDamage,
            AmmoConsumption,
            SummonDamage,
            MinionCapacity,
            PickaxeSpeed 
        }     
        public enum Type
        {
            PostUpdateEquips,
            ModifyHitNPC,
            CanConsumeAmmo
        }
    }
}
