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
 
        public class CubeExistsPDCOcclusionChecker : IPDCOcclusionChecker
        {
            private int MAX_ITERATIONS = 40;
            public IMyCubeGrid Grid;
            public BoundingBox GriddBB;
            double InvGridSize;
            public List<IMyCubeGrid> SubGrids;
            public float AdjustedClearance; 
            public bool HaveAdjustedClearance; 
            public CubeExistsPDCOcclusionChecker(PDCOcclusionGrid mainGrid, List<PDCOcclusionGrid> subGrids, float occlusionExtraClearance, IMyProgrammableBlock Ʈ)
            {
                Grid = mainGrid.Grid; 
                GriddBB = new BoundingBox(Grid.Min, Grid.Max); 

                InvGridSize = 1.0 / Grid.GridSize;

                if (occlusionExtraClearance != 0) 
                { 
                    HaveAdjustedClearance = true;
                    AdjustedClearance = Grid.GridSize * 0.5f + occlusionExtraClearance;
                }   
                else
                { 
                    HaveAdjustedClearance = false;
                    AdjustedClearance = 0f; 
                }

                Dictionary<IMyCubeGrid, PDCOcclusionGrid> uniqueSubGrids = new Dictionary<IMyCubeGrid, PDCOcclusionGrid>();

                foreach (PDCOcclusionGrid subGrid in subGrids) 
                {
                    if (!uniqueSubGrids.ContainsKey(subGrid.Grid)) 
                    {
                        uniqueSubGrids.Add(subGrid.Grid, subGrid); 
                    }
                }

                SubGrids = new List<IMyCubeGrid>(uniqueSubGrids.Count); 

                MatrixD matrix = Grid.WorldMatrix; 
                MatrixD.Transpose(ref matrix, out matrix); 
                foreach (PDCOcclusionGrid subGrid in uniqueSubGrids.Values) 
                { 
                    if (subGrid.Grid != Grid)
                    {
                        SubGrids.Add(subGrid.Grid);
                    } 
                }
            }
            public bool TestWorldRayHit(Vector3 startPosition, Vector3 aimVector)
            {
                float startToWallAmount;
                if (Grid.WorldAABB.Contains(startPosition) == ContainmentType.Disjoint)
                {
                    double? result = Grid.WorldAABB.Intersects(new Ray(startPosition, aimVector));
                    if (result == null)
                    { 
                        return false;
                    }
                    startToWallAmount = (float)result.Value;
                }
                else 
                { 
                    double? result = Grid.WorldAABB.Intersects(new Ray(startPosition + (aimVector * 5000), -aimVector)); 
                    if (result == null) 
                    {
                        return false;
                    }
                    startToWallAmount = 5000f - (float)result.Value;
                }

                MatrixD matrix = Grid.WorldMatrix;
                MatrixD.Transpose(ref matrix, out matrix); 

                Line line = new Line(Vector3D.TransformNormal(startPosition - Grid.WorldMatrix.Translation, ref matrix) * InvGridSize, Vector3D.TransformNormal(startPosition + (aimVector * startToWallAmount) - Grid.WorldMatrix.Translation, ref matrix) * InvGridSize);
                
                if (TestLocalRayHit(ref line))
                { 
                    return true;
                }

                if (SubGrids.Count > 0)
                {
                    RayD ray = new RayD(startPosition, aimVector); 

                    foreach (IMyCubeGrid subGrid in SubGrids)
                    {
                        if (subGrid.WorldAABB.Intersects(ray) != null)
                        {
                            if (subGrid.WorldAABB.Extents.LengthSquared() < (ray.Position - subGrid.WorldAABB.Center).LengthSquared()) 
                            { 
                                return true; 
                            }
                        }
                    }
                }
                return false;
            }
            public bool TestLocalRayHit(ref Line line)
            {
                int division = Math.Min((int)Math.Ceiling(line.Length), MAX_ITERATIONS);
                float increment = 1.0f / division * line.Length;

                Vector3I startPosition = new Vector3I((int)Math.Round(line.From.X), (int)Math.Round(line.From.Y), (int)Math.Round(line.From.Z)); 

                for (int i = 1; i <= division; i++)
                {
                    Vector3 rawPosition = line.From + (line.Direction * increment * i);
                    Vector3I testPosition = new Vector3I((int)Math.Round(rawPosition.X),(int)Math.Round(rawPosition.Y), (int)Math.Round(rawPosition.Z));
                    if (Grid.CubeExists(testPosition))
                    {
                        if (testPosition != startPosition)
                        {
                            if (HaveAdjustedClearance)
                            {
                                double closest = Math.Min(Math.Abs(rawPosition.X - testPosition.X), Math.Min(Math.Abs(rawPosition.Y - testPosition.Y), Math.Abs(rawPosition.Z - testPosition.Z))); 
                                if (closest <= AdjustedClearance)
                                { 
                                    return true;
                                }
                            }
                            else 
                            { 
                                return true; 
                            }
                        }
                    }
                }
                return false;
            }
        }
    
}
