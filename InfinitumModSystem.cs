using Infinitum.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace Infinitum
{
    internal class InfinitumModSystem : ModSystem
    {
        private UserInterface customUI;
        internal InfinitumUI infinitumUI;
        private GameTime _lastUpdateUiGameTime;

        public override void Load()
        {
           // base.Load();

            if (!Main.dedServ)
            {
                customUI = new UserInterface();
                infinitumUI = new InfinitumUI();
                infinitumUI.Initialize();
                //infinitumUI.Visible = true;//static??
                customUI.SetState(infinitumUI);

            }

        }
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (!Main.gameMenu && infinitumUI.Visible)
            {
                customUI?.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //layers.Add(new LegacyGameInterfaceLayer("Cool Mod: Something UI", DrawSomethingUI, InterfaceScaleType.UI));

            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        customUI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
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
        internal void ShowMyUI()
        {
            customUI?.SetState(infinitumUI);
        }

        internal void HideMyUI()
        {
            customUI?.SetState(null);
        }
        
    }
}
