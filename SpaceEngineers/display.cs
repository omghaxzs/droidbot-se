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
using Sandbox.Game.Entities.Character.Components;

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
            //Console.WriteLine("drawing text: position: {0}", position);
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
        private int maxX = 0;
        private int maxY = 0;

        public CompositeDisplay(string id)
        {
            this.id = id;
        }

        private void CalculateViewport()
        {
            // add up the viewports of all our screens
            this.viewport = new RectangleF(0, 0, screens.First().Value.viewport.Width * (this.maxX + 1), screens.First().Value.viewport.Height * (this.maxY + 1));
            var surface = screens.First().Value.surface;
            this.characterSize = surface.MeasureStringInPixels(new StringBuilder("w"), "Monospace", this.fontSize);
            this.maxCharacterLength = (int)(viewport.Size.X / characterSize.X) - 1;
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

            if (x > maxX)
            {
                maxX = x;
            }
            if (y > maxY)
            {
                maxY = y;
            }
            CalculateViewport();

            return null;
        }

        public override void BeginDraw()
        {
            foreach (var s in screens)
            {
                s.Value.BeginDraw();
            }
        }

        public override void EndDraw()
        {
            foreach (var s in screens)
            {
                s.Value.EndDraw();
            }
        }

        private KeyValuePair<Point, Screen> GetScreenForPoint(Vector2 p)
        {
            var ret = screens.FirstOrDefault(pair =>
            {
                // generate a modified viewport with the correct X/Y positions
                var modifiedViewport = new RectangleF(pair.Key.X * pair.Value.viewport.Width, pair.Key.Y * pair.Value.viewport.Height, pair.Value.viewport.Width, pair.Value.viewport.Height);
                //Console.WriteLine("- evaluating screen {0} for point {1}\n-- viewport: {2}", pair, p, modifiedViewport);
                return p.X >= modifiedViewport.X && p.X <= modifiedViewport.Right && p.Y >= modifiedViewport.Y && p.Y <= modifiedViewport.Right;
            });

            //Console.WriteLine("point {0} returned screen {1}", p, ret);

            return ret;
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

        private KeyValuePair<bool, RectangleF> CalculateTextBoxExtent(RectangleF fullTextBox, KeyValuePair<Point, Screen> pair)
        {
            // generate a modified viewport with the correct X/Y positions
            var modifiedViewport = new RectangleF(pair.Key.X * pair.Value.viewport.Width, pair.Key.Y * pair.Value.viewport.Height, pair.Value.viewport.Width, pair.Value.viewport.Height);

            var bbViewport = new BoundingBox2(new Vector2(modifiedViewport.X, modifiedViewport.Y), new Vector2(modifiedViewport.Right, modifiedViewport.Bottom));
            var bbTextBox = new BoundingBox2(new Vector2(fullTextBox.X, fullTextBox.Y), new Vector2(fullTextBox.Right, fullTextBox.Bottom));

            if (bbViewport.Intersects(bbTextBox))
            {

                //Console.WriteLine("\t\trect: {0}\n\t\tviewport: {1}\n\t\tbbViewport: {2}\n\t\tbbTextBox: {3}\n\t\tintersect: {4}\n\t\tintersect2: {5}", fullTextBox, modifiedViewport, bbViewport, bbTextBox, bbViewport.Intersect(bbTextBox), bbTextBox.Intersect(bbViewport));
                var intersection = bbViewport.Intersect(bbTextBox);
                return new KeyValuePair<bool, RectangleF>(true, new RectangleF(pair.Key.X == 0 ? intersection.Min.X : -intersection.Min.X, pair.Key.Y == 0 ? intersection.Min.Y : -intersection.Min.X, intersection.Width, intersection.Height));
            }
            else
            {
                //Console.WriteLine("rect {0} does not intersect screen {1}", fullTextBox, pair.Key);
                return new KeyValuePair<bool, RectangleF>(false, new RectangleF(0, 0, 0, 0));
            }
        }

        public override void DrawText(string text, Vector2 position, Color color, TextAlignment alignment)
        {
            // get the text box for the text
            var textRect = CalculateTextBox(text, position, alignment);

            //Console.WriteLine("text rect: {0}", textRect);

            foreach (var screen in this.screens)
            {
                // calculate the extent for this screen
                var textRectExtentPair = CalculateTextBoxExtent(textRect, screen);
                if (textRectExtentPair.Key)
                {
                    //Console.WriteLine("\ttext rect extent for screen {0}: {1}", screen.Key, textRectExtent);
                    screen.Value.DrawText(text, new Vector2(textRectExtentPair.Value.X, textRectExtentPair.Value.Y), color, TextAlignment.LEFT);
                }
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

        public List<IMyCargoContainer> storage = new List<IMyCargoContainer>();
        public float fontSize;
        public Color textColor;
        public IMyGridTerminalSystem grid;

        public MyGridProgram prog;
        public Dictionary<string, List<Surface>> outputs = new Dictionary<string, List<Surface>>();

        public Dictionary<string, CompositeDisplay> displays = new Dictionary<string, CompositeDisplay>();

        public State(MyGridProgram p)
        {
            VISUALS = new Dictionary<string, RenderOutput>
            {
                { "storage", this.DrawStorageInfo }
            };
            this.fontSize = 0.75f;
            this.textColor = Color.Yellow;
            this.prog = p;
            this.grid = p.GridTerminalSystem;

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
                        var displayX = 0;
                        var displayY = 0;
                        try
                        {
                            displayX = Int32.Parse(customData.GetValueOrDefault("displayX", "0"));
                        }
                        catch (Exception e)
                        {
                            Log("could not figure out displayX for display ID" + displayId);
                            displayX = 0;
                        }
                        try
                        {
                            displayY = Int32.Parse(customData.GetValueOrDefault("displayY", "0"));
                        }
                        catch (Exception e)
                        {
                            Log("could not figure out displayX for display ID" + displayId);
                            displayY = 0;
                        }

                        // does the display exist already?
                        if (!this.displays.ContainsKey(displayId))
                        {
                            this.displays.Add(displayId, new CompositeDisplay(displayId));
                            this.outputs[display].Add(this.displays[displayId]);
                        }

                        // add the screen
                        var ret = this.displays[displayId].AddScreen(s, displayX, displayY);
                        if (ret != null)
                        {
                            this.Log("failed to add screen " + screen.DisplayNameText + " to display " + displayId + " reason: " + ret);
                        }
                    }
                    else
                    {
                        this.outputs[display].Add(s);
                    }
                }
            }


            // grab all storage
            this.grid.GetBlocksOfType(this.storage, s => s.CustomData.StartsWith("droid"));
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
                        output.BeginDraw();
                        this.VISUALS[outputs.Key](output);
                        output.EndDraw();
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