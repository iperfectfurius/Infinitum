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
        private UserInterface customUIBar;
        internal InfinitumUI infinitumUI;
        internal ExpBarUI expBarUI;
        private GameTime _lastUpdateUiGameTime;
        public static ModKeybind UIKey;
        public static ModKeybind NumbersDisplay;
        public override void Load()
        {
           // base.Load();

            if (!Main.dedServ)
            {
                UIKey = KeybindLoader.RegisterKeybind(Mod, "Show UI", Keys.L);
                NumbersDisplay = KeybindLoader.RegisterKeybind(Mod, "Hide Numbers", Keys.P);
                customUI = new UserInterface();
                customUIBar = new UserInterface();

                infinitumUI = new InfinitumUI();
                expBarUI = new ExpBarUI();

                infinitumUI.Initialize();
                expBarUI.Initialize();

                //infinitumUI.Visible = true;//static??
                customUI.SetState(infinitumUI);
                customUIBar.SetState(expBarUI);

            }

        }
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (!Main.gameMenu && infinitumUI.Visible)
            {              
                customUI?.Update(gameTime);
            }
            if (!Main.gameMenu)
            {
                customUIBar.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {         
            
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "InfinitumUI: Skill UI",
                    delegate
                    {
                        if (InfinitumUI.Instance.Visible)
                        {
                            customUI.Draw(Main.spriteBatch, new GameTime());
                        }
                            
                        customUIBar?.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
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
