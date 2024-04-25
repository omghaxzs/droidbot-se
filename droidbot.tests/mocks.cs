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
using System.Collections.Immutable;

namespace droidbot.tests
{

    public class IMockMyEntity : IMyEntity
    {
        public IMyEntityComponentContainer Components => throw new NotImplementedException();

        public long EntityId => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public bool HasInventory => throw new NotImplementedException();

        public int InventoryCount => throw new NotImplementedException();

        public bool Closed => throw new NotImplementedException();

        public BoundingBoxD WorldAABB => throw new NotImplementedException();

        public BoundingBoxD WorldAABBHr => throw new NotImplementedException();

        public MatrixD WorldMatrix => throw new NotImplementedException();

        public BoundingSphereD WorldVolume => throw new NotImplementedException();

        public BoundingSphereD WorldVolumeHr => throw new NotImplementedException();

        public IMyInventory _inventory = new MockMyInventory();
        public IMyInventory GetInventory()
        {
            return _inventory;
        }

        public IMyInventory GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        public Vector3D GetPosition()
        {
            throw new NotImplementedException();
        }
    }

    public class MockMyRefinery : IMockTerminalBlock, IMyRefinery
    {
        public IMyInventory _inputInventory = new MockMyInventory();
        public IMyInventory InputInventory => _inputInventory;

        public IMyInventory _outputInventory = new MockMyInventory();
        public IMyInventory OutputInventory => _outputInventory;

        public bool IsProducing => throw new NotImplementedException();

        public bool IsQueueEmpty => throw new NotImplementedException();

        public uint NextItemId => throw new NotImplementedException();

        public bool UseConveyorSystem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CustomName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CustomNameWithFaction => throw new NotImplementedException();

        public string DetailedInfo => throw new NotImplementedException();

        public string CustomInfo => throw new NotImplementedException();

        public bool ShowOnHUD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInTerminal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInToolbarConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string DisplayNameText => throw new NotImplementedException();

        public void AddQueueItem(MyDefinitionId blueprint, MyFixedPoint amount)
        {
            throw new NotImplementedException();
        }

        public void AddQueueItem(MyDefinitionId blueprint, decimal amount)
        {
            throw new NotImplementedException();
        }

        public void AddQueueItem(MyDefinitionId blueprint, double amount)
        {
            throw new NotImplementedException();
        }

        public bool CanUseBlueprint(MyDefinitionId blueprint)
        {
            throw new NotImplementedException();
        }

        public void ClearQueue()
        {
            throw new NotImplementedException();
        }

        public void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalAction GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalProperty GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        public void GetQueue(List<MyProductionItem> items)
        {
            throw new NotImplementedException();
        }

        public bool HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public void InsertQueueItem(int idx, MyDefinitionId blueprint, MyFixedPoint amount)
        {
            throw new NotImplementedException();
        }

        public void InsertQueueItem(int idx, MyDefinitionId blueprint, decimal amount)
        {
            throw new NotImplementedException();
        }

        public void InsertQueueItem(int idx, MyDefinitionId blueprint, double amount)
        {
            throw new NotImplementedException();
        }

        public bool IsSameConstructAs(IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        public void MoveQueueItemRequest(uint queueItemId, int targetIdx)
        {
            throw new NotImplementedException();
        }

        public void RemoveQueueItem(int idx, MyFixedPoint amount)
        {
            throw new NotImplementedException();
        }

        public void RemoveQueueItem(int idx, decimal amount)
        {
            throw new NotImplementedException();
        }

        public void RemoveQueueItem(int idx, double amount)
        {
            throw new NotImplementedException();
        }

        public void RequestEnable(bool enable)
        {
            throw new NotImplementedException();
        }

        public void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }
    }

    public class MockMyInventory : IMyInventory
    {
        public List<MyInventoryItem> _inventoryItems = [];

        IMyEntity IMyInventory.Owner => throw new NotImplementedException();

        bool IMyInventory.IsFull => _currentVolume >= _maxVolume;

        MyFixedPoint IMyInventory.CurrentMass => throw new NotImplementedException();

        public MyFixedPoint _maxVolume;
        MyFixedPoint IMyInventory.MaxVolume => _maxVolume;

        public MyFixedPoint _currentVolume;
        MyFixedPoint IMyInventory.CurrentVolume => _currentVolume;

