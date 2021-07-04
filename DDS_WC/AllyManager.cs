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
    
        public class AllyManager
        {
            Dictionary<long, Wrapper> alliesLookup; 
            SortedSet<Wrapper> alliesByLastUpdate;
            public AllyManager() 
            {
                alliesLookup = new Dictionary<long, Wrapper>(); 
                alliesByLastUpdate = new SortedSet<Wrapper>(new LastUpdateComparer());
            }
            public bool UpdateAlly(ref MyDetectedEntityInfo entityInfo, int clock)
            {
                if (alliesLookup.ContainsKey(entityInfo.EntityId))
                {
                    Wrapper wrapper = alliesLookup[entityInfo.EntityId];

                    wrapper.AllyTrackData.Position = entityInfo.Position;
                    wrapper.AllyTrackData.Velocity = entityInfo.Velocity;
                    wrapper.AllyTrackData.IsLargeGrid = (entityInfo.Type == MyDetectedEntityType.LargeGrid);
                    wrapper.AllyTrackData.SizeSq = entityInfo.BoundingBox.Extents.LengthSquared();
                    wrapper.AllyTrackData.LastDetectedClock = clock;

                    alliesByLastUpdate.Remove(wrapper);

                    wrapper.LastUpdatedClock = clock;

                    alliesByLastUpdate.Add(wrapper); 
                    
                    return false;
                }
                else
                {
                    AllyTrack ally = new AllyTrack(entityInfo.EntityId);

                    ally.Position = entityInfo.Position;
                    ally.Velocity = entityInfo.Velocity;
                    ally.LastDetectedClock = clock;

                    Wrapper wrapper = new Wrapper();
                    wrapper.EntityId = ally.EntityId;
                    wrapper.AllyTrackData = ally;

                    wrapper.LastUpdatedClock = clock;

                    alliesLookup.Add(entityInfo.EntityId, wrapper);
                    alliesByLastUpdate.Add(wrapper);

                    return true;
                }
            }
            public bool UpdateAlly(AllyTrack allyTrack, int clock)
            {
                if (alliesLookup.ContainsKey(allyTrack.EntityId))
                {
                    Wrapper wrapper = alliesLookup[allyTrack.EntityId];

                    wrapper.AllyTrackData.Position = allyTrack.Position;
                    wrapper.AllyTrackData.Velocity = allyTrack.Velocity;
                    wrapper.AllyTrackData.LastDetectedClock = clock;

                    alliesByLastUpdate.Remove(wrapper);

                    wrapper.LastUpdatedClock = clock;

                    alliesByLastUpdate.Add(wrapper); 

                    return false; 
                }
                else
                {
                    allyTrack.LastDetectedClock = clock; 

                    Wrapper wrapper =new Wrapper(); 
                    wrapper.EntityId = allyTrack.EntityId; 
                    wrapper.AllyTrackData = allyTrack; 

                    wrapper.LastUpdatedClock = clock; 

                    alliesLookup.Add(allyTrack.EntityId, wrapper); 
                    alliesByLastUpdate.Add(wrapper);

                    return true;
                }
            }
            public void UpdateLastUpdatedClock(long entityId, int clock)
            {
                if (alliesLookup.ContainsKey(entityId))
                {
                    Wrapper wrapper = alliesLookup[entityId]; 

                    alliesByLastUpdate.Remove(wrapper); 

                    wrapper.LastUpdatedClock = clock; 

                    alliesByLastUpdate.Add(wrapper);
                }
            }
            public bool AllyExists(long entityId) 
            { 
                return alliesLookup.ContainsKey(entityId);
            }
            public int Count() 
            { 
                return alliesLookup.Count; 
            }
            public AllyTrack GetAlly(long entityId)
            {
                Wrapper wrapper;
                if (alliesLookup.TryGetValue(entityId, out wrapper)) return wrapper.AllyTrackData; 
                else return null; }
            public List<AllyTrack> GetAllAllies()
            {
                List<AllyTrack> targetList = new List<AllyTrack>(alliesLookup.Count);
                foreach (Wrapper wrapper in alliesByLastUpdate)
                { 
                    targetList.Add(wrapper.AllyTrackData);
                }
                return targetList;
            }
            public AllyTrack GetOldestUpdatedAlly()
            { 
                if (alliesByLastUpdate.Count == 0) return null;
                else return alliesByLastUpdate.Min.AllyTrackData; 
            }
            public void RemoveAlly(long entityId)
            {
                if (alliesLookup.ContainsKey(entityId)) 
                { 
                    alliesByLastUpdate.Remove(alliesLookup[entityId]);
                    alliesLookup.Remove(entityId);
                }
            }
            public void ClearAllies()
            { 
                alliesLookup.Clear();
                alliesByLastUpdate.Clear(); 
            }
            class Wrapper 
            {
                public long EntityId; 

                public int LastUpdatedClock; 

                public AllyTrack AllyTrackData; 
            }
            class LastUpdateComparer : IComparer<Wrapper>
            {
                public int Compare(Wrapper x, Wrapper y)
                {
                    if (x == null) return (y == null ? 0 : 1);
                    else if (y == null) return -1;
                    else return (x.LastUpdatedClock < y.LastUpdatedClock ? -1 : (x.LastUpdatedClock > y.LastUpdatedClock ? 1 : (x.EntityId < y.EntityId ? -1 : (x.EntityId > y.EntityId ? 1 : 0))));
                }
            }
        }
    
}
