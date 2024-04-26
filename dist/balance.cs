


// droidbot-se
// balance

// this script is designed to move items around different places to keep the conveyor system busy and nothing gets stuck
// current functionality: 
// - it will move items from marked Connector block inventories to marked storage
// - it will move ore items from marked storage to any marked refineries
// - it will move ingots from any marked refineries to marked storage

// "marked" in this context means blocks that have custom data starting with "droid". So a storage block with the custom data starting with "droid"
// will be considered marked storage. Same for refinery blocks, connector blocks, etc...
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
            TransferIngotsFromRefineriesToStorage();
            TransferStuffFromConnectorsToStorage();

            tick++;
        }

        private void TransferStuffFromConnectorsToStorage()
        {
            // Go through each type of ingot
            foreach (var itemType in this.itemTypes)
            {
                // Now go through each of our connectors
                foreach (var connector in this.connectors)
                {
                    // does it have some?
                    var itemAmount = connector.GetInventory().GetItemAmount(itemType);
                    if (itemAmount > 0)
                    {
                        // Let's put it somewhere
                        TransferToSomeStorage(connector.GetInventory(), itemType, itemAmount);
                    }
                }
            }
        }

        private void TransferIngotsFromRefineriesToStorage()
        {
            // Go through each type of ingot
            foreach (var itemType in this.itemTypes)
            {
                if (itemType.TypeId == "MyObjectBuilder_Ingot")
                {
                    // Now go through each of our refinery output inventory
                    foreach (var refinery in this.refineries)
                    {
                        // does it have some?
                        var itemAmount = refinery.OutputInventory.GetItemAmount(itemType);
                        if (itemAmount > 0)
                        {
                            // Let's put it somewhere
                            TransferToSomeStorage(refinery.OutputInventory, itemType, itemAmount);
                        }
                    }
                }
            }
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
                            TransferFromStorageToARefinery(storage, itemType, itemAmount);
                        }
                    }
                }
            }
        }

        private void TransferToSomeStorage(IMyInventory fromInventory, MyItemType itemType, MyFixedPoint amount)
        {
            MyFixedPoint amountLeft = amount;
            // go through our storage
            foreach (var storage in this.storage)
            {
                var amountToTransfer = amountLeft;
                var storageInventory = storage.GetInventory();
                // skip if its full, the item can't be added, or if there's no conveyor connection to it
                if (!storageInventory.IsFull && storageInventory.CanItemsBeAdded(amount, itemType) && fromInventory.CanTransferItemTo(storageInventory, itemType))
                {
                    // get the item
                    var inventoryItem = fromInventory.FindItem(itemType);
                    if (inventoryItem.HasValue)
                    {
                        var res = fromInventory.TransferItemTo(storageInventory, inventoryItem.Value, amountToTransfer);
                        // if it failed, try to transfer just a little bit and then bail
                        if (!res)
                        {
                            amountToTransfer = 1;
                            if (fromInventory.TransferItemTo(storageInventory, inventoryItem.Value, amountToTransfer))
                            {
                                amountToTransfer = 0;
                            }
                        }
                    }
                }

                amountLeft -= amountToTransfer;

                // is there any left?
                if (amountLeft <= 0)
                {
                    break;
                }
            }
        }

        private void TransferFromStorageToARefinery(IMyCargoContainer storage, MyItemType itemType, MyFixedPoint amount)
        {
            MyFixedPoint amountLeft = amount;
            // go through our refineries
            foreach (var refinery in this.refineries)
            {
                var amountToTransfer = amountLeft;
                var refineryInventory = refinery.InputInventory;
                // skip if its full, the item can't be added, or if there's no conveyor connection to it
                if (!refineryInventory.IsFull && refineryInventory.CanItemsBeAdded(amount, itemType) && storage.GetInventory().CanTransferItemTo(refineryInventory, itemType))
                {
                    // get the item
                    var inventoryItem = storage.GetInventory().FindItem(itemType);
                    if (inventoryItem.HasValue)
                    {
                        var res = storage.GetInventory().TransferItemTo(refineryInventory, inventoryItem.Value, amountToTransfer);
                        // if it failed, try to transfer just a little bit and then bail
                        if (!res)
                        {
                            amountToTransfer = 1;
                            if (storage.GetInventory().TransferItemTo(refineryInventory, inventoryItem.Value, amountToTransfer))
                            {
                                amountToTransfer = 0;
                            }
                        }
                    }
                }

                amountLeft -= amountToTransfer;

                // is there any left?
                if (amountLeft <= 0)
                {
                    break;
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
public class DroidbotEnums
{
    public const int EVENT_MOVED_ITEMS = 1;
};