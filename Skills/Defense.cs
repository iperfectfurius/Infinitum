using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class Defense : Skill
    {

        public Defense(string name, string statName) : base(name, statName)
        {
            
        }

        public override void LevelUp(ref int Levels)
        {
            throw new NotImplementedException();
            
        }

        public override void OnInitialize()
        {
            //this.Level = 0;
            //effectBuff = 0f;
            //cost = 0;
            //multiplierCost = 0;
            //maxLevel = 999999;
            
        }
    }
}
