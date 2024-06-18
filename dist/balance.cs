
using VRage.Game;
using Sandbox.Definitions;


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

        public Dictionary<MyItemType, MyFixedPoint> itemCounts = new Dictionary<MyItemType, MyFixedPoint>();
        public Dictionary<MyDefinitionId, MyFixedPoint> assemblerQueueCounts = new Dictionary<MyDefinitionId, MyFixedPoint>();

        public Dictionary<MyItemType, MyDefinitionId> itemBlueprintMap = new Dictionary<MyItemType, MyDefinitionId>();
        public List<MyItemType> undiscoverableBlueprints = new List<MyItemType>();

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
            this.assemblers.Clear();

            // grab all storage
            this.grid.GetBlocksOfType(this.storage, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // grab all refineries
            this.grid.GetBlocksOfType(this.refineries, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // grab all connectors
            this.grid.GetBlocksOfType(this.connectors, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // grab all assemblers
            this.grid.GetBlocksOfType(this.assemblers, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            // get item types and put em in our list
            foreach (var storage in this.storage)
            {
                var acceptedItems = new List<MyItemType>();
                storage.GetInventory().GetAcceptedItems(acceptedItems, it => it.TypeId == "MyObjectBuilder_Ore" || it.TypeId == "MyObjectBuilder_Ingot" || it.TypeId == "MyObjectBuilder_Component" || it.TypeId == "MyObjectBuilder_AmmoMagazine");
                foreach (var itemType in acceptedItems)
                {
                    if (!this.itemTypes.Contains(itemType))
                    {
                        this.itemTypes.Add(itemType);
                    }
                }
            }

            RefreshItemCounts();
            RefreshAssemblerQueueCounts();
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

        public void RefreshAssemblerQueueCounts()
        {
            foreach (var assembler in this.assemblers)
            {
                var queueItems = new List<MyProductionItem>();
                assembler.GetQueue(queueItems);
                foreach (var queueItem in queueItems)
                {
                    if (this.assemblerQueueCounts.ContainsKey(queueItem.BlueprintId))
                    {
                        this.assemblerQueueCounts[queueItem.BlueprintId] += queueItem.Amount;
                    }
                    else
                    {
                        this.assemblerQueueCounts[queueItem.BlueprintId] = queueItem.Amount;
                    }
                }
            }
        }

        public void Tick()
        {
            RefreshItemCounts();
            RefreshAssemblerQueueCounts();
            if (tick % 10 == 0)
            {
                ScanAllResources();
            }

            TransferOreToRefineries();
            TransferIngotsFromRefineriesToStorage();
            TransferStuffFromConnectorsToStorage();
            TransferComponentsFromAssemblersToStorage();

            EnsureItemTargets();

            tick++;
        }

        private void TransferComponentsFromAssemblersToStorage()
        {
            // Go through each type of component
            foreach (var itemType in this.itemTypes)
            {
                if (itemType.TypeId == "MyObjectBuilder_Component" || itemType.TypeId == "MyObjectBuilder_AmmoMagazine")
                {
                    // Now go through each of our assembler output inventory
                    foreach (var assembler in this.assemblers)
                    {
                        // does it have some?
                        var itemAmount = assembler.OutputInventory.GetItemAmount(itemType);
                        if (itemAmount > 0)
                        {
                            // Let's put it somewhere
                            TransferToSomeStorage(assembler.OutputInventory, itemType, itemAmount);
                        }
                    }
                }
            }
        }

        private void EnsureItemTargets()
        {
            // Go through each type of component
            foreach (var itemType in this.itemTypes)
            {
                if (itemType.TypeId == "MyObjectBuilder_Component" || itemType.TypeId == "MyObjectBuilder_Ingot" || itemType.TypeId == "MyObjectBuilder_AmmoMagazine")
                {
                    if (this.itemCounts[itemType] <= 1000)
                    {
                        AssembleSomething(itemType, 10);
                    }
                }
            }
        }

        private bool AssembleSomething(MyItemType itemType, MyFixedPoint v)
        {
            // go through each of our assemblers, sorted by whats least
            if (this.assemblers.Count > 0)
            {
                var assembler = assemblers[tick % this.assemblers.Count];
                var blueprint = DiscoverBlueprint(assembler, itemType);
                if (blueprint == null)
                {
                    // if we could not get the blueprint, just bail
                    return false;
                }
                var queueItems = new List<MyProductionItem>();
                assembler.GetQueue(queueItems);
                foreach (var queueItem in queueItems)
                {
                    if (queueItem.BlueprintId == blueprint)
                    {
                        // nope, abort
                        return false;
                    }
                }

                assembler.AddQueueItem((MyDefinitionId)blueprint, v);
                return true;
            }

            return false;
        }

        private MyDefinitionId? DiscoverBlueprint(IMyAssembler assembler, MyItemType itemType)
        {
            // fast path
            if (itemBlueprintMap.ContainsKey(itemType))
            {
                return this.itemBlueprintMap[itemType];
            }

            if (this.undiscoverableBlueprints.Contains(itemType))
            {
                return null;
            }

            // try a bunch
            // holy fuck slow path
            var attempts = new List<MyDefinitionId> {
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + itemType.SubtypeId + "Component"),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Farmed" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0005_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0010_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0020_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0030_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0040_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0050_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0060_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0070_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0080_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0090_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0100_" + itemType.SubtypeId),
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/Position0110_" + itemType.SubtypeId),
            };

            foreach (var attempt in attempts)
            {
                if (assembler.CanUseBlueprint(attempt))
                {
                    // this is the one, save it
                    this.itemBlueprintMap[itemType] = attempt;
                    return attempt;
                }
            }

            // if we've made it here, we've not discovered the blueprint for this guy
            this.undiscoverableBlueprints.Add(itemType);

            return null;
        }

        private void TransferStuffFromConnectorsToStorage()
        {
            // Now go through each of our connectors
            foreach (var connector in this.connectors)
            {
                var inventory = connector.GetInventory();
                // Go through each type of item
                foreach (var itemType in this.itemTypes)
                {
                    // does it have some?
                    var itemAmount = inventory.GetItemAmount(itemType);
                    if (itemAmount > 0)
                    {
                        // Let's put it somewhere
                        TransferToSomeStorage(inventory, itemType, itemAmount);
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
            // go through our storage
            if (this.storage.Count > 0)
            {
                var randoStorage = this.storage[tick % this.storage.Count];
                var storageInventory = randoStorage.GetInventory();
                // skip if its full, the item can't be added, or if there's no conveyor connection to it
                if (!storageInventory.IsFull && storageInventory.CanItemsBeAdded(amount, itemType) && fromInventory.CanTransferItemTo(storageInventory, itemType))
                {
                    // get the item
                    var inventoryItem = fromInventory.FindItem(itemType);
                    if (inventoryItem.HasValue)
                    {
                        var res = fromInventory.TransferItemTo(storageInventory, inventoryItem.Value, amount);
                        // if it failed, try to transfer just a little bit and then bail
                        if (!res)
                        {
                            fromInventory.TransferItemTo(storageInventory, inventoryItem.Value, 1);
                        }
                    }
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
            Echo("last runtime: " + Runtime.LastRunTimeMs);
        }
public class DroidbotEnums
{
    public const int EVENT_MOVED_ITEMS = 1;
};
