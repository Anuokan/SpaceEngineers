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
    public enum AGMLaunchOptionEnum
    {
        OffsetTargeting = 1
    }

    public enum TrackTypeEnum
    {
        IsFriendly = 1,
        IsLargeGrid = 2
    }
    public enum TargetingPointTypeEnum
    {
        Center = 0,
        Offset = 1,
        Random = 2
    }
}
