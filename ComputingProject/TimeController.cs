using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject {
    class TimeController {
        double defaultTimeStep;
        public double currentTimeStep { get; private set; }
        double pauseTimeStep;

        public static bool isPaused { get; private set; } = false;

        TimeController(double timeStep) {
            defaultTimeStep = timeStep;
            currentTimeStep = timeStep;
        }

        void Pause() {
            if (isPaused)
                return;

            isPaused = true;
            pauseTimeStep = currentTimeStep;
            currentTimeStep = 0;
        }

        void UnPause() {
            if (!isPaused)
                return;

            isPaused = false;
            currentTimeStep = pauseTimeStep;
        }

        void SpeedUp(double scalar) {
            currentTimeStep *= scalar;
        }

        void DefaultSpeed() {
            currentTimeStep = defaultTimeStep;
        }

    }
}
