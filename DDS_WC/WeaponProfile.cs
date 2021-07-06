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
    
        public class WeaponProfile
        {
            public double InitialSpeed;
            public double Acceleration; 
            public double MaxSpeed; 
            public double MaxRange; 
            public double SpawnOffset; 
            public double ReloadTime; 
            public bool IsCappedSpeed; 
            public bool UseSalvo;
        
            public WeaponProfile(double initialSpeed, double acceleration, double maxSpeed, double maxRange, double spawnOffset, double reloadTime, bool isCappedSpeed, bool useSalvo)
            {
                InitialSpeed = initialSpeed;
                Acceleration = acceleration;
                MaxSpeed = maxSpeed;
                MaxRange = maxRange;
                SpawnOffset = spawnOffset;
                ReloadTime = reloadTime;
                IsCappedSpeed = isCappedSpeed;
                UseSalvo = useSalvo;
            }
            public Vector3D ComputeInterceptPoint(ref Vector3D targetLocation, ref Vector3D targetVelocity, ref Vector3D projectileLocation, ref Vector3D shipDirection, PDCTarget target)
            {
                Vector3D relativeVelocity = (IsCappedSpeed ? targetVelocity : targetVelocity - shipDirection);

                Vector3D z = targetLocation - projectileLocation;
                double k = (Acceleration == 0 ? 0 : (MaxSpeed - InitialSpeed) / Acceleration);
                double p = (0.5 * Acceleration * k * k) + (InitialSpeed * k) - (MaxSpeed * k);

                double a = (MaxSpeed * MaxSpeed) - relativeVelocity.LengthSquared();
                double b = 2 * ((p * MaxSpeed) - relativeVelocity.Dot(z));
                double c = (p * p) - z.LengthSquared(); 

                double t = SolveQuadratic(a, b, c); 
                if (double.IsNaN(t) || t < 0)
                {
                    return new Vector3D(double.NaN);
                }

                Vector3D targetPoint;

                if (target.Acceleration.LengthSquared() > 0.1)
                {
                    targetPoint = targetLocation + (relativeVelocity * t) + (0.5 * target.Acceleration * t * t); 
                } 
                else
                {
                targetPoint = targetLocation + (relativeVelocity * t); 
                }

                if (IsCappedSpeed && MaxSpeed > 0)
                {
                    int u = (int)Math.Ceiling(t *60);

                    Vector3D aimDirection;
                    Vector3D stepAcceleration; 
                    Vector3D currentPosition; 
                    Vector3D currentDirection; 

                    aimDirection = Vector3D.Normalize(targetPoint - projectileLocation); 
                    stepAcceleration = (aimDirection * Acceleration) / 60; 
                    currentPosition = projectileLocation; currentDirection = shipDirection + (aimDirection * InitialSpeed); 
                    for (int i = 0; i < u; i++)
                    {
                        currentDirection += stepAcceleration;
                    
                        double speed = currentDirection.Length(); 
                        if (speed > MaxSpeed) 
                        {
                            currentDirection = currentDirection / speed * MaxSpeed; 
                        }
                        currentPosition += (currentDirection / 60); 
                        if ((i + 1) % 60 == 0) 
                        {
                            if (Vector3D.Distance(projectileLocation, currentPosition) > MaxRange) 
                            {
                                return targetPoint; 
                            } 
                        }
                    }
                    return targetPoint + targetPoint - currentPosition;
                }
                else 
                {
                    return targetPoint; 
                }
            }
            public double SolveQuadratic(double a, double b, double c)
            {
                if (a == 0) 
                { 
                    return -(c / b); 
                }
                double u = (b * b) - (4 * a * c); 
                if (u < 0)
                {
                    return Double.NaN;
                }
                u = Math.Sqrt(u);

                double t1 = (-b + u) / (2 * a); 
                double t2 = (-b - u) / (2 * a); 
                return (t1 > 0 ? (t2 > 0 ? (t1 < t2 ? t1 : t2) : t1) : t2);
            }
        }
    
}
