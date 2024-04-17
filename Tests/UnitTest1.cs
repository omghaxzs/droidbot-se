using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Droidbot.Display;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using System.Collections.Generic;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Game.Components.Interfaces;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using System.Text;
using VRage.Game.GUI.TextPanel;
using System.Diagnostics;
using Sandbox.Game;
using VRage;

namespace Tests
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

        bool IMyInventory.CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.CanTransferItemTo(IMyInventory otherInventory, MyItemType itemType)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.ContainItems(MyFixedPoint amount, MyItemType itemType)
        {
            throw new NotImplementedException();
        }

        MyInventoryItem? IMyInventory.FindItem(MyItemType itemType)
        {
            throw new NotImplementedException();
        }

        void IMyInventory.GetAcceptedItems(List<MyItemType> itemsTypes, Func<MyItemType, bool> filter)
        {
            throw new NotImplementedException();
        }

        MyFixedPoint IMyInventory.GetItemAmount(MyItemType itemType)
        {
            throw new NotImplementedException();
        }

        MyInventoryItem? IMyInventory.GetItemAt(int index)
        {
            throw new NotImplementedException();
        }

        MyInventoryItem? IMyInventory.GetItemByID(uint id)
        {
            throw new NotImplementedException();
        }

        void IMyInventory.GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.IsConnectedTo(IMyInventory otherInventory)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.IsItemAt(int position)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.TransferItemFrom(IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.TransferItemFrom(IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.TransferItemTo(IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount)
        {
            throw new NotImplementedException();
        }

        bool IMyInventory.TransferItemTo(IMyInventory dst, int sourceItemIndex, int? targetItemIndex, bool? stackIfPossible, MyFixedPoint? amount)
        {
            throw new NotImplementedException();
        }
    }

    public class MockMyCargoContainer : IMyCargoContainer
    {

        string IMyTerminalBlock.CustomName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        public string _customData = "";
        string IMyTerminalBlock.CustomData { get => _customData; set => _customData = value; }
        bool IMyTerminalBlock.ShowOnHUD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInTerminal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInToolbarConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        void IMyTerminalBlock.GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect)
        {
            throw new NotImplementedException();
        }

        ITerminalAction IMyTerminalBlock.GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

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

        float IMyTextSurface.FontSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Color IMyTextSurface.FontColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Color IMyTextSurface.BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        byte IMyTextSurface.BackgroundAlpha { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        float IMyTextSurface.ChangeInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IMyTextSurface.Font { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        TextAlignment IMyTextSurface.Alignment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string _script = "";
        string IMyTextSurface.Script { get => _script; set => _script = value; }

        public ContentType _contentType = ContentType.NONE;
        ContentType IMyTextSurface.ContentType { get => _contentType; set => _contentType = value; }

        Vector2 IMyTextSurface.SurfaceSize => new Vector2(512, 512);


        Vector2 IMyTextSurface.TextureSize => new Vector2(512, 512);

        bool IMyTextSurface.PreserveAspectRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        float IMyTextSurface.TextPadding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Color IMyTextSurface.ScriptBackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Color IMyTextSurface.ScriptForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IMyTextSurface.Name => throw new NotImplementedException();

        string IMyEntity.Name => throw new NotImplementedException();

        string IMyTextSurface.DisplayName => throw new NotImplementedException();

        string IMyEntity.DisplayName => throw new NotImplementedException();

        bool IMyFunctionalBlock.Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IMyTerminalBlock.CustomName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        public string _customData = "";
        string IMyTerminalBlock.CustomData { get => _customData; set => _customData = value; }
        bool IMyTerminalBlock.ShowOnHUD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInTerminal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInToolbarConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        public MySpriteCollection _spriteCollection = new MySpriteCollection();
        MySpriteDrawFrame IMyTextSurface.DrawFrame()
        {
            this._frame = new MySpriteDrawFrame(f =>
            {
                // dump out the collection
                this._spriteCollection = f.ToCollection();
            });
            return this._frame;
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
            return new Vector2(10, 10);
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
        string IMyTerminalBlock.CustomName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IMyTerminalBlock.CustomNameWithFaction => throw new NotImplementedException();

        string IMyTerminalBlock.DetailedInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomInfo => throw new NotImplementedException();

        string IMyTerminalBlock.CustomData { get => "meow"; set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowOnHUD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInTerminal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInToolbarConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IMyTerminalBlock.ShowInInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            foreach (var block in this.blocks)
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
            this.GridTerminalSystem = new MockGridTerminalSystem(blocks);
            this.Echo = s => Console.WriteLine(s);
        }
    }

    [TestClass]
    public class DroidbotDisplayTests
    {
        [TestMethod]
        public void ScreenScanning()
        {
            var blocks = new List<IMyTerminalBlock>
            {
                new MockMyTextPanel {
                    _customData = "droid\ndisplay: storage"
                }
            };
            State s = new State(new MockGridProgram(blocks));
            // we should have 1 screen
            Assert.AreEqual(1, s.outputs["storage"].Count);
        }

        [TestMethod]
        public void SingleScreen()
        {
            var blocks = new List<IMyTerminalBlock>
            {
                new MockMyTextPanel {
                    _customData = "droid\ndisplay: storage"
                }
            };
            State s = new State(new MockGridProgram(blocks));
            s.Tick();

            // Check the sprite collection
            // should just be one sprite
            var screen = s.outputs["storage"][0] as Screen;
            Assert.IsNotNull(screen);

            var mockSurface = screen.surface as MockMyTextPanel;
            Assert.IsNotNull(mockSurface);

            var spriteCollection = mockSurface._spriteCollection.Sprites;
            Assert.AreEqual(1, spriteCollection.Length);

            // Now double check the sprite
            // which should be the "[storage]" text
            Assert.AreEqual(SpriteType.TEXT, spriteCollection[0].Type);
            Assert.AreEqual(new Vector2(0, 502), spriteCollection[0].Position);
        }

        [TestMethod]
        public void DualScreenWideStorageWithNoStorage()
        {
            var blocks = new List<IMyTerminalBlock>
            {
                new MockMyTextPanel {
                    _displayNameText = "display 1",
                    _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                },
                new MockMyTextPanel {
                    _displayNameText = "display 2",
                    _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                }
            };
            State s = new State(new MockGridProgram(blocks));
            s.Tick();

            Assert.IsTrue(s.outputs.ContainsKey("storage"));
            Assert.AreEqual(1, s.outputs["storage"].Count);
            var compositeDisplay = s.outputs["storage"][0] as CompositeDisplay;
            Assert.IsNotNull(compositeDisplay);

            // double check the viewport is 1024x512
            Assert.AreEqual(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

            // this should only draw to the first screen
            var screen1 = compositeDisplay.screens[new Point(0, 0)];
            Assert.IsNotNull(screen1);
            var mockSurface1 = screen1.surface as MockMyTextPanel;
            Assert.IsNotNull(mockSurface1);
            Assert.AreEqual(1, mockSurface1._spriteCollection.Sprites.Length);

            // Now double check the sprite
            // which should be the "[storage]" text
            Assert.AreEqual(SpriteType.TEXT, mockSurface1._spriteCollection.Sprites[0].Type);
            Assert.AreEqual(new Vector2(0, 502), mockSurface1._spriteCollection.Sprites[0].Position);

            var screen2 = compositeDisplay.screens[new Point(1, 0)];
            Assert.IsNotNull(screen2);
            var mockSurface2 = screen2.surface as MockMyTextPanel;
            Assert.IsNotNull(mockSurface2);

            Assert.IsNull(mockSurface2._spriteCollection.Sprites);
        }

        [TestMethod]
        public void DualScreenWideStorageWithStorage()
        {
            var blocks = new List<IMyTerminalBlock>
                    {
                        new MockMyTextPanel {
                            _displayNameText = "display 1",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 0\ndisplayY: 0"
                        },
                        new MockMyTextPanel {
                            _displayNameText = "display 2",
                            _customData = "droid\ndisplay: storage\ndisplayId: main\ndisplayX: 1\ndisplayY: 0"
                        },
                        new MockMyCargoContainer {
                            _customData = "droid",
                            _inventory = new MockMyInventory {
                                _currentVolume = 0,
                                _maxVolume = 3000
                            }
                        }
                    };
            State s = new State(new MockGridProgram(blocks));
            s.Tick();

            Assert.IsTrue(s.outputs.ContainsKey("storage"));
            Assert.AreEqual(1, s.outputs["storage"].Count);
            var compositeDisplay = s.outputs["storage"][0] as CompositeDisplay;
            Assert.IsNotNull(compositeDisplay);

            // double check the viewport is 1024x512
            Assert.AreEqual(new RectangleF(0, 0, 1024, 512), compositeDisplay.viewport);

            // this should only draw to the first screen
            var screen1 = compositeDisplay.screens[new Point(0, 0)];
            Assert.IsNotNull(screen1);
            var mockSurface1 = screen1.surface as MockMyTextPanel;
            Assert.IsNotNull(mockSurface1);
            Assert.AreEqual(1, mockSurface1._spriteCollection.Sprites.Length);

            // Now double check the sprite
            // which should be the "[storage]" text
            Assert.AreEqual(SpriteType.TEXT, mockSurface1._spriteCollection.Sprites[0].Type);
            Assert.AreEqual(new Vector2(0, 502), mockSurface1._spriteCollection.Sprites[0].Position);

            var screen2 = compositeDisplay.screens[new Point(1, 0)];
            Assert.IsNotNull(screen2);
            var mockSurface2 = screen2.surface as MockMyTextPanel;
            Assert.IsNotNull(mockSurface2);

            Assert.IsNull(mockSurface2._spriteCollection.Sprites);
        }
    }
}
