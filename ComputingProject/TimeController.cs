using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject {
    public class TimeController {
        double defaultTimeStep;
        public double currentTimeStep { get; private set; }
        double pauseTimeStep;

        public static bool isPaused { get; private set; } = false;

        public TimeController(double timeStep) {
            defaultTimeStep = timeStep;
            currentTimeStep = timeStep;
        }

        /// <summary>
        /// Pause the simulation
        /// </summary>
        public void Pause() {
            if (isPaused)
                return;

            isPaused = true;
            pauseTimeStep = currentTimeStep;
            currentTimeStep = 0;
        }

        /// <summary>
        /// Un pause the simulation
        /// </summary>
        public void UnPause() {
            if (!isPaused)
                return;

            isPaused = false;
            currentTimeStep = pauseTimeStep;
        }

        /// <summary>
        /// Change the speed by a scalar
        /// </summary>
        /// <param name="scalar"></param>
        public void SpeedUp(double scalar) {
            currentTimeStep *= scalar;
        }

        /// <summary>
        /// The timestep is set to the default timestep
        /// </summary>
        public void DefaultSpeed() {
            currentTimeStep = defaultTimeStep;
        }

        public override string ToString()
        {
            return "Current Time Step: " + currentTimeStep + 
                   "\nDefault Time Step: " + defaultTimeStep;
        }
    }
}
