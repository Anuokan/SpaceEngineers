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
   
        //------------------ Default Settings ------------------
        #region Default Settings
        public class GeneralSettings
        {
            

            public Program Context; // WC 0.1
            public WcPbApi WCAPI;   // WC 0.1
            
            public string DDSEnabled  = "PB | DDS enabled";
            public string DDSDisabled = "PB | DDS disabled";

            public bool DisplayStatusInName = false;

            public int MainBlocksReloadTicks = 600;

            public int TargetTracksTransmitIntervalTicks = 45;

            public int ManualAimBroadcastDurationTicks = 3600;
            public double ManualAimRaycastDistance = 5000;
            public int ManualAimRaycastRefreshInterval = 30;

            public int MaxDesignatorUpdatesPerTick = 1;

            public float MaxPDCUpdatesPerTick = 1;
            public int MinPDCRefreshRate = 15;

            public bool UseDesignatorReset = true;
            public int DesignatorResetInterval = 45;

            public bool UseRangeSweeper = true;
            public int RangeSweeperInterval = 2;
            public int RangeSweeperPerTick = 3;

            public int TargetFoundHoldTicks = 60;

            public bool UsePDCSpray = true;
            public double PDCSprayMinTargetSize = 12;

            public int RandomOffsetProbeInterval = 15;

            public double MaxRaycastTrackingDistance = 3000;

            public int RaycastTargetRefreshTicks = 15;
            public int RaycastGlobalRefreshTicks = 3;

            public int PriorityMinRefreshTicks = 15;
            public int PriorityMaxRefreshTicks = 90;
            public int PriorityGlobalRefreshTicks = 2;

            public int TargetSlippedTicks = 15;
            public int TargetLostTicks = 60;

            public double RaycastExtensionDistance = 5;

            public double MinTargetSizeEngage = 2;
            public double MinTargetSizePriority = 4;

            public bool AutoMissileLaunch = true;
            public double MissileMinTargetSize = 12;
            public double MissileCountPerSize = 36;

            public double MaxMissileLaunchDistance = 2000;
            public double MissileOffsetRadiusFactor = 0.5;
            public double MissileOffsetProbability = 0.75;

            public double PriorityDowngradeConstant = 800;

            public int MissileStaggerWaitTicks = 60;
            public int MissileReassignIntervalTicks = 600;
            public int MissilePBGridReloadTicks = 300;
            public int MissileTransmitDurationTicks = 600;
            public int MissileTransmitIntervalTicks = 15;

            public double MissileLaunchSpeedLimit = 60;

            public double PDCFireDotLimit = 0.995;
            public bool ConstantFireMode = true;

            public bool RotorUseLimitSnap = false;

            public float RotorCtrlDeltaGain = 1f;
            public float RotorCtrlOutputGain = 60f;
            public float RotorCtrlOutputLimit = 60f;

            public float RotorSnapVelocityGain = 50f;
            public float RotorSnapSpeedLimit = 60f;

            public int ReloadCheckTicks = 90;
            public int ReloadedCooldownTicks = 300;

            public double ReloadMaxAngle = 88;
            public double ReloadLockStateAngle = 79;

            public int DisplaysRefreshInterval = 30;

            public int AllyTrackLostTicks = 120;

            public bool CheckSelfOcclusion = true;
            public bool UseAABBOcclusionChecker = false;
            public float OcclusionExtraClearance = 0f;
            public bool UseFourPointOcclusion = false; // new in 5.6?
            public int OcclusionCheckerInitBlockLimit = 500;

        }
        #endregion
 
}
