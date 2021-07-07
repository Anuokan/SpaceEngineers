using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace DiamondDomeDefense
{   
    public class ScriptData
    {
        public IMyTerminalBlock block { get; private set; }
        public string startString { get; private set; }
        public string endString { get; private set; }

        public ScriptData(IMyTerminalBlock block, string startString, string endString)
        {
            this.block = block;
            this.startString = startString;
            this.endString = endString;
        }

        public string GetScriptData()
        {
            var rawData = block.CustomData;

            if (rawData.Contains(startString) == false || rawData.Contains(endString) == false)
            {
                return "Error: data Indexes not found";
            }

            var startIndex = rawData.IndexOf(startString);
            var endIndex = rawData.IndexOf(endString);
            var length = endIndex - startIndex;
            if (length < 0)
            {
                return "Error: endIndex is before startIndex";
            }
            var data = rawData.Substring(startIndex + startString.Length, length - startString.Length);
            return data;
        }
        public void RemoveScriptData()
        {
            var rawData = block.CustomData;
            if (rawData.Contains(startString) == false || rawData.Contains(endString) == false)
            {
                throw new Exception("Error: RemoveScriptData can not find Indexes");
            }
            var startIndex = rawData.IndexOf(startString);
            var endIndex = rawData.IndexOf(endString);
            var length = endIndex - startIndex;
            if (length < 0)
            {
                throw new Exception("Error: RemoveScriptData endIndex is before startIndex");
            }
            block.CustomData = rawData.Remove(startIndex, length + endString.Length);
        }
        public void SetScriptData(string data)
        {
            var rawData = block.CustomData;
            if (rawData.Contains(startString) == true && rawData.Contains(endString) == true)
            {
                RemoveScriptData();
            }
            rawData = block.CustomData;
            var scriptData = startString + data + endString;
            block.CustomData = rawData + scriptData;
        }
    }
}
