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

namespace Droidbot.Display
{
    public abstract class Surface
    {
        public RectangleF viewport = new RectangleF();
        public int maxCharacterLength = 0;
        public Vector2 characterSize;
        public float fontSize = 0.75f;
        //public string displayId;
        //public Point displayCoords;

        public abstract void BeginDraw();
        public abstract void DrawText(string text, Vector2 position, Color color, TextAlignment alignment);
        public abstract void DrawProgressBar(Vector2 pos, MyFixedPoint cur, MyFixedPoint total, Color color);
        public abstract void EndDraw();
    }

    public class Screen : Surface
    {
        public IMyTextSurface surface;
        private MySpriteDrawFrame frame;

        public Screen(IMyTextSurface surface)
        {
            this.surface = surface;
            this.characterSize = surface.MeasureStringInPixels(new StringBuilder("w"), "Monospace", this.fontSize);
            this.viewport = new RectangleF((surface.TextureSize - surface.SurfaceSize) / 2f,
                                     surface.SurfaceSize
                                 );
            this.maxCharacterLength = (int)(viewport.Size.X / characterSize.X) - 1;
        }

        public override void BeginDraw()
        {
            this.frame = surface.DrawFrame();
        }

        public override void EndDraw()
        {
            this.frame.Dispose();
        }

        public override void DrawText(string text, Vector2 position, Color color, TextAlignment alignment)
        {
            var sprite = new MySprite()
            {
                Type = SpriteType.TEXT,
                Data = text,
                Position = position + viewport.Position,
                RotationOrScale = fontSize /* 80 % of the font's default size */,
                Color = color,
                Alignment = alignment /* Center the text on the position */,
                FontId = "Monospace"
            };
            // Add the sprite to the frame
            frame.Add(sprite);
        }

        public override void DrawProgressBar(Vector2 pos, MyFixedPoint cur, MyFixedPoint total, Color color)
        {
            var finalString = "[";
            var maxCharLength = maxCharacterLength - 2;
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
            DrawText(finalString, pos, color, TextAlignment.LEFT);
        }
    }

    public class CompositeDisplay : Surface
    {
        public string id = null;
        public Dictionary<Point, Screen> screens = new Dictionary<Point, Screen>();

        public CompositeDisplay(string id)
        {
            this.id = id;
        }

        public string AddScreen(Screen s, int x, int y)
        {
            var p = new Point(x, y);
            if (this.screens.ContainsKey(p))
            {
                return "duplicate display coords";
            }

            // otherwise add it
            this.screens.Add(p, s);

            return "";
        }

        public override void BeginDraw()
        {
            screens.ForEach(s => s.Value.BeginDraw());
        }

        public override void EndDraw()
        {
            screens.ForEach(s => s.Value.EndDraw());
        }

        private KeyValuePair<Point, Screen> GetScreenForPoint(Vector2 p)
        {
            return screens.First(pair => p.X >= pair.Value.viewport.X && p.X <= pair.Value.viewport.Right && p.Y >= pair.Value.viewport.Y && p.Y <= pair.Value.viewport.Right);
        }

        private RectangleF CalculateTextBox(string text, Vector2 position, TextAlignment alignment)
        {
            var textLength = text.Length;
            var textLengthX = textLength * this.characterSize.X;
            var textLengthY = this.characterSize.Y;
            switch (alignment)
            {
                case TextAlignment.LEFT:
                    {
                        return new RectangleF(position.X, position.Y, textLengthX, textLengthY);
                    }
                case TextAlignment.CENTER:
                    {
                        var textLengthHalf = textLengthX / 2;
                        return new RectangleF(position.X + textLengthHalf, position.Y, textLengthHalf, textLengthY);
                    }
                case TextAlignment.RIGHT:
                    {
                        return new RectangleF(position.X - textLengthX, position.Y, textLengthX, textLengthY);
                    }
            }

            return new RectangleF(0, 0, 0, 0);
        }

        private RectangleF CalculateTextBoxExtent(RectangleF fullTextBox, RectangleF viewport)
        {
            return new RectangleF(
                fullTextBox.X < viewport.X ? viewport.X : fullTextBox.X, // if X is less than the left most point of our viewport, use the left most point of our viewport
                fullTextBox.Y < viewport.Y ? viewport.Y : fullTextBox.Y, // if Y is less than the top most point of our viewport, use the top most point of our viewport
                fullTextBox.Right > viewport.Right ? viewport.Right : fullTextBox.Right, // if Right is larger than the right most point of our viewport, use the right most point of our viewport
                fullTextBox.Bottom > viewport.Bottom ? viewport.Bottom : fullTextBox.Bottom); // if Right is larger than the right most point of our viewport, use the right most point of our viewport
        }

