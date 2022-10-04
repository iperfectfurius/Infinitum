using IL.Terraria.Utilities.Terraria.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Infinitum.WorldChanges
{
    internal class AdaptativeDifficulty
    {
        private float hp;
        private float speed;
        private float defense;
        private float damage;
        private Difficulties difficulty;
        private List<Boss> bosses;

        public float Hp { get => hp; set => hp = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Defense { get => defense; set => defense = value; }
        public float Damage { get => damage; set => damage = value; }
        public Difficulties Difficulty { get => difficulty; set => difficulty = value; }
        internal List<Boss> Bosses { get => bosses; set => bosses = value; }

        public AdaptativeDifficulty(TagCompound data)
        {
            SetDefaults();
            SetDifficultyOnLoad(data);
        }

        private void SetDefaults()
        {
            hp = 1f;
            speed = 1f;
            defense = 1f;
            damage = 1f;
            difficulty = Difficulties.Normal;
        }

        private void SetDifficultyOnLoad(TagCompound data)
        {
            
        }
    }
}
