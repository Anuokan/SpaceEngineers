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

    public class Profiler
    {
        public int HistoryMaxCount;
        public double NewValueFactor;
        public double AverageRuntime;
        public double PeakRuntime; 

        public double AverageComplexity; 
        public double PeakComplexity; 
        public IMyGridProgramRuntimeInfo Runtime { get;private set; }
        public Queue<double> HistoryRuntime = new Queue<double>();
        public Queue<double> HistoryComplexity = new Queue<double>();
        public Dictionary<string, SectionValues> AverageBreakdown =new Dictionary<string, SectionValues>();

        private double invMaxRuntimePercent;
        private double invMaxInstCountPercent;
        public Profiler(IMyGridProgramRuntimeInfo runtime, int historyMaxCount, double newValueFactor)
            {
                Runtime  = runtime;
                HistoryMaxCount = historyMaxCount;
                NewValueFactor = newValueFactor;
                invMaxRuntimePercent = 6;
                invMaxInstCountPercent = 100.0 / (Runtime .MaxInstructionCount == 0 ? 50000 : Runtime .MaxInstructionCount);
            }
            public void Clear()
            {
                AverageRuntime = 0;
                HistoryRuntime.Clear();
                PeakRuntime = 0;
                AverageComplexity = 0;
                HistoryComplexity.Clear();
                PeakComplexity =0;
            }
            public void UpdateRuntime() 
            { 
                double runtime = Runtime .LastRunTimeMs;
                AverageRuntime += (runtime - AverageRuntime) * NewValueFactor;

                HistoryRuntime.Enqueue(runtime);
                if (HistoryRuntime.Count > HistoryMaxCount) HistoryRuntime.Dequeue(); 
                PeakRuntime = HistoryRuntime.Max(); 
            }
            public void UpdateComplexity()
            {
                double complexity = Runtime .CurrentInstructionCount;
                AverageComplexity += (complexity - AverageComplexity) * NewValueFactor;

                HistoryComplexity.Enqueue(complexity);
                if (HistoryComplexity.Count > HistoryMaxCount) HistoryComplexity.Dequeue();
                PeakComplexity = HistoryComplexity.Max();
            }
            public void PrintPerformance(StringBuilder sb)
            {
                sb.AppendLine($"Avg Runtime = {AverageRuntime:0.0000}ms   ({AverageRuntime * invMaxRuntimePercent:0.00}%)");
                sb.AppendLine($"Peak Runtime = {PeakRuntime:0.0000}ms\n");
                sb.AppendLine($"Avg Complexity = {AverageComplexity:0.00}   ({AverageComplexity * invMaxInstCountPercent:0.00}%)");
                sb.AppendLine($"Peak Complexity = {PeakComplexity:0.00}");
            }
            public void StartSectionWatch(string section)
            {
                SectionValues sectionValues;
                if (AverageBreakdown.ContainsKey(section)) 
                {
                    sectionValues = AverageBreakdown[section]; 
                }
                else
                {
                    sectionValues = new SectionValues();
                    AverageBreakdown [section] = sectionValues;
                }
                
                sectionValues.StartTicks = DateTime.Now.Ticks;
            }
            public void StopSectionWatch(string section)
            {
                SectionValues sectionValues;
                if (AverageBreakdown.TryGetValue(section, out sectionValues))
                {
                    long current = DateTime.Now.Ticks;
                    double runtime = (current - sectionValues.StartTicks) * 0.0001;
                    sectionValues.AccumulatedCount++;
                    sectionValues.AccumulatedRuntime += runtime;
                    sectionValues.StartTicks = current;
                }
            }
            public void PrintSectionBreakdown(StringBuilder sb)
            {
                foreach (KeyValuePair<string, SectionValues> entry in AverageBreakdown)
                {
                    double runtime = (entry.Value.AccumulatedCount == 0 ? 0 : entry.Value.AccumulatedRuntime / entry.Value.AccumulatedCount);
                    sb.AppendLine($"{entry.Key} = {runtime:0.0000}ms");
                }
            }
            public class SectionValues
            {
                public long AccumulatedCount;
                public double AccumulatedRuntime;
                public long StartTicks;
            }
        }

    
}
