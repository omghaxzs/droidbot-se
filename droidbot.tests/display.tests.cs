using Droidbot.Display;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Game.Components.Interfaces;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage;

namespace droidbot.tests
{
    namespace display
    {
        public class DroidbotDisplayTests
        {
            private void DumpSurface(MockMyTextPanel p)
            {
                var failStr = "\n";
                foreach (var sprite in p._spriteCollection.Sprites)
                {
                    failStr += sprite.Data + "\n";
                }
                Assert.Fail(failStr);
            }

            [Fact]
            public void ScreenScanning()
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>
            {
                new MockMyTextPanel {
                    _customData = "droid\ndisplay: storage"
                }
            };
                State s = new State(new MockGridProgram(blocks));
                // we should have 1 screen
                Assert.Single(s.outputs["storage"]);
            }

            [Fact]
            public void SingleScreen()
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>
            {
                new MockMyTextPanel {
                    _customData = "droid\ndisplay: storage"
                }
            };
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                // Check the sprite collection
                // should just be one sprite
                Screen screen = s.outputs["storage"][0] as Screen;
                Assert.NotNull(screen);

                MockMyTextPanel mockSurface = screen.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface);

                MySprite[] spriteCollection = mockSurface._spriteCollection.Sprites;
                Assert.Single(spriteCollection);

