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
    
        public class RaycastHandler
        {
            float coneLimit;
            double sideScale;
            bool noFwdScale;

            List<IMyCameraBlock> m_cameras;
            public List<IMyCameraBlock> Cameras
            {
                get { return m_cameras; }
                set
                {
                    foreach (IMyCameraBlock camera in value)
                    {
                        camera.Enabled = true;
                        camera.EnableRaycast = true;
                    }
                    m_cameras = value;

                    CompileCameraGroups();

                }
            }
            
            List<CameraGroup> m_cameraGroups;

            public RaycastHandler(List<IMyCameraBlock> cameras)
            {
                if (cameras.Count > 0)
                {
                    coneLimit = cameras[0].RaycastConeLimit;
                    if (coneLimit == 0f || coneLimit == 180f) sideScale = double.NaN;
                    else sideScale = Math.Tan(MathHelper.ToRadians(90 - coneLimit));

                    noFwdScale = double.IsNaN(sideScale) || double.IsInfinity(sideScale);
                    if (noFwdScale) sideScale = 1;
                }
                else 
                {
                    coneLimit = 45;
                    sideScale = 1;
                    noFwdScale = false;
                }

                Cameras = cameras;

                MyMath.InitializeFastSin();

            }

            private void CompileCameraGroups()
            {
                if (coneLimit <= 0 || coneLimit >= 180) 
                { 
                m_cameraGroups = new List<CameraGroup>();
                return;
                }
                Dictionary<string, CameraGroup> cameraGroupsLookup = new Dictionary<string, CameraGroup>();
                
                foreach (IMyCameraBlock camera in m_cameras)
                {
                    string key = camera.CubeGrid.EntityId.ToString() + "-" + ((int) camera.Orientation.Forward).ToString();
                    CameraGroup cameraGroup;
                    if (cameraGroupsLookup.ContainsKey(key))
                    {
                        cameraGroup = cameraGroupsLookup[key]; 
                    }
                    else
                    {
                        cameraGroup = new CameraGroup();
                        cameraGroup.Cameras = new List<IMyCameraBlock>();
                        cameraGroup.SideScale = sideScale;
                        cameraGroup.NoFwdScale = noFwdScale;
                        cameraGroupsLookup[key] = cameraGroup;
                    }

                    cameraGroup.Cameras.Add(camera);
                }

                m_cameraGroups = cameraGroupsLookup.Values.ToList();

                foreach (CameraGroup cameraGroup in m_cameraGroups)
                {
                    cameraGroup.Grid = cameraGroup.Cameras[0].CubeGrid;

                    int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue, minZ = int.MaxValue, maxZ = int.MinValue;

                    foreach (IMyCameraBlock camera in cameraGroup.Cameras)
                    {
                        minX = Math.Min(minX, camera.Position.X);
                        maxX = Math.Max(maxX, camera.Position.X);
                        minY = Math.Min(minY, camera.Position.Y);
                        maxY = Math.Max(maxY, camera.Position.Y);
                        minZ = Math.Min(minZ, camera.Position.Z);
                        maxZ = Math.Max(maxZ, camera.Position.Z);
                    }

                    Base6Directions.Direction gridUp = cameraGroup.Grid.WorldMatrix.GetClosestDirection(cameraGroup.Cameras[0].WorldMatrix.Up);
                    Base6Directions.Direction gridLeft = cameraGroup.Grid.WorldMatrix.GetClosestDirection(cameraGroup.Cameras[0].WorldMatrix.Left);
                    Base6Directions.Direction gridForward = cameraGroup.Grid.WorldMatrix.GetClosestDirection(cameraGroup.Cameras[0].WorldMatrix.Forward);

                    cameraGroup.DirectionUp = CameraGroup.GetDirectionFunction(gridUp);
                    cameraGroup.DirectionLeft = CameraGroup.GetDirectionFunction(gridLeft);
                    cameraGroup.DirectionForward = CameraGroup.GetDirectionFunction(gridForward);

                    cameraGroup.PointUp = CameraGroup.H(gridUp, minX, maxX, minY, maxY, minZ, maxZ);
                    cameraGroup.PointDown = CameraGroup.H(Base6Directions.GetOppositeDirection(gridUp), minX, maxX, minY, maxY, minZ, maxZ);
                    cameraGroup.PointLeft = CameraGroup.H(gridLeft, minX, maxX, minY, maxY, minZ, maxZ); 
                    cameraGroup.PointRight = CameraGroup.H(Base6Directions.GetOppositeDirection(gridLeft), minX, maxX, minY, maxY, minZ, maxZ);
                }
            }

            public bool Raycast(ref Vector3D targetPosition, out MyDetectedEntityInfo entityInfo, double extraDistance = 0)
            {
                IMyCameraBlock camera = GetRaycastable(ref targetPosition);
                if (camera != null)
                {
                    if (extraDistance == 0)
                    {
                        entityInfo = Raycast(camera, ref targetPosition);
                    }
                    else
                    {
                        Vector3D targetRangeVector = targetPosition- camera.WorldMatrix.Translation;
                        Vector3D adjustedPosition = targetPosition + ((extraDistance / Math.Max(targetRangeVector.Length(), 0.000000000000001)) * targetRangeVector);
                        entityInfo = Raycast(camera, ref adjustedPosition);
                    }
                    return true;
                }
                else
                {
                    entityInfo = default(MyDetectedEntityInfo);
                    return false;
                }
            }

            IMyCameraBlock GetRaycastable(ref Vector3D targetPosition)
            {
                foreach (CameraGroup tryGroup in m_cameraGroups)
                {
                    if (tryGroup.WithinLimits(ref targetPosition))
                    {
                        return GetFromCameraGroup(tryGroup, ref targetPosition
);
                    }
                }
                return null;
            }
            IMyCameraBlock GetFromCameraGroup(CameraGroup group, ref Vector3D targetPosition)
            {
                bool checkGroupLimit = true;

                for (int i = 0; i < group.Cameras.Count; i++)
                {
                    if (group.StaggerIndex >= group.Cameras.Count) 
                    {
                        group.StaggerIndex = 0;
                    }
                    IMyCameraBlock camera = group.Cameras[group.StaggerIndex++]; 
                    if (camera.IsWorking) 
                    {
                        if (CanScan(camera, ref targetPosition)) 
                        {
                            return camera; 
                        }
                        else if (checkGroupLimit) 
                        { 
                            checkGroupLimit = false; 
                            if (!group.WithinLimits(ref targetPosition)) 
                            {
                                break;
                            } 
                        } 
                    }
                }

                return null;
            }
            bool CanScan(IMyCameraBlock camera, ref Vector3D position)
            {
                Vector3D forward = (noFwdScale ? Vector3D.Zero : camera.WorldMatrix.Forward);
                Vector3D scaleLeft = camera.WorldMatrix.Left;
                Vector3D scaleUp = camera.WorldMatrix.Up;

                Vector3D direction = position - camera.WorldMatrix.Translation;
                if (sideScale >= 0)
                {
                    return (camera.AvailableScanRange * camera.AvailableScanRange >= direction.LengthSquared()) &&
                    direction.Dot(forward + scaleLeft) >= 0 &&
                    direction.Dot(forward - scaleLeft) >= 0 &&
                    direction.Dot(forward + scaleUp) >= 0 &&
                    direction.Dot(forward - scaleUp) >= 0;
                }
                else
                {
                    return (camera.AvailableScanRange * camera.AvailableScanRange >= direction.LengthSquared()) &&
                    (direction.Dot(forward + scaleLeft) >= 0 || 
                    direction.Dot(forward - scaleLeft) >= 0 || 
                    direction.Dot(forward + scaleUp) >= 0 || 
                    direction.Dot(forward - scaleUp) >= 0);
                }
            }

            void GetRaycastParameters(IMyCameraBlock camera, ref Vector3D position, out double distance,out double pitch, out double yaw)
            {
                Vector3D targetVector = position - camera.WorldMatrix.Translation;
                targetVector = Vector3D.TransformNormal(targetVector, MatrixD.Transpose(camera.WorldMatrix));
                
                Vector3D yawBaseVector = Vector3D.Normalize(new Vector3D(targetVector.X, 0, targetVector.Z));
                
                distance = targetVector.Normalize();
                
                yaw = MathHelper.ToDegrees(Math.Acos(MathHelper.Clamp(yawBaseVector.Dot(Vector3D.Forward), -1, 1)) * Math.Sign(targetVector.X));
                pitch = MathHelper.ToDegrees(Math.Acos(MathHelper.Clamp(yawBaseVector.Dot(targetVector), -1, 1)) * Math.Sign(targetVector.Y));
            }
            MyDetectedEntityInfo Raycast(IMyCameraBlock camera, ref Vector3D position)
            {
                double raycastDistance, raycastPitch, raycastYaw;
                GetRaycastParameters(camera, ref position, out raycastDistance, out raycastPitch, out raycastYaw);
                return camera.Raycast(raycastDistance, (float)raycastPitch, (float)raycastYaw);
            }
            public class CameraGroup
            {
                public List<IMyCameraBlock> Cameras;
                public int StaggerIndex;
                
                public double SideScale;
                public bool NoFwdScale;
                
                public IMyCubeGrid Grid;
                
                public Vector3I PointUp;
                public Vector3I PointDown;
                public Vector3I PointLeft; 
                public Vector3I PointRight; 
                
                public Func<IMyCubeGrid, Vector3D> DirectionUp; 
                public Func<IMyCubeGrid, Vector3D> DirectionLeft;
                public Func<IMyCubeGrid, Vector3D> DirectionForward; 
                
                public static Vector3D GetDirectionUp(IMyCubeGrid J) { return J.WorldMatrix.Up; }
                public static Vector3D GetDirectionDown(IMyCubeGrid J) { return J.WorldMatrix.Down; }
                public static Vector3D GetDirectionLeft(IMyCubeGrid J) { return J.WorldMatrix.Left; }
                public static Vector3D GetDirectionRight(IMyCubeGrid J) { return J.WorldMatrix.Right; }
                public static Vector3D GetDirectionForward(IMyCubeGrid J) { return J.WorldMatrix.Forward; }
                public static Vector3D GetDirectionBackward(IMyCubeGrid J) { return J.WorldMatrix.Backward; }
                
                public static Func<IMyCubeGrid, Vector3D> GetDirectionFunction(Base6Directions.Direction dir)
                {
                    switch (dir)
                    {
                        case Base6Directions.Direction.Up: return GetDirectionUp;
                        case Base6Directions.Direction.Down:return GetDirectionDown;
                        case Base6Directions.Direction.Left: return GetDirectionLeft;
                        case Base6Directions.Direction.Right: return GetDirectionRight;
                        case Base6Directions.Direction.Forward:return GetDirectionForward;
                        case Base6Directions.Direction.Backward: return GetDirectionBackward;
                        default: return GetDirectionForward;
                    }
                }
                public static Vector3I H(Base6Directions.Direction dir, int minX, int maxX, int minY, int maxY, int minZ, int I)
                {
                    switch (dir)
                    {
                        case Base6Directions.Direction.Up:
                            return newVector3I((minX + maxX) / 2, maxY, (minZ + I) / 2);
                        case Base6Directions.Direction.Down: return new Vector3I((minX + maxX) / 2, minY, (minZ + I) / 2);
                        case Base6Directions.Direction.Left:
                            return new Vector3I(minX, (minY + maxY) / 2, (minZ + I) / 2);
                        case Base6Directions.Direction.Right:
                            return new Vector3I(maxX, (minY + maxY) / 2, (minZ + I) / 2);
                        case Base6Directions.Direction.Forward: return new Vector3I((minX + maxX) / 2, (minY + maxY) / 2, minZ);
                        case Base6Directions.Direction.Backward:
                            return new Vector3I((minX + maxX) / 2, (minY + maxY) / 2, I);
                        default: return new Vector3I((minX + maxX) / 2, (minY + maxY) / 2, minZ);
                    }
                }
                Vector3D GetAim(ref Vector3D position, ref Vector3In)
                { 
                return position - Grid.GridIntegerToWorld(refPoint); 
                }

                public bool WithinLimits(ref Vector3D position)
                {
                    Vector3D forward = (NoFwdScale ? Vector3D.Zero : DirectionForward(Grid));
                    Vector3D scaleLeft = SideScale * DirectionLeft(Grid);
                    Vector3D scaleUp = SideScale * DirectionUp(Grid); if (SideScale >= 0)
                    {
                        return (GetAim(ref position, ref PointRight).Dot(forward + scaleLeft) >= 0 &&
                                GetAim(ref position, ref PointLeft).Dot(forward - scaleLeft) >= 0 &&
                                GetAim(ref position, ref PointDown).Dot(forward + scaleUp) >= 0 && 
                                GetAim(ref position,ref PointUp).Dot(forward - scaleUp) >= 0);
                    }
                    else
                    {
                        return (GetAim(ref position, ref PointRight).Dot(forward + scaleLeft) >= 0 || 
                                GetAim(ref position, ref PointLeft).Dot(forward - scaleLeft) >= 0 || 
                                GetAim(ref position, ref PointDown).Dot(forward + scaleUp) >= 0 || 
                                GetAim(refZ, ref PointUp).Dot(forward - scaleUp) >= 0);
                    }
                }
            }
        }
    
}
