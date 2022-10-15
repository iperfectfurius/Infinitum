using IL.Terraria.GameContent.Generation;
using Infinitum.Items;
using Infinitum.Items.Ores;
using Infinitum.UI;
using Infinitum.WorldBuilding.Tiles;
using Infinitum.WorldChanges;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NVorbis.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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
        public static ModKeybind ChangeSet;
        public override void Load()
        {

            if (!Main.dedServ)
            {
                UIKey = KeybindLoader.RegisterKeybind(Mod, "Show UI", Keys.L);
                NumbersDisplay = KeybindLoader.RegisterKeybind(Mod, "Hide Numbers", Keys.P);
                ChangeSet = KeybindLoader.RegisterKeybind(Mod, "Change Set", Keys.C);

                customUI = new UserInterface();
                customUIBar = new UserInterface();

                infinitumUI = new InfinitumUI();
                expBarUI = new ExpBarUI();

                infinitumUI.Initialize();
                expBarUI.Initialize();

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
        public override void OnWorldLoad()
        {   
            base.OnWorldLoad();
        }
        public override void LoadWorldData(TagCompound tag)
        {
            Infinitum.instance.Difficulty.ChangeDifficulty(Difficulties.Normal);
            base.LoadWorldData(tag);
        }
        public override void SaveWorldData(TagCompound tag)
        {
            //InfinitumNPCs testing = (InfinitumNPCs)GetContent<InfinitumNPCs>();
            tag.Add("test", 0);
            base.SaveWorldData(tag);
        }
        internal class SanjacobosOrePass : GenPass
        {
            public SanjacobosOrePass(string name, float loadWeight) : base(name, loadWeight)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Sanjacobo things";

                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.00008); k++)
                {

                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);


                    int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);

                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(12, 20), WorldGen.genRand.Next(8, 14), ModContent.TileType<SanjacobosMineralTile>());

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
