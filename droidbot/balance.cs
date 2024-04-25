using System; // FILTER
using System.Linq; // FILTER
using System.Text; // FILTER
using System.Collections.Generic; // FILTER

using VRageMath; // FILTER
using Sandbox.ModAPI.Ingame; // FILTER
using VRage.Game.GUI.TextPanel; // FILTER
using VRage; // FILTER
using VRage.Game.ModAPI.Ingame; // FILTER

namespace Droidbot.Balance // FILTER
{ // FILTER
    public class State
    {
        public List<IMyCargoContainer> storage = new List<IMyCargoContainer>();
        public List<IMyRefinery> refineries = new List<IMyRefinery>();
        public List<IMyShipConnector> connectors = new List<IMyShipConnector>();
        public List<IMyAssembler> assemblers = new List<IMyAssembler>();
        public List<MyItemType> itemTypes = new List<MyItemType>();

        public IMyGridTerminalSystem grid;
        public MyGridProgram prog;

        public short tick = 0;

        public State(MyGridProgram p)
        {
            this.prog = p;
            this.grid = p.GridTerminalSystem;

            ScanAllResources();
        }

        public void Log(string text)
        {
            this.prog.Echo(text);
        }

        public void ScanAllResources()
        {
            this.storage.Clear();
            this.refineries.Clear();
            this.connectors.Clear();
            this.itemTypes.Clear();

            // grab all storage
            this.grid.GetBlocksOfType(this.storage, s => s.CustomData.StartsWith("droid"));

            // grab all refineries
            this.grid.GetBlocksOfType(this.refineries, s => s.CustomData.StartsWith("droid"));

            // grab all connectors
            this.grid.GetBlocksOfType(this.connectors, s => s.CustomData.StartsWith("droid"));

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
                    }
                }
            }
        }

        public void Tick()
        {
            if (tick % 10 == 0)
            {
                ScanAllResources();
            }

            TransferOreToRefineries();

            tick++;
        }

        private void TransferOreToRefineries()
        {
            // Go through each type of ore
            foreach (var itemType in this.itemTypes)
            {
                if (itemType.TypeId == "MyObjectBuilder_Ore")
                {
                    // Now go through each of our storage units to see if it has any
                    foreach (var storage in this.storage)
                    {
                        // does it have some?
                        var itemAmount = storage.GetInventory().GetItemAmount(itemType);
                        if (itemAmount > 0)
                        {
                            // Let's put it somewhere
                            TransferOreFromStorageToARefinery(storage, itemType, itemAmount);
                        }
                    }
                }
            }
        }

        private void TransferOreFromStorageToARefinery(IMyCargoContainer storage, MyItemType itemType, MyFixedPoint amount)
        {
            // go through our refineries
            foreach (var refinery in this.refineries)
            {
                var refineryInventory = refinery.InputInventory;
                // skip if its full, the item can't be added, or if there's no conveyor connection to it
                if (!refineryInventory.IsFull && refineryInventory.CanItemsBeAdded(amount, itemType) && storage.GetInventory().CanTransferItemTo(refineryInventory, itemType))
                {
                    // get the item
                    var inventoryItem = storage.GetInventory().FindItem(itemType);
                    if (inventoryItem.HasValue)
                    {
                        var res = storage.GetInventory().TransferItemTo(refineryInventory, inventoryItem.Value);
                        // if it failed, try to transfer just a little bit and then bail
                        if (!res)
                        {
                            storage.GetInventory().TransferItemTo(refineryInventory, inventoryItem.Value, 1);
                        }
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
    };

    public class Program : MyGridProgram // FILTER
    { // FILTER
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
    } // FILTER
} // FILTER
