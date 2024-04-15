using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using Sandbox.Game.GameSystems;
using VRage.Game.GUI.TextPanel;
using VRage;
using VRage.Utils;

namespace SpaceEngineers.Droidbot.Display
{
    struct Screen
    {
        public IMyTextSurface surface;
        public RectangleF viewport;
        public int maxCharacterLength;
        public Vector2 characterSize;
        public string displayId;
        public int displayX;
        public int displayY;

    }

    struct Display
    {

    }

    class State
    {
        readonly Dictionary<string, RenderOutput> VISUALS;
        public delegate void RenderOutput(Screen s);

        public List<IMyCargoContainer> storage;
        public float fontSize;
        public Color textColor;
        public IMyGridTerminalSystem grid;
        public Dictionary<string, List<Screen>> outputs = new Dictionary<string, List<Screen>>();

        public State(IMyGridTerminalSystem g)
        {
            VISUALS = new Dictionary<string, RenderOutput>
            {
                { "storage", this.DrawStorageInfo }
            };
            this.fontSize = 0.75f; this.textColor = Color.Yellow;
            this.grid = g;

            foreach (var vis in VISUALS)
            {
                outputs[vis.Key] = new List<Screen>();
            }

            // search all of the screens for the ones that match our maps
            var screens = new List<IMyTextPanel>();
            this.grid.GetBlocksOfType(screens, s => s.CustomData.StartsWith("droid"));
            foreach (var screen in screens)
            {
                var customData = ParseCustomData(screen);
                var display = customData.GetValueOrDefault("display", null);
                if (display != null && VISUALS.ContainsKey(display))
                {
                    Screen s;
                    s.surface = screen;
                    s.characterSize = screen.MeasureStringInPixels(new StringBuilder("w"), "Monospace", this.fontSize);
                    s.viewport = new RectangleF(
                                     (screen.TextureSize - screen.SurfaceSize) / 2f,
                                     screen.SurfaceSize
                                 );
                    s.maxCharacterLength = (int)(s.viewport.Size.X / s.characterSize.X) - 1;
                    // Prepare the screen
                    PrepareTextSurfaceForSprites(screen);
                    this.outputs[display].Add(s);
                }
            }

            // grab all storage
            var storage = new List<IMyCargoContainer>();
            this.grid.GetBlocksOfType(storage, s => s.CustomData.StartsWith("droid"));
        }

        public void Tick()
        {
            // go through all of our outputs and render them
            foreach (var outputs in this.outputs)
            {
                if (this.VISUALS.ContainsKey(outputs.Key))
                {
                    foreach (var output in outputs.Value)
                    {
                        this.VISUALS[outputs.Key](output);
                    }
                }
            }
        }

        public Dictionary<string, string> ParseCustomData(IMyTerminalBlock block)
        {
            var results = new Dictionary<string, string>();
            // split by newline first
            var customDataSplit = block.CustomData.Split('\n');
            if (customDataSplit.Length > 1)
            {
                // go with the last lines, skip the first one
                var remainingLines = customDataSplit.Skip(1);
                // go through each one
                foreach (var line in remainingLines)
                {
                    // split that by colon
                    var lineSplit = line.Split(':');

                    // now convert that into the dictionary
                    results[lineSplit[0].Trim()] = lineSplit[1].Trim();
                }
            }

            return results;
        }
        public void DrawProgressBar(ref Screen s, ref MySpriteDrawFrame frame, Vector2 pos, MyFixedPoint cur, MyFixedPoint total, Color color)
        {
            var finalString = "[";
            var maxCharLength = s.maxCharacterLength - 2;
            var ratio = (float)cur.ToIntSafe() / (float)total.ToIntSafe();
            var filledCharLength = (int)(maxCharLength * ratio);
            for (var i = 0; i < maxCharLength; i++)
            {
                if (i < filledCharLength)
                {
                    finalString += "=";
                }
                else
                {
                    finalString += " ";
                }
            }
            finalString += "]";
            DrawText(ref s,ref frame, finalString, pos, color, TextAlignment.LEFT);
        }

        public void DrawStorageInfo(Screen s)
        {
            var frame = s.surface.DrawFrame();
            //_state.fontSize += 0.01f;
            var posY = 0.0f;

            foreach (var storage in storage)
            {
                var inventory = storage.GetInventory();
                var ratio = (float)inventory.CurrentVolume.ToIntSafe() / (float)inventory.MaxVolume.ToIntSafe();
                var pbColor = Color.White;
                if (ratio < 0.5f)
                {
                    pbColor = Color.Lime;
                }
                else if (ratio > 0.5f)
                {
                    pbColor = Color.Yellow;
                }
                else if (ratio > 0.75f)
                {
                    pbColor = Color.Red;
                }
                DrawText(ref s, ref frame, storage.DisplayNameText, new Vector2(0, posY), textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y;
                DrawProgressBar(ref s, ref frame, new Vector2(0, posY), inventory.CurrentVolume, inventory.MaxVolume, pbColor);
                posY += s.characterSize.Y;
                DrawText(ref s, ref frame, inventory.CurrentVolume.ToString() + " / " + inventory.MaxVolume.ToString(), new Vector2(0, posY), textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y;
                posY += s.characterSize.Y;
            }

            // bottom part
            DrawText(ref s, ref frame, "[storage]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), textColor, TextAlignment.LEFT);

            frame.Dispose();
        }
        public void DrawText(ref Screen s, ref MySpriteDrawFrame frame, string text, Vector2 position, Color color, TextAlignment alignment)
        {
            var sprite = new MySprite()
            {
                Type = SpriteType.TEXT,
                Data = text,
                Position = position + s.viewport.Position,
                RotationOrScale = fontSize /* 80 % of the font's default size */,
                Color = color,
                Alignment = alignment /* Center the text on the position */,
                FontId = "Monospace"
            };
            // Add the sprite to the frame
            frame.Add(sprite);
        }
        public void PrepareTextSurfaceForSprites(IMyTextSurface textSurface)
        {
            // Set the sprite display mode
            textSurface.ContentType = ContentType.SCRIPT;
            // Make sure no built-in script has been selected
            textSurface.Script = "";
        }
    };


    class Program : MyGridProgram
    {

        State _state;
        public Program()
        {
            _state = new State(GridTerminalSystem);
            // Set the continuous update frequency of this script
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            _state.Tick();
        }
    }

}