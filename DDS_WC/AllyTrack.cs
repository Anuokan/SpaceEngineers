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
    
        public class AllyTrack
        {
            public long EntityId;
            public Vector3D Position;
            public Vector3D Velocity;
            public bool IsLargeGrid;
            public double SizeSq;
            public int LastDetectedClock;
            public AllyTrack(long entityId)
            {
                EntityId = entityId;
            }
        }
    
}