        int IMyInventory.ItemCount => _inventoryItems.Count;

        float IMyInventory.VolumeFillFactor => throw new NotImplementedException();

        bool IMyInventory.CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType) => _currentVolume + (1 * amount) > _maxVolume;

        bool IMyInventory.CanTransferItemTo(IMyInventory otherInventory, MyItemType itemType) => true;

        bool IMyInventory.ContainItems(MyFixedPoint amount, MyItemType itemType) => throw new NotImplementedException();

        MyInventoryItem? IMyInventory.FindItem(MyItemType itemType)
        {
            return _inventoryItems.Any(i => i.Type == itemType) ? _inventoryItems.First(i => i.Type == itemType) : null;
        }

        public List<MyItemType> _acceptedItems = new List<MyItemType>();
        void IMyInventory.GetAcceptedItems(List<MyItemType> itemsTypes, Func<MyItemType, bool> filter)
        {
            foreach (var item in _acceptedItems)
            {
                if (filter == null || (filter != null && filter(item)))
                {
                    itemsTypes.Add(item);
                }
            }
        }

        MyFixedPoint IMyInventory.GetItemAmount(MyItemType itemType)
        {
            MyFixedPoint total = 0;
            foreach (var iitem in _inventoryItems)
            {
                if (iitem.Type == itemType)
                {
                    total += iitem.Amount;
                }
            }
            return total;
        }

        MyInventoryItem? IMyInventory.GetItemAt(int index) => throw new NotImplementedException();

        MyInventoryItem? IMyInventory.GetItemByID(uint id) => throw new NotImplementedException();

