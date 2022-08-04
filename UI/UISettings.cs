using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.UI
{
    internal class UISettings
    {
        private Vector2 expBarPos;

        public Vector2 ExpBarPos { get => expBarPos; set => expBarPos = value; }

        public void SetSettings()
        {
            SetLastPostExpBar();
        }
        private bool SetLastPostExpBar()
        {
            ExpBarUI.Instance.SetLastPos(expBarPos);
            return true;
        }
    }
}
