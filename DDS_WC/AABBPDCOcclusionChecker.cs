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
    public interface IPDCOcclusionChecker
    {
        bool TestWorldRayHit(Vector3 startPosition, Vector3 aimVector);
    }
    public class AABBPDCOcclusionChecker : IPDCOcclusionChecker
        {
            public Vector3I[] ADJACENT_VECTORS = { Vector3I.Left, Vector3I.Right, Vector3I.Up, Vector3I.Down, Vector3I.Forward, Vector3I.Backward }; 

            public MyDynamicAABBTree AABBTree;

            public IMyCubeGrid Grid;
            public AABBPDCOcclusionChecker() { }
            public IEnumerator<int> Init(PDCOcclusionGrid mainGrid, List<PDCOcclusionGrid> subGrids, float occlusionExtraClearance, int limit)
            {
                if (limit <= 0) limit = 1000000000; 

                int counter = 0;

                Grid = mainGrid.Grid;
                AABBTree = new MyDynamicAABBTree();

                Vector3 offset = new Vector3(0.5f * Grid.GridSize);
                if (occlusionExtraClearance != 0f) 
                {
                    offset += new Vector3(occlusionExtraClearance); 
                }

                Stack<Vector3I> remaining = new Stack<Vector3I>();
                HashSet<Vector3I> tested = new HashSet<Vector3I>(); 
                BoundingBox hitBox;

                remaining.Push(mainGrid.StartPosition);
                tested.Add(mainGrid.StartPosition);

                hitBox = new BoundingBox((mainGrid.StartPosition * Grid.GridSize) - offset, (mainGrid.StartPosition * Grid.GridSize) + offset);
                AABBTree.AddProxy(ref hitBox, hitBox, 0);

                Vector3I next; 

                while (remaining.Count > 0)
                {
                    Vector3I current = remaining.Pop();
                    for (int i = 0; i < 6; i++)
                    {
                        next = current + ADJACENT_VECTORS[i];
                        if (!tested.Contains(next))
                        {
                            tested.Add(next);
                            if (Grid.CubeExists(next))
                            {
                                remaining.Push(next); hitBox = new BoundingBox((next * Grid.GridSize) - offset, (next * Grid.GridSize) + offset);
                                AABBTree.AddProxy(ref hitBox, hitBox, 0);
                            }
                        }
                    }
                    counter++;
                    if (counter % limit == 0)
                    { 
                        yield return counter;
                    }
                }

                Dictionary<IMyCubeGrid, PDCOcclusionGrid> uniqueSubGrids = new Dictionary<IMyCubeGrid, PDCOcclusionGrid>();

                foreach (PDCOcclusionGrid subGrid in subGrids)
                {
                    if (!uniqueSubGrids.ContainsKey(subGrid.Grid))
                    {
                        uniqueSubGrids.Add(subGrid.Grid, subGrid); 
                    } 
                }

                MatrixD matrix = Grid.WorldMatrix;
                MatrixD.Transpose(ref matrix, out matrix);
                foreach (PDCOcclusionGrid subGrid in uniqueSubGrids.Values)
                {
                    if (subGrid.Grid != Grid)
                    {
                        Vector3 min = Vector3D.TransformNormal((subGrid.Grid.WorldAABB.Min - Grid.WorldMatrix.Translation), ref matrix); 
                        Vector3 max = Vector3D.TransformNormal((subGrid.Grid.WorldAABB.Max - Grid.WorldMatrix.Translation), ref matrix); 
                        
                        hitBox = new BoundingBox(min - offset, max + offset); 
                        AABBTree.AddProxy(ref hitBox, hitBox, 0);
                    }
                }
            }
            public bool TestWorldRayHit(Vector3 startPosition, Vector3 aimVector)
            {
                MatrixD matrix = Grid.WorldMatrix;
                MatrixD.Transpose(ref matrix, out matrix);

                Line line = new Line(Vector3D.TransformNormal(startPosition - Grid.WorldMatrix.Translation, ref matrix), Vector3D.TransformNormal(aimVector, ref matrix) * 1000); 

                return TestLocalRayHit(ref line);
            }
            public bool TestLocalRayHit(ref Line line)
            {
                List<MyLineSegmentOverlapResult<BoundingBox>> result = new List<MyLineSegmentOverlapResult<BoundingBox>>(0);
                AABBTree.OverlapAllLineSegment(ref line, result);
                foreach (MyLineSegmentOverlapResult<BoundingBox> hit in result)
                {
                    if (hit.Element.Extents.LengthSquared() < (line.From - hit.Element.Center).LengthSquared())
                    {
                        return true;
                    } 
                }
                return false;
            }
        }
    
}
