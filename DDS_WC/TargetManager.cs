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
    
        public class TargetManager
        {
            const int TICKS_PER_SECOND = Program.TICKS_PER_SECOND;

            Dictionary<long, Wrapper> targetsLookup;
            SortedSet<Wrapper> targetsByRaycastRefresh; 

            HashSet<long> targetsBlackList;
            public TargetManager()
            {
                targetsLookup = new Dictionary<long, Wrapper>();
                targetsByRaycastRefresh = new SortedSet<Wrapper>(new RaycastRefreshComparer());
                targetsBlackList = new HashSet<long>();
            }
            public bool UpdateTarget(ref MyDetectedEntityInfo entityInfo, int clock) 
            {
                PDCTarget dummyTarget;
                return UpdateTarget(ref entityInfo, clock, out dummyTarget); 
            }
            public bool UpdateTarget(ref MyDetectedEntityInfo entityInfo,int clock, out PDCTarget target)
            {
                if (targetsBlackList.Contains(entityInfo.EntityId))
                    {
                        target = null;
                        return false;
                    }
                if (targetsLookup.ContainsKey(entityInfo.EntityId))
                {
                    Wrapper wrapper = targetsLookup[entityInfo.EntityId];
                    target = wrapper.Target;

                    int deltaTicks = Math.Max(clock - target.DetectedClock, 1);
                    double deltaTimeR = 1.0 / deltaTicks;

                    target.LastVelocity = target.Velocity;
                    target.LastDetectedClock = target.DetectedClock;

                    if (entityInfo.HitPosition != null)
                    {
                        target.Position = entityInfo.HitPosition.Value; 

                        target.CenterPosition = entityInfo.Position;
                        target.RaycastDetectedClock = clock; 

                        target.Orientation = entityInfo. Orientation; 
                        target.OrientationUpdatedClock = clock;
                    }
                    else 
                    { 
                        target.Position = entityInfo.Position; 
                    }

                    target.IsLargeGrid = (entityInfo.Type == MyDetectedEntityType.LargeGrid);
                    target.Velocity = entityInfo.Velocity;
                    target.DetectedClock = clock; 
                    Vector3D acceleration = (target.Velocity - target.LastVelocity) * deltaTimeR * TICKS_PER_SECOND;
                    if (acceleration.LengthSquared() > 1) 
                    {
                        target.Acceleration = (target.Acceleration * 0.25) + (acceleration * 0.75); 
                    }
                    else
                    {
                       target.Acceleration = Vector3D.Zero; 
                    }

                    wrapper.LastLocalDetectedClock = clock;
                    target.TargetSizeSq = entityInfo.BoundingBox.Extents.LengthSquared() * 0.5;
                    return false;
                }
                else
                {
                    target = new PDCTarget(entityInfo.EntityId);

                    target.Position = entityInfo.Position;
                    target.Velocity = target.LastVelocity = entityInfo.Velocity;
                    target.DetectedClock = target.LastDetectedClock = clock;

                    Wrapper wrapper = new Wrapper();
                    wrapper.EntityId = target.EntityId;
                    wrapper.Target = target;

                    wrapper.LastLocalDetectedClock = clock;

                    targetsLookup.Add(entityInfo.EntityId, wrapper);
                    target.TargetSizeSq = entityInfo.BoundingBox.Extents.LengthSquared() * 0.5;
                    targetsByRaycastRefresh.Add(wrapper);

                    return true;
                }
            }

            public bool UpdateTarget(TargetData targetData, int clock, bool isLocal = true)
            {
                if (targetsBlackList.Contains(targetData.EntityId)) 
                { 
                    return false; 
                }
                if (targetsLookup.ContainsKey(targetData.EntityId))
                {
                    Wrapper wrapper = targetsLookup[targetData.EntityId];
                    PDCTarget target = wrapper.Target;

                    int deltaTicks = Math.Max(clock - target.DetectedClock, 1);
                    double deltaTimeR = 1.0 / deltaTicks;

                    target.LastVelocity = target.Velocity;
                    target.LastDetectedClock = target.DetectedClock;

                    target.Position = targetData.Position;
                    target.Velocity = targetData.Velocity;
                    target.DetectedClock = clock;

                    Vector3D acceleration = (target.Velocity - target.LastVelocity) * deltaTimeR * TICKS_PER_SECOND;
                    if (acceleration.LengthSquared() > 1)
                    {
                        target.Acceleration = (target.Acceleration * 0.25) + (acceleration * 0.75);
                    }
                    else 
                    {
                        target.Acceleration = Vector3D.Zero; 
                    }
                    if (isLocal) 
                    {
                        wrapper.LastLocalDetectedClock = clock; 
                    }
                    return false;
                }
                else
                {
                    PDCTarget target = new PDCTarget(targetData.EntityId);
                    target.Position = targetData.Position;
                    target.Velocity = target.LastVelocity = targetData.Velocity;
                    target.DetectedClock = target.LastDetectedClock = clock;
                    Wrapper wrapper = new  Wrapper();
                    wrapper.EntityId = target.EntityId;
                    wrapper.Target = target;
                if (isLocal) 
                {
                    wrapper.LastLocalDetectedClock = clock; 
                }
                    targetsLookup.Add(targetData.EntityId, wrapper);
                    targetsByRaycastRefresh.Add(wrapper); 
                    return true;
                }
            }
            public void UpdateRaycastRefreshClock(long entityId, int clock)
            {
                if (targetsLookup.ContainsKey(entityId))
                {
                    Wrapper wrapper = targetsLookup[entityId];
                    wrapper.Target.RaycastRefreshClock = clock;
                    targetsByRaycastRefresh.Remove(wrapper);
                    wrapper.RaycastRefreshClock = clock;
                    targetsByRaycastRefresh.Add(wrapper);
                }
            }
            public bool TargetExists(long entityId)
            { 
                return targetsLookup.ContainsKey(entityId); 
            }
            public bool TargetUpToLocalDate(long entityId, int minimumLastClock)
            {
                if (targetsLookup.ContainsKey(entityId))
                {
                    Wrapper wrapper = targetsLookup[entityId];   
                    return (wrapper.LastLocalDetectedClock >= minimumLastClock);
                }
                else 
                {
                    return false; 
                }
            }
            public int Count() 
            { 
                return targetsLookup.Count; 
            }

        public PDCTarget GetTarget(long entityId)
            {
                Wrapper wrapperr; 
                if (targetsLookup.TryGetValue(entityId, out wrapperr)) 
                return wrapperr.Target;
                else
                return null;
            }
            public List<PDCTarget> GetAllTargets()
            {
                List<PDCTarget> targetList = new List<PDCTarget>(targetsLookup.Count); 
                foreach (Wrapper wrapper in targetsByRaycastRefresh) 
                {
                    targetList.Add(wrapper.Target); 
                }
                return targetList;
            }
            public PDCTarget GetOldestRaycastUpdatedTarget() 
            {
                if (targetsByRaycastRefresh.Count == 0)
                    return null; 
                else
                    return targetsByRaycastRefresh.Min.Target; 
            }
            public PDCTarget FindLargestTarget()
            {
                double largestSizeSq = double.MinValue; 
                PDCTarget largestTarget = null; 

                foreach (Wrapper wrapper in targetsByRaycastRefresh) 
                {
                    if (wrapper.Target.TargetSizeSq > largestSizeSq)
                    {
                        largestSizeSq = wrapper.Target.TargetSizeSq; 
                        largestTarget = wrapper.Target; 
                    }
                }
                return largestTarget;
            }
            public void RemoveTarget(long entityId) 
            {
                if (targetsLookup.ContainsKey(entityId)) 
                {
                    targetsByRaycastRefresh.Remove(targetsLookup[entityId]); 
                    targetsLookup.Remove(entityId); 
                } 
            }
            public void ClearTargets() 
            {
                targetsLookup.Clear(); 
                targetsByRaycastRefresh.Clear(); 
            }
            public void AddToBlackList(long entityId) 
            {
                targetsBlackList.Add(entityId); 
            }
            public void ClearBlackList() 
            { 
                targetsBlackList.Clear(); 
            }
            class Wrapper
            {
                public long EntityId;
                public int RaycastRefreshClock;
                public int LastLocalDetectedClock; 
                public PDCTarget Target;
            }
            class RaycastRefreshComparer : IComparer<Wrapper>
            {
                public int Compare(Wrapper x, Wrapper y)
                {
                    if (x == null) 
                    return (y == null ? 0 : 1);
                    else if (y == null) return -1;
                    else return (x.RaycastRefreshClock < y.RaycastRefreshClock ? -1 : (x.RaycastRefreshClock > y.RaycastRefreshClock ? 1 : (x.EntityId < y.EntityId ? -1 : (x.EntityId > y.EntityId ? 1 : 0))));
                }
            }
        }
    
}
