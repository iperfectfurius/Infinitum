using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class SkillEnums
    {
        public enum Actions : ushort
        {
            LevelUp = 0,
            LevelDown = 1,
            LevelUpAll = 2
        }
        public enum SkillOrder : ushort
        {
            Defense = 0,
            MovementSpeed = 1,
            LifeRegen = 2,
            LifeSteal = 3,
            GlobalCriticalChance = 4,
            MeleeDamage = 5,
            MeleeAttackSpeed = 6,
            MagicDamage = 7,
            ManaConsumption = 8,
            RangedDamage = 9,
            AmmoConsumption = 10,
            SummonDamage = 11,
            MinionCapacity = 12,
            PickaxeSpeed = 13
        }

        public static int GetNumberOfSkills = Enum.GetNames(typeof(SkillOrder)).Length;
        public enum Type : ushort
        {
            PostUpdateEquips = 0,
            ModifyHitNPC = 1,
            CanConsumeAmmo = 2
        }     
    }
}
