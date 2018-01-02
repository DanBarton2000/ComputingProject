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

        public void Pause() {
            if (isPaused)
                return;

            isPaused = true;
            pauseTimeStep = currentTimeStep;
            currentTimeStep = 0;
        }

        public void UnPause() {
            if (!isPaused)
                return;

            isPaused = false;
            currentTimeStep = pauseTimeStep;
        }

        public void SpeedUp(double scalar) {
            currentTimeStep *= scalar;
        }

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
