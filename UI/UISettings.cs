using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Infinitum.UI
{
    internal class UISettings
    {
        private Vector2 expBarPos = new Vector2(ExpBarUI.DefaultPos.X, ExpBarUI.DefaultPos.Y);

        public Vector2 ExpBarPos { get => expBarPos; set => expBarPos = value; }
        public void SetSettings()
        {
            SetLastPostExpBar();
        }
        private bool SetLastPostExpBar()
        {
            ExpBarUI.Instance.ChangeOwnPos(expBarPos);
            return true;
        }
        public void loadMyData(TagCompound UI)
        {//check for empty
            TagCompound expBar = UI.GetCompound("ExpBar");
            if (expBar.Count == 0) return;

            Vector2 lastExpBarPos = new Vector2(expBar.GetFloat("X"), expBar.GetFloat("Y"));

            expBarPos = lastExpBarPos;
        }
        public TagCompound SaveMyData()
        {
            TagCompound UI = new();
            TagCompound expBar = new();

            Vector2 expBarPos = ExpBarUI.Instance.GetCurrentPos();

            expBar.Add("X", expBarPos.X);
            expBar.Add("Y", expBarPos.Y);

            UI.Add("ExpBar", expBar);

            return UI;
        }
    }
}
