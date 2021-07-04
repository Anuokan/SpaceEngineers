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
            float Ũ;
            double ŧ;
            bool Ŧ;
            List<IMyCameraBlock> ť;
        public List<IMyCameraBlock> Cameras
            {
                get { return ť; }
                set
                {
                    foreach (IMyCameraBlock ª in value)
                    {
                        ª.Enabled = true;
                        ª.EnableRaycast = true;
                    }
                    ť = value;
                    Ţ();

                }
            }
            
            List<Ï> Ť;

            public RaycastHandler(List<IMyCameraBlock> ţ)
            {
                if (ţ.Count > 0)
                {
                    Ũ = ţ[0].RaycastConeLimit;
                    if (Ũ == 0f || Ũ == 180f) ŧ = double.NaN;
                    else ŧ = Math.Tan(MathHelper.ToRadians(90 - Ũ));
                    Ŧ = double.IsNaN(ŧ) || double.IsInfinity(ŧ);
                    if (Ŧ) ŧ = 1;
                }
                else 
                {
                Ũ = 45;
                ŧ = 1;
                Ŧ = false;
                }

                Cameras = ţ;

                MyMath.InitializeFastSin();

            }

            private void Ţ()
            {
                if (Ũ <= 0 || Ũ >= 180) 
                { 
                Ť = new List<Ï>();
                return;
                }
                Dictionary<string, Ï> š = new Dictionary<string, Ï>();
                
                foreach (IMyCameraBlock ª in ť)
                {
                    string Š = ª.CubeGrid.EntityId.ToString() + "-" + ((int) ª.Orientation.Forward).ToString();
                    Ï ş;
                    if (š.ContainsKey(Š))
                    {
                        ş = š[Š]; 
                    }
                    else
                    {
                        ş = new Ï();
                        ş.Î = new List<IMyCameraBlock>();
                        ş.Ì = ŧ;
                        ş.Ë = Ŧ;
                        š[Š] = ş;
                    }

                    ş.Î.Add(ª);
                }

                Ť = š.Values.ToList();
                foreach (Ï ş in Ť)
                {
                    ş.Ò = ş.Î[0].CubeGrid; int F = int.MaxValue, E = int.MinValue, D = int
.MaxValue, C = int.MinValue, B = int.MaxValue, I = int.MinValue; foreach (IMyCameraBlock ª in ş.Î)
                    {
                        F = Math.Min(F, ª.Position.X); E = Math
.Max(E, ª.Position.X); D = Math.Min(D, ª.Position.Y); C = Math.Max(C, ª.Position.Y); B = Math.Min(B, ª.Position.Z); I = Math.Max(I, ª.
Position.Z);
                    }
                    Base6Directions.Direction ų = ş.Ò.WorldMatrix.GetClosestDirection(ş.Î[0].WorldMatrix.Up); Base6Directions.Direction Ų =
          ş.Ò.WorldMatrix.GetClosestDirection(ş.Î[0].WorldMatrix.Left); Base6Directions.Direction ű = ş.Ò.WorldMatrix.
           GetClosestDirection(ş.Î[0].WorldMatrix.Forward); ş.Æ = Ï.N(ų); ş.Å = Ï.N(Ų); ş.Ä = Ï.N(ű); ş.Ê = Ï.H(ų, F, E, D, C, B, I); ş.É = Ï.H(Base6Directions.
                              GetOppositeDirection(ų), F, E, D, C, B, I); ş.È = Ï.H(Ų, F, E, D, C, B, I); ş.Ç = Ï.H(Base6Directions.GetOppositeDirection(Ų), F, E, D, C, B, I);
                }
            }
            public bool Raycast(ref
Vector3D Ę, out MyDetectedEntityInfo Ŵ, double Ű = 0)
            {
                IMyCameraBlock ª = ŭ(ref Ę); if (ª != null)
                {
                    if (Ű == 0) { Ŵ = µ(ª, ref Ę); }
                    else
                    {
                        Vector3D ů = Ę
- ª.WorldMatrix.Translation; Vector3D Ů = Ę + ((Ű / Math.Max(ů.Length(), 0.000000000000001)) * ů); Ŵ = µ(ª, ref Ů);
                    }
                    return true;
                }
                else
                {
                    Ŵ =
default(MyDetectedEntityInfo); return false;
                }
            }
            IMyCameraBlock ŭ(ref Vector3D Ę)
            {
                foreach (Ï Ŭ in Ť)
                {
                    if (Ŭ.l(ref Ę))
                    {
                        return ū(Ŭ, ref Ę
);
                    }
                }
                return null;
            }
            IMyCameraBlock ū(Ï Ū, ref Vector3D Ę)
            {
                bool Â = true; for (int Q = 0; Q < Ū.Î.Count; Q++)
                {
                    if (Ū.Í >= Ū.Î.Count) { Ū.Í = 0; }
                    IMyCameraBlock ª = Ū.Î[Ū.Í++]; if (ª.IsWorking) { if (À(ª, ref Ę)) { return ª; } else if (Â) { Â = false; if (!Ū.l(ref Ę)) { break; } } }
                }
                return null;
            }
            bool À(
IMyCameraBlock ª, ref Vector3D Z)
            {
                Vector3D j = (Ŧ ? Vector3D.Zero : ª.WorldMatrix.Forward); Vector3D h = ª.WorldMatrix.Left; Vector3D g = ª.
WorldMatrix.Up; Vector3D Á = Z - ª.WorldMatrix.Translation; if (ŧ >= 0)
                {
                    return (ª.AvailableScanRange * ª.AvailableScanRange >= Á.LengthSquared())
&& Á.Dot(j + h) >= 0 && Á.Dot(j - h) >= 0 && Á.Dot(j + g) >= 0 && Á.Dot(j - g) >= 0;
                }
                else
                {
                    return (ª.AvailableScanRange * ª.AvailableScanRange >= Á.
LengthSquared()) && (Á.Dot(j + h) >= 0 || Á.Dot(j - h) >= 0 || Á.Dot(j + g) >= 0 || Á.Dot(j - g) >= 0);
                }
            }
            void º(IMyCameraBlock ª, ref Vector3D Z, out double w,
out double v, out double s)
            {
                Vector3D r = Z - ª.WorldMatrix.Translation; r = Vector3D.TransformNormal(r, MatrixD.Transpose(ª.
WorldMatrix)); Vector3D q = Vector3D.Normalize(new Vector3D(r.X, 0, r.Z)); w = r.Normalize(); s = MathHelper.ToDegrees(Math.Acos(MathHelper.
   Clamp(q.Dot(Vector3D.Forward), -1, 1)) * Math.Sign(r.X)); v = MathHelper.ToDegrees(Math.Acos(MathHelper.Clamp(q.Dot(r), -1, 1)) * Math.
        Sign(r.Y));
            }
            MyDetectedEntityInfo µ(IMyCameraBlock ª, ref Vector3D Z)
            {
                double Ó, Ñ, Ð; º(ª, ref Z, out Ó, out Ñ, out Ð); return ª.
Raycast(Ó, (float)Ñ, (float)Ð);
            }
            public class Ï
            {
                public List<IMyCameraBlock> Î; public int Í; public double Ì; public bool Ë; public
IMyCubeGrid Ò; public Vector3I Ê; public Vector3I É; public Vector3I È; public Vector3I Ç; public Func<IMyCubeGrid, Vector3D> Æ; public
Func<IMyCubeGrid, Vector3D> Å; public Func<IMyCubeGrid, Vector3D> Ä; public static Vector3D Ã(IMyCubeGrid J)
                {
                    return J.WorldMatrix.
Up;
                }
                public static Vector3D o(IMyCubeGrid J) { return J.WorldMatrix.Down; }
                public static Vector3D A(IMyCubeGrid J)
                {
                    return J.
WorldMatrix.Left;
                }
                public static Vector3D M(IMyCubeGrid J) { return J.WorldMatrix.Right; }
                public static Vector3D L(IMyCubeGrid J)
                {
                    return J.WorldMatrix.Forward;
                }
                public static Vector3D K(IMyCubeGrid J) { return J.WorldMatrix.Backward; }
                public static Func<
IMyCubeGrid, Vector3D> N(Base6Directions.Direction G)
                {
                    switch (G)
                    {
                        case Base6Directions.Direction.Up: return Ã;
                        case Base6Directions.
Direction.Down:
                            return o;
                        case Base6Directions.Direction.Left: return A;
                        case Base6Directions.Direction.Right: return M;
                        case
Base6Directions.Direction.Forward:
                            return L;
                        case Base6Directions.Direction.Backward: return K;
                        default: return L;
                    }
                }
                public static Vector3I H
(Base6Directions.Direction G, int F, int E, int D, int C, int B, int I)
                {
                    switch (G)
                    {
                        case Base6Directions.Direction.Up:
                            return new
Vector3I((F + E) / 2, C, (B + I) / 2);
                        case Base6Directions.Direction.Down: return new Vector3I((F + E) / 2, D, (B + I) / 2);
                        case Base6Directions.
Direction.Left:
                            return new Vector3I(F, (D + C) / 2, (B + I) / 2);
                        case Base6Directions.Direction.Right:
                            return new Vector3I(E, (D + C) / 2, (B + I) / 2)
;
                        case Base6Directions.Direction.Forward: return new Vector3I((F + E) / 2, (D + C) / 2, B);
                        case Base6Directions.Direction.Backward:
                            return new Vector3I((F + E) / 2, (D + C) / 2, I);
                        default: return new Vector3I((F + E) / 2, (D + C) / 2, B);
                    }
                }
                Vector3D P(ref Vector3D Z, ref Vector3I
n)
                { return Z - Ò.GridIntegerToWorld(n); }
                public bool l(ref Vector3D Z)
                {
                    Vector3D j = (Ë ? Vector3D.Zero : Ä(Ò)); Vector3D h = Ì * Å(Ò);
                    Vector3D g = Ì * Æ(Ò); if (Ì >= 0)
                    {
                        return (P(ref Z, ref Ç).Dot(j + h) >= 0 && P(ref Z, ref È).Dot(j - h) >= 0 && P(ref Z, ref É).Dot(j + g) >= 0 && P(ref Z,
ref Ê).Dot(j - g) >= 0);
                    }
                    else
                    {
                        return (P(ref Z, ref Ç).Dot(j + h) >= 0 || P(ref Z, ref È).Dot(j - h) >= 0 || P(ref Z, ref É).Dot(j + g) >= 0 || P(ref
Z, ref Ê).Dot(j - g) >= 0);
                    }
                }
            }
        }
    
}
