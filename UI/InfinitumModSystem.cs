using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Infinitum.UI
{
    internal class InfinitumModSystem : ModSystem
    {
        private UserInterface customUI;
        internal InfinitumUI infinitumUI;

        public override void Load()
        {
            base.Load();

            if (!Main.dedServ)
            {
                customUI = new UserInterface();
                infinitumUI = new InfinitumUI();
                infinitumUI.Activate();
                //infinitumUI.Visible = true;//static??
                customUI.SetState(infinitumUI);

            }

        }
        public override void UpdateUI(GameTime gameTime)
        {
 
            if (!Main.gameMenu && infinitumUI.Visible)
            {
                infinitumUI?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new LegacyGameInterfaceLayer("Cool Mod: Something UI", DrawSomethingUI, InterfaceScaleType.UI));
        }
        private bool DrawSomethingUI()
        {
            // it will only draw if the player is not on the main menu
            if (!Main.gameMenu
                && infinitumUI.Visible)
            {
                customUI.Draw(Main.spriteBatch, new GameTime());
            }
            return true;
        }
    }
}
