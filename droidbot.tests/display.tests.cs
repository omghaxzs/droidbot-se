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

namespace display
{

    public class MockMyInventory : IMyInventory
    {
        IMyEntity IMyInventory.Owner => throw new NotImplementedException();

        bool IMyInventory.IsFull => throw new NotImplementedException();

        MyFixedPoint IMyInventory.CurrentMass => throw new NotImplementedException();

        public MyFixedPoint _maxVolume;
        MyFixedPoint IMyInventory.MaxVolume => _maxVolume;

        public MyFixedPoint _currentVolume;
        MyFixedPoint IMyInventory.CurrentVolume => _currentVolume;

        int IMyInventory.ItemCount => throw new NotImplementedException();

        float IMyInventory.VolumeFillFactor => throw new NotImplementedException();

        bool IMyInventory.CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType) => throw new NotImplementedException();

        bool IMyInventory.CanTransferItemTo(IMyInventory otherInventory, MyItemType itemType) => throw new NotImplementedException();

        bool IMyInventory.ContainItems(MyFixedPoint amount, MyItemType itemType) => throw new NotImplementedException();

        MyInventoryItem? IMyInventory.FindItem(MyItemType itemType) => throw new NotImplementedException();

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

        public Dictionary<MyItemType, MyFixedPoint> _itemCounts = new Dictionary<MyItemType, MyFixedPoint>();
        MyFixedPoint IMyInventory.GetItemAmount(MyItemType itemType) => _itemCounts.GetValueOrDefault(itemType, 0);

        MyInventoryItem? IMyInventory.GetItemAt(int index) => throw new NotImplementedException();

        MyInventoryItem? IMyInventory.GetItemByID(uint id) => throw new NotImplementedException();

        void IMyInventory.GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter) => throw new NotImplementedException();

        bool IMyInventory.IsConnectedTo(IMyInventory otherInventory) => throw new NotImplementedException();

        bool IMyInventory.IsItemAt(int position) => throw new NotImplementedException();

        bool IMyInventory.TransferItemFrom(IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount) => throw new NotImplementedException();

        bool IMyInventory.TransferItemFrom(IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount) => throw new NotImplementedException();

        bool IMyInventory.TransferItemTo(IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount)
        {
            throw new NotImplementedException();
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

    public class MockMyCargoContainer : IMyCargoContainer
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

        public IMyInventory _inventory = new MockMyInventory();
        IMyInventory IMyEntity.GetInventory()
        {
            return _inventory;
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

    public class MockTerminalBlock : IMyTerminalBlock
    {
        string IMyTerminalBlock.CustomName
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomData
        {
            get => "meow";

            set => throw new NotImplementedException();
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
                                _itemCounts = {
                                    { MyItemType.MakeOre("uranium"), 394823 }
                                },
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
                                _itemCounts = {
                                    { MyItemType.MakeOre("uranium"), 394823 }
                                },
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
                                _itemCounts = {
                                    { MyItemType.MakeOre("uranium"), 394823 }
                                },
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
