


    public abstract class Surface
    {
        public RectangleF viewport = new RectangleF();
        public int maxCharacterLength = 0;
        public Vector2 characterSize;
        public float fontSize = 1.0f;
        //public string displayId;
        //public Point displayCoords;

        public Dictionary<string, string> customData;

        public Vector4 padding = new Vector4(0.0f);

        public abstract void BeginDraw();
        public abstract void DrawText(string text, Vector2 position, Color color, TextAlignment alignment);
        public abstract void EndDraw();

        public Surface(Dictionary<string, string> custom)
        {
            customData = custom;

            try
            {
                fontSize = float.Parse(customData.GetValueOrDefault("fontSize", fontSize.ToString()));
            }
            catch (Exception e)
            {
                // do nothing
            }

            try
            {
                padding.X = float.Parse(customData.GetValueOrDefault("paddingLeft", "0"));
                padding.Y = float.Parse(customData.GetValueOrDefault("paddingRight", "0"));
                padding.Z = float.Parse(customData.GetValueOrDefault("paddingTop", "0"));
                padding.W = float.Parse(customData.GetValueOrDefault("paddingBottom", "0"));
            }
            catch (Exception e) { }
        }

        public void DrawProgressBar(Vector2 pos, MyFixedPoint cur, MyFixedPoint total, Color color)
        {
            var finalString = "[";
            var maxCharLength = maxCharacterLength - finalString.Length - 1; // -1 for the final ]
            var ratio = (float)cur.ToIntSafe() / (float)total.ToIntSafe();
            var filledCharLength = (int)(maxCharLength * ratio);
            for (var i = 0; i < maxCharLength; i++)
            {
                if (i < filledCharLength)
                {
                    finalString += "█";
                }
                else
                {
                    finalString += " ";
                }
            }
            finalString += "]";
            DrawText(finalString, pos, color, TextAlignment.LEFT);
        }

        public void DrawProgressBar(Vector2 pos, MyFixedPoint cur, MyFixedPoint total, Color color, string prefix, string suffix)
        {
            var prefixString = prefix + " [";
            var finalString = prefixString;
            var availableCharLength = maxCharacterLength - prefixString.Length - suffix.Length - 3; // -1 for the last ]

            var ratio = (float)cur.ToIntSafe() / (float)total.ToIntSafe();
            var filledCharLength = (int)(availableCharLength * ratio);
            for (var i = 0; i < availableCharLength; i++)
            {
                if (i < filledCharLength)
                {
                    finalString += "█";
                }
                else
                {
                    finalString += " ";
                }
            }
            finalString += "] " + suffix;
            DrawText(finalString, pos, color, TextAlignment.LEFT);
        }
    }

    public class Screen : Surface
    {
        public IMyTextSurface surface;
        private MySpriteDrawFrame frame;

        public Screen(IMyTextSurface surface, Dictionary<string, string> custom) : base(custom)
        {

            this.surface = surface;
            this.characterSize = surface.MeasureStringInPixels(new StringBuilder("w"), "Monospace", this.fontSize);
            this.viewport = new RectangleF((surface.TextureSize - surface.SurfaceSize) / 2f,
                                     surface.SurfaceSize
                                 );
            this.maxCharacterLength = (int)Math.Floor(viewport.Size.X / characterSize.X);
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
                Position = position + viewport.Position + new Vector2(padding.X, padding.Y),
                RotationOrScale = fontSize /* 80 % of the font's default size */,
                Color = surface.ScriptForegroundColor,
                Alignment = alignment /* Center the text on the position */,
                FontId = "Monospace"
            };
            // Add the sprite to the frame
            frame.Add(sprite);
        }
    }

    public class CompositeDisplay : Surface
    {
        public string id = null;
        public Dictionary<Point, Screen> screens = new Dictionary<Point, Screen>();
        private int maxX = 0;
        private int maxY = 0;

        public CompositeDisplay(string id, Dictionary<string, string> custom) : base(custom)
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
                return new KeyValuePair<bool, RectangleF>(true, new RectangleF(fullTextBox.X - modifiedViewport.X, fullTextBox.Y - modifiedViewport.Y, intersection.Width, intersection.Height));
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
                    //Console.WriteLine("\ttext rect extent for screen {0}: {1}", screen.Key, textRectExtentPair);
                    screen.Value.DrawText(text, new Vector2(textRectExtentPair.Value.X, textRectExtentPair.Value.Y), color, TextAlignment.LEFT);
                }
            }
        }
    }

    public class State
    {
        readonly Dictionary<string, RenderOutput> VISUALS = new Dictionary<string, RenderOutput> {
                { "storage", DrawStorageInfo },
                { "itemdetail", DrawItemDetail },
                { "power", DrawPowerOverview },
                { "h2o", DrawGasDetail },
                { "log", DrawLogs }
        };

        readonly Dictionary<int, EventHandler> EVENT_HANDLERS = new Dictionary<int, EventHandler> {
                { DroidbotEnums.EVENT_MOVED_ITEMS, HandleMovedItemsEvent },
        };

        public delegate void RenderOutput(State state, Surface s);
        public delegate void EventHandler(State state, string data);

        public List<IMyCargoContainer> storage = new List<IMyCargoContainer>();
        public List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
        public List<IMyPowerProducer> powerProducers = new List<IMyPowerProducer>();

        public List<IMyGasGenerator> gasGenerators = new List<IMyGasGenerator>();
        public float fontSize;
        public Color textColor;
        public IMyGridTerminalSystem grid;

        public MyGridProgram prog;
        public Dictionary<string, List<Surface>> outputs = new Dictionary<string, List<Surface>>();
        public Dictionary<string, CompositeDisplay> displays = new Dictionary<string, CompositeDisplay>();
        public List<MyItemType> itemTypes = new List<MyItemType>();
        public Dictionary<MyItemType, MyFixedPoint> itemCounts = new Dictionary<MyItemType, MyFixedPoint>();
        public Dictionary<MyItemType, MyFixedPoint> itemTargets = new Dictionary<MyItemType, MyFixedPoint>();

        public List<string> logMessages = new List<string>(50);

        public IMyBroadcastListener broadcastListener;

        public short tick = 0;

        public State(MyGridProgram p)
        {
            this.fontSize = 1.0f;
            this.textColor = Color.Yellow;
            this.prog = p;
            this.grid = p.GridTerminalSystem;

            broadcastListener = p.IGC.RegisterBroadcastListener("droid");
            broadcastListener.SetMessageCallback("droid");


            ScanScreens();
            ScanAllResources();
            RefreshItemCounts();
        }

        private void ScanScreens()
        {
            this.outputs.Clear();
            this.displays.Clear();
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

                    Screen s = new Screen(screen, customData);
                    s.customData = customData;

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
                            this.displays.Add(displayId, new CompositeDisplay(displayId, customData));
                            this.displays[displayId].customData = customData;
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
        }

        public void Log(string text)
        {
            this.prog.Echo(text);
        }

        public void RefreshItemCounts()
        {
            //go through all of our item types and query each of our storage
            foreach (var itemType in this.itemTypes)
            {
                this.itemCounts[itemType] = 0;
                foreach (var storage in this.storage)
                {
                    this.itemCounts[itemType] += storage.GetInventory().GetItemAmount(itemType);
                }
            }
        }

        public void ScanAllResources()
        {
            ScanScreens();
            this.storage.Clear();
            this.batteries.Clear();
            this.powerProducers.Clear();
            this.itemTypes.Clear();
            this.itemCounts.Clear();
            this.gasGenerators.Clear();

            // grab all storage
            this.grid.GetBlocksOfType(this.storage, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // grab all batteries
            this.grid.GetBlocksOfType(this.batteries, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // grab all power producers
            this.grid.GetBlocksOfType(this.powerProducers, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // grab all gas generators
            this.grid.GetBlocksOfType(this.gasGenerators, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // get item types and put em in our list
            foreach (var storage in this.storage)
            {
                var acceptedItems = new List<MyItemType>();
                storage.GetInventory().GetAcceptedItems(acceptedItems, it => it.TypeId == "MyObjectBuilder_Ore" || it.TypeId == "MyObjectBuilder_Ingot" || it.TypeId == "MyObjectBuilder_Component");
                foreach (var itemType in acceptedItems)
                {
                    if (!this.itemTypes.Contains(itemType))
                    {
                        this.itemTypes.Add(itemType);
                        this.itemCounts[itemType] = 0;
                    }
                }
            }
        }

        public void Tick(string argument, UpdateType updateType)
        {
            if (updateType == UpdateType.IGC)
            {
                // read broadcast messages
                ProcessBroadcastMessages();
            }

            if (tick % 10 == 0)
            {
                ScanAllResources();
            }

            RefreshItemCounts();
            // go through all of our outputs and render them
            foreach (var outputs in this.outputs)
            {
                if (VISUALS.ContainsKey(outputs.Key))
                {
                    foreach (var output in outputs.Value)
                    {
                        output.BeginDraw();
                        VISUALS[outputs.Key](this, output);
                        output.EndDraw();
                    }
                }
            }
            tick++;
        }

        private void ProcessBroadcastMessages()
        {
            // do we have a message?
            if (broadcastListener.HasPendingMessage)
            {
                var myIGCMessage = broadcastListener.AcceptMessage();
                var data = myIGCMessage.As<MyTuple<int, string>>();
                // do we have a handler?
                if (EVENT_HANDLERS.ContainsKey(data.Item1))
                {
                    EVENT_HANDLERS[data.Item1](this, data.Item2);
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

        public static void DrawStorageInfo(State state, Surface s)
        {
            //_state.fontSize += 0.01f;
            var posY = 0.0f;

            foreach (var storage in state.storage)
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
                s.DrawText(storage.DisplayNameText, new Vector2(0, posY), state.textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y;
                s.DrawProgressBar(new Vector2(0, posY), inventory.CurrentVolume, inventory.MaxVolume, pbColor);
                posY += s.characterSize.Y;
                s.DrawText(inventory.CurrentVolume.ToString() + " / " + inventory.MaxVolume.ToString(), new Vector2(0, posY), state.textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y;
                posY += s.characterSize.Y;
            }

            // bottom part
            s.DrawText("[storage]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), state.textColor, TextAlignment.LEFT);
        }

        public static void DrawItemDetail(State state, Surface s)
        {
            var posY = 0.0f;

            var target = 1000;

            var surfaceFilter = s.customData.GetValueOrDefault("detail", "").Split(',');

            // calculate padding for the prefix
            var prefixPadding = 0;
            var suffixPadding = 0;
            foreach (var itemCountPair in state.itemCounts)
            {
                var surfaceFilterMatch = itemCountPair.Key.TypeId.Replace("MyObjectBuilder_", "").ToLower();
                if (surfaceFilter[0] != "" && !surfaceFilter.Contains(surfaceFilterMatch))
                {
                    continue;
                }
                var prefixLength = String.Format(" {0} {1}", itemCountPair.Key.SubtypeId, itemCountPair.Key.TypeId.Replace("MyObjectBuilder_", "")).Length;
                var suffixLength = String.Format("{0} / {1}", itemCountPair.Value, target).Length;
                if (prefixLength > prefixPadding)
                {
                    prefixPadding = prefixLength;
                }
                if (suffixLength > suffixPadding)
                {
                    suffixPadding = suffixLength;
                }
            }

            // go through each of our item types
            foreach (var itemCountPair in state.itemCounts)
            {
                var surfaceFilterMatch = itemCountPair.Key.TypeId.Replace("MyObjectBuilder_", "").ToLower();
                if (surfaceFilter[0] != "" && !surfaceFilter.Contains(surfaceFilterMatch))
                {
                    continue;
                }
                var prefix = String.Format("{0," + prefixPadding + "}", String.Format(" {0} {1}", itemCountPair.Key.SubtypeId.ToLower(), itemCountPair.Key.TypeId.Replace("MyObjectBuilder_", "").ToLower()));
                var suffix = String.Format("{0," + suffixPadding + "}", String.Format("{0} / {1}", itemCountPair.Value, target));
                s.DrawProgressBar(new Vector2(0, posY), itemCountPair.Value, target, Color.White, prefix, suffix);
                posY += s.characterSize.Y + 2;
            }

            // bottom part
            s.DrawText("[item detail]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), state.textColor, TextAlignment.LEFT);
        }

        public static void DrawPowerOverview(State state, Surface s)
        {
            var posY = 0.0f;

            MyFixedPoint currentOutput = 0;
            MyFixedPoint maxOutput = 0;

            MyFixedPoint currentStoredPower = 0;
            MyFixedPoint maxStoredPower = 0;

            foreach (var powerProducer in state.powerProducers)
            {
                MyFixedPoint convCurr = (MyFixedPoint)powerProducer.CurrentOutput;
                MyFixedPoint convMax = (MyFixedPoint)powerProducer.MaxOutput;
                currentOutput += convCurr;
                maxOutput += convMax;
            }

            foreach (var battery in state.batteries)
            {
                MyFixedPoint convCurr = (MyFixedPoint)battery.CurrentStoredPower;
                MyFixedPoint convMax = (MyFixedPoint)battery.MaxStoredPower;
                currentStoredPower += convCurr;
                maxStoredPower += convMax;
            }

            var suffix = String.Format("{0} MW / {1} MW", currentOutput, maxOutput);
            s.DrawProgressBar(new Vector2(0, posY), currentOutput, maxOutput, Color.White, "power output", suffix);
            posY += s.characterSize.Y * 2;

            suffix = String.Format("{0} MWh / {1} MWh", currentStoredPower, maxStoredPower);
            s.DrawProgressBar(new Vector2(0, posY), currentStoredPower, maxStoredPower, Color.White, "battery", suffix);

            // bottom part
            s.DrawText("[power]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), state.textColor, TextAlignment.LEFT);
        }

        public static void DrawGasDetail(State state, Surface s)
        {
            var posY = 0.0f;

            MyFixedPoint currentOutput = 0;
            MyFixedPoint maxOutput = 0;

            MyFixedPoint currentStoredPower = 0;
            MyFixedPoint maxStoredPower = 0;

            var suffix = String.Format("{0} MW / {1} MW", currentOutput, maxOutput);
            s.DrawProgressBar(new Vector2(0, posY), currentOutput, maxOutput, Color.White, "power output", suffix);
            posY += s.characterSize.Y * 2;

            suffix = String.Format("{0} MWh / {1} MWh", currentStoredPower, maxStoredPower);
            s.DrawProgressBar(new Vector2(0, posY), currentStoredPower, maxStoredPower, Color.White, "battery", suffix);

            // bottom part
            s.DrawText("[power]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), state.textColor, TextAlignment.LEFT);
        }

        public static void DrawLogs(State state, Surface s)
        {
            var posY = 0.0f;
            // how many log messages can we print onto this surface?
            var numLinesToPrint = (int)Math.Floor(s.viewport.Height / (s.characterSize.Y + 2)) - 1;

            foreach (var line in state.logMessages.Skip(Math.Max(0, state.logMessages.Count - numLinesToPrint)))
            {
                s.DrawText(line, new Vector2(0, posY), state.textColor, TextAlignment.LEFT);
                posY += s.characterSize.Y + 2;
            }

            // bottom part
            s.DrawText("[base log]", new Vector2(0, s.viewport.Size.Y - s.characterSize.Y), state.textColor, TextAlignment.LEFT);
        }

        private static void HandleMovedItemsEvent(State state, string data)
        {
            state.ReportLog(data);
        }

        private void ReportLog(string data)
        {
            if (this.logMessages.Count == this.logMessages.Capacity)
            {
                // remove the oldest log message
                this.logMessages.RemoveAt(0);
            }
            this.logMessages.Add(data);
        }

        public void PrepareTextSurfaceForSprites(IMyTextSurface textSurface)
        {
            // Set the sprite display mode
            textSurface.ContentType = ContentType.SCRIPT;
            // Make sure no built-in script has been selected
            textSurface.Script = "";
        }
    };

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
            _state.Tick(argument, updateSource);
        }
public class DroidbotEnums
{
    public const int EVENT_MOVED_ITEMS = 1;
};