using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class ArmorPenetration : Skill
    {
        public ArmorPenetration(int level) : base(level)
        {}
        public override void OnInitialize()
        {
            Name = "ArmorPenetration";
            DisplayName = "Armor Penetration";
            StatName = "GetArmorPenetration";
            PreText = '+';
            Cost = 1000;
            EffectBuff = 0;
            MultiplierEffect = 1f;
            MultiplierCost = 0.25f;
            Type = SkillEnums.Type.PostUpdateEquips;
        }
        public override void ApplyStatToPlayer()
        {
            player.GetArmorPenetration(DamageClass.Generic) = player.GetArmorPenetration(DamageClass.Generic) + EffectBuff; 
        }
        public override string GetStatText()
        {
            return $"+ {Level}";
        }
    }
}
