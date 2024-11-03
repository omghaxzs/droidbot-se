using System;
using Sandbox.ModAPI.Ingame;

using System.Linq; // FILTER
using System.Collections.Generic; // FILTER

using Sandbox.ModAPI.Ingame; // FILTER
using VRage; // FILTER
using VRage.Game.ModAPI.Ingame; // FILTER
using VRage.Game; // FILTER

namespace Droidbot.Airlock // FILTER
{ // FILTER

    // droidbot-se
    // airlock

    // this script is designed to keep two doors from being open at the same time.
    // fitting the use case of an airlock.

    // the doors have to be marked with custom data like so:
    //
    // droid
    // airlockId: <make_up_an_id_to_identify_this_airlock>
    //

    // the script will error if there are more than two doors

    // "marked" in this context means blocks that have custom data starting with "droid".
    public class State
    {
        public Dictionary<string, List<IMyDoor>> airlocks = new Dictionary<string, List<IMyDoor>>();

        public IMyGridTerminalSystem grid;
        public MyGridProgram prog;

        public short tick = 0;

        public State(MyGridProgram p)
        {
            this.prog = p;
            this.grid = p.GridTerminalSystem;

            ScanForAirlocks();
        }

        public void Log(string text)
        {
            this.prog.Echo(text);
        }

        public void ScanForAirlocks()
        {
            var doors = new List<IMyDoor>();
            // grab all doors
            this.grid.GetBlocksOfType(doors, s => s.CubeGrid == prog.Me.CubeGrid && s.CustomData.StartsWith("droid"));

            var newAirlocks = new Dictionary<string, List<IMyDoor>>();

            // go through each one
            foreach (var door in doors)
            {
                // parse the custom data
                var customData = ParseCustomData(door);

                // is there an airlockID?
                var airlockID = customData.GetValueOrDefault("airlockId", null);
                if (airlockID != null)
                {
                    // add it to the list
                    if (!newAirlocks.ContainsKey(airlockID))
                    {
                        newAirlocks[airlockID] = new List<IMyDoor>();
                    }

                    newAirlocks[airlockID].Add(door);
                }
            }

            // ok, now filter out the bad airlocks
            var toDelete = new List<string>();
            foreach (var airlock in newAirlocks)
            {
                if (airlock.Value.Count() != 2)
                {
                    // delete
                    toDelete.Add(airlock.Key);

                    this.Log("failed to add airlock " + airlock.Key + " there are " + airlock.Value.Count() + " associated doors.");
                }
            }

            // now delete
            foreach (var del in toDelete)
            {
                newAirlocks.Remove(del);
            }

            // now we have our new airlocks
            this.airlocks = newAirlocks;
        }

        public void EnsureAirlock(List<IMyDoor> airlock)
        {
            // we should be guaranteed to have two doors in this
            var door1 = airlock[0];
            var door2 = airlock[1];

            var door1Status = door1.Status;
            var door2Status = door2.Status;

            // is door1 open or opening?
            if (door1Status == DoorStatus.Open || door1Status == DoorStatus.Opening)
            {
                // make sure door2 is closing or is closed
                if (door2Status != DoorStatus.Closing && door2Status != DoorStatus.Closed)
                {
                    // close it!
                    door2.CloseDoor();
                }

                if (door2Status == DoorStatus.Closed) {
                    // turn it off
                    door2.Enabled = false;
                }
            }

            // is door2 open or opening?
            if (door2Status == DoorStatus.Open || door2Status == DoorStatus.Opening)
            {
                // make sure door1 is closing or is closed
                if (door1Status != DoorStatus.Closing && door1Status != DoorStatus.Closed)
                {
                    // close it!
                    door1.CloseDoor();
                }

                if (door1Status == DoorStatus.Closed) {
                    // turn it off
                    door1.Enabled = false;
                }
            }

            if (door1Status == DoorStatus.Closed && door2Status == DoorStatus.Closed) {
                // enable the doors
                door1.Enabled = true;
                door2.Enabled = true;
            }

        }

        public void Tick()
        {
            if (tick % 10000 == 0)
            {
                ScanForAirlocks();
            }

            // go through each airlock and ensure it
            foreach (var airlock in this.airlocks)
            {
                EnsureAirlock(airlock.Value);
            }

            tick++;
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
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
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
    } // FILTER
} // FILTER