        public override void DrawText(string text, Vector2 position, Color color, TextAlignment alignment)
        {
            // get the text box for the text
            var textRect = CalculateTextBox(text, position, alignment);

            // which screen does the left most point start?
            var leftPointScreen = this.GetScreenForPoint(textRect.Position);
            var rightPointScreen = this.GetScreenForPoint(new Vector2(textRect.Right, textRect.Bottom));
            // subtract their X coordinates
            var xCoordDiff = rightPointScreen.Key.X - leftPointScreen.Key.X;
            if (xCoordDiff > 0)
            {
                for (var i = 0; i < xCoordDiff; i++)
                {
                    // calculate the split text box
                    var textRectExtent = CalculateTextBoxExtent(textRect, viewport);
                }
            }
            else
            {
                // it's displaying on just one screen, whew
                leftPointScreen.Value.DrawText(text, position, color, alignment);
            }
        }

        public override void DrawProgressBar(Vector2 pos, MyFixedPoint cur, MyFixedPoint total, Color color)
        {
            var finalString = "[";
            var maxCharLength = maxCharacterLength - 2;
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
            DrawText(finalString, pos, color, TextAlignment.LEFT);
        }

    }

    public class State
    {
        readonly Dictionary<string, RenderOutput> VISUALS;
        public delegate void RenderOutput(Surface s);

        public List<IMyCargoContainer> storage;
        public float fontSize;
        public Color textColor;
        public IMyGridTerminalSystem grid;

        public MyGridProgram prog;
        public Dictionary<string, List<Surface>> outputs = new Dictionary<string, List<Surface>>();

        public Dictionary<string, CompositeDisplay> displays = new Dictionary<string, CompositeDisplay>();

        public State(MyGridProgram prog)
        {
            VISUALS = new Dictionary<string, RenderOutput>
            {
                { "storage", this.DrawStorageInfo }
            };
            this.fontSize = 0.75f;
            this.textColor = Color.Yellow;
            this.prog = prog;
            this.grid = prog.GridTerminalSystem;

            foreach (var vis in VISUALS)
            {
                outputs[vis.Key] = new List<Surface>();
            }

            // search all of the screens for the ones that match our maps
            var screens = new List<IMyTextPanel>();
            this.grid.GetBlocksOfType(screens, s => s.CustomData.StartsWith("droid"));
            foreach (var screen in screens)
            {
                var customData = ParseCustomData(screen);
                var display = customData.GetValueOrDefault("display", null);
                var displayId = customData.GetValueOrDefault("displayId", null);
                if (display != null && VISUALS.ContainsKey(display))
                {
                    // Prepare the screen
                    PrepareTextSurfaceForSprites(screen);

                    Screen s = new Screen(screen);

                    // check to see if there's a display
                    if (displayId != null)
                    {
                        if (!Int32.TryParse(customData.GetValueOrDefault("displayX", "0"), out int displayX))
                        {
                            Log("could not figure out displayX for display ID" + displayId);
                            displayX = 0;
                        }
                        if (!Int32.TryParse(customData.GetValueOrDefault("displayY", "0"), out int displayY))
                        {
                            Log("could not figure out displayY for display ID" + displayId);
                            displayY = 0;
                        }

                        // does the display exist already?
                        if (!this.displays.ContainsKey(displayId))
                        {
                            this.displays.Add(displayId, new CompositeDisplay(displayId));
                        }

                        // add the screen
                        var ret = this.displays[displayId].AddScreen(s, displayX, displayY);
                        if (ret != null)
                        {
                            this.Log("failed to add screen " + screen.DisplayNameText + " to display " + displayId);
                        }
                    }
                    else
                    {
                        this.outputs[display].Add(s);
                    }
                }
            }

            // grab all storage
            var storage = new List<IMyCargoContainer>();
            this.grid.GetBlocksOfType(storage, s => s.CustomData.StartsWith("droid"));
        }

        public void Log(string text)
        {
            this.prog.Echo(text);
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

        public void DrawStorageInfo(Surface s)
        {
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
                s.DrawText(storage.DisplayNameText, new Vector2(0, posY), textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y;
                s.DrawProgressBar(new Vector2(0, posY), inventory.CurrentVolume, inventory.MaxVolume, pbColor);
                posY += s.characterSize.Y;
                s.DrawText(inventory.CurrentVolume.ToString() + " / " + inventory.MaxVolume.ToString(), new Vector2(0, posY), textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y;
                posY += s.characterSize.Y;
            }

            // bottom part
            s.DrawText("[storage]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), textColor, TextAlignment.LEFT);
        }

        public void PrepareTextSurfaceForSprites(IMyTextSurface textSurface)
        {
            // Set the sprite display mode
            textSurface.ContentType = ContentType.SCRIPT;
            // Make sure no built-in script has been selected
            textSurface.Script = "";
        }
    };


    public class Program : MyGridProgram
    {

        State _state;
        public Program()
        {
            _state = new State(this);
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