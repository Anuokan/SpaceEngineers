using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public static class AGMChecker
        {
            const string INI_SAVED_BLOCKS_SECTION = "AGMSAVE"; 
            const string PARTIAL_TAG = "[NOTREADY]";
            public static bool CheckSave(IMyProgrammableBlock missilePB)
            {
                if (missilePB != null && missilePB.CustomName.IndexOf(PARTIAL_TAG, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    Vector3I[] vec ={-Base6Directions.GetIntVector(missilePB.Orientation.Left),Base6Directions.GetIntVector(missilePB.Orientation.Up),- Base6Directions.GetIntVector(missilePB.Orientation.Forward)};
                   
                    MyIni iniConfig = new MyIni();

                    if (iniConfig.TryParse(missilePB.CustomData) && iniConfig.ContainsSection(INI_SAVED_BLOCKS_SECTION))
                    {
                        char[] delimiters ={','};

                        if (!CheckConfigSavedBlockList("DetachBlock", missilePB, iniConfig, delimiters, ref vec, true)) return false;
                        if (!CheckConfigSavedBlockList("DampenerBlock", missilePB, iniConfig, delimiters, ref vec, true)) return false;
                        if (!CheckConfigSavedBlockList("ForwardBlock", missilePB, iniConfig, delimiters, ref vec, false)) return false;
                        if (!CheckConfigSavedBlockList("RemoteControl", missilePB, iniConfig, delimiters, ref vec, false)) return false; 
                        if (!CheckConfigSavedBlockList("Gyroscopes", missilePB, iniConfig, delimiters, ref vec,false)) return false; 
                        if (!CheckConfigSavedBlockList("Thrusters", missilePB, iniConfig, delimiters, ref vec, false)) return false;
                        if (!CheckConfigSavedBlockList("PowerBlocks", missilePB, iniConfig, delimiters, ref vec, true)) return false; 
                        if (!CheckConfigSavedBlockList("RaycastCameras", missilePB, iniConfig, delimiters, ref vec, true)) return false;
                    }

                    missilePB.CustomName = missilePB.CustomName.Replace(PARTIAL_TAG, "").Trim();
                }
                return true;
            }

            static bool CheckConfigSavedBlockList(string key, IMyTerminalBlock origin, MyIni iniConfig, char[] delimiters, ref Vector3I[] vec, bool acceptEmpty)
            {
                string[] values = iniConfig.Get(INI_SAVED_BLOCKS_SECTION, key).ToString().Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
                return ((values.Length > 0 || acceptEmpty) && VerifyBlocksExist(values, origin, ref vec));
            }
            static bool VerifyBlocksExist(string[] input, IMyTerminalBlock origin, ref Vector3I[] vec)
            {
                foreach (string line in input)
                {
                    if (line != null && line.Length == 12)
                    {
                        Vector3I result = new Vector3I();
                        result.X = BitConverter.ToInt16(Convert.FromBase64String(line.Substring(0, 4)), 0);
                        result.Y = BitConverter.ToInt16(Convert.FromBase64String(line.Substring(4, 4)), 0);
                        result.Z = BitConverter.ToInt16(Convert.FromBase64String(line.Substring(8, 4)), 0); 
                        result = (result.X * vec[0]) + (result.Y * vec[1]) + (result.Z * vec[2]); 
                        result += origin.Position;
                        if (!origin.CubeGrid.CubeExists(result))
                        {
                            return false; 
                        }
                    }
                    else
                    { 
                        return false; 
                    }
                }
                return true;
            }
        }
}
