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
using Droidbot.Balance;

namespace droidbot.tests
{
    namespace balance
    {

        public class DroidbotBalanceTests
        {
            [Fact]
            public void BalanceTestBasic()
            {
                var cargoContainer = new MockMyCargoContainer
                {
                    _displayNameText = "storage",
                    _customData = "droid",
                    _inventory = new MockMyInventory
                    {
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
                };
                var refinery = new MockMyRefinery
                {
                    _displayNameText = "storage",
                    _customData = "droid",
                    _inputInventory = new MockMyInventory
                    {
                        _acceptedItems = [
                                    MyItemType.MakeOre("uranium"),
                                    MyItemType.MakeOre("platinum"),
                                    MyItemType.MakeComponent("construction"),
                                    MyItemType.MakeAmmo("construction"),
                                ],
                        _inventoryItems = [
                                ],
                        _currentVolume = 0,
                        _maxVolume = 3000
                    }
                };
                List<IMyTerminalBlock> blocks = [cargoContainer, refinery];
                State s = new State(new MockGridProgram(blocks));
                s.Tick();

                // confirm that the refinery now has the uranium instead of the cargo container
                Assert.Empty((cargoContainer._inventory as MockMyInventory)._inventoryItems);
                Assert.NotEmpty((refinery._inputInventory as MockMyInventory)._inventoryItems);
            }
        }
    }
}