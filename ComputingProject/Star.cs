﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject
{
    public class Star : CelestialObject
    {
        public double SolarMassOfStar { get { return this.Mass / Constants.SolarMass; } }
        public double Luminosity { get {
                return Constants.SolarLuminosity * Math.Pow(SolarMassOfStar, 3.5);
            }
        }

        public double LifeTime() {
            return (this.SolarMassOfStar / Constants.SolarMass) * Math.Pow(10, 10);
        }

        public double[] HabitableZone() {
            double[] zone = new double[2];
            double outer = Math.Sqrt(Luminosity/0.53);
            double inner = Math.Sqrt(Luminosity/1.1);

            zone[0] = outer;
            zone[1] = inner;

            return zone;
        }
    }
}
