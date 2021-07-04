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
    public class PDCOcclusionGrid
    {
        public IMyCubeGrid Grid;
        public Vector3I StartPosition;
        public PDCOcclusionGrid(IMyCubeGrid grid, Vector3I startPosition)
        {
            Grid = grid; StartPosition = startPosition;
        }
    }

    public class ManualPDCTarget : PDCTarget
    {


        public string CodeId;

        public IMyTerminalBlock AimingBlock;
        public IMyLargeTurretBase AimingTurret;

        public IMyTextSurface DisplayStatus;
        public IMyTerminalBlock AlertBlock;

        public bool Enabled = false;
        public long SelectedEntityId = -1;

        public Vector3D? OffsetPoint;
        public TargetingPointTypeEnum TargetingPointType = TargetingPointTypeEnum.Center;

        public double MaxManualRaycastDistance;
        public ManualPDCTarget(string codeId, IMyTerminalBlock aimingBlock, IMyTextSurface displayStatus, IMyTerminalBlock alertBlock) : base(-1)
        {
            CodeId = codeId;
            AimingBlock = aimingBlock;
            AimingTurret = aimingBlock as IMyLargeTurretBase;
            DisplayStatus = displayStatus;
            AlertBlock = alertBlock;
        }
        public Vector3D GetForwardViewDirection()
        {
            if (AimingBlock == null)
            {
                return Vector3D.Zero;
            }
            if (AimingTurret != null)
            {
                Vector3D direction;
                Vector3D.CreateFromAzimuthAndElevation(AimingTurret.Azimuth, AimingTurret.Elevation, out direction);
                return Vector3D.TransformNormal(direction, AimingTurret.WorldMatrix);
            }
            else
            {
                return AimingBlock.WorldMatrix.Forward;
            }
        }
        public string GetTargetingPointTypeName()
        {
            switch (TargetingPointType)
            {
                case TargetingPointTypeEnum.Center: return "Center";
                case TargetingPointTypeEnum.Offset: return "Offset";
                case TargetingPointTypeEnum.Random: return "Random";
            }
            return "-";
        }
    }

    public class PDCTarget
    {
        public long EntityId;

        public Vector3D Position;
        public int DetectedClock;

        public Vector3D CenterPosition;
        public int RaycastDetectedClock;
        public int RaycastRefreshClock;

        public Vector3D Velocity;
        public Vector3D Acceleration;

        public Vector3D LastVelocity;
        public int LastDetectedClock;

        public MatrixD? Orientation;
        public int OrientationUpdatedClock;

        public bool IsLargeGrid;

        public double TargetSizeSq;
        public double PriorityValue;
        public double CheckTargetSizeSq;

        public double MaxAllowedRaycastDistance;
        public double MaxAllowedMissileLaunchDistance;

        public double PDCTargetedCount;

        public int MissileRemainingCount;
        public int MissileLaunchLastClock;



        public List<OffsetPoint> OffsetPoints;
        public PDCTarget(long entityId)
        { EntityId = entityId; }
    }

    public class PDCTargetCommsSorting : IComparer<PDCTarget>
    {
        public int Compare(PDCTarget x, PDCTarget y)
        {
            if (x == null) return (y == null ? 0 : 1);
            else if (y == null) return -1;
            else return (x.DetectedClock < y.DetectedClock ? -1 : (x.DetectedClock > y.DetectedClock ? 1 : (x.EntityId < y.EntityId ? -1 : (x.EntityId > y.EntityId ? 1 : 0))));
        }
    }

    public class PDCTurret
        {
            public GeneralSettings settings; 
            

            public MyIni TurretConfig;
        

            public string GroupName;

            public IMyMotorStator AzimuthRotor;
            public double AngleOffsetX;
            public double LowerLimitX; 
            public double UpperLimitX;
            public bool HaveLimitX;

            public double ActualAzimuth;

            public PredictiveController AzimuthRotorCtrl;

            public ElevationGroup[] ElevationRotors;

            public WeaponProfile WeaponStats; 

            public double TurretPrioritySize;

            public float RotorSpeedLimit; 

            public IMyShipController Controller;

            public ITerminalProperty<bool> WeaponShootProperty;
            public ITerminalAction WeaponShootAction;

            public IPDCOcclusionChecker GridClearanceCheck;

            public double TargetedCountGainPerHit;

            public PDCTarget TargetInfo;

            public bool IsDamaged;

            public IMyShipConnector ReloadBaseConnector;
            public IMyShipConnector ReloadTurretConnector;

            public int LastReloadConnectAttempt;

            public int LastReloadCheckClock;
            public int LastReloadOperationCheck;

            public ReloadState CurrentReloadState;
            public double ReloadMaxAngleRadians;
            public double ReloadLockStateAngleRadians;

            long previousAimEntityId;
            Vector3D previousAverageAimVector;
            
            const int OFFSET_POINTS_MOVE_ANGLE_PER_SECOND = Program.OFFSET_POINTS_MOVE_ANGLE_PER_SECOND;
            const int TICKS_PER_SECOND = Program.TICKS_PER_SECOND;
            const double INV_ONE_TICK = Program.INV_ONE_TICK;
            const string INI_SECTION = Program.INI_SECTION;
            const int TARGET_ORIENTATION_EXPIRE_TICKS = Program.TARGET_ORIENTATION_EXPIRE_TICKS;
            const double COS_45_DEGREES = Program.COS_45_DEGREES;

            double offsetMoveAngleTanPerSecond = Math.Tan(MathHelperD.ToRadians(OFFSET_POINTS_MOVE_ANGLE_PER_SECOND));
            
            long currentOffsetEntityId;
            int currentOffsetIndex;
            Vector3D currentOffsetStart;
            Vector3D currentOffsetEnd;
            double currentOffsetLastClock;
            double currentOffsetMoveFactorPerTick;
            double currentOffsetAmount; 
            

            public PDCTurret(string groupName, IMyMotorStator azimuthRotor, IMyMotorStator elevationRotor, IMyTerminalBlock aimBlock, IMyShipController controller, List<IMyUserControllableGun> weapons, WeaponProfile weaponStats, GeneralSettings settings, IPDCOcclusionChecker gridClearanceCheck =null) 
                : this(groupName, azimuthRotor, new List<IMyMotorStator>(new IMyMotorStator[] { elevationRotor }), new List<IMyTerminalBlock>(new IMyTerminalBlock[] { aimBlock }), controller, new List<List<IMyUserControllableGun>>() { weapons }, weaponStats, settings, gridClearanceCheck){ }
            public PDCTurret(string groupName, IMyMotorStator azimuthRotor, List<IMyMotorStator> elevationRotors, List<IMyTerminalBlock> aimBlocks, IMyShipController controller, List<List<IMyUserControllableGun>> weapons, WeaponProfile weaponStats, GeneralSettings settings, IPDCOcclusionChecker gridClearanceCheck = null)
            {
                this.settings = settings;

                GroupName = groupName;

                Controller = controller;
                WeaponStats = weaponStats;

                RotorSpeedLimit = MathHelper.RPMToRadiansPerSecond * (this.settings.RotorUseLimitSnap ? this.settings.RotorSnapSpeedLimit : this.settings.RotorCtrlOutputLimit);

                GridClearanceCheck = gridClearanceCheck; 

                TurretConfig = new MyIni(); 
                if (TurretConfig.TryParse(azimuthRotor.CustomData))
                {
                    if (TurretConfig.ContainsSection(INI_SECTION))
                    {
                        double weaponInitialSpeed = TurretConfig.Get(INI_SECTION,"WeaponInitialSpeed").ToDouble(0);
                        double weaponAcceleration = TurretConfig.Get(INI_SECTION, "WeaponAcceleration").ToDouble(0);
                        double weaponMaxSpeed = TurretConfig.Get(INI_SECTION, "WeaponMaxSpeed").ToDouble(0);
                        double weaponMaxRange = TurretConfig.Get(INI_SECTION, "WeaponMaxRange").ToDouble(0); 
                        double weaponSpawnOffset = TurretConfig.Get(INI_SECTION, "WeaponSpawnOffset").ToDouble(0); 
                        double weaponReloadTime = TurretConfig.Get(INI_SECTION,"WeaponReloadTime").ToDouble(0);
                        bool weaponIsCappedSpeed = TurretConfig.Get(INI_SECTION, "WeaponIsCappedSpeed").ToBoolean(false); 
                        bool weaponUseSalvo = TurretConfig.Get(INI_SECTION, "WeaponUseSalvo").ToBoolean(false); 

                        TurretPrioritySize = TurretConfig.Get(INI_SECTION, "TurretPrioritySize").ToDouble(0); 

                        if (weaponMaxRange > 0 && (weaponInitialSpeed > 0 || weaponAcceleration > 0))
                        {
                            weaponInitialSpeed = MathHelper.Clamp(weaponInitialSpeed, 0, 1000000000);
                            weaponAcceleration = MathHelper.Clamp(weaponAcceleration, 0, 1000000000); 
                            weaponMaxSpeed = MathHelper.Clamp(weaponMaxSpeed, 0, 1000000000);
                            if (weaponMaxSpeed == 0) weaponMaxSpeed = 1000000000; 
                            weaponMaxRange = MathHelper.Clamp(weaponMaxRange, 0, 1000000000);
                            WeaponStats = new WeaponProfile(weaponInitialSpeed, weaponAcceleration, weaponMaxSpeed, weaponMaxRange, weaponSpawnOffset, weaponReloadTime, weaponIsCappedSpeed, weaponUseSalvo);
                        }
                    }
                }
                else
                { 
                    TurretPrioritySize = 0; 
                }

                if (weapons.Count > 0 && weapons[0].Count > 0)
                {
                    WeaponShootProperty = weapons[0][0].GetProperty("Shoot").As<bool>();
                    WeaponShootAction = weapons[0][0].GetActionWithName("ShootOnce");
                }

                AzimuthRotor = azimuthRotor;
                HaveLimitX = GetOrSetRotorLimitConfig(AzimuthRotor, out LowerLimitX, out UpperLimitX);

                if (!this.settings.RotorUseLimitSnap)
                {
                    AzimuthRotorCtrl = new PredictiveController(this.settings.RotorCtrlOutputGain, this.settings.RotorCtrlDeltaGain, RotorSpeedLimit);
                    SetRotorLimits(AzimuthRotor, LowerLimitX, UpperLimitX);
                }

                ElevationRotors = new ElevationGroup[elevationRotors.Count];
                for (int i = 0; i < elevationRotors.Count; i++)
                {
                    ElevationGroup elevationRotor = new ElevationGroup();
                    ElevationRotors[i] = elevationRotor;

                    elevationRotor.Rotor = elevationRotors[i];
                    elevationRotor.AimBlock = aimBlocks[i];

                    double lowerLimitY, upperLimitY;
                    GetOrSetRotorLimitConfig(elevationRotor.Rotor, out lowerLimitY, out upperLimitY);
                    elevationRotor.HaveLimitY = (lowerLimitY > double.MinValue && upperLimitY < double.MaxValue);
                    elevationRotor.LowerLimitY = lowerLimitY;
                    elevationRotor.UpperLimitY = upperLimitY;

                    SetElevationOffsetAndReverse(elevationRotor);

                    if (!this.settings.RotorUseLimitSnap)
                    {
                        elevationRotor.RotorCtrl = new PredictiveController(this.settings.RotorCtrlOutputGain, this.settings.RotorCtrlDeltaGain, RotorSpeedLimit);
                        SetRotorLimits(elevationRotor.Rotor, lowerLimitY, upperLimitY);
                    }

                    elevationRotor.Weapons = weapons[i];

                    if (WeaponStats.UseSalvo)
                    {
                        elevationRotor.WeaponStaggerInterval = (int)Math.Ceiling((WeaponStats.ReloadTime * TICKS_PER_SECOND) / Math.Max(elevationRotor.Weapons.Count, 1));
                    }
                
                    if (elevationRotor.Weapons != null && elevationRotor.Weapons.Count > 0)
                    {
                        elevationRotor.Ⱦ = elevationRotor.Weapons[0].CubeGrid; 
                        if (this.settings.UseFourPointOcclusion)
                        {
                            elevationRotor.Ƚ = new Vector3I[4]; 
                            Vector3I î = new Vector3I(); 
                            Vector3I í = new Vector3I();
                            foreach (IMyUserControllableGun ê in elevationRotor.Weapons)
                            {
                                î = Vector3I.Min(î, ê.Position); í = Vector3I.Max(í, ê.Position);
                            }
                            Vector3I j = Base6Directions.GetIntVector(elevationRotor.Weapons[0].Orientation.Forward);
                            if (j.X != 0)
                            {
                                elevationRotor.Ƚ[0] = new Vector3I(0, î.Y, î.Z); 
                                elevationRotor.Ƚ[1] = new Vector3I(0, î.Y, í.Z); 
                                elevationRotor.Ƚ[2] = new Vector3I(0, í.Y, î.Z); 
                                elevationRotor.Ƚ[3] = new Vector3I(0, í.Y, í.Z);
                            }
                            else if (j.Y != 0)
                            {
                                elevationRotor.Ƚ[0] = new Vector3I(î.X, 0, î.Z);
                                elevationRotor.Ƚ[1] = new Vector3I(î.X, 0, í.Z);
                                elevationRotor.Ƚ[2] = new Vector3I(í.X, 0, î.Z); 
                                elevationRotor.Ƚ[3] = new Vector3I(í.X, 0, í.Z);
                            }
                            else
                            {
                                elevationRotor.Ƚ[0] = new Vector3I(î.X, î.Y, 0);
                                elevationRotor.Ƚ[1] = new Vector3I(î.X, í.Y, 0);
                                elevationRotor.Ƚ[2] = new Vector3I(í.X, î.Y, 0);
                                elevationRotor.Ƚ[3] = new Vector3I(í.X, í.Y, 0);
                            }
                        }
                        else
                        {
                            Vector3I ë = new Vector3I(); 
                            foreach (IMyUserControllableGun ê in elevationRotor.Weapons)
                            { 
                                ë += ê.Position; 
                            }
                            ë = new Vector3I((int)((float)ë.X / elevationRotor.Weapons.Count), (int)((float)ë.Y / elevationRotor.Weapons.Count), (int)((float)ë.Z / elevationRotor.Weapons.Count)); elevationRotor.Ƚ = new Vector3I[] { ë };
                        }
                    }
                }
                if (ElevationRotors.Length > 0) { SetAzimuthOffsetAndReverse(AzimuthRotor, ElevationRotors[0], ref AngleOffsetX); }
            }
            public void TransferTarget(PDCTurret prevPDC)
            { 
                TargetInfo = prevPDC.TargetInfo;

                previousAimEntityId = prevPDC.previousAimEntityId;
                previousAverageAimVector = prevPDC.previousAverageAimVector;

                currentOffsetEntityId = prevPDC.currentOffsetEntityId;
                currentOffsetIndex = prevPDC.currentOffsetIndex;
                currentOffsetStart = prevPDC.currentOffsetStart;
                currentOffsetEnd = prevPDC.currentOffsetEnd;
                currentOffsetLastClock = prevPDC.currentOffsetLastClock;
                currentOffsetMoveFactorPerTick = prevPDC.currentOffsetMoveFactorPerTick;
                currentOffsetAmount = prevPDC.currentOffsetAmount;
            }
            public void SetAzimuthOffsetAndReverse(IMyMotorStator azimuthRotor, ElevationGroup pilotElevationRotor, ref double angleOffsetX)
            {
                IMyMotorStator elevationRotor = pilotElevationRotor.Rotor;

                double cosAngle = Math.Cos(elevationRotor.Angle);
                double sinAngle = Math.Sin(elevationRotor.Angle);
                Vector3D elevGridBackward = (elevationRotor.WorldMatrix.Backward * cosAngle) + (elevationRotor.WorldMatrix.Left * sinAngle);
                Vector3D elevGridLeft = (elevationRotor.WorldMatrix.Left * cosAngle) + (elevationRotor.WorldMatrix.Forward * sinAngle);

                double elevationZero = pilotElevationRotor.AngleOffsetY + ComputeOffsetFromVector(pilotElevationRotor.AimBlock.WorldMatrix.Forward, elevGridBackward, elevGridLeft);
                if (elevationZero >=MathHelperD.TwoPi) elevationZero -= MathHelperD.TwoPi;

                cosAngle = Math.Cos(elevationZero);
                sinAngle = Math.Sin(elevationZero);
                Vector3D elevationAimForward = (pilotElevationRotor.Rotor.WorldMatrix.Backward * cosAngle) + (pilotElevationRotor.Rotor.WorldMatrix.Left * sinAngle);

                cosAngle = Math.Cos(azimuthRotor.Angle);
                sinAngle = Math.Sin(azimuthRotor.Angle); 
                Vector3D azimuthGridBackward = (azimuthRotor.WorldMatrix.Backward * cosAngle) + (azimuthRotor.WorldMatrix.Left * sinAngle);
                Vector3D azimuthGridLeft = (azimuthRotor.WorldMatrix.Left * cosAngle) + (azimuthRotor.WorldMatrix.Forward * sinAngle);

                angleOffsetX = -ComputeOffsetFromVector(elevationAimForward, azimuthGridBackward, azimuthGridLeft);
                angleOffsetX = (Math.Round(angleOffsetX / MathHelper.PiOver2) % 4) * MathHelper.PiOver2;
            }
            public void SetElevationOffsetAndReverse(ElevationGroup elevationRotor)
            {
                IMyMotorStator rotor = elevationRotor.Rotor;
                
                double cosAngle = Math.Cos(rotor.Angle);
                double sinAngle = Math.Sin(rotor.Angle);
                Vector3D elevGridBackward = (rotor.WorldMatrix.Backward * cosAngle) + (rotor.WorldMatrix.Left * sinAngle);
                Vector3D elevGridLeft = (rotor.WorldMatrix.Left * cosAngle) + (rotor.WorldMatrix.Forward * sinAngle);
                
                elevationRotor.AngleOffsetY = ComputeOffsetFromVector(elevationRotor.AimBlock.WorldMatrix.Forward, elevGridBackward, elevGridLeft); 
               
                double lowerLimit = rotor.LowerLimitRad;
                double upperLimit = rotor.UpperLimitRad; 
                
                double midAngle;
                if (rotor.LowerLimitDeg == float.MinValue || rotor.UpperLimitDeg == float.MaxValue) 
                {
                    midAngle = rotor.Angle + elevationRotor.AngleOffsetY; 
                }
                else 
                { 
                    midAngle = ((upperLimit + lowerLimit) * 0.5) + elevationRotor.AngleOffsetY; 
                }

                Limit2PILite(ref midAngle);

                cosAngle = Math.Cos(midAngle);
                sinAngle = Math.Sin(midAngle);
                Vector3D adjustedVector = (rotor.WorldMatrix.Backward * cosAngle) + (rotor.WorldMatrix.Left * sinAngle);

                Vector3D crossForward = rotor.WorldMatrix.Up.Cross(AzimuthRotor.WorldMatrix.Up); 
                elevationRotor.ReverseY = (crossForward.Dot(adjustedVector) < 0);

                double rotorToForwardDeviation = ComputeOffsetFromVector(rotor.WorldMatrix.Backward, elevationRotor.ReverseY ? -crossForward : crossForward, AzimuthRotor.WorldMatrix.Up);
                if (elevationRotor.ReverseY) rotorToForwardDeviation = MathHelper.TwoPi - rotorToForwardDeviation;
                elevationRotor.AngleOffsetY = -rotorToForwardDeviation - elevationRotor.AngleOffsetY;
                elevationRotor.AngleOffsetY = (Math.Round(elevationRotor.AngleOffsetY / MathHelper.PiOver2) % 4) * MathHelper.PiOver2;
            }
            public double ComputeOffsetFromVector(Vector3D checkVector, Vector3D baseRef0DegVector, Vector3D baseRef90DegVector)
            {
                double offsetDotCheck = Math.Round(baseRef0DegVector.Dot(checkVector));
                if (offsetDotCheck == 0)
                {
                    if (baseRef90DegVector.Dot(checkVector) > 0) return MathHelperD.PiOver2;
                    else return MathHelperD.PiOver2 + MathHelperD.Pi;
                }
                else
                {
                    if (offsetDotCheck > 0) return 0;
                    else return MathHelperD.Pi; 
                }
            }

            public void ResetRotors()
            {
                AzimuthRotor.TargetVelocityRad = 0f;
                for (int i = 0; i < ElevationRotors.Length; i++) 
                {
                    ElevationRotors[i].Rotor.TargetVelocityRad = 0f; 
                }
            }

            public bool IsAzimuthRotorIntact()
            { 
                return (AzimuthRotor.IsWorking && AzimuthRotor.IsAttached);
            }

            public bool IsElevationRotorIntact(ElevationGroup elevationRotor)
            { 
                return (elevationRotor.Rotor.IsWorking && elevationRotor.Rotor.IsAttached && elevationRotor.AimBlock.IsFunctional); 
            }

            public bool IsElevationRotorIntact()
            {
                foreach (ElevationGroup elevationRotor in ElevationRotors)
                {
                    if (elevationRotor.Rotor.IsWorking && elevationRotor.Rotor.IsAttached && elevationRotor.AimBlock.IsFunctional)
                    {
                        return true; 
                    }
                }
                return false;
            }

            public ElevationGroup GetPilotElevationRotor()
            {
                foreach (ElevationGroup elevationRotor in ElevationRotors)
                {
                    if (elevationRotor.Rotor.IsWorking && elevationRotor.Rotor.IsAttached && elevationRotor.AimBlock.IsFunctional) 
                    { 
                        return elevationRotor;
                    }
                }
                return ElevationRotors[0];
            }

            public bool ComputeTargetVectorAndDistance(PDCTarget target, int currentClock, out Vector3D targetVector, out double targetDistance)
            {
                Vector3D shipPosition = ElevationRotors[0].AimBlock.WorldMatrix.Translation;

                Vector3D selectedPosition;
                if (target.OffsetPoints != null)
                {
                    selectedPosition = SelectOffsetPosition(target, currentClock);
                }
                else
                {
                    selectedPosition = target.Position;
                }
                Vector3D targetPosition = (target.DetectedClock == currentClock ? selectedPosition : selectedPosition + ((currentClock - target.DetectedClock) * INV_ONE_TICK * target.Velocity));
                Vector3D shipDirection = (Controller == null ? Vector3D.Zero : Controller.GetShipVelocities().LinearVelocity);

                if (WeaponStats.SpawnOffset != 0)
                {
                    shipPosition += ElevationRotors[0].AimBlock.WorldMatrix.Forward * WeaponStats.SpawnOffset;
                }

                targetPosition = WeaponStats.ComputeInterceptPoint(ref targetPosition, ref target.Velocity, ref shipPosition, ref shipDirection, target);

                if (double.IsNaN(targetPosition.Sum))
                {
                    targetVector = Vector3D.Zero;
                    targetDistance = 0;
                    return false;
                }
                else
                {
                    targetVector = targetPosition - shipPosition;
                    targetDistance = targetVector.Length();
                    targetVector = (targetDistance == 0 ? Vector3D.Zero : targetVector / targetDistance);
                    return true;
                }
            }

            public Vector3D SelectOffsetPosition(PDCTarget target, int currentClock)
            {
                if (target.OffsetPoints?.Count == 0 || currentClock > target.OrientationUpdatedClock + TARGET_ORIENTATION_EXPIRE_TICKS || target.Orientation == null)
                {
                    return target.Position;
                }

                bool resetMoveAmount = false;

                if (currentOffsetEntityId != target.EntityId)
                {
                    currentOffsetEntityId = target.EntityId;
                    currentOffsetIndex = 0;

                    currentOffsetStart = Vector3D.TransformNormal(target.Position - target.CenterPosition, MatrixD.Transpose(target.Orientation.Value));
                    currentOffsetEnd = target.OffsetPoints[0].Point;

                    resetMoveAmount = true;
                }
                else if (currentOffsetAmount > 1)
                {
                    currentOffsetIndex++;
                    if (currentOffsetIndex >= target.OffsetPoints.Count)
                    {
                        currentOffsetIndex = 0;

                    }

                    currentOffsetStart = currentOffsetEnd;
                    currentOffsetEnd = target.OffsetPoints[currentOffsetIndex].Point;

                    resetMoveAmount = true;
                }

                if (resetMoveAmount)
                {
                    currentOffsetLastClock = currentClock;

                    double offsetApartLength = (currentOffsetStart - currentOffsetEnd).Length();

                    if (offsetApartLength < 1)
                    {
                        currentOffsetMoveFactorPerTick = INV_ONE_TICK;
                    }
                    else
                    {
                        currentOffsetMoveFactorPerTick = INV_ONE_TICK / offsetApartLength * (offsetMoveAngleTanPerSecond * (target.CenterPosition - AzimuthRotor.WorldMatrix.Translation).Length());
                    }
                    currentOffsetAmount = 0;
                }

                currentOffsetAmount += (currentClock - currentOffsetLastClock) * currentOffsetMoveFactorPerTick;
                currentOffsetLastClock = currentClock;
                return target.CenterPosition + Vector3D.TransformNormal(Vector3D.Lerp(currentOffsetStart, currentOffsetEnd, currentOffsetAmount), target.Orientation.Value);
            }

            private bool CheckAndSetAllRotors(double aimAzimuth, double aimElevation, int currentClock, bool testOnly = false)
            {
                //                settings.Context.debug.AppendLine("CheckAndSetAllRotors aA="+aimAzimuth+" aE="+aimElevation+" AA="+ActualAzimuth+" LLX="+LowerLimitX+" ULX="+UpperLimitX);
                aimAzimuth += AngleOffsetX;
                if (aimAzimuth >= MathHelperD.TwoPi) aimAzimuth -= MathHelperD.TwoPi;

                double yaw;
                if (HaveLimitX)
                {
                    if (!GetClampedCorrectionValue(out yaw, aimAzimuth, ActualAzimuth, LowerLimitX, UpperLimitX))
                    {
                        //                        settings.Context.debug.AppendLine(" >>1 GetClampedCorrectionValue = false");
                        return false;
                    }
                }
                else
                {
                    yaw = aimAzimuth - ActualAzimuth;

                    if (yaw > Math.PI) yaw -= MathHelperD.TwoPi;
                    else if (yaw < -Math.PI) yaw += MathHelperD.TwoPi;
                }
                if (!testOnly)
                {
                    if (AzimuthRotorCtrl == null)
                    {
                        AzimuthRotor.TargetVelocityRad = Math.Min(RotorSpeedLimit, Math.Max(-RotorSpeedLimit, (float)(yaw * settings.RotorSnapVelocityGain)));
                        SnapRotorLimitsToAngle(AzimuthRotor, yaw);
                    }
                    else
                    {
                        AzimuthRotor.TargetVelocityRad = (float)AzimuthRotorCtrl.Filter(ActualAzimuth, aimAzimuth, currentClock);
                    }
                }
                for (int i = 0; i < ElevationRotors.Length; i++)
                {
                    ElevationGroup ElevationRotor = ElevationRotors[i];

                    if (!IsElevationRotorIntact(ElevationRotor)) continue;
                    
                    double curElevation;
                    curElevation = ElevationRotor.CurrentElevation;

                    double aimElevLocal = aimElevation;

                    if (ElevationRotor.ReverseY)
                    {
                        curElevation = ElevationRotor.AngleOffsetY - curElevation;
                        if (curElevation < -MathHelperD.TwoPi) curElevation += MathHelperD.TwoPi;
                        
                        aimElevLocal = ElevationRotor.AngleOffsetY - aimElevLocal;
                        if (aimElevLocal < -MathHelperD.TwoPi) aimElevLocal += MathHelperD.TwoPi;
                    }
                    else
                    {
                        curElevation += ElevationRotor.AngleOffsetY;
                        if (curElevation >= MathHelperD.TwoPi) curElevation -= MathHelperD.TwoPi;
                        
                        aimElevLocal += ElevationRotor.AngleOffsetY;
                        if (aimElevLocal >=MathHelperD.TwoPi) aimElevLocal -= MathHelperD.TwoPi;
                    }

                    double pitch;
                    if (ElevationRotor.HaveLimitY)
                    {
                        if (!GetClampedCorrectionValue(out pitch, aimElevLocal, curElevation, ElevationRotor.LowerLimitY, ElevationRotor.UpperLimitY))
                        {
                            //                            settings.Context.debug.AppendLine(" >>2 GetClampedCorrectionValue = false");
                            return false;
                        }
                    }
                    else
                    {
                        pitch = aimElevLocal - curElevation;

                        if (pitch > Math.PI) pitch -= MathHelperD.TwoPi;
                        else if (pitch < -Math.PI) pitch += MathHelperD.TwoPi;
                    }

                    if (!testOnly)
                    {
                        if (ElevationRotor.RotorCtrl == null)
                        {
                            ElevationRotor.Rotor.TargetVelocityRad = Math.Min(RotorSpeedLimit, Math.Max(-RotorSpeedLimit, (float)(pitch * settings.RotorSnapVelocityGain)));
                            SnapRotorLimitsToAngle(ElevationRotor.Rotor, pitch);
                        }
                        else 
                        { 
                            ElevationRotor.Rotor.TargetVelocityRad = (float)ElevationRotor.RotorCtrl.Filter(curElevation, aimElevLocal, currentClock); 
                        }
                    }
                    else 
                    { 
                        break; 
                    }
                }
                return true;
            }

            public bool CheckReloadRequired()
            {
                long startTiming = DateTime.Now.Ticks; 
                if (ReloadBaseConnector == null || ReloadTurretConnector == null)
                {
                    return false; 
                }

                if ((ReloadBaseConnector.GetInventory()?.IsItemAt(0) ?? false) && (!ReloadTurretConnector.GetInventory()?.IsItemAt(0) ?? false)) 
                {
                    return true;
                }
                return false;
            }

            public bool PerformReloadProcedure(int currentClock)
            {
                if (ReloadBaseConnector == null || ReloadTurretConnector == null) 
                {
                    return true; 
                }

                if (IsDamaged)
                { 
                    CurrentReloadState = ReloadState.NONE;
                    return true; 
                }
                else if (!IsAzimuthRotorIntact() || !IsElevationRotorIntact()) 
                {
                    CurrentReloadState = ReloadState.NONE;
                    IsDamaged = true;
                    return true;
                }

                if (CurrentReloadState == ReloadState.NONE) 
                {
                    ReleaseWeapons(true);
                    CurrentReloadState = ReloadState.MAX_ANGLE;
                }

                ElevationGroup pilotElevationRotor = GetPilotElevationRotor();

                double aimElevation;

                switch (CurrentReloadState)
                {
                    case ReloadState.MAX_ANGLE:
                        UpdateRotorBearings(ElevationRotors, pilotElevationRotor, AzimuthRotor);
                        aimElevation = ReloadMaxAngleRadians;
                        if (Math.Abs(aimElevation - pilotElevationRotor.CurrentElevation) > 0.0018) 
                        {
                            CheckAndSetAllRotors(ActualAzimuth, aimElevation, currentClock, false); 
                        }
                        else
                        {
                            CurrentReloadState = ReloadState.LOCK_STATE_ANGLE; 
                        }

                        break;
                    case ReloadState.LOCK_STATE_ANGLE:
                        UpdateRotorBearings(ElevationRotors, pilotElevationRotor, AzimuthRotor);
                        aimElevation = ReloadLockStateAngleRadians;

                        if (Math.Abs(aimElevation - pilotElevationRotor.CurrentElevation) > 0.0018) 
                        { 
                            CheckAndSetAllRotors(ActualAzimuth, aimElevation, currentClock, false);
                        }
                        else 
                        { 
                            CurrentReloadState = ReloadState.LOCK_CONNECTORS;
                        }

                        break;
                    case ReloadState.LOCK_CONNECTORS:
                        if (ReloadTurretConnector.Status == MyShipConnectorStatus.Connected)
                        {
                            CurrentReloadState = ReloadState.TRANSFER_ITEMS;
                        }
                        else if (ReloadTurretConnector.Status == MyShipConnectorStatus.Connectable)
                        { 
                            if (currentClock >= LastReloadConnectAttempt + 30)
                            { 
                                ReloadTurretConnector.Connect();
                                LastReloadConnectAttempt = currentClock;
                            } 
                        }

                        break;
                    case ReloadState.TRANSFER_ITEMS:
                        if (ReloadBaseConnector.GetInventory()?.IsItemAt(0) ?? false)
                        {
                            ReloadTurretConnector.GetInventory()?.TransferItemFrom(ReloadBaseConnector.GetInventory(), 0, 0, true);
                        }
                        ReloadTurretConnector.Disconnect();
                        ReloadBaseConnector.Disconnect();
                        CurrentReloadState = ReloadState.NONE;
                        return true;
                }
                return false;
            }

            public bool IsTargetable(PDCTarget target, int currentClock)
            {
                //                settings.Context.debug.AppendLine("IsTargetable");
                if (IsDamaged)
                { 
                    return false; 
                }
                else if (!IsAzimuthRotorIntact() || !IsElevationRotorIntact())
                {
                    IsDamaged = true;
                    return false;
                }

                //                  settings.Context.debug.AppendLine("IsTargetable Not Damaged");

                if (CurrentReloadState != ReloadState.NONE)
                { 
                    return false;
                }

                //                settings.Context.debug.AppendLine("IsTargetable Has Ammo");
                Vector3D targetVector;
                double targetDistance;
                bool interceptable = ComputeTargetVectorAndDistance(target, currentClock, out targetVector, out targetDistance);
                //                settings.Context.debug.AppendLine("IsTargetable interceptable" + interceptable);

                if (interceptable)
                {
                    ElevationGroup pilotElevationRotor = GetPilotElevationRotor();

                    if (HaveLimitX || pilotElevationRotor.HaveLimitY)
                    {
                        double aimAzimuth, aimElevation;
                        UpdateRotorBearings(ElevationRotors, pilotElevationRotor, AzimuthRotor);
                        GetAzimuthElevation(targetVector, AzimuthRotor, out aimAzimuth, out aimElevation);

                        if (!CheckAndSetAllRotors(aimAzimuth, aimElevation, currentClock, true))
                        {
                            //                            settings.Context.debug.AppendLine("IsTargetable rotor failure");
                            return false;
                        }
                    }
                    if (GridClearanceCheck != null && GridClearanceCheck.TestWorldRayHit(pilotElevationRotor.AimBlock.WorldMatrix.Translation, targetVector))
                    {
                        //                        settings.Context.debug.AppendLine("IsTargetable clearance failure");
                        return false;
                    }
                    if (targetDistance <= WeaponStats.MaxRange)
                    {
                        //                        settings.Context.debug.AppendLine("IsTargetable success");
                        return true;
                    }
                }
                //                settings.Context.debug.AppendLine("IsTargetable range failure");
                return false;
            }
            public bool AimAndFire(PDCTarget target, int currentClock)
            {
                if (IsDamaged)
                {
                    ReleaseWeapons(true);
                    ResetRotors();
                    return false;
                }
                else if (!IsAzimuthRotorIntact() || !IsElevationRotorIntact()) 
                {
                    ReleaseWeapons(true);
                    ResetRotors();
                    IsDamaged = true;
                    return false;
                }

                if (CurrentReloadState != ReloadState.NONE) 
                {
                    return false; 
                }

                Vector3D targetVector;
                double targetDistance;
                bool interceptable = ComputeTargetVectorAndDistance(target, currentClock, out targetVector, out targetDistance);

                if (interceptable)
                {
                    ElevationGroup pilotElevationRotor = GetPilotElevationRotor();

                    double aimAzimuth, aimElevation;
                    UpdateRotorBearings(ElevationRotors, pilotElevationRotor, AzimuthRotor);
                    GetAzimuthElevation(targetVector, AzimuthRotor, out aimAzimuth, out aimElevation);

                    if (CheckAndSetAllRotors(aimAzimuth, aimElevation, currentClock, false))
                    {
                        if (targetDistance <= WeaponStats.MaxRange)
                        {
                            bool haveAimedWeapons = false;

                            foreach (ElevationGroup elevationRotor in ElevationRotors) 
                            {
                                //changes
                                if (GridClearanceCheck != null && elevationRotor.Ⱦ != null && elevationRotor.Ƚ != null)
                                {
                                    bool ɒ = false; for (int Q = 0; Q < elevationRotor.Ƚ.Length; Q++)
                                    {
                                        if (GridClearanceCheck.TestWorldRayHit(ɉ(elevationRotor.Ⱦ, ref elevationRotor.Ƚ[Q]), targetVector))
                                        {
                                            ɒ =true; break;
                                        }
                                    }
                                    if (ɒ) 
                                    { 
                                        ReleaseWeapons(elevationRotor);
                                        continue;
                                    }
                                }
                                double dotAngle = elevationRotor.AimBlock.WorldMatrix.Forward.Dot(targetVector);
                                bool withinRange = (dotAngle >= settings.PDCFireDotLimit);
                                if (!withinRange && dotAngle >= COS_45_DEGREES && previousAimEntityId == target.EntityId)
                                {
                                    double dotAngle2 = elevationRotor.AimBlock.WorldMatrix.Forward.Dot(previousAverageAimVector);
                                    double dotAngle3 = targetVector.Dot(previousAverageAimVector);
                                    withinRange = (dotAngle2 >= settings.PDCFireDotLimit) || (dotAngle >= dotAngle3 && dotAngle2 >= dotAngle3);
                                }
                                if (withinRange) 
                                { 
                                    FireWeapons(elevationRotor, currentClock, WeaponStats.UseSalvo);
                                    haveAimedWeapons = true;
                                } 
                                else 
                                { 
                                    ReleaseWeapons(elevationRotor);
                                }
                            }
                            if (haveAimedWeapons)
                            {
                                target.PDCTargetedCount += TargetedCountGainPerHit
;
                            }
                            previousAimEntityId = target.EntityId;
                            previousAverageAimVector = targetVector; 
                            return true;
                        }
                    }
                }
                ReleaseWeapons();
                ResetRotors(); 
                return false;
            }
            public void UpdateRotorBearings(ElevationGroup[] elevationRotors, ElevationGroup pilotElevationRotor, IMyMotorStator azRotor)
            {
                ActualAzimuth = azRotor.Angle;
                Limit2PILite(ref ActualAzimuth);
                foreach (ElevationGroup elevationRotor in elevationRotors)
                {
                    if (elevationRotor == pilotElevationRotor || IsElevationRotorIntact(elevationRotor))
                    {
                        double dot = elevationRotor.AimBlock.WorldMatrix.Forward.Dot(azRotor.WorldMatrix.Up);
                        elevationRotor.CurrentElevation = MathHelperD.PiOver2 - Math.Acos(MathHelper.Clamp(dot, -1, 1));
                    }
                }
            }
            public void GetAzimuthElevation(Vector3D aimVector, IMyMotorStator azRotor, out double azimuth, out double elevation)
            {
                double dot = aimVector.Dot(azRotor.WorldMatrix.Up);
                elevation = MathHelperD.PiOver2 - Math.Acos(MathHelper.Clamp(dot, -1, 1));
                
                double cosAngle = Math.Cos(elevation);
                Vector3D azForward = (cosAngle == 0 ? Vector3D.Zero : (aimVector - (azRotor.WorldMatrix.Up * dot)) / cosAngle);
                azimuth = Math.Acos(MathHelper.Clamp(azRotor.WorldMatrix.Backward.Dot(azForward), -1, 1)); 
                if (azRotor.WorldMatrix.Right.Dot(azForward) > 0) azimuth = MathHelperD.TwoPi - azimuth;
            }

            public bool GetClampedCorrectionValue(out double correction, double desiredAngle, double currentAngle, double lowerLimit, double upperLimit)
            {
                upperLimit -= lowerLimit;
                desiredAngle -= lowerLimit;
                currentAngle -= lowerLimit;

                Limit2PILite(ref upperLimit);
                Limit2PILite(ref desiredAngle);
                Limit2PILite(ref currentAngle);

                if (desiredAngle >= 0 && desiredAngle <= upperLimit) 
                {
                    if (currentAngle > upperLimit)
                    {
                        currentAngle = (currentAngle - upperLimit <= MathHelperD.TwoPi - currentAngle ? upperLimit : 0);
                    } 

                    correction = desiredAngle - currentAngle;
                    return true;
                }
                else 
                { 
                    correction = 0;
                    return false;
                }
            }

            public void SnapRotorLimitsToAngle(IMyMotorStator rotor,double diffAngle)
            {
                float desiredAngle = rotor.Angle + (float)diffAngle;
                if (desiredAngle < -MathHelper.TwoPi || desiredAngle > MathHelper.TwoPi)
                {
                    rotor.SetValueFloat("LowerLimit", float.MinValue);
                    rotor.SetValueFloat("UpperLimit", float.MaxValue);
                }
                else if (diffAngle < 0) 
                {
                    rotor.UpperLimitRad = rotor.Angle;
                    rotor.LowerLimitRad = desiredAngle; 
                }
                else if (diffAngle > 0)
                {
                    rotor.LowerLimitRad = rotor.Angle;
                    rotor.UpperLimitRad = desiredAngle;
                }
            }

            public bool GetOrSetRotorLimitConfig(IMyMotorStator rotor, out double lowerLimit, out double upperLimit)
            {
                double lowerLimitDeg = double.MinValue;
                double upperLimitDeg = double.MaxValue;

                if (settings.RotorUseLimitSnap)
                {
                    MyIni cfg = new MyIni();
                    cfg.TryParse(rotor.CustomData);
                    lowerLimitDeg = cfg.Get(INI_SECTION, "LowerLimit").ToDouble(lowerLimitDeg);
                    upperLimitDeg = cfg.Get(INI_SECTION, "UpperLimit").ToDouble(upperLimitDeg);
                    if (lowerLimitDeg == double.MinValue || upperLimitDeg == double.MaxValue)
                    {
                        lowerLimitDeg = MathHelper.Clamp(Math.Round(rotor.LowerLimitDeg, 3), -361, 361);
                        upperLimitDeg = MathHelper.Clamp(Math.Round(rotor.UpperLimitDeg, 3), -361, 361);
                        cfg.Set(INI_SECTION, "LowerLimit", lowerLimitDeg);
                        cfg.Set(INI_SECTION, "UpperLimit", upperLimitDeg);
                        rotor.CustomData = cfg.ToString();
                    }
                }
                else
                {
                    lowerLimitDeg = MathHelper.Clamp(Math.Round(rotor.LowerLimitDeg, 3), -361, 361);
                    upperLimitDeg = MathHelper.Clamp(Math.Round(rotor.UpperLimitDeg, 3), -361, 361);
                }
                if (lowerLimitDeg < -360)
                    lowerLimitDeg = double.MinValue;
                if (upperLimitDeg > 360)
                    upperLimitDeg = double.MaxValue;
                if (upperLimitDeg < lowerLimitDeg || upperLimitDeg - lowerLimitDeg >= 360)
                {
                    lowerLimitDeg = double.MinValue;
                    upperLimitDeg = double.MaxValue;
                }

                if (lowerLimitDeg > double.MinValue && upperLimitDeg < double.MaxValue)
                {
                    lowerLimit = MathHelperD.ToRadians(lowerLimitDeg);
                    upperLimit = MathHelperD.ToRadians(upperLimitDeg);

                    return true;
                }
                else
                {
                    lowerLimit = double.MinValue;
                    upperLimit = double.MaxValue;

                    return false;
                }
            }

            public void Limit2PILite(ref double value)
            {
                if (value < 0)
                {
                    if (value <= -MathHelperD.TwoPi) value += MathHelperD.FourPi; else value += MathHelperD.TwoPi;
                }
                else if (value >= MathHelperD.TwoPi) 
                { 
                    if (value >= MathHelperD.FourPi) value -= MathHelperD.FourPi;
                    else value -= MathHelperD.TwoPi; 
                }
            }

            public void SetRotorLimits(IMyMotorStator rotor, double lowerLimit, double upperLimit)
            {
                if (lowerLimit == double.MinValue) rotor.SetValueFloat("LowerLimit", float.MinValue);
                else rotor.LowerLimitRad = (float)lowerLimit;

                if (upperLimit == double.MaxValue) rotor.SetValueFloat("UpperLimit", float.MaxValue);
                else rotor.UpperLimitRad = (float)upperLimit;
            }

            public void FireWeapons(ElevationGroup elevationRotor, int clock, bool isSalvo)
            {
                if (isSalvo)
                {
                    if (elevationRotor.Weapons.Count > 0 && elevationRotor.WeaponNextFireClock <= clock)
                    {
                        if (elevationRotor.WeaponStaggerIndex >= elevationRotor.Weapons.Count)
                        { 
                            elevationRotor.WeaponStaggerIndex = 0;
                        }
                        IMyUserControllableGun weapon = elevationRotor.Weapons[elevationRotor.WeaponStaggerIndex];
                        if (settings.WCAPI != null)
                        {
                            settings.WCAPI.ToggleWeaponFire(weapon, true, true);
                            elevationRotor.WCSalvoCleanup = true;
                        }
                        else
                            WeaponShootAction.Apply(weapon);

                        elevationRotor.WeaponNextFireClock = clock + elevationRotor.WeaponStaggerInterval;
                        elevationRotor.WeaponStaggerIndex++;
                    }
                }
                else
                {
                    if (!elevationRotor.FireLatch)
                    {
                        foreach (IMyUserControllableGun weapon in elevationRotor.Weapons)
                        {
                            if (settings.WCAPI != null)
                                settings.WCAPI.ToggleWeaponFire(weapon, true, true);
                            else
                                WeaponShootProperty.SetValue(weapon, true);
                        }
                        elevationRotor.FireLatch = true;
                    }
                }
            }

            public void ReleaseWeapons(bool force = false)
            {
                foreach (ElevationGroup elevationRotor in ElevationRotors)
                { 
                    ReleaseWeapons(elevationRotor, force);
                }
            }
            public void ReleaseWeapons(ElevationGroup elevationRotor, bool force = false)
            {
                if (elevationRotor.FireLatch || elevationRotor.WCSalvoCleanup || force)
                {
                    foreach (IMyUserControllableGun weapon in elevationRotor.Weapons)
                    {
                        if (settings.WCAPI != null)
                            settings.WCAPI.ToggleWeaponFire(weapon, false, true);
                        else
                            WeaponShootProperty.SetValue(weapon, false);
                    }
                    elevationRotor.FireLatch = false;
                    elevationRotor.WCSalvoCleanup = false;
                }
            }
            public Vector3D ɉ(IMyCubeGrid J, ref Vector3I ɇ)
            {
                MatrixD ƣ = J.WorldMatrix;
                return (ƣ.Translation + (ƣ.Right * ɇ.X) + (ƣ.Up * ɇ.Y) + (ƣ.Backward * ɇ.Z));
            }

            public class ElevationGroup
            {
                public IMyMotorStator Rotor;
                public double AngleOffsetY; 
                public double LowerLimitY;
                public double UpperLimitY;
                public bool HaveLimitY;
                public bool ReverseY;

                public IMyTerminalBlock AimBlock;
                public double CurrentElevation;

                public IMyCubeGrid Ⱦ;
                public Vector3I[] Ƚ; 

                public PredictiveController RotorCtrl; 

                public List<IMyUserControllableGun> Weapons;
                public bool FireLatch;

                public bool WCSalvoCleanup;

                public int WeaponStaggerIndex; 
                public int WeaponNextFireClock;
                public int WeaponStaggerInterval;
            }
            public enum ReloadState 
            {
                NONE,
                MAX_ANGLE,
                LOCK_STATE_ANGLE,
                LOCK_CONNECTORS,
                TRANSFER_ITEMS
            }
        }
    
}