        void IMyInventory.GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter) => throw new NotImplementedException();

        bool IMyInventory.IsConnectedTo(IMyInventory otherInventory) => throw new NotImplementedException();

        bool IMyInventory.IsItemAt(int position) => throw new NotImplementedException();

        bool IMyInventory.TransferItemFrom(IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount) => throw new NotImplementedException();

        bool IMyInventory.TransferItemFrom(IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount) => throw new NotImplementedException();

        bool IMyInventory.TransferItemTo(IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount)
        {
            // let's try it
            // take it out of my inventory
            _inventoryItems.RemoveAt((int)item.ItemId);
            var mockInventory = dstInventory as MockMyInventory;
            if (mockInventory != null)
            {
                mockInventory._inventoryItems.Add(item);
            }
            else
            {
                return false;
            }

            return true;
        }

        bool IMyInventory.TransferItemTo(IMyInventory dst, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount)
        {
            throw new NotImplementedException();
        }
    }

    public class MockIMyBatteryBlock : MockIMyPowerProducer, IMyBatteryBlock
    {
        public bool HasCapacityRemaining => throw new NotImplementedException();

        public float _currentStoredPower = 0;
        public float CurrentStoredPower => _currentStoredPower;

        public float _maxStoredPower = 0;
        public float MaxStoredPower => _maxStoredPower;

        public float CurrentInput => throw new NotImplementedException();

        public float MaxInput => throw new NotImplementedException();

        public bool IsCharging => throw new NotImplementedException();

        public ChargeMode _chargeMode = ChargeMode.Auto;
        public ChargeMode ChargeMode { get => _chargeMode; set => _chargeMode = value; }
        public bool OnlyRecharge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool OnlyDischarge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool SemiautoEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class MockIMyPowerProducer : IMyPowerProducer
    {
        public float _currentOutput = 0;
        public float CurrentOutput => _currentOutput;

        public float _maxOutput = 0;
        public float MaxOutput => _maxOutput;

        public float CurrentOutputRatio => _maxOutput > 0 ? _currentOutput / _maxOutput : 0;

        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CustomName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CustomNameWithFaction => throw new NotImplementedException();

        public string DetailedInfo => throw new NotImplementedException();

        public string CustomInfo => throw new NotImplementedException();

        public string _customData = "";
        string IMyTerminalBlock.CustomData { get => _customData; set => _customData = value; }

        public bool ShowOnHUD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInTerminal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInToolbarConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SerializableDefinitionId BlockDefinition => throw new NotImplementedException();

        public IMyCubeGrid CubeGrid => throw new NotImplementedException();

        public string DefinitionDisplayNameText => throw new NotImplementedException();

        public float DisassembleRatio => throw new NotImplementedException();

        public string DisplayNameText => throw new NotImplementedException();

        public bool IsBeingHacked => throw new NotImplementedException();

        public bool IsFunctional => throw new NotImplementedException();

        public bool IsWorking => throw new NotImplementedException();

        public Vector3I Max => throw new NotImplementedException();

        public float Mass => throw new NotImplementedException();

        public Vector3I Min => throw new NotImplementedException();

        public int NumberInGrid => throw new NotImplementedException();

        public MyBlockOrientation Orientation => throw new NotImplementedException();

        public long OwnerId => throw new NotImplementedException();

        public Vector3I Position => throw new NotImplementedException();

        public IMyEntityComponentContainer Components => throw new NotImplementedException();

        public long EntityId => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public bool HasInventory => throw new NotImplementedException();

        public int InventoryCount => throw new NotImplementedException();

        public bool Closed => throw new NotImplementedException();

        public BoundingBoxD WorldAABB => throw new NotImplementedException();

        public BoundingBoxD WorldAABBHr => throw new NotImplementedException();

        public MatrixD WorldMatrix => throw new NotImplementedException();

        public BoundingSphereD WorldVolume => throw new NotImplementedException();

        public BoundingSphereD WorldVolumeHr => throw new NotImplementedException();

        public void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalAction GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory()
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        public string GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        public Vector3D GetPosition()
        {
            throw new NotImplementedException();
        }

        public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalProperty GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public bool HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public bool IsSameConstructAs(IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        public void RequestEnable(bool enable)
        {
            throw new NotImplementedException();
        }

        public void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        public void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }

    public class IMockMyCubeBlock : IMockMyEntity, IMyCubeBlock
    {
        public SerializableDefinitionId BlockDefinition => throw new NotImplementedException();

        public IMyCubeGrid CubeGrid => throw new NotImplementedException();

        public string DefinitionDisplayNameText => throw new NotImplementedException();

        public float DisassembleRatio => throw new NotImplementedException();

        public string _displayNameText = "";
        string IMyCubeBlock.DisplayNameText => _displayNameText;

        public bool IsBeingHacked => throw new NotImplementedException();

        public bool IsFunctional => throw new NotImplementedException();

        public bool IsWorking => throw new NotImplementedException();

        public Vector3I Max => throw new NotImplementedException();

        public float Mass => throw new NotImplementedException();

        public Vector3I Min => throw new NotImplementedException();

        public int NumberInGrid => throw new NotImplementedException();

        public MyBlockOrientation Orientation => throw new NotImplementedException();

        public long OwnerId => throw new NotImplementedException();

        public Vector3I Position => throw new NotImplementedException();

        public string GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }

    public class MockMyCargoContainer : IMockMyCubeBlock, IMyCargoContainer
    {

        string IMyTerminalBlock.CustomName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        public string _customData = "";
        string IMyTerminalBlock.CustomData
        {
            get => _customData;
            set => _customData = value;
        }
        bool IMyTerminalBlock.ShowOnHUD
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInTerminal
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInToolbarConfig
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInInventory
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        SerializableDefinitionId IMyCubeBlock.BlockDefinition => throw new NotImplementedException();

        IMyCubeGrid IMyCubeBlock.CubeGrid => throw new NotImplementedException();

        string IMyCubeBlock.DefinitionDisplayNameText => throw new NotImplementedException();

        float IMyCubeBlock.DisassembleRatio => throw new NotImplementedException();

        bool IMyCubeBlock.IsBeingHacked => throw new NotImplementedException();

        bool IMyCubeBlock.IsFunctional => throw new NotImplementedException();

        bool IMyCubeBlock.IsWorking => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Max => throw new NotImplementedException();

        float IMyCubeBlock.Mass => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Min => throw new NotImplementedException();

        int IMyCubeBlock.NumberInGrid => throw new NotImplementedException();

        MyBlockOrientation IMyCubeBlock.Orientation => throw new NotImplementedException();

        long IMyCubeBlock.OwnerId => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Position => throw new NotImplementedException();

        IMyEntityComponentContainer IMyEntity.Components => throw new NotImplementedException();

        long IMyEntity.EntityId => throw new NotImplementedException();

        string IMyEntity.Name => throw new NotImplementedException();

        string IMyEntity.DisplayName => throw new NotImplementedException();

        bool IMyEntity.HasInventory => throw new NotImplementedException();

        int IMyEntity.InventoryCount => throw new NotImplementedException();

        bool IMyEntity.Closed => throw new NotImplementedException();

        BoundingBoxD IMyEntity.WorldAABB => throw new NotImplementedException();

        BoundingBoxD IMyEntity.WorldAABBHr => throw new NotImplementedException();

        MatrixD IMyEntity.WorldMatrix => throw new NotImplementedException();

        BoundingSphereD IMyEntity.WorldVolume => throw new NotImplementedException();

        BoundingSphereD IMyEntity.WorldVolumeHr => throw new NotImplementedException();

        void IMyTerminalBlock.GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect) => throw new NotImplementedException();

        ITerminalAction IMyTerminalBlock.GetActionWithName(string name) => throw new NotImplementedException();

        IMyInventory IMyEntity.GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        string IMyCubeBlock.GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        MyRelationsBetweenPlayerAndBlock IMyCubeBlock.GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        Vector3D IMyEntity.GetPosition()
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect)
        {
            throw new NotImplementedException();
        }

        ITerminalProperty IMyTerminalBlock.GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        MyRelationsBetweenPlayerAndBlock IMyCubeBlock.GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.IsSameConstructAs(IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        void IMyCubeBlock.UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        void IMyCubeBlock.UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }

    public class MockMyTextPanel : IMyTextPanel
    {
        string IMyTextSurface.CurrentlyShownImage => throw new NotImplementedException();

        float IMyTextSurface.FontSize
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        Color IMyTextSurface.FontColor
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        Color IMyTextSurface.BackgroundColor
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        byte IMyTextSurface.BackgroundAlpha
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        float IMyTextSurface.ChangeInterval
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        string IMyTextSurface.Font
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        TextAlignment IMyTextSurface.Alignment
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        public string _script = "";
        string IMyTextSurface.Script
        {
            get => _script;

            set => _script = value;
        }

        public ContentType _contentType = ContentType.NONE;
        ContentType IMyTextSurface.ContentType
        {
            get => _contentType;

            set => _contentType = value;
        }

        Vector2 IMyTextSurface.SurfaceSize => new Vector2(512, 512);


        Vector2 IMyTextSurface.TextureSize => new Vector2(512, 512);

        bool IMyTextSurface.PreserveAspectRatio
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        float IMyTextSurface.TextPadding
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        Color IMyTextSurface.ScriptBackgroundColor
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        public Color _scriptForegroundColor = Color.White;
        Color IMyTextSurface.ScriptForegroundColor
        {
            get => _scriptForegroundColor;

            set => _scriptForegroundColor = value;
        }

        string IMyTextSurface.Name => throw new NotImplementedException();

        string IMyEntity.Name => throw new NotImplementedException();

        string IMyTextSurface.DisplayName => throw new NotImplementedException();

        string IMyEntity.DisplayName => throw new NotImplementedException();

        bool IMyFunctionalBlock.Enabled
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        string IMyTerminalBlock.CustomName
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        public string _customData = "";
        string IMyTerminalBlock.CustomData
        {
            get => _customData;

            set => _customData = value;
        }
        bool IMyTerminalBlock.ShowOnHUD
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInTerminal
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInToolbarConfig
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInInventory
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        SerializableDefinitionId IMyCubeBlock.BlockDefinition => throw new NotImplementedException();

        IMyCubeGrid IMyCubeBlock.CubeGrid => throw new NotImplementedException();

        string IMyCubeBlock.DefinitionDisplayNameText => throw new NotImplementedException();

        float IMyCubeBlock.DisassembleRatio => throw new NotImplementedException();

        public string _displayNameText = "";
        string IMyCubeBlock.DisplayNameText => _displayNameText;

        bool IMyCubeBlock.IsBeingHacked => throw new NotImplementedException();

        bool IMyCubeBlock.IsFunctional => throw new NotImplementedException();

        bool IMyCubeBlock.IsWorking => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Max => throw new NotImplementedException();

        float IMyCubeBlock.Mass => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Min => throw new NotImplementedException();

        int IMyCubeBlock.NumberInGrid => throw new NotImplementedException();

        MyBlockOrientation IMyCubeBlock.Orientation => throw new NotImplementedException();

        long IMyCubeBlock.OwnerId => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Position => throw new NotImplementedException();

        IMyEntityComponentContainer IMyEntity.Components => throw new NotImplementedException();

        long IMyEntity.EntityId => throw new NotImplementedException();

        bool IMyEntity.HasInventory => throw new NotImplementedException();

        int IMyEntity.InventoryCount => throw new NotImplementedException();

        bool IMyEntity.Closed => throw new NotImplementedException();

        BoundingBoxD IMyEntity.WorldAABB => throw new NotImplementedException();

        BoundingBoxD IMyEntity.WorldAABBHr => throw new NotImplementedException();

        MatrixD IMyEntity.WorldMatrix => throw new NotImplementedException();

        BoundingSphereD IMyEntity.WorldVolume => throw new NotImplementedException();

        BoundingSphereD IMyEntity.WorldVolumeHr => throw new NotImplementedException();

        void IMyTextSurface.AddImagesToSelection(List<string> ids, bool checkExistence)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.AddImageToSelection(string id, bool checkExistence)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.ClearImagesFromSelection()
        {
            throw new NotImplementedException();
        }

        public MySpriteDrawFrame _frame;
        public MySpriteCollection _spriteCollection;
        MySpriteDrawFrame IMyTextSurface.DrawFrame()
        {
            _frame = new MySpriteDrawFrame(f =>
            {
                // dump out the collection
                _spriteCollection = f.ToCollection();
            });
            return _frame;
        }

        void IMyTerminalBlock.GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect)
        {
            throw new NotImplementedException();
        }

        ITerminalAction IMyTerminalBlock.GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.GetFonts(List<string> fonts)
        {
            throw new NotImplementedException();
        }

        IMyInventory IMyEntity.GetInventory()
        {
            throw new NotImplementedException();
        }

        IMyInventory IMyEntity.GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        string IMyCubeBlock.GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        MyRelationsBetweenPlayerAndBlock IMyCubeBlock.GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        Vector3D IMyEntity.GetPosition()
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect)
        {
            throw new NotImplementedException();
        }

        ITerminalProperty IMyTerminalBlock.GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        string IMyTextPanel.GetPublicTitle()
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.GetScripts(List<string> scripts)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.GetSelectedImages(List<string> output)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.GetSprites(List<string> sprites)
        {
            throw new NotImplementedException();
        }

        string IMyTextSurface.GetText()
        {
            throw new NotImplementedException();
        }

        MyRelationsBetweenPlayerAndBlock IMyCubeBlock.GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.IsSameConstructAs(IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        Vector2 IMyTextSurface.MeasureStringInPixels(StringBuilder text, string font, float scale)
        {
            scale *= 0.77837837f;
            return new Vector2((30 * scale) * text.Length, 42 * scale);
        }

        void IMyTextSurface.ReadText(StringBuilder buffer, bool append)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.RemoveImageFromSelection(string id, bool removeDuplicates)
        {
            throw new NotImplementedException();
        }

        void IMyTextSurface.RemoveImagesFromSelection(List<string> ids, bool removeDuplicates)
        {
            throw new NotImplementedException();
        }

        void IMyFunctionalBlock.RequestEnable(bool enable)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        void IMyCubeBlock.UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        void IMyCubeBlock.UpdateVisual()
        {
            throw new NotImplementedException();
        }

        bool IMyTextPanel.WritePublicTitle(string value, bool append)
        {
            throw new NotImplementedException();
        }

        bool IMyTextSurface.WriteText(string value, bool append)
        {
            throw new NotImplementedException();
        }

        bool IMyTextSurface.WriteText(StringBuilder value, bool append)
        {
            throw new NotImplementedException();
        }
    }

    public class IMockTerminalBlock : IMockMyCubeBlock, IMyTerminalBlock
    {
        string IMyTerminalBlock.CustomName
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        public string _customData = "";
        public string CustomData { get => _customData; set => _customData = value; }
        bool IMyTerminalBlock.ShowOnHUD
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInTerminal
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInToolbarConfig
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }
        bool IMyTerminalBlock.ShowInInventory
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        SerializableDefinitionId IMyCubeBlock.BlockDefinition => throw new NotImplementedException();

        IMyCubeGrid IMyCubeBlock.CubeGrid => throw new NotImplementedException();

        string IMyCubeBlock.DefinitionDisplayNameText => throw new NotImplementedException();

        float IMyCubeBlock.DisassembleRatio => throw new NotImplementedException();

        string IMyCubeBlock.DisplayNameText => throw new NotImplementedException();

        bool IMyCubeBlock.IsBeingHacked => throw new NotImplementedException();

        bool IMyCubeBlock.IsFunctional => throw new NotImplementedException();

        bool IMyCubeBlock.IsWorking => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Max => throw new NotImplementedException();

        float IMyCubeBlock.Mass => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Min => throw new NotImplementedException();

        int IMyCubeBlock.NumberInGrid => throw new NotImplementedException();

        MyBlockOrientation IMyCubeBlock.Orientation => throw new NotImplementedException();

        long IMyCubeBlock.OwnerId => throw new NotImplementedException();

        Vector3I IMyCubeBlock.Position => throw new NotImplementedException();

        IMyEntityComponentContainer IMyEntity.Components => throw new NotImplementedException();

        long IMyEntity.EntityId => throw new NotImplementedException();

        string IMyEntity.Name => throw new NotImplementedException();

        string IMyEntity.DisplayName => throw new NotImplementedException();

        bool IMyEntity.HasInventory => throw new NotImplementedException();

        int IMyEntity.InventoryCount => throw new NotImplementedException();

        bool IMyEntity.Closed => throw new NotImplementedException();

        BoundingBoxD IMyEntity.WorldAABB => throw new NotImplementedException();

        BoundingBoxD IMyEntity.WorldAABBHr => throw new NotImplementedException();

        MatrixD IMyEntity.WorldMatrix => throw new NotImplementedException();

        BoundingSphereD IMyEntity.WorldVolume => throw new NotImplementedException();

        BoundingSphereD IMyEntity.WorldVolumeHr => throw new NotImplementedException();

        void IMyTerminalBlock.GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect)
        {
            throw new NotImplementedException();
        }

        ITerminalAction IMyTerminalBlock.GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        IMyInventory IMyEntity.GetInventory()
        {
            throw new NotImplementedException();
        }

        IMyInventory IMyEntity.GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        string IMyCubeBlock.GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        MyRelationsBetweenPlayerAndBlock IMyCubeBlock.GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        Vector3D IMyEntity.GetPosition()
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect)
        {
            throw new NotImplementedException();
        }

        ITerminalProperty IMyTerminalBlock.GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        MyRelationsBetweenPlayerAndBlock IMyCubeBlock.GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
        {
            throw new NotImplementedException();
        }

        bool IMyTerminalBlock.IsSameConstructAs(IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        void IMyTerminalBlock.SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        void IMyCubeBlock.UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        void IMyCubeBlock.UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }

    public class MockGridTerminalSystem : IMyGridTerminalSystem
    {

        public List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

        public MockGridTerminalSystem(List<IMyTerminalBlock> blocks)
        {
            this.blocks = blocks;
        }

        public bool CanAccess(IMyTerminalBlock block, MyTerminalAccessScope scope = MyTerminalAccessScope.All)
        {
            throw new NotImplementedException();
        }

        public bool CanAccess(IMyCubeGrid grid, MyTerminalAccessScope scope = MyTerminalAccessScope.All)
        {
            throw new NotImplementedException();
        }

        public void GetBlockGroups(List<IMyBlockGroup> blockGroups, Func<IMyBlockGroup, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public IMyBlockGroup GetBlockGroupWithName(string name)
        {
            throw new NotImplementedException();
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            throw new NotImplementedException();
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            foreach (IMyTerminalBlock block in this.blocks)
            {
                if (block as T != null && collect(block as T))
                {
                    blocks.Add(block as T);
                }
            }
        }

        public IMyTerminalBlock GetBlockWithId(long id)
        {
            throw new NotImplementedException();
        }

        public IMyTerminalBlock GetBlockWithName(string name)
        {
            throw new NotImplementedException();
        }

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            throw new NotImplementedException();
        }
    }

    public class MockGridProgram : MyGridProgram
    {
        public MockGridProgram(List<IMyTerminalBlock> blocks)
        {
            GridTerminalSystem = new MockGridTerminalSystem(blocks);
            Echo = s => Console.WriteLine(s);
        }
    }
}