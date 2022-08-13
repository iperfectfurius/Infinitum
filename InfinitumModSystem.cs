using Infinitum.Items.Ores;
using Infinitum.UI;
using Infinitum.WorldBuilding.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;
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
            if (!Main.gameMenu)
            {
                customUIBar.Update(gameTime);
            }
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
                    "InfinitumUI: Skill UI",
                    delegate
                    {
                        customUIBar?.Draw(Main.spriteBatch, new GameTime());
                        if (InfinitumUI.Instance.Visible)
                        {
                            customUI.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

            if (ShiniesIndex != -1)
            {
                // Next, we insert our pass directly after the original "Shinies" pass.
                // ExampleOrePass is a class seen bellow
                tasks.Insert(ShiniesIndex + 1, new SanjacobosOrePass("Sanjacobo's ore", 237.4298f));
            }

        }
        internal class SanjacobosOrePass : GenPass
        {
            public SanjacobosOrePass(string name, float loadWeight) : base(name, loadWeight)
            {
                
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                // progress.Message is the message shown to the user while the following code is running.
                // Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
                progress.Message = "Sanjacobo things";
                
                // Ores are quite simple, we simply use a for loop and the WorldGen.TileRunner to place splotches of the specified Tile in the world.
                // "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.00008); k++)
                {
                    // The inside of this for loop corresponds to one single splotch of our Ore.
                    // First, we randomly choose any coordinate in the world by choosing a random x and y value.
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    

                    // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.
                    int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);

                    // Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place.
                    // Feel free to experiment with strength and step to see the shape they generate.
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(12, 20), WorldGen.genRand.Next(8, 14), ModContent.TileType<SanjacobosMineralTile>());

                    // Alternately, we could check the tile already present in the coordinate we are interested.
                    // Wrapping WorldGen.TileRunner in the following condition would make the ore only generate in Snow.
                    // Tile tile = Framing.GetTileSafely(x, y);
                    // if (tile.active() && tile.type == TileID.SnowBlock)
                    // {
                    // 	WorldGen.TileRunner(.....);
                    // }
                }
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
