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
    public class Program : MyGridProgram
    {
        #region Script Constants
        const string DISPLAY_VERSION = "5.6(WC 0.1)";
        const string DISPLAY_SCRIPT_NAME = "Diamond Dome";

        public const string INI_SECTION = "DDS";
        const string MISSILE_INI_SECTION = "AGM";

        const string DESIGNATOR_GRP_TAG = "DDS Designator";
        const string RAYCAST_CAMERA_GRP_TAG = "DDS Camera";
        const string MANUAL_AIMING_GRP_TAG = "DDS Aiming";
        public const string SILO_DOOR_GRP_TAG = "DDS Door";
        const string DISPLAY_GRP_TAG = "DDS Display";
        const string DDS_TAG = "[DDS]";

        const string PDC_GRP_TAG = "DDS Turret";
        const string AZIMUTH_ROTOR_TAG = "Azimuth";
        const string ELEVATION_ROTOR_TAG = "Elevation";
        const string AIM_BLOCK_TAG = "Aiming";
        const string RELOAD_CONNECTOR_TAG = "Reload";

        const string MANUAL_AIMING_AIMBLOCK_TAG = "Forward";
        const string MANUAL_AIMING_STATUS_TAG = "Status";
        const string MANUAL_AIMING_ALERT_TAG = "Alert";
        const string MANUAL_AIMING_GRP_PREFIX = "AIM";

        const string MISSILE_PB_GRP_TAG = "DDS Missile";
        const string TORPEDO_PB_GRP_TAG = "DDS Torpedo";

        const string USE_CGE_TAG = "[CGE]";

        const string MANUAL_LAUNCH_CMD = "FIRE";
        const string MANUAL_LAUNCH_LARGEST_TAG = "LARGEST";
        const string MANUAL_LAUNCH_TORPEDO_TAG = "BIG";
        const string MANUAL_LAUNCH_EXTENDED_TAG = "EXTEND";
        const string MANUAL_AIM_TRACK_TAG = "TRACK";
        const string MANUAL_AIM_RELEASE_TAG = "RELEASE";
        const string MANUAL_AIM_VALUE_SET_TAG = "SET";
        const string MANUAL_AIM_VALUE_SET_CENTER_TAG = "CENTER";
        const string MANUAL_AIM_VALUE_SET_OFFSET_TAG = "OFFSET";
        const string MANUAL_AIM_VALUE_SET_RANDOM_TAG = "RANDOM";
        const string MANUAL_AIM_VALUE_SET_RANGE_TAG = "RANGE";
        const string MANUAL_AIM_VALUE_INC_RANGE_TAG = "INCRANGE";
        const string MANUAL_AIM_VALUE_DEC_RANGE_TAG = "DECRANGE";
        const string MANUAL_AIM_VALUE_CYCLE_OFFSET_TAG = "CYCLEOFFSET";

        const string CMD_TOGGLE_ON_OFF = "TOGGLE";
        const string CMD_TOGGLE_ON = "ENABLE";
        const string CMD_TOGGLE_OFF = "DISABLE";
        const string CMD_AUTOLAUNCH_ON_OFF = "AUTOLAUNCH";      // new in 5.6?
        const string CMD_AUTOLAUNCH_ON = "AUTOLAUNCH_ON";   // new in 5.6?
        const string CMD_AUTOLAUNCH_OFF = "AUTOLAUNCH_OFF";  // new in 5.6
        const string CMD_DEBUG_MODE = "DEBUGMODE";

        public const double COS_45_DEGREES = 0.707;

        const int OFFSET_POINTS_MAX_COUNT = 7;
        public const int OFFSET_POINTS_MOVE_ANGLE_PER_SECOND = 30;
        const int OFFSET_POINTS_EXPIRE_TICKS = 240;
        public const int TARGET_ORIENTATION_EXPIRE_TICKS = 120;
        const double OFFSET_PROBE_RANDOM_FACTOR = 0.75;

        const int STATUS_REFRESH_INTERVAL = 60;

        const double PROFILER_NEW_VALUE_FACTOR = 0.005;
        const int PROFILER_HISTORY_COUNT = (int)(1 / PROFILER_NEW_VALUE_FACTOR);

        public const double INV_ONE_TICK = 1.0 / 60.0;
        public const int TICKS_PER_SECOND = 60;

        const UpdateType ARG_COMMAND_FLAGS = UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script;

        const string IGC_MSG_TRACKS_INFO = "IGCMSG_TK_IF";
        const string IGC_MSG_TARGET_TRACKS = "IGCMSG_TR_TK";
        const string IGC_MSG_TARGET_DATALINK = "IGCMSG_TR_DL";
        const string IGC_MSG_TARGET_SWITCH_LOST = "IGCMSG_TR_SW";

        const string CGE_MSG_TARGET_DATALINK = "CGE_MSG_TR_DL";

        const string FAKE = "SIMJ";

        const float standardTextSizeFactor = 0.032f;

        Color statusThisScriptTextColor = Color.White;
        Color statusThisScriptBoxColor = new Color(40, 5, 100);
        Color statusAimIdTextColor = new Color(245, 230, 255);
        Color statusAimIdBoxColor = new Color(40, 15, 5);

        Color statusTitleTextColor = Color.White;
        Color statusNoTargetTextColor = new Color(0, 0, 0);
        Color statusNoTargetBoxColor = new Color(50, 50, 50);
        Color statusSeekingTextColor = new Color(255, 255, 255);
        Color statusSeekingBoxColor = new Color(100, 100, 0);
        Color statusLockedTextColor = Color.White;
        Color statusLockedBoxColor = Color.Green;
        Color statusCurrentTargetTextColor = Color.White;
        Color statusCurrentTargetBoxColor = Color.DarkOrchid;
        Color statusOptionsTextColor = Color.White;
        Color statusOptionsBoxColor = new Color(0, 0, 90);

        char[] splitDelimiterCommand = new char[] { ':' };
        
        IComparer<PDCTarget> sortCommsTargetPriority = new PDCTargetCommsSorting();

        Random rnd = new Random();
        List<IMyTerminalBlock> dummyBlocks = new List<IMyTerminalBlock>(0);
        StringBuilder sb = new StringBuilder();
        StringBuilder debug = new StringBuilder(); // WC 0.1
        Dictionary<MyDetectedEntityInfo, float> WCThreatsScratchpad = new Dictionary<MyDetectedEntityInfo, float>(); // WC 0.1


        #endregion

        #region Global Variables
        int loadedCustomDataHashCode = 0;

        GeneralSettings settings;

        DefaultWeaponProfiles weaponProfiles;

        IMyRemoteControl remote; // TODO, this should be an IMyController

        List<PDCTurret> pdcList;
        RoundRobin<PDCTurret> pdcAssignRR;
        RoundRobin<PDCTurret> pdcFireRR;

        List<IMyTextSurface> displayPanels;

        int curPDCUpdatesPerTick = 1;
        int curPDCUpdatesSkipTicks = 0;
        int curPDCNextUpdateClock = 0;

        List<Designator> designators;
        RoundRobin<Designator> designatorTargetRR;
        RoundRobin<Designator> designatorOperationRR;
        int designatorTargetAcquiredClock = -10000;

        List<IMyCameraBlock> raycastCameras;
        RaycastHandler raycastHandler;
        int nextRaycastGlobalClock;

        TargetManager targetManager;

        AllyManager allyManager;

        SortedDictionary<double, PDCTarget> sortedEntityIds;
        double maxPriorityValue = 0;
        int nextPriorityRefreshClock;
        bool curHaveTargets = false;
        int assignmentState = 0;

        List<IMyProgrammableBlock> missileComputers;
        List<IMyProgrammableBlock> torpedoComputers;

        List<ManualPDCTarget> manualTargeters;
        Dictionary<string, ManualPDCTarget> manualTargetersLookup;

        byte[] genUniqueIdBuffer = new byte[8];
        long[] genUniqueIdResult = new long[1];

        int nextAutoMissileLaunchClock = 0;

        IPDCOcclusionChecker occlusionChecker;
        IEnumerator<int> iterOcclusionCreator;

        IMyBroadcastListener igcTargetTracksListener;
        IMyBroadcastListener igcTracksInfoListener;

        Queue<MissileCommsTarget> guidanceCommsTargets;
        Queue<PDCTarget> tracksCommsTargets;
        int curCommsQueuePriority = 0;
        bool haveIGCTransmitted = false;

        double gridSpeedLimit = 100;
        double shipRadius = 0;

        int nextTracksTranmissionClock = 0;
        int nextMyShipTranmissionClock = 0;

        long timeSinceLastRun = 0;
        bool switchedOn = true;

        Profiler profiler;
        bool debugMode;



        int clock = 0;
        int subMode = 0;
        bool init = false;
        #endregion

        Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            profiler = new Profiler(Runtime, PROFILER_HISTORY_COUNT, PROFILER_NEW_VALUE_FACTOR);

            settings = new GeneralSettings();
            settings.Context = this;
            settings.WCAPI = new WcPbApi();
            if (!settings.WCAPI.Activate(Me))
                settings.WCAPI = null;

            weaponProfiles = new DefaultWeaponProfiles();
         
        }
        public void Main(string args, UpdateType updateType)
        {
            if (!init)
            {
                if (!InitLoop())
                {
                    return;
                }

                nextPriorityRefreshClock = -100000;
                assignmentState = 0;

                switchedOn = true;
                debugMode = false;

                profiler.Clear();

                clock = -1;

                init = true;
                return;
            }

            profiler.UpdateRuntime();

            // Reacquire WCAPI if null, this can happen on pasted ships & NPC ships sometimes.
            if (settings.WCAPI == null)
            {
                settings.WCAPI = new WcPbApi();
                if (!settings.WCAPI.Activate(Me))
                    settings.WCAPI = null;
            }

            if (debugMode) profiler.StartSectionWatch("InterGridComms");

            if (args.Length > 0)
            {
                if (args.Equals(IGC_MSG_TARGET_TRACKS))
                {
                    ProcessCommsMessage();
                }
                else
                {
                    if ((updateType & ARG_COMMAND_FLAGS) != 0)
                    {
                        ProcessCommands(args);
                    }
                }
            }

            if (debugMode) profiler.StopSectionWatch("InterGridComms");

            timeSinceLastRun += Runtime.TimeSinceLastRun.Ticks;

            if ((updateType & UpdateType.Update1) == 0 || timeSinceLastRun == 0)
            {
                return;
            }
            timeSinceLastRun = 0;

            clock++;

            if (!switchedOn)
            {
                if (clock % STATUS_REFRESH_INTERVAL == 0)
                {
                    DisplayStatus();
                }
                return;
            }
            if (clock % STATUS_REFRESH_INTERVAL == 0)
            {
                DisplayStatus();
            }

            if (debugMode) profiler.StartSectionWatch("AutoMissileLaunch");

            if (clock >= nextAutoMissileLaunchClock)
            {
                LaunchAutomaticMissiles();
            }

            if (debugMode) profiler.StopSectionWatch("AutoMissileLaunch");
            if (debugMode) profiler.StartSectionWatch("MissileReload");

            if (settings.MissilePBGridReloadTicks > 0 && clock % settings.MissilePBGridReloadTicks == 0)
            {
                ReloadMissilesAndTorpedos();
            }

            if (debugMode) profiler.StopSectionWatch("MissileReload");
            if (debugMode) profiler.StartSectionWatch("ManualAimReload");

            if (settings.MainBlocksReloadTicks > 0 && clock % settings.MainBlocksReloadTicks == 0)
            {
                ReloadMainBlocks();
            }

            if (debugMode) profiler.StopSectionWatch("ManualAimReload");
            if (debugMode) profiler.StartSectionWatch("ManualRaycast");

            UpdateManualAimRaycast();

            if (debugMode) profiler.StopSectionWatch("ManualRaycast");
            if (debugMode) profiler.StartSectionWatch("Designator");

            UpdateDesignatorTargets();

            if (debugMode) profiler.StopSectionWatch("Designator");
            if (debugMode) profiler.StartSectionWatch("Allies");

            UpdateAllies();

            if (debugMode) profiler.StopSectionWatch("Allies");

            if (targetManager.Count() > 0)
            {
                curHaveTargets = true;

                if (debugMode) profiler.StartSectionWatch("RaycastTracking");

                if (clock >= nextRaycastGlobalClock)
                {
                    UpdateRaycastTargets();
                }

                if (debugMode) profiler.StopSectionWatch("RaycastTracking");
                if (debugMode) profiler.StartSectionWatch("AssignTargets");

                if (clock % settings.PriorityGlobalRefreshTicks == 0)
                {
                    AssignTargets();
                }

                if (debugMode) profiler.StopSectionWatch("AssignTargets");
                if (debugMode) profiler.StartSectionWatch("PDCAimFireReload");

                AimFireReloadPDC();

                if (debugMode) profiler.StopSectionWatch("PDCAimFireReload");
            }
            else if (curHaveTargets)
            {
                curHaveTargets = false;

                foreach (PDCTurret pdcReset in pdcList)
                {
                    pdcReset.TargetInfo = null;
                    pdcReset.ReleaseWeapons();
                    pdcReset.ResetRotors();
                }

                assignmentState = 0;
            }

            if (debugMode) profiler.StartSectionWatch("TransmitIGCMessages");

            if (targetManager.Count() > 0 || guidanceCommsTargets.Count() > 0)
            {
                TransmitIGCMessages();
            }

            TransmitMyShipInformation();

            if (debugMode) profiler.StopSectionWatch("TransmitIGCMessages");

            if (!switchedOn)
            {
                if (clock % settings.DisplaysRefreshInterval == 0)
                {
                    RefreshDisplay();
                    RefreshDisplays();
                }
                return;
            }
            if (clock % settings.DisplaysRefreshInterval == 0)
            {
                RefreshDisplay();
                RefreshDisplays();       
            }

            

            

            profiler.UpdateComplexity();
        }

        #region Initialization
        void ForceJITCompilation()
        {
            try
            {
                PDCTarget fakeTarget;
                IMyTerminalBlock fakeTargetBlock = (raycastHandler.Cameras.Count > 0 ? raycastHandler.Cameras[0] : (IMyTerminalBlock)Me);
                MyDetectedEntityInfo fakeEntityInfo = new MyDetectedEntityInfo(9801, FAKE, MyDetectedEntityType.LargeGrid, fakeTargetBlock.WorldMatrix.Translation + (fakeTargetBlock.WorldMatrix.Forward * 0.1), fakeTargetBlock.WorldMatrix, Vector3D.Zero, MyRelationsBetweenPlayerAndBlock.Enemies, fakeTargetBlock.WorldAABB.Inflate(100), 1);
                targetManager.UpdateTarget(ref fakeEntityInfo, 1, out fakeTarget);

                ProcessCommsMessage();
                ProcessCommands(FAKE);

                LaunchAutomaticMissiles();

                LaunchMissileForTarget(fakeTarget, false, true);
                LaunchManualForTarget(torpedoComputers, fakeTarget, TargetingPointTypeEnum.Center, null, false, true);

                double savedValue = settings.ManualAimRaycastDistance;
                settings.ManualAimRaycastDistance = 0.1f;
                manualTargeters = new List<ManualPDCTarget>();
                manualTargeters.Add(new ManualPDCTarget("AIM0", Me, null, null));
                UpdateManualAimRaycast();
                settings.ManualAimRaycastDistance = savedValue;
                manualTargeters = null;

                UpdateRaycastTargets();
                AssignTargets();
                AimFireReloadPDC();

                UpdateRaycastTargets();
                AssignTargets();
                AimFireReloadPDC();

                targetManager.ClearBlackList();
                targetManager.ClearTargets();

                curHaveTargets = false;

                foreach (PDCTurret pdcReset in pdcList)
                {
                    pdcReset.TargetInfo = null;
                    pdcReset.ReleaseWeapons();
                    pdcReset.ResetRotors();
                }

                assignmentState = 0;

                guidanceCommsTargets.Clear();
                tracksCommsTargets.Clear();

                TransmitIGCMessages();
                TransmitIGCMessages();

                guidanceCommsTargets.Clear();
                tracksCommsTargets.Clear();

                haveIGCTransmitted = false;
                curCommsQueuePriority = 0;
            }
            catch (Exception) { }
        }
        bool InitLoop()
        {
            if (subMode == 0)
            {
                InitConfiguration();

                List<IMyRemoteControl> remoteBlocks = new List<IMyRemoteControl>(0);
                GridTerminalSystem.GetBlocksOfType(remoteBlocks, (b) => { if (remote == null) { remote = b; } return false; });

                if (remote != null)
                {
                    float currentValue = remote.SpeedLimit;
                    remote.SpeedLimit = float.MaxValue;
                    gridSpeedLimit = remote.SpeedLimit;
                    remote.SpeedLimit = currentValue;
                }

                if (settings.CheckSelfOcclusion)
                {
                    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                    GridTerminalSystem.GetBlocksOfType(blocks, (b) => { return (b is IMyMechanicalConnectionBlock || b is IMyShipConnector); });

                    List<PDCOcclusionGrid> refBlocks = new List<PDCOcclusionGrid>();
                    foreach (IMyTerminalBlock block in blocks)
                    {
                        if (block is IMyMechanicalConnectionBlock)
                        {
                            IMyMechanicalConnectionBlock mech = block as IMyMechanicalConnectionBlock;
                            if (mech.Top != null)
                            {
                                refBlocks.Add(new PDCOcclusionGrid(mech.Top.CubeGrid, mech.Top.Position));
                            }
                        }
                        else
                        {
                            IMyShipConnector connector = block as IMyShipConnector;
                            if (connector != null && connector.OtherConnector != null)
                            {
                                refBlocks.Add(new PDCOcclusionGrid(connector.OtherConnector.CubeGrid, connector.OtherConnector.Position));
                            }
                        }
                    }

                    if (settings.UseAABBOcclusionChecker)
                    {
                        AABBPDCOcclusionChecker aabbChecker = new AABBPDCOcclusionChecker();
                        iterOcclusionCreator = aabbChecker.Init(new PDCOcclusionGrid(Me.CubeGrid, Me.Position), refBlocks, settings.OcclusionExtraClearance, settings.OcclusionCheckerInitBlockLimit);

                        occlusionChecker = aabbChecker;

                        subMode = 1;
                    }
                    else
                    {
                        CubeExistsPDCOcclusionChecker cubeExistsChecker = new CubeExistsPDCOcclusionChecker(new PDCOcclusionGrid(Me.CubeGrid, Me.Position), refBlocks, settings.OcclusionExtraClearance, Me);

                        occlusionChecker = cubeExistsChecker;

                        subMode = 2;
                    }
                }
                else
                {
                    occlusionChecker = null;
                }

                raycastHandler = new RaycastHandler(new List<IMyCameraBlock>(0));

                targetManager = new TargetManager();

                allyManager = new AllyManager();

                sortedEntityIds = new SortedDictionary<double, PDCTarget>();

                ReloadMainBlocks();

                ReloadMissilesAndTorpedos();

                guidanceCommsTargets = new Queue<MissileCommsTarget>();
                tracksCommsTargets = new Queue<PDCTarget>();

                settings.ManualAimRaycastDistance = Math.Min(Math.Max(settings.ManualAimRaycastDistance, 1000), 100000);

                igcTargetTracksListener = IGC.RegisterBroadcastListener(IGC_MSG_TARGET_TRACKS);
                igcTracksInfoListener = IGC.RegisterBroadcastListener(IGC_MSG_TRACKS_INFO);

                shipRadius = Me.CubeGrid.WorldAABB.HalfExtents.Length();

                ForceJITCompilation();
            }

            if (subMode == 1)
            {
                if (iterOcclusionCreator.MoveNext())
                {
                    Echo("--- Creating Occlusion Checker ---\nBlocks Processed:" + iterOcclusionCreator.Current);
                    return false;
                }
                else
                {
                    Echo("--- Occlusion Checker Created ---");
                    iterOcclusionCreator.Dispose();
                    iterOcclusionCreator = null;

                    subMode = 2;
                }
            }
            return true;
        }
        void InitConfiguration()
        {
           
            int latestHashCode = Me.CustomData.GetHashCode();
            if (loadedCustomDataHashCode == 0 || loadedCustomDataHashCode != latestHashCode)
            {
                loadedCustomDataHashCode = latestHashCode;
                MyIni iniConfig = new MyIni();
                if (iniConfig.TryParse(Me.CustomData))
                {
                    if (iniConfig.ContainsSection(INI_SECTION))
                    {

                        settings.MainBlocksReloadTicks = iniConfig.Get(INI_SECTION, "MainBlocksReloadTicks").ToInt32(settings.MainBlocksReloadTicks);

                        settings.TargetTracksTransmitIntervalTicks = iniConfig.Get(INI_SECTION, "TargetTracksTransmitIntervalTicks").ToInt32(settings.TargetTracksTransmitIntervalTicks);

                        settings.ManualAimBroadcastDurationTicks = iniConfig.Get(INI_SECTION, "ManualAimBroadcastDurationTicks").ToInt32(settings.ManualAimBroadcastDurationTicks);
                        settings.ManualAimRaycastDistance = iniConfig.Get(INI_SECTION, "ManualAimRaycastDistance").ToDouble(settings.ManualAimRaycastDistance);
                        settings.ManualAimRaycastRefreshInterval = iniConfig.Get(INI_SECTION, "ManualAimRaycastRefreshInterval").ToInt32(settings.ManualAimRaycastRefreshInterval);

                        settings.MaxDesignatorUpdatesPerTick = iniConfig.Get(INI_SECTION, "MaxDesignatorUpdatesPerTick").ToInt32(settings.MaxDesignatorUpdatesPerTick);

                        settings.MaxPDCUpdatesPerTick = iniConfig.Get(INI_SECTION, "MaxPDCUpdatesPerTick").ToSingle(settings.MaxPDCUpdatesPerTick);
                        settings.MinPDCRefreshRate = iniConfig.Get(INI_SECTION, "MinPDCRefreshRate").ToInt32(settings.MinPDCRefreshRate);

                        settings.UseDesignatorReset = iniConfig.Get(INI_SECTION, "UseDesignatorReset").ToBoolean(settings.UseDesignatorReset);
                        settings.DesignatorResetInterval = iniConfig.Get(INI_SECTION, "DesignatorResetInterval").ToInt32(settings.DesignatorResetInterval);

                        settings.UseRangeSweeper = iniConfig.Get(INI_SECTION, "UseRangeSweeper").ToBoolean(settings.UseRangeSweeper);
                        settings.RangeSweeperPerTick = iniConfig.Get(INI_SECTION, "RangeSweeperPerTick").ToInt32(settings.RangeSweeperPerTick);
                        settings.RangeSweeperInterval = iniConfig.Get(INI_SECTION, "RangeSweeperInterval").ToInt32(settings.RangeSweeperInterval);

                        settings.TargetFoundHoldTicks = iniConfig.Get(INI_SECTION, "TargetFoundHoldTicks").ToInt32(settings.TargetFoundHoldTicks);

                        settings.DisplayStatusInName = iniConfig.Get(INI_SECTION, "DisplayStatusInName").ToBoolean(settings.DisplayStatusInName);

                        settings.UsePDCSpray = iniConfig.Get(INI_SECTION, "UsePDCSpray").ToBoolean(settings.UsePDCSpray);
                        settings.PDCSprayMinTargetSize = iniConfig.Get(INI_SECTION, "PDCSprayMinTargetSize").ToDouble(settings.PDCSprayMinTargetSize);

                        settings.MaxRaycastTrackingDistance = iniConfig.Get(INI_SECTION, "MaxRaycastTrackingDistance").ToDouble(settings.MaxRaycastTrackingDistance);

                        settings.RaycastTargetRefreshTicks = iniConfig.Get(INI_SECTION, "RaycastTargetRefreshTicks").ToInt32(settings.RaycastTargetRefreshTicks);
                        settings.RaycastGlobalRefreshTicks = Math.Max(iniConfig.Get(INI_SECTION, "RaycastGlobalRefreshTicks").ToInt32(settings.RaycastGlobalRefreshTicks), 1);

                        settings.PriorityMinRefreshTicks = iniConfig.Get(INI_SECTION, "PriorityMinRefreshTicks").ToInt32(settings.PriorityMinRefreshTicks);
                        settings.PriorityMaxRefreshTicks = iniConfig.Get(INI_SECTION, "PriorityMaxRefreshTicks").ToInt32(settings.PriorityMaxRefreshTicks);
                        settings.PriorityGlobalRefreshTicks = Math.Max(iniConfig.Get(INI_SECTION, "priorityGlobalRefreshTicks").ToInt32(settings.PriorityGlobalRefreshTicks), 1);

                        settings.TargetSlippedTicks = iniConfig.Get(INI_SECTION, "TargetSlippedTicks").ToInt32(settings.TargetSlippedTicks);
                        settings.TargetLostTicks = iniConfig.Get(INI_SECTION, "TargetLostTicks").ToInt32(settings.TargetLostTicks);

                        settings.RandomOffsetProbeInterval = iniConfig.Get(INI_SECTION, "RandomOffsetProbeInterval").ToInt32(settings.RandomOffsetProbeInterval);

                        settings.RaycastExtensionDistance = iniConfig.Get(INI_SECTION, "RaycastExtensionDistance").ToDouble(settings.RaycastExtensionDistance);

                        settings.MinTargetSizeEngage = iniConfig.Get(INI_SECTION, "MinTargetSizeEngage").ToDouble(settings.MinTargetSizeEngage);
                        settings.MinTargetSizePriority = iniConfig.Get(INI_SECTION, "MinTargetSizePriority").ToDouble(settings.MinTargetSizePriority);

                        settings.AutoMissileLaunch = iniConfig.Get(INI_SECTION, "AutoMissileLaunch").ToBoolean(settings.AutoMissileLaunch);
                        settings.MissileMinTargetSize = iniConfig.Get(INI_SECTION, "MissileMinTargetSize").ToDouble(settings.MissileMinTargetSize);
                        settings.MissileCountPerSize = iniConfig.Get(INI_SECTION, "MissileCountPerSize").ToDouble(settings.MissileCountPerSize);

                        settings.MaxMissileLaunchDistance = iniConfig.Get(INI_SECTION, "MaxMissileLaunchDistance").ToDouble(settings.MaxMissileLaunchDistance);
                        settings.MissileOffsetRadiusFactor = iniConfig.Get(INI_SECTION, "MissileOffsetRadiusFactor").ToDouble(settings.MissileOffsetRadiusFactor);
                        settings.MissileOffsetProbability = iniConfig.Get(INI_SECTION, "MissileOffsetProbability").ToDouble(settings.MissileOffsetProbability);

                        settings.MissileStaggerWaitTicks = iniConfig.Get(INI_SECTION, "MissileStaggerWaitTicks").ToInt32(settings.MissileStaggerWaitTicks);
                        settings.MissileReassignIntervalTicks = iniConfig.Get(INI_SECTION, "MissileReassignIntervalTicks").ToInt32(settings.MissileReassignIntervalTicks);
                        settings.MissilePBGridReloadTicks = iniConfig.Get(INI_SECTION, "MissilePBGridReloadTicks").ToInt32(settings.MissilePBGridReloadTicks);
                        settings.MissileTransmitDurationTicks = iniConfig.Get(INI_SECTION, "MissileTransmitDurationTicks").ToInt32(settings.MissileTransmitDurationTicks);
                        settings.MissileTransmitIntervalTicks = iniConfig.Get(INI_SECTION, "MissileTransmitIntervalTicks").ToInt32(settings.MissileTransmitIntervalTicks);

                        settings.MissileLaunchSpeedLimit = iniConfig.Get(INI_SECTION, "MissileLaunchSpeedLimit").ToDouble(settings.MissileLaunchSpeedLimit);

                        settings.PDCFireDotLimit = iniConfig.Get(INI_SECTION, "PDCFireDotLimit").ToDouble(settings.PDCFireDotLimit);
                        settings.ConstantFireMode = iniConfig.Get(INI_SECTION, "ConstantFireMode").ToBoolean(settings.ConstantFireMode);

                        settings.RotorUseLimitSnap = iniConfig.Get(INI_SECTION, "RotorUseLimitSnap").ToBoolean(settings.RotorUseLimitSnap);

                        settings.RotorCtrlDeltaGain = iniConfig.Get(INI_SECTION, "RotorCtrlDeltaGain").ToSingle(settings.RotorCtrlDeltaGain);

                        settings.ReloadCheckTicks = iniConfig.Get(INI_SECTION, "ReloadCheckTicks").ToInt32(settings.ReloadCheckTicks);
                        settings.ReloadedCooldownTicks = iniConfig.Get(INI_SECTION, "ReloadedCooldownTicks").ToInt32(settings.ReloadedCooldownTicks);

                        settings.ReloadMaxAngle = iniConfig.Get(INI_SECTION, "ReloadMaxAngle").ToDouble(settings.ReloadMaxAngle);
                        settings.ReloadLockStateAngle = iniConfig.Get(INI_SECTION, "ReloadLockStateAngle").ToDouble(settings.ReloadLockStateAngle);

                        settings.DisplaysRefreshInterval = Math.Max(iniConfig.Get(INI_SECTION, "DisplaysRefreshInterval").ToInt32(settings.DisplaysRefreshInterval), 1);

                        settings.AllyTrackLostTicks = iniConfig.Get(INI_SECTION, "AllyTrackLostTicks").ToInt32(settings.AllyTrackLostTicks);

                        settings.CheckSelfOcclusion = iniConfig.Get(INI_SECTION, "CheckSelfOcclusion").ToBoolean(settings.CheckSelfOcclusion);
                        settings.UseAABBOcclusionChecker = iniConfig.Get(INI_SECTION, "UseAABBOcclusionChecker").ToBoolean(settings.UseAABBOcclusionChecker);
                        settings.OcclusionExtraClearance = iniConfig.Get(INI_SECTION, "OcclusionExtraClearance").ToSingle(settings.OcclusionExtraClearance);
                        settings.UseFourPointOcclusion = iniConfig.Get(INI_SECTION, "UseFourPointOcclusion").ToBoolean(settings.UseFourPointOcclusion);
                        settings.OcclusionCheckerInitBlockLimit = iniConfig.Get(INI_SECTION, "OcclusionCheckerInitBlockLimit").ToInt32(settings.OcclusionCheckerInitBlockLimit);
                    }
                }
            }
        }
        void ReloadMainBlocks()
        {
            CompileDesignators();

            CompileRaycastCameras();

            CompilePDCGroups();

            CompileManualTargeters();

            CompileDisplayPanels();

            GetBlocks();
        }
        void CompileDesignators()
        {
            List<IMyLargeTurretBase> turrets;
            GetBlocksFromGroups(out turrets, DESIGNATOR_GRP_TAG);

            if (turrets == null)
            {
                turrets = new List<IMyLargeTurretBase>(0);
            }

            List<Designator> newDesignators = new List<Designator>(turrets.Count);
            foreach (IMyLargeTurretBase turret in turrets)
            {
                newDesignators.Add(new Designator(turret));
            }

            designators = newDesignators;
            designatorTargetRR = new RoundRobin<Designator>(newDesignators, FuncDesignatorHasTarget);
            designatorOperationRR = new RoundRobin<Designator>(newDesignators, FuncDesignatorIsWorking);
        }

        void CompileRaycastCameras()
        {
            List<IMyCameraBlock> newRaycastCameras;
            GetBlocksFromGroups(out newRaycastCameras, RAYCAST_CAMERA_GRP_TAG, (b) => { b.EnableRaycast = true; b.Enabled = true; return true; });
            if (newRaycastCameras == null)
            {
                newRaycastCameras = new List<IMyCameraBlock>(0);
            }
            raycastCameras = newRaycastCameras;
            raycastHandler.Cameras = raycastCameras;
        }

        void CompilePDCGroups()
        {
            Dictionary<long, PDCTurret> previousTurrets = null;
            {
                if (pdcList != null && pdcList.Count > 0)
                {
                    previousTurrets = new Dictionary<long, PDCTurret>();
                    foreach (PDCTurret pdc in pdcList)
                    {
                        if (!previousTurrets.ContainsKey(pdc.AzimuthRotor.EntityId))
                        {
                            previousTurrets.Add(pdc.AzimuthRotor.EntityId, pdc);
                        }
                    }
                }
            }

            HashSet<IMyTerminalBlock> uniqueFilter = new HashSet<IMyTerminalBlock>();

            List<IMyMotorStator> pdcAzimuthRotors = new List<IMyMotorStator>();
            Dictionary<long, List<IMyMotorStator>> pdcElevationRotors = new Dictionary<long, List<IMyMotorStator>>();
            Dictionary<long, List<IMyUserControllableGun>> pdcWeapons = new Dictionary<long, List<IMyUserControllableGun>>();
            Dictionary<long, IMyTerminalBlock> pdcAimBlock = new Dictionary<long, IMyTerminalBlock>();
            Dictionary<long, IMyShipController> pdcRemote = new Dictionary<long, IMyShipController>();
            Dictionary<long, IMyShipConnector> pdcConnector = new Dictionary<long, IMyShipConnector>();

            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            List<PDCTurret> newPDCList = new List<PDCTurret>();
            GridTerminalSystem.GetBlockGroups(groups, (b) => { return b.Name.IndexOf(PDC_GRP_TAG, StringComparison.OrdinalIgnoreCase) > -1; });

            foreach (IMyBlockGroup group in groups)
            {
                group.GetBlocksOfType(dummyBlocks, (block) =>
                {
                    if (Me.IsSameConstructAs(block) && uniqueFilter.Add(block))
                    {
                        if (block is IMyMotorStator)
                        {
                            if (NameContains(block, AZIMUTH_ROTOR_TAG)) { pdcAzimuthRotors.Add(block as IMyMotorStator); }
                            else if (NameContains(block, ELEVATION_ROTOR_TAG))
                            {
                                if (pdcElevationRotors.ContainsKey(block.CubeGrid.EntityId))
                                {
                                    pdcElevationRotors[block.CubeGrid.EntityId].Add(block as IMyMotorStator);
                                }
                                else
                                {
                                    List<IMyMotorStator> list = new List<IMyMotorStator>();
                                    list.Add(block as IMyMotorStator);
                                    pdcElevationRotors.Add(block.CubeGrid.EntityId, list);
                                }
                            }
                        }
                        else if (block is IMyUserControllableGun)
                        {
                            if (pdcWeapons.ContainsKey(block.CubeGrid.EntityId))
                            {
                                pdcWeapons[block.CubeGrid.EntityId].Add(block as IMyUserControllableGun);
                            }
                            else
                            {
                                List<IMyUserControllableGun> list = new List<IMyUserControllableGun>();
                                list.Add(block as IMyUserControllableGun);
                                pdcWeapons.Add(block.CubeGrid.EntityId, list);
                            }
                        }
                        else if (block is IMyShipController)
                        {
                            if (!pdcRemote.ContainsKey(block.CubeGrid.EntityId))
                            {
                                pdcRemote.Add(block.CubeGrid.EntityId, block as IMyShipController);
                            }
                        }
                        else if (block is IMyShipConnector)
                        {
                            if (!pdcConnector.ContainsKey(block.CubeGrid.EntityId))
                            {
                                if (NameContains(block, RELOAD_CONNECTOR_TAG))
                                {
                                    pdcConnector.Add(block.CubeGrid.EntityId, block as IMyShipConnector);
                                }
                            }
                        }
                        else if (NameContains(block, AIM_BLOCK_TAG))
                        {
                            if (!pdcAimBlock.ContainsKey(block.CubeGrid.EntityId))
                            {
                                pdcAimBlock.Add(block.CubeGrid.EntityId, block);
                            }
                        }
                    }
                    return false;
                });
            }
            foreach (IMyMotorStator azimuthRotor in pdcAzimuthRotors)
            {
                if (azimuthRotor.TopGrid != null)
                {
                    List<IMyMotorStator> checkElevationRotors;
                    if (pdcElevationRotors.TryGetValue(azimuthRotor.TopGrid.EntityId, out checkElevationRotors))
                    {
                        List<IMyMotorStator> elevationRotors = new List<IMyMotorStator>();
                        List<List<IMyUserControllableGun>> weapons = new List<List<IMyUserControllableGun>>();
                        List<IMyTerminalBlock> aimBlocks = new List<IMyTerminalBlock>();

                        WeaponProfile profile = null;

                        IMyTerminalBlock connectorRefBlock = null;

                        foreach (IMyMotorStator elevationRotor in checkElevationRotors)
                        {
                            if (elevationRotor.TopGrid != null)
                            {
                                List<IMyUserControllableGun> checkWeapons;
                                if (pdcWeapons.TryGetValue(elevationRotor.TopGrid.EntityId, out checkWeapons))
                                {
                                    IMyTerminalBlock aimBlock;
                                    if (pdcAimBlock.ContainsKey(elevationRotor.TopGrid.EntityId))
                                    {
                                        aimBlock = pdcAimBlock[elevationRotor.TopGrid.EntityId];
                                    }
                                    else
                                    {
                                        aimBlock = checkWeapons[0];
                                    }

                                    elevationRotors.Add(elevationRotor);
                                    weapons.Add(checkWeapons);
                                    aimBlocks.Add(aimBlock);

                                    if (profile == null && checkWeapons.Count > 0)
                                    {
                                        profile = GetWeaponProfile(checkWeapons[0]);
                                    }
                                    if (connectorRefBlock == null)
                                    {
                                        connectorRefBlock = aimBlock;
                                    }
                                }
                            }
                        }
                        if (elevationRotors.Count > 0)
                        {
                            IMyShipController controller;
                            if (pdcRemote.ContainsKey(elevationRotors[0].TopGrid.EntityId))
                            {
                                controller = pdcRemote[elevationRotors[0].TopGrid.EntityId];
                            }
                            else
                            {
                                controller = remote;
                            }

                            if (profile == null)
                            {
                                profile = weaponProfiles.gatlingProfile;
                            }

                            PDCTurret pdc = new PDCTurret(azimuthRotor.CustomName, azimuthRotor, elevationRotors, aimBlocks, controller, weapons, profile, settings);

                            IMyShipConnector baseConnector = null;
                            if (pdcConnector.ContainsKey(elevationRotors[0].CubeGrid.EntityId))
                            {
                                baseConnector = pdcConnector[elevationRotors[0].CubeGrid.EntityId];
                            }

                            IMyShipConnector turretConnector = null;
                            if (pdcConnector.ContainsKey(connectorRefBlock.CubeGrid.EntityId))
                            {
                                turretConnector = pdcConnector[connectorRefBlock.CubeGrid.EntityId];
                            }

                            if (baseConnector != null && turretConnector != null)
                            {
                                pdc.ReloadBaseConnector = baseConnector;
                                pdc.ReloadTurretConnector = turretConnector;
                            }

                            if (previousTurrets != null && previousTurrets.ContainsKey(pdc.AzimuthRotor.EntityId))
                            {
                                pdc.TransferTarget(previousTurrets[pdc.AzimuthRotor.EntityId]);
                            }
                            else
                            {
                                pdc.ReleaseWeapons(true);
                                pdc.ResetRotors();
                            }
                            newPDCList.Add(pdc);
                        }
                    }
                }
            }
            if (settings.MaxPDCUpdatesPerTick == 0)
            {
                curPDCUpdatesPerTick = Math.Max((int)Math.Ceiling(settings.MinPDCRefreshRate * newPDCList.Count / (double)TICKS_PER_SECOND), 1);
                curPDCUpdatesSkipTicks = 0;
            }
            else
            {
                if (settings.MaxPDCUpdatesPerTick < 1f && settings.MaxPDCUpdatesPerTick > 0)
                {
                    curPDCUpdatesPerTick = 1;
                    curPDCUpdatesSkipTicks = Math.Min((int)Math.Floor(1f / settings.MaxPDCUpdatesPerTick), TICKS_PER_SECOND);
                }
                else
                {
                    curPDCUpdatesPerTick = Math.Max(Math.Min(newPDCList.Count, (int)Math.Ceiling(settings.MaxPDCUpdatesPerTick)), 1);
                    curPDCUpdatesSkipTicks = 0;
                }
            }
            double refreshPerPass = (double)newPDCList.Count / curPDCUpdatesPerTick;
            double targetedCountGainPerHit = refreshPerPass / Math.Max(settings.PriorityMaxRefreshTicks, 1);

            foreach (PDCTurret pdc in newPDCList)
            {
                pdc.TargetedCountGainPerHit = targetedCountGainPerHit;
                pdc.ReloadMaxAngleRadians = MathHelperD.ToRadians(settings.ReloadMaxAngle);
                pdc.ReloadLockStateAngleRadians = MathHelperD.ToRadians(settings.ReloadLockStateAngle);

                pdc.GridClearanceCheck = occlusionChecker;
            }

            pdcList = newPDCList;
            pdcAssignRR = new RoundRobin<PDCTurret>(newPDCList, FuncPDCIsWorking);
            pdcFireRR = new RoundRobin<PDCTurret>(newPDCList, FuncPDCIsWorking);
        }

        void CompileManualTargeters()
        {
            List<ManualPDCTarget> newManualTargeters = new List<ManualPDCTarget>();
            Dictionary<string, ManualPDCTarget> newManualTargetersLookup = new Dictionary<string, ManualPDCTarget>();

            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups, (b) => { return NameContains(b, MANUAL_AIMING_GRP_TAG); });
            foreach (IMyBlockGroup group in groups)
            {
                group.GetBlocksOfType(dummyBlocks, (b) =>
                {
                    int codeId = 1;
                    int pos = b.CustomName.IndexOf(MANUAL_AIMING_GRP_PREFIX, StringComparison.OrdinalIgnoreCase);
                    if (pos > -1)
                    {
                        if (pos + MANUAL_AIMING_AIMBLOCK_TAG.Length < b.CustomName.Length)
                        {
                            if (int.TryParse($"{b.CustomName[pos + MANUAL_AIMING_GRP_PREFIX.Length]}", out codeId))
                            {
                                if (codeId < 1 || codeId > 9)
                                {
                                    codeId = 1;
                                }
                            }
                            else
                            {
                                codeId = 1;
                            }
                        }
                    }
                    string codeIdStr = MANUAL_AIMING_GRP_PREFIX + codeId;
                    ManualPDCTarget manualTarget;
                    if (newManualTargetersLookup.ContainsKey(codeIdStr))
                    {
                        manualTarget = newManualTargetersLookup[codeIdStr];
                    }
                    else
                    {
                        if (manualTargetersLookup != null && manualTargetersLookup.ContainsKey(codeIdStr))
                        {
                            manualTarget = manualTargetersLookup[codeIdStr];
                        }
                        else
                        {
                            manualTarget = new ManualPDCTarget(codeIdStr, null, null, null);
                            manualTarget.MaxManualRaycastDistance = settings.ManualAimRaycastDistance;
                        }

                        newManualTargeters.Add(manualTarget);
                        newManualTargetersLookup.Add(codeIdStr, manualTarget);

                    }

                    if (NameContains(b, MANUAL_AIMING_AIMBLOCK_TAG))
                    {
                        manualTarget.AimingBlock = b;
                        manualTarget.AimingTurret = b as IMyLargeTurretBase;
                    }
                    else if (NameContains(b, MANUAL_AIMING_ALERT_TAG))
                    {
                        manualTarget.AlertBlock = b;
                    }
                    else if (NameContains(b, MANUAL_AIMING_STATUS_TAG))
                    {
                        IMyTextSurfaceProvider surfaceProvider = b as IMyTextSurfaceProvider;
                        if (surfaceProvider != null)
                        {
                            try { manualTarget.DisplayStatus = surfaceProvider.GetSurface(0); } catch (Exception) { }
                        }
                    }
                    else
                    {
                        if (manualTarget.AimingBlock == null)
                        {
                            manualTarget.AimingBlock = b;
                            manualTarget.AimingTurret = b as IMyLargeTurretBase;
                        }

                        if (manualTarget.DisplayStatus == null && b is IMyTextSurfaceProvider)
                        {
                            try { manualTarget.DisplayStatus = ((IMyTextSurfaceProvider)b).GetSurface(0); } catch (Exception) { }
                        }
                    }

                    return false;

                });

                break;
            }
            int index = 0;

            while (index < newManualTargeters.Count)
            {
                ManualPDCTarget manualTarget = newManualTargeters[index];
                if (manualTarget.AimingBlock == null)
                {
                    if (index + 1 == newManualTargeters.Count)
                    {
                        newManualTargeters.RemoveAt(index);
                    }
                    else
                    {
                        newManualTargeters[index] = newManualTargeters[newManualTargeters.Count - 1];
                        newManualTargeters.RemoveAt(newManualTargeters.Count - 1);
                    }
                    newManualTargetersLookup.Remove(manualTarget.CodeId);
                }
                index++;
            }
            manualTargeters = newManualTargeters;
            manualTargetersLookup = newManualTargetersLookup;
        }
        void CompileDisplayPanels()
        {
            GetBlocksFromGroups(out displayPanels, DISPLAY_GRP_TAG);
        }

        private void GetBlocks()
        {
                        GridTerminalSystem.GetBlocksOfType(myTextPanels, block => block.IsSameConstructAs(Me) && block.CustomName.Contains(DDS_TAG));
        }
        #endregion

        #region Main Processing
        void UpdateManualAimRaycast()
        {
            if (manualTargeters?.Count > 0 && settings.ManualAimRaycastRefreshInterval > 0 && (clock % settings.ManualAimRaycastRefreshInterval < manualTargeters.Count))
            {
                ManualPDCTarget manualTarget = manualTargeters[clock % settings.ManualAimRaycastRefreshInterval];
                if (IsWorking(manualTarget.AimingBlock) && manualTarget.Enabled)
                {
                    Vector3D aimVector = manualTarget.GetForwardViewDirection();
                    Vector3D targetPosition = manualTarget.AimingBlock.WorldMatrix.Translation + (aimVector * manualTarget.MaxManualRaycastDistance);
                    MyDetectedEntityInfo entityInfo;
                    raycastHandler.Raycast(ref targetPosition, out entityInfo, settings.RaycastExtensionDistance);
                    if (!entityInfo.IsEmpty() && IsValidTarget(ref entityInfo))
                    {
                        manualTarget.SelectedEntityId = entityInfo.EntityId;
                        manualTarget.Position = (entityInfo.HitPosition.HasValue ? entityInfo.HitPosition.Value : entityInfo.Position);
                        manualTarget.Velocity = entityInfo.Velocity;
                        manualTarget.DetectedClock = clock;

                        Vector3D estimatedTargetOffsetPoint = manualTarget.Position - manualTarget.AimingBlock.WorldMatrix.Translation;
                        estimatedTargetOffsetPoint = Vector3D.ProjectOnVector(ref estimatedTargetOffsetPoint, ref aimVector) + manualTarget.AimingBlock.WorldMatrix.Translation;
                        manualTarget.OffsetPoint = Vector3D.TransformNormal(estimatedTargetOffsetPoint - entityInfo.Position, MatrixD.Transpose(entityInfo.Orientation));
                        PDCTarget target;
                        targetManager.UpdateTarget(ref entityInfo, clock, out target);
                        if (target != null)
                        {
                            manualTarget.Enabled = false;

                            if (IsWorking(manualTarget.AlertBlock))
                            {
                                if (manualTarget.AlertBlock is IMySoundBlock)
                                {
                                    ((IMySoundBlock)manualTarget.AlertBlock).Play();
                                }
                                else if (manualTarget.AlertBlock is IMyTimerBlock)
                                {
                                    ((IMyTimerBlock)manualTarget.AlertBlock).Trigger();
                                }
                                else
                                {
                                    IMyFunctionalBlock funcBlock = manualTarget.AlertBlock as IMyFunctionalBlock;
                                    if (funcBlock != null && !funcBlock.Enabled)
                                    {
                                        funcBlock.Enabled = true;
                                    }
                                }
                            }
                            target.MaxAllowedRaycastDistance = manualTarget.MaxAllowedRaycastDistance;
                            target.MaxAllowedMissileLaunchDistance = manualTarget.MaxAllowedMissileLaunchDistance;
                            foreach (MissileCommsTarget commsTarget in guidanceCommsTargets)
                            {
                                if (commsTarget.Target == manualTarget)
                                {
                                    commsTarget.Target = target;
                                    commsTarget.TransmitUntilClock = clock + settings.MissileTransmitDurationTicks;
                                }
                            }
                        }
                    }
                }
                else if (manualTarget.SelectedEntityId > 0 && !targetManager.TargetExists(manualTarget.SelectedEntityId))
                {
                    manualTarget.SelectedEntityId = -1;
                }
            }
        }
        void UpdateDesignatorTargets()
        {
            var gridEntityId = Me.CubeGrid.EntityId;

            if (settings.WCAPI != null)
            {
                WCThreatsScratchpad.Clear();
                settings.WCAPI.GetSortedThreats(Me, WCThreatsScratchpad);
                foreach (var target in WCThreatsScratchpad.Keys)
                {
                    MyDetectedEntityInfo entityInfo = target;
                    if (targetManager.UpdateTarget(ref entityInfo, clock))
                    {
                        nextPriorityRefreshClock = clock + settings.PriorityMinRefreshTicks;
                    }
                }
            }

            else
            {
                designatorTargetRR.Begin();
                int maxCount = (settings.MaxDesignatorUpdatesPerTick == 0 ? designators.Count : settings.MaxDesignatorUpdatesPerTick);
                for (int i = 0; i < maxCount; i++)
                {
                    Designator designator = designatorTargetRR.GetNext();
                    if (designator != null)
                    {
                        MyDetectedEntityInfo entityInfo = designator.Turret.GetTargetedEntity();

                        if (targetManager.UpdateTarget(ref entityInfo, clock))
                        {
                            nextPriorityRefreshClock = clock + settings.PriorityMinRefreshTicks;
                        }

                        designatorTargetAcquiredClock = clock;

                        if (settings.UseDesignatorReset)
                        {
                            if (clock >= designator.NextResetClock)
                            {
                                designator.Turret.ResetTargetingToDefault();
                                designator.Turret.EnableIdleRotation = false;
                                designator.NextResetClock = clock + settings.DesignatorResetInterval;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (settings.UseRangeSweeper)
                {
                    if (clock <= designatorTargetAcquiredClock + settings.TargetFoundHoldTicks)
                    {
                        designatorOperationRR.Begin();

                        maxCount = (settings.RangeSweeperPerTick == 0 ? designators.Count : settings.RangeSweeperPerTick);
                        for (int i = 0; i < maxCount; i++)
                        {
                            Designator designator = designatorOperationRR.GetNext();
                            if (designator != null)
                            {
                                if (clock >= designator.NextSweeperClock)
                                {
                                    designator.SetMaxRange();

                                    designator.NextSweeperClock = clock + settings.RangeSweeperInterval;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        void UpdateRaycastTargets()
        {
            PDCTarget target = targetManager.GetOldestRaycastUpdatedTarget();
            if (target != null)
            {
                if (clock - target.RaycastRefreshClock >= settings.RaycastTargetRefreshTicks)
                {
                    if (raycastHandler.Cameras.Count > 0)
                    {
                        double maxAllowedRaycastDistance = (target.MaxAllowedRaycastDistance == 0 ? settings.MaxRaycastTrackingDistance : target.MaxAllowedRaycastDistance); if ((target.Position - Me.WorldMatrix.Translation).LengthSquared() <= maxAllowedRaycastDistance * maxAllowedRaycastDistance)
                        {
                            Vector3D targetPosition = target.Position + (target.Velocity * (clock - target.DetectedClock) * INV_ONE_TICK);
                            MyDetectedEntityInfo entityInfo;
                            if (raycastHandler.Raycast(ref targetPosition, out entityInfo, settings.RaycastExtensionDistance))
                            {
                                HandleRaycastParameters(ref entityInfo);
                            }
                            nextRaycastGlobalClock = clock + settings.RaycastGlobalRefreshTicks;
                        }
                    }
                    targetManager.UpdateRaycastRefreshClock(target.EntityId, clock);

                    HandlePriorityAssigment(target);
                }
            }
            if (settings.UsePDCSpray && settings.RandomOffsetProbeInterval > 0 && clock % settings.RandomOffsetProbeInterval == 0)
            {
                PDCTarget largestTarget = targetManager.FindLargestTarget();
                if (largestTarget != null && largestTarget.Orientation !=null && largestTarget.TargetSizeSq >= settings.PDCSprayMinTargetSize * settings.PDCSprayMinTargetSize)
                {
                    double maxAllowedRaycastDistance = (largestTarget.MaxAllowedRaycastDistance == 0 ? settings.MaxRaycastTrackingDistance : largestTarget.MaxAllowedRaycastDistance);
                    Vector3D largestPosition = largestTarget.CenterPosition + (largestTarget.Velocity * (clock - largestTarget.DetectedClock) * INV_ONE_TICK);

                    if ((largestPosition - Me.WorldMatrix.Translation).LengthSquared() <= maxAllowedRaycastDistance * maxAllowedRaycastDistance)
                    {
                        double probeRadius = Math.Sqrt(largestTarget.TargetSizeSq) * 0.5 * OFFSET_PROBE_RANDOM_FACTOR;

                        Vector3D partialOffsetVector = new Vector3D(((rnd.NextDouble() * 2) - 1) * probeRadius, ((rnd.NextDouble() * 2) - 1) * probeRadius, ((rnd.NextDouble() * 2) - 1) * probeRadius);
                        Vector3D targetPosition = largestPosition + Vector3D.TransformNormal(partialOffsetVector, largestTarget.Orientation.Value);

                        MyDetectedEntityInfo entityInfo;
                        if (raycastHandler.Raycast(ref targetPosition, out entityInfo, settings.RaycastExtensionDistance))
                        {
                            HandleRaycastParameters(ref entityInfo);
                        }
                        nextRaycastGlobalClock = clock + settings.RaycastGlobalRefreshTicks;
                    }
                }
            }
        }
        void HandleRaycastParameters(ref MyDetectedEntityInfo entityInfo)
        {
            if (entityInfo.IsEmpty())
            {
                return;
            }
            if (IsValidTarget(ref entityInfo))
            {
                PDCTarget target;
                targetManager.UpdateTarget(ref entityInfo, clock, out target);
                if (target == null)
                {
                    return;
                }
                target.TargetSizeSq = entityInfo.BoundingBox.Extents.LengthSquared();
                if (target.CheckTargetSizeSq == 0)
                {
                    target.CheckTargetSizeSq = target.TargetSizeSq;
                }
                if (settings.UsePDCSpray && target.TargetSizeSq >= settings.PDCSprayMinTargetSize * settings.PDCSprayMinTargetSize && entityInfo.HitPosition.HasValue)
                {
                    if (target.OffsetPoints == null)
                    {
                        target.OffsetPoints = new List<OffsetPoint>(OFFSET_POINTS_MAX_COUNT);
                    }

                    Vector3D offsetPoint = Vector3D.TransformNormal(entityInfo.HitPosition.Value - entityInfo.Position, MatrixD.Transpose(entityInfo.Orientation));

                    if (target.OffsetPoints.Count >= OFFSET_POINTS_MAX_COUNT)
                    {
                        int selectedIndex = 0;
                        double selectedDistanceSq = double.MaxValue;
                        for (int i = 0; i < target.OffsetPoints.Count; i++)
                        {
                            if (clock > target.OffsetPoints[i].LastUpdatedClock + OFFSET_POINTS_EXPIRE_TICKS)
                            {
                                selectedIndex = i;
                                selectedDistanceSq = 0;
                                break;
                            }

                            double distance = (target.OffsetPoints[i].Point - offsetPoint).LengthSquared();
                            if (distance < selectedDistanceSq)
                            {
                                selectedIndex = i;
                                selectedDistanceSq = distance;
                            }
                        }
                        if (selectedDistanceSq < double.MaxValue)
                        {
                            target.OffsetPoints[selectedIndex] = new OffsetPoint(ref offsetPoint, clock);
                        }
                    }
                    else
                    {
                        target.OffsetPoints.Add(new OffsetPoint(ref offsetPoint, clock));
                    }
                }
            }
            else
            {
                targetManager.AddToBlackList(entityInfo.EntityId);
                targetManager.RemoveTarget(entityInfo.EntityId);
            }
        }
        void HandlePriorityAssigment(PDCTarget target)
        {
            Vector3D shipPosition = Me.GetPosition();
            Vector3D shipVelocity = (remote != null ? remote.GetShipVelocities().LinearVelocity : Vector3D.Zero);

            if (target.TargetSizeSq > 0 && target.TargetSizeSq < settings.MinTargetSizeEngage * settings.MinTargetSizeEngage)
            {
                targetManager.AddToBlackList(target.EntityId);
                targetManager.RemoveTarget(target.EntityId);
            }
            else if (clock - target.DetectedClock <= settings.TargetLostTicks)
            {
                double priorityValue = ComputePriority(shipRadius, shipPosition, shipVelocity, target);
                priorityValue += rnd.NextDouble() * 0.000000000001;

                target.PriorityValue = priorityValue;

                maxPriorityValue = Math.Max(priorityValue, maxPriorityValue);

                if (remote == null || remote.GetShipVelocities().LinearVelocity.LengthSquared() <= settings.MissileLaunchSpeedLimit * settings.MissileLaunchSpeedLimit)
                {
                    if (target.TargetSizeSq >= settings.MissileMinTargetSize * settings.MissileMinTargetSize)
                    {
                        if (target.MissileLaunchLastClock == 0 || clock >= target.MissileLaunchLastClock + settings.MissileReassignIntervalTicks)
                        {
                            target.MissileLaunchLastClock = clock;
                            double maxAllowedMissileLaunchDistance = (target.MaxAllowedMissileLaunchDistance == 0 ? settings.MaxMissileLaunchDistance : target.MaxAllowedMissileLaunchDistance);
                            if ((target.Position - Me.WorldMatrix.Translation).LengthSquared() <= maxAllowedMissileLaunchDistance * maxAllowedMissileLaunchDistance)
                            {
                                if (target.TargetSizeSq == 0)
                                {
                                    target.MissileRemainingCount = 1;
                                }
                                else
                                {
                                    target.MissileRemainingCount = (int)Math.Ceiling(Math.Sqrt(target.TargetSizeSq) / Math.Max(settings.MissileCountPerSize, 1));
                                }
                            }
                        }
                    }
                }
            }
            else { targetManager.RemoveTarget(target.EntityId); }
        }
        void UpdateAllies()
        {
            AllyTrack ally = allyManager.GetOldestUpdatedAlly();
            if (ally != null)
            {
                if (clock - ally.LastDetectedClock > settings.AllyTrackLostTicks)
                {
                    allyManager.RemoveAlly(ally.EntityId);
                }
            }
        }
        void AssignTargets()
        {
            //             if ( debugMode && assignmentState == 0 )
            //             {
            //                 debug.Clear();
            //                 debug.AppendLine("AssignTargets");
            //             }
            switch (assignmentState)
            {
                case 0:
                    //                    if (debugMode) debug.AppendLine("case 0");
                    if (clock >= nextPriorityRefreshClock)
                    {
                        if (targetManager.Count() > 0)
                        {
                            nextPriorityRefreshClock = clock + settings.PriorityMaxRefreshTicks;

                            sortedEntityIds.Clear();
                            maxPriorityValue = 0;

                            List<PDCTarget> pdcTargets = targetManager.GetAllTargets();
                            foreach (PDCTarget target in pdcTargets)
                            {
                                if (!sortedEntityIds.ContainsKey(target.PriorityValue))
                                {
                                    sortedEntityIds.Add(target.PriorityValue, target);

                                    maxPriorityValue = Math.Max(target.PriorityValue, maxPriorityValue);
                                }
                            }

                            pdcAssignRR.Reset();
                            pdcAssignRR.Begin();

                            assignmentState = 1;
                        }
                    }
                    break;
                case 1:
                    //                    if (debugMode) debug.AppendLine("case 1");
                    PDCTurret pdc = pdcAssignRR.GetNext();
                    if (pdc != null)
                    {
                        //                        if (debugMode) debug.AppendLine("case 1 s0");
                        if (pdc != null && !pdc.IsDamaged)
                        {
                            //                            if (debugMode) debug.AppendLine("case 1 s1");
                            double selectedPriority = 0;
                            PDCTarget selectedTarget = null;

                            double preferredPriority = 0;
                            PDCTarget preferredTarget = null;

                            foreach (KeyValuePair<double, PDCTarget> keyTarget in sortedEntityIds)
                            {
                                //                                if (debugMode) debug.AppendLine("IsTargetable");
                                if (pdc.IsTargetable(keyTarget.Value, clock))
                                {
                                    //                                    if (debugMode) debug.AppendLine("=true");
                                    selectedPriority = keyTarget.Key;
                                    selectedTarget = keyTarget.Value;

                                    if (pdc.TurretPrioritySize > 0)
                                    {
                                        if (selectedTarget.TargetSizeSq >= pdc.TurretPrioritySize * pdc.TurretPrioritySize)
                                        {
                                            preferredPriority = selectedPriority;
                                            preferredTarget = selectedTarget;

                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            if (preferredTarget != null)
                            {
                                //                                if (debugMode) debug.AppendLine("case 1 s2");
                                selectedPriority = preferredPriority;
                                selectedTarget = preferredTarget;
                            }
                            if (selectedTarget != null)
                            {
                                //                                if (debugMode) debug.AppendLine("TargetAssigned to pcd" );
                                pdc.TargetInfo = selectedTarget;

                                if (sortedEntityIds.ContainsKey(selectedPriority))
                                {
                                    sortedEntityIds.Remove(selectedPriority);
                                    maxPriorityValue += 1000; selectedPriority = maxPriorityValue;
                                    sortedEntityIds.Add(selectedPriority, selectedTarget);
                                }
                            }
                            else
                            {
                                if (pdc.TargetInfo != null && !targetManager.TargetExists(pdc.TargetInfo.EntityId)) { pdc.TargetInfo = null; }
                                pdc.ReleaseWeapons(); pdc.ResetRotors();
                            }
                        }
                    }
                    else
                    {
                        assignmentState = 0;
                    }
                    break;
            }
        }
        double ComputePriority(double shipRadius, Vector3D shipPosition, Vector3D shipVelocity, PDCTarget target)
        {
            Vector3D rangeVector = target.Position - shipPosition;
            double priorityValue = rangeVector.Length();

            PlaneD plane;
            if (shipVelocity.LengthSquared() < 0.01)
            {
                plane = new PlaneD(shipPosition, Vector3D.Normalize(rangeVector));
            }
            else
            {
                plane = new PlaneD(shipPosition, shipPosition + shipVelocity, shipPosition + rangeVector.Cross(shipVelocity));
            }
            Vector3D intersectPoint = plane.Intersection(ref target.Position, ref target.Velocity);
            Vector3D targetTravelVector = intersectPoint - target.Position;

            if (targetTravelVector.Dot(ref target.Velocity) < 0)
            {
                priorityValue += settings.PriorityDowngradeConstant * 4;
            }
            else
            {
                double t = Math.Sqrt(targetTravelVector.LengthSquared() / Math.Max(target.Velocity.LengthSquared(), 0.000000000000001));
                if ((intersectPoint - (shipPosition + (shipVelocity * t))).LengthSquared() > shipRadius * shipRadius)
                {
                    priorityValue += settings.PriorityDowngradeConstant * 2;
                }
                else if (target.TargetSizeSq <= 0)
                {
                    priorityValue += settings.PriorityDowngradeConstant;
                }
                else if (target.TargetSizeSq < settings.MinTargetSizePriority * settings.MinTargetSizePriority)
                {
                    priorityValue += settings.PriorityDowngradeConstant * 3;
                }
            }
            if (target.CheckTargetSizeSq > target.TargetSizeSq)
            {
                priorityValue += settings.PriorityDowngradeConstant * Math.Max(target.PDCTargetedCount, 1);
            }
            else
            {
                priorityValue += settings.PriorityDowngradeConstant * Math.Min(target.PDCTargetedCount, 1);

            }
            return priorityValue;
        }
        void AimFireReloadPDC()
        {
            if (curPDCUpdatesSkipTicks > 0)
            {
                if (clock >= curPDCNextUpdateClock)
                {
                    curPDCNextUpdateClock = clock + curPDCUpdatesSkipTicks;
                }
                else
                {
                    return;
                }
            }

            pdcFireRR.Begin();

            int maxCount = (curPDCUpdatesPerTick == 0 ? pdcList.Count : curPDCUpdatesPerTick);
            for (int i = 0; i < maxCount; i++)
            {
                PDCTurret pdc = pdcFireRR.GetNext();
                if (pdc != null)
                {
                    if (clock >= pdc.LastReloadCheckClock + settings.ReloadCheckTicks)
                    {
                        pdc.LastReloadCheckClock = clock;
                        if (pdc.CheckReloadRequired())
                        {
                            if (clock >= pdc.LastReloadOperationCheck + settings.ReloadedCooldownTicks)
                            {
                                pdc.LastReloadOperationCheck = clock; pdc.PerformReloadProcedure(clock);
                            }
                        }
                    }
                    if (pdc.CurrentReloadState == PDCTurret.ReloadState.NONE)
                    {
                        if (pdc.TargetInfo != null)
                        {
                            pdc.AimAndFire(pdc.TargetInfo, clock);
                        }
                    }
                    else
                    {
                        pdc.PerformReloadProcedure(clock);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        #region Misc Processing
        void ReloadMissilesAndTorpedos()
        {
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            missileComputers = new List<IMyProgrammableBlock>();
            GridTerminalSystem.GetBlockGroups(groups, (b) => { return NameContains(b, MISSILE_PB_GRP_TAG); });
            foreach (IMyBlockGroup group in groups)
            {
                group.GetBlocksOfType(missileComputers, (b) =>
                {
                    if (b.Enabled && Me.IsSameConstructAs(b))
                    {
                        return AGMChecker.CheckSave(b);
                    }
                    else
                    {
                        return false;
                    }
                });
                break;
            }

            groups.Clear();
            torpedoComputers = new List<IMyProgrammableBlock>();
            GridTerminalSystem.GetBlockGroups(groups, (b) => { return NameContains(b, TORPEDO_PB_GRP_TAG); });
            foreach (IMyBlockGroup group in groups)
            {
                group.GetBlocksOfType(torpedoComputers, (b) =>
                {
                    if (b.Enabled && Me.IsSameConstructAs(b))
                    {
                        return AGMChecker.CheckSave(b);
                    }
                    else
                    {
                        return false;
                    }
                });
                break;
            }
        }
        void ProcessCommsMessage()
        {
            while (igcTargetTracksListener.HasPendingMessage)
            {
                object data = igcTargetTracksListener.AcceptMessage().Data;
                if (data is MyTuple<long, long, Vector3D, Vector3D, double>)
                {
                    MyTuple<long, long, Vector3D, Vector3D, double> targetTracksData = (MyTuple<long, long, Vector3D, Vector3D, double>)data;
                    if (!targetManager.TargetExists(targetTracksData.Item2) || clock - targetManager.GetTarget(targetTracksData.Item2).LastDetectedClock >= settings.TargetSlippedTicks)
                    {
                        TargetData targetData = new TargetData();
                        targetData.EntityId = targetTracksData.Item2;
                        targetData.Position = targetTracksData.Item3;
                        targetData.Velocity = targetTracksData.Item4;
                        targetManager.UpdateTarget(targetData, clock - 1, false);
                        if (targetTracksData.Item5 > 0)
                        {
                            PDCTarget target = targetManager.GetTarget(targetData.EntityId);
                            if (target != null && target.TargetSizeSq == 0)
                            {
                                target.TargetSizeSq = targetTracksData.Item5;
                            }
                        }
                    }
                }
            }
            while (igcTracksInfoListener.HasPendingMessage)
            {
                object data = igcTracksInfoListener.AcceptMessage().Data;
                if (data is MyTuple<long, long, Vector3D, Vector3D, double>)
                {
                    MyTuple<long, Vector3D, Vector3D, double, int, long> tracksInfoData = (MyTuple<long, Vector3D, Vector3D, double, int, long>)data;
                    if (!targetManager.TargetExists(tracksInfoData.Item1) || clock - targetManager.GetTarget(tracksInfoData.Item1).LastDetectedClock >= settings.TargetSlippedTicks)
                    {
                        if ((tracksInfoData.Item5 & (int)TrackTypeEnum.IsFriendly) == 0)
                        {
                            TargetData targetData = new TargetData();
                            targetData.EntityId = tracksInfoData.Item1;
                            targetData.Position = tracksInfoData.Item2;
                            targetData.Velocity = tracksInfoData.Item3;
                            targetManager.UpdateTarget(targetData, clock - 1, false);
                            if (tracksInfoData.Item4 > 0)
                            {
                                PDCTarget target = targetManager.GetTarget(targetData.EntityId);
                                if (target != null)
                                {
                                    if (target.TargetSizeSq == 0)
                                    {
                                        target.TargetSizeSq = tracksInfoData.Item4;
                                    }
                                    target.IsLargeGrid = (tracksInfoData.Item5 & (int)TrackTypeEnum.IsLargeGrid) > 0;
                                }
                            }
                        }
                        else
                        {
                            AllyTrack ally = new AllyTrack(tracksInfoData.Item1);
                            ally.Position = tracksInfoData.Item2;
                            ally.Velocity = tracksInfoData.Item3;
                            ally.SizeSq = tracksInfoData.Item4;
                            ally.IsLargeGrid = (tracksInfoData.Item5 & (int)TrackTypeEnum.IsLargeGrid) > 0;

                            allyManager.UpdateAlly(ally, clock);
                        }
                    }
                }
            }
        }

        void ProcessCommands(string arguments)
        {
            string[] tokens = arguments.Split(splitDelimiterCommand, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return;

            string command = tokens[0].Trim().ToUpper();

            ManualPDCTarget manualTarget = null;

            if (command.StartsWith(MANUAL_AIMING_GRP_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                int codeId;
                if (command.Length == MANUAL_AIMING_GRP_PREFIX.Length)
                {
                    codeId = 1;
                }
                else if (!int.TryParse(command.Substring(MANUAL_AIMING_GRP_PREFIX.Length).Trim(), out codeId))
                {
                    codeId = 0;
                }
                if (codeId >= 1 && manualTargetersLookup.ContainsKey(MANUAL_AIMING_GRP_PREFIX + codeId))
                {
                    manualTarget = manualTargetersLookup[MANUAL_AIMING_GRP_PREFIX + codeId];
                }
                if (manualTarget != null && tokens.Length <= 1)
                {
                    return;
                }
            }
            if (manualTarget != null)
            {
                command = tokens[1].Trim().ToUpper();
            }
            else if (manualTargeters?.Count > 0)
            {
                manualTarget = manualTargeters[0];
            }

            switch (command)
            {
                case MANUAL_LAUNCH_CMD:
                    bool useTorpedo = TokenContainsMatch(tokens, MANUAL_LAUNCH_TORPEDO_TAG);
                    List<IMyProgrammableBlock> guidanceComputers = (useTorpedo ? torpedoComputers : missileComputers);

                    if (TokenContainsMatch(tokens, MANUAL_LAUNCH_LARGEST_TAG))
                    {
                        PDCTarget largestTarget = targetManager.FindLargestTarget();
                        if (largestTarget != null)
                        {
                            LaunchManualForTarget(guidanceComputers, largestTarget, manualTarget?.TargetingPointType ?? TargetingPointTypeEnum.Center, manualTarget?.OffsetPoint, useTorpedo);
                        }
                    }
                    else if (manualTarget != null)
                    {
                        bool LaunchAutomaticMissiles = TokenContainsMatch(tokens, MANUAL_LAUNCH_EXTENDED_TAG);
                        if (manualTarget.SelectedEntityId > 0 && targetManager.TargetExists(manualTarget.SelectedEntityId))
                        {
                            PDCTarget currentTarget = targetManager.GetTarget(manualTarget.SelectedEntityId);
                            if (currentTarget != null)
                            {
                                if (LaunchAutomaticMissiles)
                                {
                                    currentTarget.MaxAllowedMissileLaunchDistance = Math.Max(manualTarget.MaxManualRaycastDistance, settings.MaxMissileLaunchDistance);
                                }
                                LaunchManualForTarget(guidanceComputers, currentTarget, manualTarget.TargetingPointType, manualTarget.OffsetPoint, useTorpedo);
                            }
                        }
                        else
                        {
                            manualTarget.Enabled = true;
                            manualTarget.SelectedEntityId = -1;
                            manualTarget.OffsetPoint = Vector3D.Zero;
                            manualTarget.Position = manualTarget.AimingBlock.WorldMatrix.Translation + (manualTarget.GetForwardViewDirection() * manualTarget.MaxManualRaycastDistance);
                            manualTarget.MaxAllowedRaycastDistance = Math.Max(manualTarget.MaxManualRaycastDistance, settings.MaxRaycastTrackingDistance);
                            manualTarget.MaxAllowedMissileLaunchDistance = (LaunchAutomaticMissiles ? Math.Max(manualTarget.MaxManualRaycastDistance, settings.MaxMissileLaunchDistance) : 0);

                            LaunchManualForTarget(guidanceComputers, manualTarget, manualTarget.TargetingPointType, manualTarget.OffsetPoint, useTorpedo);
                        }
                    }
                    break;

                case MANUAL_AIM_TRACK_TAG:
                    if (manualTarget != null)
                    {
                        if (manualTarget.Enabled && manualTarget.SelectedEntityId == -1)
                        {
                            manualTarget.Enabled = false;
                            manualTarget.OffsetPoint = Vector3D.Zero;
                        }
                        else
                        {
                            bool LaunchAutomaticMissiles = TokenContainsMatch(tokens, MANUAL_LAUNCH_EXTENDED_TAG);
                            manualTarget.Enabled = true;
                            manualTarget.SelectedEntityId = -1;
                            manualTarget.OffsetPoint = Vector3D.Zero;
                            manualTarget.Position = manualTarget.AimingBlock.WorldMatrix.Translation + (manualTarget.GetForwardViewDirection() * manualTarget.MaxManualRaycastDistance);
                            manualTarget.MaxAllowedRaycastDistance = Math.Max(manualTarget.MaxManualRaycastDistance, settings.MaxRaycastTrackingDistance);
                            manualTarget.MaxAllowedMissileLaunchDistance = (LaunchAutomaticMissiles ? Math.Max(manualTarget.MaxManualRaycastDistance, settings.MaxMissileLaunchDistance) : 0);
                        }
                    }
                    break;


                case MANUAL_AIM_RELEASE_TAG:
                    if (manualTarget != null)
                    {
                        manualTarget.Enabled = false;
                        manualTarget.SelectedEntityId = -1;
                        manualTarget.OffsetPoint = Vector3D.Zero;
                    }
                    break;

                case MANUAL_AIM_VALUE_SET_TAG:
                    if (manualTarget != null && tokens.Length >= 3)
                    {
                        switch (tokens[2].ToUpper().Trim())
                        {
                            case MANUAL_AIM_VALUE_SET_CENTER_TAG:
                                manualTarget.TargetingPointType = TargetingPointTypeEnum.Center;
                                break;
                            case MANUAL_AIM_VALUE_SET_OFFSET_TAG:
                                manualTarget.TargetingPointType = TargetingPointTypeEnum.Offset;
                                break;
                            case MANUAL_AIM_VALUE_SET_RANDOM_TAG:
                                manualTarget.TargetingPointType = TargetingPointTypeEnum.Random;
                                break;
                            case MANUAL_AIM_VALUE_SET_RANGE_TAG:
                                if (tokens.Length >= 4)
                                {
                                    double value;
                                    if (double.TryParse(tokens[3].Trim(), out value))
                                    {
                                        manualTarget.MaxManualRaycastDistance = Math.Min(Math.Max(value, 1000), 100000); ;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case MANUAL_AIM_VALUE_CYCLE_OFFSET_TAG:
                    if (manualTarget != null)
                    {
                        manualTarget.TargetingPointType = (TargetingPointTypeEnum)(((int)manualTarget.TargetingPointType + 1) % 3);
                    }
                    break;
                case MANUAL_AIM_VALUE_INC_RANGE_TAG:
                    if (manualTarget != null)
                    {
                        manualTarget.MaxManualRaycastDistance = Math.Min(Math.Max(manualTarget.MaxManualRaycastDistance + 1000, 1000), 100000); ;
                    }
                    break;
                case MANUAL_AIM_VALUE_DEC_RANGE_TAG:
                    if (manualTarget != null)
                    {
                        manualTarget.MaxManualRaycastDistance = Math.Min(Math.Max(manualTarget.MaxManualRaycastDistance - 1000, 1000), 100000); ;
                    }
                    break;

                case CMD_TOGGLE_ON_OFF:
                    switchedOn = !switchedOn;
                    DisplayStatusInName();
                    if (switchedOn == false)
                    {
                        foreach (PDCTurret pdc in pdcList)
                        {
                            pdc.TargetInfo = null;
                            pdc.ReleaseWeapons(true);
                            pdc.ResetRotors();
                        }
                    }
                    break;
                case CMD_TOGGLE_ON:
                    switchedOn = true;
                    DisplayStatusInName();
                    break;
                case CMD_TOGGLE_OFF:
                    switchedOn = false;
                    DisplayStatusInName();
                    foreach (PDCTurret pdc in pdcList)
                    {
                        pdc.TargetInfo = null;
                        pdc.ReleaseWeapons(true);
                        pdc.ResetRotors();
                    }
                    break;
                case CMD_AUTOLAUNCH_ON_OFF:
                    settings.AutoMissileLaunch = !settings.AutoMissileLaunch;
                    break;
                case CMD_AUTOLAUNCH_ON:
                    settings.AutoMissileLaunch = true;
                    break;
                case CMD_AUTOLAUNCH_OFF:
                    settings.AutoMissileLaunch = false;
                    break;
                case CMD_DEBUG_MODE:
                    debugMode = !debugMode;
                    break;
            }
        }
        #endregion

        #region Missile Launch

        void LaunchAutomaticMissiles()
        {
            if (settings.AutoMissileLaunch)
            {
                if (targetManager.Count() > 0)
                {
                    PDCTarget target = targetManager.GetOldestRaycastUpdatedTarget();
                    if (target != null)
                    {
                        if (target.MissileRemainingCount > 0 && targetManager.TargetExists(target.EntityId))
                        {
                            target.MissileRemainingCount--;

                            bool useOffsetTargeting = (target.MissileRemainingCount <= 0 ? true : (rnd.NextDouble() <= settings.MissileOffsetProbability));
                            LaunchMissileForTarget(target, useOffsetTargeting);

                            nextAutoMissileLaunchClock = clock + settings.MissileStaggerWaitTicks;
                        }
                    }
                }
            }
        }
        void LaunchMissileForTarget(PDCTarget target, bool useOffsetTargeting = false, bool fakeSimulation = false)
        {
            IMyProgrammableBlock missileComputer = null;
            double closest = double.MaxValue;
            foreach (IMyProgrammableBlock block in missileComputers)
            {
                if (block.IsWorking && GridTerminalSystem.GetBlockWithId(block.EntityId) != null)
                {
                    double distanceSq = (block.WorldMatrix.Translation - target.Position).LengthSquared();
                    if (distanceSq < closest)
                    {
                        closest = distanceSq;
                        missileComputer = block;
                    }
                }
            }

            if (missileComputer != null && !fakeSimulation)
            {
                MissileCommsTarget commsTarget = new MissileCommsTarget();
                commsTarget.TransmitUniqueId = GenerateUniqueId();
                commsTarget.Target = target;
                commsTarget.TransmitUntilClock = clock + settings.MissileTransmitDurationTicks;
                commsTarget.UseOffsetTargeting = useOffsetTargeting;
                guidanceCommsTargets.Enqueue(commsTarget);

                MissileCommsTarget switchLostTarget = new MissileCommsTarget();
                switchLostTarget.Target = target;
                switchLostTarget.TransmitUntilClock = int.MaxValue;
                switchLostTarget.TransmitAsSwitchLost = true;
                guidanceCommsTargets.Enqueue(switchLostTarget);

                MyTuple<long, long, long, int, float> launchData = new MyTuple<long, long, long, int, float>();

                launchData.Item1 = Me.EntityId;
                launchData.Item2 = commsTarget.TransmitUniqueId;
                launchData.Item3 = Me.EntityId;
                launchData.Item4 |= (int)AGMLaunchOptionEnum.OffsetTargeting;
                launchData.Item5 = (useOffsetTargeting ? (float)(Math.Sqrt(target.TargetSizeSq) * 0.5 * settings.MissileOffsetRadiusFactor) : 0f);

                bool sentIGCLaunchSuccess = IGC.SendUnicastMessage(missileComputer.EntityId, "", launchData);

                if (!sentIGCLaunchSuccess)
                {
                    MyIni config = new MyIni();
                    if (missileComputer.CustomData.Length > 0) config.TryParse(missileComputer.CustomData);
                    config.Set(MISSILE_INI_SECTION, "UniqueId", launchData.Item2);
                    config.Set(MISSILE_INI_SECTION, "GroupId", launchData.Item3);
                    if (useOffsetTargeting)
                    {
                        config.Set(MISSILE_INI_SECTION, "OffsetTargeting", launchData.Item4);
                        config.Set(MISSILE_INI_SECTION, "RandomOffsetAmount", launchData.Item5);
                    }
                    missileComputer.CustomData = config.ToString();
                    missileComputer.TryRun("FIRE:" + Me.EntityId);
                }
                missileComputers.Remove(missileComputer);
            }
        }
        void LaunchManualForTarget(List<IMyProgrammableBlock> guidanceComputers, PDCTarget target, TargetingPointTypeEnum targetingPointType = TargetingPointTypeEnum.Center, Vector3D? offsetPoint = null, bool checkUseCGE = false, bool fakeSimulation = false)
        {
            for (int i = 0; i < guidanceComputers.Count; i++)
            {
                IMyProgrammableBlock computer = guidanceComputers[i];

                if (computer.IsWorking && GridTerminalSystem.GetBlockWithId(computer.EntityId) != null && !fakeSimulation)
                {
                    MissileCommsTarget commsTarget = new MissileCommsTarget();
                    commsTarget.TransmitUniqueId = GenerateUniqueId();
                    commsTarget.Target = target;
                    commsTarget.TransmitUntilClock = clock + settings.ManualAimBroadcastDurationTicks;
                    if (checkUseCGE && NameContains(computer, USE_CGE_TAG)) commsTarget.CacheMissilePB = computer; guidanceCommsTargets.Enqueue(commsTarget);

                    MyIni config = null;
                    //[Notes] TargetDatalinkData(LaunchControlId, SetUniqueId, SetGroupId, SetBitMaskOptions(1=OffsetTargeting), SetRandomOffsetAmount) => MyTuple<long, long, long, int, float>
                    MyTuple<long, long, long, int, float> launchData = new MyTuple<long, long, long, int, float>();

                    launchData.Item1 = Me.EntityId;
                    launchData.Item2 = commsTarget.TransmitUniqueId;
                    launchData.Item3 = Me.EntityId;

                    switch (targetingPointType)
                    {
                        case TargetingPointTypeEnum.Offset:
                            launchData.Item4 |= (int)AGMLaunchOptionEnum.OffsetTargeting;

                            config = new MyIni();
                            if (computer.CustomData.Length > 0) config.TryParse(computer.CustomData);
                            config.Set(MISSILE_INI_SECTION, "ProbeOffsetVector", Vector3ToBase64(offsetPoint != null ? offsetPoint.Value : Vector3D.Zero));
                            computer.CustomData = config.ToString();

                            break;
                        case TargetingPointTypeEnum.Random:
                            launchData.Item4 |= (int)AGMLaunchOptionEnum.OffsetTargeting;
                            launchData.Item5 = (float)(Math.Sqrt(target.TargetSizeSq) * 0.5 * settings.MissileOffsetRadiusFactor);

                            break;
                        default:
                            launchData.Item5 = 0f;

                            break;
                    }
                    bool sentIGCLaunchSuccess = IGC.SendUnicastMessage(computer.EntityId, "", launchData);
                    if (!sentIGCLaunchSuccess)
                    {
                        if (config == null)
                        {
                            config = new MyIni();
                            if (computer.CustomData.Length > 0) config.TryParse(computer.CustomData);
                        }
                        config.Set(MISSILE_INI_SECTION, "UniqueId", launchData.Item2);
                        config.Set(MISSILE_INI_SECTION, "GroupId", launchData.Item3);
                        switch (targetingPointType)
                        {
                            case TargetingPointTypeEnum.Offset:
                                config.Set(MISSILE_INI_SECTION, "OffsetTargeting", launchData.Item4);
                                config.Set(MISSILE_INI_SECTION, "ProbeOffsetVector", Vector3ToBase64(offsetPoint != null ? offsetPoint.Value : Vector3D.Zero));
                                break;
                            case TargetingPointTypeEnum.Random:
                                config.Set(MISSILE_INI_SECTION, "OffsetTargeting", launchData.Item4);
                                config.Set(MISSILE_INI_SECTION, "RandomOffsetAmount", launchData.Item5);

                                break;
                        }
                        computer.CustomData = config.ToString();
                        computer.TryRun("FIRE:" + Me.EntityId);
                    }
                    if (guidanceComputers.Count == 1)
                    {
                        guidanceComputers.Clear();
                    }
                    else
                    {
                        guidanceComputers[i] = guidanceComputers[guidanceComputers.Count - 1];
                        guidanceComputers.RemoveAt(guidanceComputers.Count - 1);
                    }
                    break;
                }
            }
        }

        #endregion

        #region IGC Transmission

        void TransmitIGCMessages()
        {
            haveIGCTransmitted = false;
            if (curCommsQueuePriority == 0)
            {
                TransmitMissileInformation();
                if (!haveIGCTransmitted)
                {
                    TransmitTargetTracksInformation();
                }
                curCommsQueuePriority = 1;
            }
            else
            { TransmitTargetTracksInformation();
                if (!haveIGCTransmitted)
                {
                    TransmitMissileInformation();
                }
                curCommsQueuePriority = 0;
            }
        }
        void TransmitMissileInformation()
        {
            while (guidanceCommsTargets.Count > 0)
            {
                MissileCommsTarget current = guidanceCommsTargets.Dequeue();
                if (clock <= current.TransmitUntilClock)
                {
                    if (!(current.Target.EntityId == -1 || targetManager.TargetExists(current.Target.EntityId)))
                    {
                        break;
                    }
                    if (clock >= current.NextTransmitClock)
                    {
                        if (current.CacheMissilePB == null)
                        {
                            MyTuple<bool, long, long, long, Vector3D, Vector3D> targetDatalinkData = new MyTuple<bool, long, long, long, Vector3D, Vector3D>();
                            targetDatalinkData.Item1 = current.TransmitAsSwitchLost;
                            targetDatalinkData.Item2 = (current.TransmitAsSwitchLost ? Me.EntityId : current.TransmitUniqueId);
                            targetDatalinkData.Item3 = 0;
                            targetDatalinkData.Item4 = current.Target.EntityId;
                            targetDatalinkData.Item5 = current.Target.Position + (current.Target.Velocity * (clock - current.Target.DetectedClock + 1) * INV_ONE_TICK);
                            targetDatalinkData.Item6 = current.Target.Velocity; IGC.SendBroadcastMessage(current.TransmitAsSwitchLost ? IGC_MSG_TARGET_SWITCH_LOST : IGC_MSG_TARGET_DATALINK, targetDatalinkData);
                        }
                        else
                        {
                            Vector3D targetPosition = current.Target.Position + (current.Target.Velocity * (clock - current.Target.DetectedClock + 1) * INV_ONE_TICK);
                            MyIni cgeDatalink = new MyIni();
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "EntityId", current.Target.EntityId);
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "PositionX", targetPosition.X);
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "PositionY", targetPosition.Y);
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "PositionZ", targetPosition.Z);
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "VelocityX", current.Target.Velocity.X);
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "VelocityY", current.Target.Velocity.Y);
                            cgeDatalink.Set(CGE_MSG_TARGET_DATALINK, "VelocityZ", current.Target.Velocity.Z);

                            current.CacheMissilePB.TryRun(CGE_MSG_TARGET_DATALINK + cgeDatalink.ToString());
                        }
                        current.NextTransmitClock = clock + settings.MissileTransmitIntervalTicks;
                        haveIGCTransmitted = true;
                    }
                    if (!current.TransmitAsSwitchLost)
                    {
                        guidanceCommsTargets.Enqueue(current);
                    }
                    break;
                }
            }
        }
        void TransmitTargetTracksInformation()
        {
            if (clock >= nextTracksTranmissionClock && tracksCommsTargets.Count == 0)
            {
                if (targetManager.Count() > 0)
                {
                    List<PDCTarget> pdcTargets = targetManager.GetAllTargets();
                    pdcTargets.Sort(sortCommsTargetPriority);
                    foreach (PDCTarget target in pdcTargets)
                    {
                        tracksCommsTargets.Enqueue(target);
                    }
                }
                nextTracksTranmissionClock = clock + settings.TargetTracksTransmitIntervalTicks;
            }
            while (tracksCommsTargets.Count > 0)
            {
                PDCTarget target = tracksCommsTargets.Dequeue();

                if (targetManager.TargetUpToLocalDate(target.EntityId, clock - settings.TargetLostTicks))
                {
                    MyTuple<long, Vector3D, Vector3D, double, int, long> tracksInfoData = new MyTuple<long, Vector3D, Vector3D, double, int, long>();
                    tracksInfoData.Item1 = target.EntityId;
                    tracksInfoData.Item2 = target.Position + (target.Velocity * (clock - target.DetectedClock + 1) * INV_ONE_TICK);
                    tracksInfoData.Item3 = target.Velocity;
                    tracksInfoData.Item4 = target.TargetSizeSq;

                    if (target.IsLargeGrid) tracksInfoData.Item5 |= (int)TrackTypeEnum.IsLargeGrid;

                    tracksInfoData.Item6 = 0;

                    IGC.SendBroadcastMessage(IGC_MSG_TRACKS_INFO, tracksInfoData);
                    haveIGCTransmitted = true;

                    break;
                }
            }
        }
        void TransmitMyShipInformation()
        {
            if (clock >= nextMyShipTranmissionClock)
            {
                MyTuple<long, Vector3D, Vector3D, double, int, long> tracksInfoData = new MyTuple<long, Vector3D, Vector3D, double, int, long>();
                tracksInfoData.Item1 = Me.CubeGrid.EntityId;
                tracksInfoData.Item2 = Me.CubeGrid.WorldAABB.Center;
                tracksInfoData.Item3 = (remote != null ? remote.GetShipVelocities().LinearVelocity : Vector3D.Zero);
                tracksInfoData.Item4 = shipRadius * shipRadius * 4;
                tracksInfoData.Item5 |= (int)TrackTypeEnum.IsFriendly;
                if (Me.CubeGrid.GridSizeEnum == MyCubeSize.Large) tracksInfoData.Item5 |= (int)TrackTypeEnum.IsLargeGrid;
                tracksInfoData.Item6 = 0;
                IGC.SendBroadcastMessage(IGC_MSG_TRACKS_INFO, tracksInfoData);
                nextMyShipTranmissionClock = clock + settings.TargetTracksTransmitIntervalTicks;
            }
        }

        #endregion

        #region Display Logic

        void RefreshDisplays()
        {
            foreach (ManualPDCTarget manualTarget in manualTargeters)
            {
                if (manualTarget.DisplayStatus != null)
                {
                    IMyTextSurface surface = manualTarget.DisplayStatus;
                    Vector2 screenOffset;
                    Vector2 stepSize;
                    if
                        (surface.SurfaceSize.X == surface.SurfaceSize.Y) { screenOffset = (surface.TextureSize - surface.SurfaceSize) * 0.5f; stepSize = new Vector2(surface.SurfaceSize.X) * 0.0009765625f; }
                    else if
                        (surface.SurfaceSize.X > surface.SurfaceSize.Y)
                    {
                        screenOffset = (surface.TextureSize - surface.SurfaceSize + new Vector2(surface.SurfaceSize.X - surface.SurfaceSize.Y, 0f)) * 0.5f; stepSize = new Vector2(surface.SurfaceSize.Y) * 0.0009765625f;
                    }
                    else
                    {
                        screenOffset = (surface.TextureSize - surface.SurfaceSize + new Vector2(0f, surface.SurfaceSize.Y - surface.SurfaceSize.X)) * 0.5f; stepSize = new Vector2(surface.SurfaceSize.Y) * 0.0009765625f;
                    }
                    List<MySprite> sprites = new List<MySprite>();
                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", surface.TextureSize * 0.5f, null, Color.Black));
                    Vector2 fullBarSize = stepSize * new Vector2(1024f, 100f);
                    Vector2 fullBarMidWidth = new Vector2(fullBarSize.X * 0.5f, 0f);
                    Vector2 fullBarMidPosition = fullBarSize * 0.5f;
                    Vector2 verticalSpacing = new Vector2(0f, fullBarSize.Y);
                    Vector2 verticalGap = new Vector2(0f, stepSize.Y * 20f);
                    Vector2 halfBarSize = stepSize * new Vector2(480f, 100f);
                    Vector2 halfBarMidWidth = new Vector2(halfBarSize.X * 0.5f, 0f);
                    Vector2 halfBarMidPosition = halfBarSize * 0.5f;
                    Vector2 horizontalSpacing = stepSize * new Vector2(544f, 0f);

                    float textSize = standardTextSizeFactor * fullBarSize.Y;

                    Vector2 currentOffset = screenOffset + verticalGap;

                    string text;
                    Color textColor;
                    Color boxColor;

                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", currentOffset + fullBarMidPosition, fullBarSize, statusThisScriptBoxColor));
                    sprites.Add(new MySprite(SpriteType.TEXT, $"{DISPLAY_SCRIPT_NAME} v{DISPLAY_VERSION}", currentOffset + fullBarMidWidth, null, statusThisScriptTextColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing;

                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", currentOffset + fullBarMidPosition, fullBarSize, statusAimIdBoxColor));
                    sprites.Add(new MySprite(SpriteType.TEXT, $"Manual Targeter {manualTarget.CodeId}", currentOffset + fullBarMidWidth, null, statusAimIdTextColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing + verticalGap;

                    sprites.Add(new MySprite(SpriteType.TEXT, "Status", currentOffset + fullBarMidWidth, null, statusTitleTextColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing;

                    if (manualTarget.SelectedEntityId > 0)
                    {
                        text = "Target Locked";
                        textColor = statusLockedTextColor;
                        boxColor = statusLockedBoxColor;
                    }
                    else if
                        (manualTarget.Enabled)
                    { text = "Seeking";
                        textColor = statusSeekingTextColor;
                        boxColor = statusSeekingBoxColor;
                    }
                    else
                    {
                        text = "No Target";
                        textColor = statusNoTargetTextColor;
                        boxColor = statusNoTargetBoxColor;
                    }

                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", currentOffset + fullBarMidPosition, fullBarSize, boxColor));
                    sprites.Add(new MySprite(SpriteType.TEXT, text, currentOffset + fullBarMidWidth, null, textColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing + verticalGap;

                    sprites.Add(new MySprite(SpriteType.TEXT, "Current Target", currentOffset + fullBarMidWidth, null, statusTitleTextColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing;

                    if
                        (manualTarget.SelectedEntityId > 0)
                    {
                        text = GetTargetCode(manualTarget.SelectedEntityId);
                        textColor = statusCurrentTargetTextColor;
                        boxColor = statusCurrentTargetBoxColor;
                    }
                    else
                    {
                        text = "-";
                        textColor = statusNoTargetTextColor;
                        boxColor = statusNoTargetBoxColor;
                    }

                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", currentOffset + fullBarMidPosition, fullBarSize, boxColor));
                    sprites.Add(new MySprite(SpriteType.TEXT, text, currentOffset + fullBarMidWidth, null, textColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing + verticalGap;

                    sprites.Add(new MySprite(SpriteType.TEXT, "Range", currentOffset + halfBarMidWidth, null, statusTitleTextColor, "DEBUG", TextAlignment.CENTER, textSize));

                    sprites.Add(new MySprite(SpriteType.TEXT, "Hit Point", currentOffset + horizontalSpacing + halfBarMidWidth, null, statusTitleTextColor, "DEBUG", TextAlignment.CENTER, textSize));

                    currentOffset += verticalSpacing;

                    text = $"{Math.Round(manualTarget.MaxManualRaycastDistance * 0.001, 1):n1}km";

                    textColor = statusOptionsTextColor;
                    boxColor = statusOptionsBoxColor;

                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", currentOffset + halfBarMidPosition, halfBarSize, boxColor));
                    sprites.Add(new MySprite(SpriteType.TEXT, text, currentOffset + halfBarMidWidth, null, textColor, "DEBUG", TextAlignment.CENTER, textSize));

                    text = manualTarget.GetTargetingPointTypeName();
                    textColor = statusOptionsTextColor;
                    boxColor = statusOptionsBoxColor;

                    sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", currentOffset + horizontalSpacing + halfBarMidPosition, halfBarSize, boxColor));
                    sprites.Add(new MySprite(SpriteType.TEXT, text, currentOffset + horizontalSpacing + halfBarMidWidth, null, textColor, "DEBUG", TextAlignment.CENTER, textSize));

                    surface.ContentType = ContentType.SCRIPT;
                    surface.Script = "";
                    MySpriteDrawFrame frame = surface.DrawFrame();
                    frame.AddRange(sprites);
                    frame.Dispose();
                }
            }
        }

        void RefreshDisplay()
        {

            foreach (var d in myTextPanels)
            {
                IMyTextSurface myTextSurface = d as IMyTextSurface;
                RectangleF _viewport = new RectangleF((myTextSurface.TextureSize - myTextSurface.SurfaceSize) / 2f, myTextSurface.SurfaceSize);
                Sprite.PrepareTextSurfaceForSprites(d);
                Draw(d.DrawFrame(), d.CustomData, _viewport);
            }
        }

        void DisplayStatusInName()
        {
            if (settings.DisplayStatusInName == true)
            {
                if (switchedOn == true)
                {
                    Me.CustomName = settings.DDSEnabled;
                }
                else if (switchedOn == false)
                {
                    Me.CustomName = settings.DDSDisabled;
                }

            }
        }

        void DisplayStatus()
        {
            sb.Clear();
            if (switchedOn)
            {
                sb.AppendLine($"====[ Diamond Dome System ]===");
            }
            else
            {
                sb.AppendLine($"====[       DISABLED      ]===");
            }

            bool wc = settings.WCAPI != null;
            sb.AppendLine($"WeaponCoreAPI : {wc}\n");

            sb.AppendLine($"Tracked Targets : {targetManager.Count()}");

            sb.AppendLine($"PDCs : {pdcList.Count(b => { return !b.IsDamaged; })}");
            sb.AppendLine($"Designators : {designators.Count}");
            sb.AppendLine($"Raycast Cameras : {raycastCameras.Count}");
            sb.AppendLine($"Guided Missiles : {missileComputers.Count}");
            sb.AppendLine($"Guided Torpedos : {torpedoComputers.Count}");

            sb.AppendLine("\n---- Runtime Performance ---\n");
            profiler.PrintPerformance(sb);

            if (debugMode)
            {
                sb.AppendLine("\n>>>>>>> Debug Mode <<<<<<<");
                sb.AppendLine(debug.ToString());
                sb.AppendLine("\n---- Debug Performance ---\n");
                profiler.PrintSectionBreakdown(sb);
            }
            Echo(sb.ToString());
        }

        private List<IMyTextPanel> myTextPanels = new List<IMyTextPanel>();
    
        void Draw(MySpriteDrawFrame _frame, string customData, RectangleF viewport, bool smalltext = false)

        {
            bool wc = settings.WCAPI != null;
            string DisplayPDCs = $"PDCs : {pdcList.Count(b => { return !b.IsDamaged; })}\n";
            string DisplayDesignator = $"Designators : {designators.Count}\n";
            string DisplayCameras = $"Raycast Cameras : {raycastCameras.Count}\n";
            string DisplayMissiles = $"Guided Missiles : {missileComputers.Count}\n";
            string DisplayTorpedos = $"Guided Torpedos : {torpedoComputers.Count}\n";
            string DisplayTargers = $"Tracked Targets : {targetManager.Count()}\n";

            var cd = customData;
            var cds = cd.Split('\n');
            var TotalVars = cds.Length;

            for (int i = 0; i < TotalVars; i++)
            {
                string text = "";

                if (cds[i].ToLower().Contains("pdc"))
                {
                    text = DisplayPDCs;
                }
                else
                if (cds[i].ToLower().Contains("designator"))
                {
                    text = DisplayDesignator;
                }
                else
                if (cds[i].ToLower().Contains("cameras"))
                {
                    text = DisplayCameras;
                }
                else
                if (cds[i].ToLower().Contains("missiles"))
                {
                    text = DisplayMissiles;
                }
                else
                if (cds[i].ToLower().Contains("torpedos"))
                {
                    text = DisplayTorpedos;
                }
                else
                if (cds[i].ToLower().Contains("targets"))
                {
                    text = DisplayTargers;
                }
                else
                {
                    continue;
                }

                var spacer = (viewport.Height / TotalVars) * 0.05f;
                Vector2 size = new Vector2(viewport.Width * 0.9f, (viewport.Height / TotalVars) * 0.9f);

                float posY = ((viewport.Height / (TotalVars + 1)) * (i + 1)) - (size.Y / 2) - spacer;
                MySprite boundry = Sprite.DrawShape(Sprite.Shape.SquareHollow, new Vector2(viewport.Width * 0.05f, posY), size, 0f, Color.Black);
                _frame.Add(boundry);
                float x = viewport.Width * 1 * 0.9f;
                float y = viewport.Height / TotalVars * 0.9f;
                Vector2 size2 = new Vector2(x, y);
                _frame.Add(Sprite.DrawShape(Sprite.Shape.SquareTapered, (Vector2)boundry.Position, size2, 0f, Color.Black));
                _frame.Add(Sprite.DrawText(text, new Vector2(viewport.Center.X, boundry.Position.Value.Y - (size.Y / 3)), smalltext ? 0.8f : 2f, TextAlignment.CENTER, Color.NavajoWhite));
            }
            // We are done with the frame, send all the sprites to the text panel
            _frame.Dispose();
        }

        public static class Sprite
        {
            public enum Shape
            {
                CircleHollow,
                Circle,
                SemiCircle,
                Triangle,
                TriangleHollow,
                Square,
                SquareTapered,
                SquareHollow

            }
            public static void PrepareTextSurfaceForSprites(IMyTextSurface textSurface)
            {
                // Set the sprite display mode
                textSurface.ContentType = ContentType.SCRIPT;
                textSurface.ScriptBackgroundColor = new Color(0, 0, 0);
                // Make sure no built-in script has been selected
                textSurface.Script = "";
            }
            public static MySprite DrawText(string text, Vector2 position, float scale, TextAlignment alignment, Color color)
            {
                var sprite = new MySprite()
                {
                    Type = SpriteType.TEXT,
                    Data = text,
                    Position = position,
                    RotationOrScale = scale,
                    Color = color,
                    Alignment = alignment,
                    FontId = "Blue"
                };
                return sprite;
            }
            public static MySprite DrawShape(Shape shape, Vector2 position, Vector2 size, float rotation, Color color)
            {
                var sprite = new MySprite()
                {
                    Type = SpriteType.TEXTURE,
                    Data = shape.ToString(),
                    Position = position,
                    Size = size,
                    RotationOrScale = rotation,
                    Color = color
                };
                return sprite;
            }
        }



        #endregion

        #region Helper Methods

        long GenerateUniqueId()
        {
            rnd.NextBytes(genUniqueIdBuffer);
            Buffer.BlockCopy(genUniqueIdBuffer, 0, genUniqueIdResult, 0, 8);
            return genUniqueIdResult[0];
        }
        string GetTargetCode(long entityId)
        {
            return $"T{entityId % 100000:00000}"; 
        }
        string Vector3ToBase64(Vector3D vector)
        {
            return Convert.ToBase64String(BitConverter.GetBytes((float)vector.X)) + Convert.ToBase64String(BitConverter.GetBytes((float)vector.Y)) + Convert.ToBase64String(BitConverter.GetBytes((float)vector.Z));
        }
        bool IsWorking(IMyTerminalBlock block)
        { 
            return (block != null && block.IsWorking); 
        }
        bool IsValidTarget(ref MyDetectedEntityInfo entityInfo)
        {
            if (entityInfo.Type == MyDetectedEntityType.LargeGrid || entityInfo.Type == MyDetectedEntityType.SmallGrid) 
            {
                return IsHostileTarget(ref entityInfo);
            }
            return false;
        }
        bool IsHostileTarget(ref MyDetectedEntityInfo entityInfo)
        {
            return !(entityInfo.Relationship == MyRelationsBetweenPlayerAndBlock.Owner || entityInfo.Relationship == MyRelationsBetweenPlayerAndBlock.FactionShare)
            ;
        }
        WeaponProfile GetWeaponProfile(IMyUserControllableGun weapon)
        {
            if (weapon.BlockDefinition.SubtypeId.Contains("Gatling")) 
            {
                return weaponProfiles.gatlingProfile;
            }
            else if (weapon.BlockDefinition.SubtypeId.Contains("Missile") || weapon.BlockDefinition.SubtypeId.Contains("Rocket")) 
            {
                return weaponProfiles.rocketProfile;
            }
            return weaponProfiles.gatlingProfile;
        }
        bool FuncDesignatorHasTarget(Designator designator)
        {
            return designator.Turret.HasTarget;
        }
        bool FuncDesignatorIsWorking(Designator designator) 
        {
            return designator.Turret.IsWorking;
        }
        bool FuncPDCIsWorking(PDCTurret pdc)
        {
            return !pdc.IsDamaged;
        }
        bool NameContains(IMyTerminalBlock block, string tag)
        {
            return (block.CustomName.IndexOf(tag, StringComparison.OrdinalIgnoreCase) > -1);
        }
        bool NameContains(IMyBlockGroup group, string tag)
        {
            return (group.Name.IndexOf(tag,StringComparison.OrdinalIgnoreCase) > -1);
        }
        bool TokenContainsMatch(string[] tokens, string tag)
        {
            foreach (string token in tokens)
            {
                if (token.Trim().Equals(tag, StringComparison.OrdinalIgnoreCase))
                {
                    return true; 
                }
            }
            return false;
        }
        void GetBlocksFromGroups<Т>(out List<Т> result, string groupNameTag, Func<Т, bool> collect = null) where Т : class
        {
            result = null; 
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups, (b) => 
            {
                return b.Name.IndexOf(groupNameTag, StringComparison.OrdinalIgnoreCase) > -1;
            });
            foreach (IMyBlockGroup group in groups)
            {
                List<Т> blocks = new List<Т>();
                group.GetBlocksOfType(blocks, collect);
                if (result == null)
                { 
                    result = blocks; 
                }
                else
                { 
                    result.AddList(blocks);
                }
            }
        }

        #endregion

    }
}

