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
 
        public class Designator
        {
            public IMyLargeTurretBase Turret;
            public float MaxDesignatorRange;
            public ITerminalProperty<float> RangeProperty;
            public int NextResetClock = 0;
            public int NextSweeperClock = 0;
            Random rnd = new Random();
            public Designator(IMyLargeTurretBase turret)
            {
                Turret = turret;
                MaxDesignatorRange = turret.GetMaximum<float>("Range");
                RangeProperty = turret.GetProperty("Range").As<float>();
            }
            public void SetMaxRange()
            { 
                RangeProperty.SetValue(Turret, MaxDesignatorRange - 0.01f);
                RangeProperty.SetValue(Turret, MaxDesignatorRange);
            }
        }
    
}
