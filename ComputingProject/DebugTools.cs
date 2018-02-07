using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ComputingProject {
    public class DebugTools {
        // Bool that is responsible for printing generic information to the console
        public static bool DebugMode = false;
        // Does the simulation use collision detection?
        public static bool UseCollision = true;
        // Bool that draws lines that represent the velocity
        public static bool DrawVelocityArrows = true;
        // If true, print the velocities after a collision
        public static bool PrintCollisionVelocities = true;
        // Update the visuals or not
        public static bool UpdateVisuals = true;
        // Print the forces to the console
        public static bool PrintForces = true;
    }
}
