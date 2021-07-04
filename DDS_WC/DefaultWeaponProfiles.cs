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
    
    public class DefaultWeaponProfiles
        {

        const double GATLING_INITIAL_SPEED = 400;
        const double GATLING_ACCELERATION = 0;
        const double GATLING_MAX_SPEED = 400;
        const double GATLING_MAX_RANGE = 800;
        const double GATLING_SPAWN_OFFSET = 0;
        const double GATLING_RELOAD_TIME = 0;
        const bool GATLING_IS_CAPPED_SPEED = false;
        const bool GATLING_USE_SALVO = true;

        const double ROCKET_INITIAL_SPEED = 100;
        const double ROCKET_ACCELERATION = 600;
        const double ROCKET_MAX_SPEED = 200;
        const double ROCKET_MAX_RANGE = 800;
        const double ROCKET_SPAWN_OFFSET = 4;
        const double ROCKET_RELOAD_TIME = 1;
        const bool ROCKET_IS_CAPPED_SPEED = true;
        const bool ROCKET_USE_SALVO = true;

        public WeaponProfile gatlingProfile = new WeaponProfile(GATLING_INITIAL_SPEED, GATLING_ACCELERATION, GATLING_MAX_SPEED, GATLING_MAX_RANGE, GATLING_SPAWN_OFFSET, GATLING_RELOAD_TIME, GATLING_IS_CAPPED_SPEED, GATLING_USE_SALVO);
            public WeaponProfile rocketProfile = new WeaponProfile(ROCKET_INITIAL_SPEED, ROCKET_ACCELERATION, ROCKET_MAX_SPEED, ROCKET_MAX_RANGE, ROCKET_SPAWN_OFFSET, ROCKET_RELOAD_TIME, ROCKET_IS_CAPPED_SPEED, ROCKET_USE_SALVO);
        }
   
}
