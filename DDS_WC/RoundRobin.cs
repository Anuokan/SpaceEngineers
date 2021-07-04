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
    
        public class RoundRobin<Т>
        {
            private List<Т> items;
            private Func<Т, bool> isReady;
            private int start;
            private int current; 
            private bool available;
            public RoundRobin(List<Т> dispatchItems, Func<Т, bool> isReadyFunc = null) 
            {
                items = dispatchItems; 
                isReady = isReadyFunc;
            
                start = current = 0; 
                available = false;

                if (items == null) items = new List<Т>(); 
            }

            public void Reset() 
            { 
                start = current = 0; 
            }
        
            public void Begin()
            { 
                start = current;
                available = (items.Count > 0); 
            }
        
            public Т GetNext()
            {
                if (start >= items.Count) start = 0; 
                if (current >= items.Count) 
                {
                    current = 0; 
                    available = (items.Count > 0); 
                }
            
                Т result = default(Т); 
            
                while (available)
                {
                    Т item = items[current++];
                
                    if (current >= items.Count) current = 0;
                    if (current == start) available = false;

                    if (isReady == null || isReady(item))
                    {
                        result = item;
                        break;
                    }
                }

                return result;
            
            }
        
            public void ReloadList(List<Т> dispatchItems)
            {
                items = dispatchItems;
                if (items == null) items = new List<Т>();

                if (start >= items.Count) start = 0;
                if (current >= items.Count) current = 0;

                available = false;
            }
        }
    
}
