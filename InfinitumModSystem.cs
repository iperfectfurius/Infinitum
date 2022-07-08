using Infinitum.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
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
        public static ModKeybind UIKey;
        public override void Load()
        {
           // base.Load();

            if (!Main.dedServ)
            {
                UIKey = KeybindLoader.RegisterKeybind(Mod, "Show UI", Keys.L);
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
            
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "YourMod: A Description",
                    delegate
                    {
                        if(InfinitumUI.Instance.Visible)
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
        public void ShowMyUI()
        {
            customUI?.SetState(infinitumUI);
        }

        public void HideMyUI()
        {
            customUI?.SetState(null);
        }
        
    }
}