                // Now double check the sprite
                // which should be the "[storage]" text
                Assert.Equal(SpriteType.TEXT, spriteCollection[0].Type);
                Assert.Equal(new Vector2(0, screen.viewport.Height - screen.characterSize.Y), spriteCollection[0].Position);
            }

            [Fact]
            public void DualScreenWideStorageWithNoStorage()
            {
                List<IMyTerminalBlock> blocks =
                [
                    new MockMyTextPanel {
                    _displayNameText = "display 1",
                    _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                },
                new MockMyTextPanel {
                    _displayNameText = "display 2",
                    _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                }
                ];
                State s = new(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("storage"));
                Assert.Single(s.outputs["storage"]);
                CompositeDisplay compositeDisplay = s.outputs["storage"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should only draw to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Single(mockSurface1._spriteCollection.Sprites);

                // Now double check the sprite
                // which should be the "[storage]" text
                Assert.Equal(SpriteType.TEXT, mockSurface1._spriteCollection.Sprites[0].Type);
                Assert.Equal(new Vector2(0, screen1.viewport.Height - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[0].Position);

                Screen screen2 = compositeDisplay.screens[new Point(1, 0)];
                Assert.NotNull(screen2);
                MockMyTextPanel mockSurface2 = screen2.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface2);

                Assert.Null(mockSurface2._spriteCollection.Sprites);
            }

            [Fact]
            public void DualScreenWideStorageWithStorage()
            {
                List<IMyTerminalBlock> blocks =
                [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                        },
                        new MockMyCargoContainer {
                            _displayNameText = "storage",
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("storage"));
                Assert.Single(s.outputs["storage"]);
                CompositeDisplay compositeDisplay = s.outputs["storage"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should draw 4 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(4, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "storage"
                Assert.Equal(new Vector2(0, screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[1].Position); // "progress bar"
                Assert.Equal(new Vector2(0, screen1.characterSize.Y * 2), mockSurface1._spriteCollection.Sprites[2].Position); // "0 / 3000"
                Assert.Equal(new Vector2(0, screen1.viewport.Bottom - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[3].Position); // "[storage]"

                // should just be one drawing for the second screen
                Screen screen2 = compositeDisplay.screens[new Point(1, 0)];
                Assert.NotNull(screen2);
                MockMyTextPanel mockSurface2 = screen2.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface2);
                Assert.Single(mockSurface2._spriteCollection.Sprites);
                Assert.Equal(new Vector2(-512, screen2.characterSize.Y), mockSurface2._spriteCollection.Sprites[0].Position); // "progress bar"
            }

            [Fact]
            public void QuadScreenStorageWithStorage()
            {
                List<IMyTerminalBlock> blocks =
                        [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 3",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 0\ndisplayY: 1"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 4",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 1\ndisplayY: 1"
                        },
                        new MockMyCargoContainer {
                            _displayNameText = "storage",
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("storage"));
                Assert.Single(s.outputs["storage"]);
                CompositeDisplay compositeDisplay = s.outputs["storage"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 1024), compositeDisplay.viewport);

                // this should draw 3 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(3, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "storage"
                Assert.Equal(new Vector2(0, screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[1].Position); // "progress bar"
                Assert.Equal(new Vector2(0, screen1.characterSize.Y * 2), mockSurface1._spriteCollection.Sprites[2].Position); // "0 / 3000"

                // should just be one drawing for the second screen
                Screen screen2 = compositeDisplay.screens[new Point(1, 0)];
                Assert.NotNull(screen2);
                MockMyTextPanel mockSurface2 = screen2.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface2);
                Assert.Single(mockSurface2._spriteCollection.Sprites);
                Assert.Equal(new Vector2(-512, screen2.characterSize.Y), mockSurface2._spriteCollection.Sprites[0].Position); // "progress bar"

                // should just be one drawing for the third screen
                Screen screen3 = compositeDisplay.screens[new Point(0, 1)];
                Assert.NotNull(screen3);
                MockMyTextPanel mockSurface3 = screen3.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface3);
                Assert.Single(mockSurface3._spriteCollection.Sprites);
                Assert.Equal(new Vector2(0, screen3.viewport.Height - screen3.characterSize.Y), mockSurface3._spriteCollection.Sprites[0].Position); // "[storage]"
            }

            [Fact]
            public void DualScreenWideDetailWithStorage()
            {
                List<IMyTerminalBlock> blocks =
                [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                        },
                        new MockMyCargoContainer {
                            _displayNameText = "storage",
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _acceptedItems = [
                                    MyItemType.MakeOre("uranium"),
                                ],
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("itemdetail"));
                Assert.Single(s.outputs["itemdetail"]);
                CompositeDisplay compositeDisplay = s.outputs["itemdetail"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should draw 2 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(2, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "uranium thing"
                Assert.Equal(new Vector2(0, screen1.viewport.Bottom - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[1].Position); // "[item detail]"

                Assert.True(mockSurface1._spriteCollection.Sprites[0].Data.Length * compositeDisplay.characterSize.X < 1024);
            }

            [Fact]
            public void DualScreenWideDetailWithMultipleStorage()
            {
                List<IMyTerminalBlock> blocks =
                [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                        },
                        new MockMyCargoContainer {
                            _displayNameText = "storage",
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _acceptedItems = [
                                    MyItemType.MakeOre("uranium"),
                                    MyItemType.MakeOre("platinum"),
                                    MyItemType.MakeComponent("construction"),
                                ],
                                _inventoryItems = [
                                    new MyInventoryItem(MyItemType.MakeOre("uranium"), 0, 394823)
                                ],
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("itemdetail"));
                Assert.Single(s.outputs["itemdetail"]);
                CompositeDisplay compositeDisplay = s.outputs["itemdetail"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should draw 2 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(4, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "uranium thing"
                Assert.Equal(new Vector2(0, screen1.viewport.Bottom - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[3].Position); // "[item detail]"

                Assert.True(mockSurface1._spriteCollection.Sprites[0].Data.Length * compositeDisplay.characterSize.X < 1024);
            }

            [Fact]
            public void DualScreenWideDetailWithMultipleStorageOreOnly()
            {
                List<IMyTerminalBlock> blocks =
                [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 0\ndisplayY: 0\ndetail: ore"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 1\ndisplayY: 0\ndetail: ore"
                        },
                        new MockMyCargoContainer {
                            _displayNameText = "storage",
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _acceptedItems = [
                                    MyItemType.MakeOre("uranium"),
                                    MyItemType.MakeOre("platinum"),
                                    MyItemType.MakeComponent("construction"),
                                ],
                                _inventoryItems = [
                                    new MyInventoryItem(MyItemType.MakeOre("uranium"), 0, 394823)
                                ],
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("itemdetail"));
                Assert.Single(s.outputs["itemdetail"]);
                CompositeDisplay compositeDisplay = s.outputs["itemdetail"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should draw 2 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(3, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "uranium thing"
                Assert.Equal(new Vector2(0, screen1.viewport.Bottom - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[2].Position); // "[item detail]"

                Assert.True(mockSurface1._spriteCollection.Sprites[0].Data.Length * compositeDisplay.characterSize.X < 1024);
            }

            [Fact]
            public void DualScreenWideDetailWithMultipleStorageOreAndComponent()
            {
                List<IMyTerminalBlock> blocks =
                [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 0\ndisplayY: 0\ndetail: ore,component"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: itemdetail\ndisplayId: main\ndisplayX: 1\ndisplayY: 0\ndetail: ore,component"
                        },
                        new MockMyCargoContainer {
                            _displayNameText = "storage",
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _acceptedItems = [
                                    MyItemType.MakeOre("uranium"),
                                    MyItemType.MakeOre("platinum"),
                                    MyItemType.MakeComponent("construction"),
                                    MyItemType.MakeAmmo("construction"),
                                ],
                                _inventoryItems = [
                                    new MyInventoryItem(MyItemType.MakeOre("uranium"), 0, 394823)
                                ],
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("itemdetail"));
                Assert.Single(s.outputs["itemdetail"]);
                CompositeDisplay compositeDisplay = s.outputs["itemdetail"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should draw 2 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(4, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "uranium thing"
                Assert.Equal(new Vector2(0, screen1.viewport.Bottom - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[3].Position); // "[item detail]"

                Assert.True(mockSurface1._spriteCollection.Sprites[0].Data.Length * compositeDisplay.characterSize.X < 1024);
            }

            [Fact]
            public void DualScreenWidePower()
            {
                List<IMyTerminalBlock> blocks =
                [
                            new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: power\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: power\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                        },
                        new MockIMyPowerProducer {
                            _customData = "droid",
                            _maxOutput = 1000,
                            _currentOutput = 1000,
                        },
                        new MockIMyBatteryBlock {
                            _customData = "droid",
                            _maxStoredPower = 1000,
                            _currentStoredPower = 500,
                        }
                        ];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                Assert.True(s.outputs.ContainsKey("power"));
                Assert.Single(s.outputs["power"]);
                CompositeDisplay compositeDisplay = s.outputs["power"][0] as CompositeDisplay;
                Assert.NotNull(compositeDisplay);

                // double check the viewport is 1024x512
                Assert.Equal(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

                // this should draw 2 things to the first screen
                Screen screen1 = compositeDisplay.screens[new Point(0, 0)];
                Assert.NotNull(screen1);
                MockMyTextPanel mockSurface1 = screen1.surface as MockMyTextPanel;
                Assert.NotNull(mockSurface1);
                Assert.Equal(3, mockSurface1._spriteCollection.Sprites.Length);

                // Now double check the sprite positions
                Assert.Equal(new Vector2(0, 0), mockSurface1._spriteCollection.Sprites[0].Position); // "current power"
                Assert.Equal(new Vector2(0, screen1.viewport.Bottom - screen1.characterSize.Y), mockSurface1._spriteCollection.Sprites[2].Position); // "[power]"

                Assert.True(mockSurface1._spriteCollection.Sprites[0].Data.Length * compositeDisplay.characterSize.X < 1024);
            }
        }
    }

}