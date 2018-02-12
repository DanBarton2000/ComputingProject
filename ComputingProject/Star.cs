using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputingProject.Collision;
using System.Xml.Serialization;

namespace ComputingProject
{
    [Serializable]
    [XmlRoot(ElementName = "Star")]
    public class Star : CelestialObject
    {
        public Star() { }

        public Star(string name, double mass, Vector2 velocity, Vector2 position, Collider2D col, ObjectVisuals visuals) : base(name, mass, velocity, position, col, visuals) {

        }

        [XmlElement]
        public double SolarMassOfStar { get {
                return Mass / Constants.SolarMass;
            }
        }

        [XmlElement]
        public double Luminosity { get {
                return Constants.SolarLuminosity * Math.Pow(SolarMassOfStar, 3.5);
            }
        }

        /// <summary>
        /// Calculates the lifetime of a star
        /// </summary>
        /// <returns></returns>
        public double LifeTime() {
            return Math.Pow(Mass/Constants.SolarMass, -2.5) * Math.Pow(10, 10);
        }

        /// <summary>
        /// Calculates the habitable zone of the star
        /// </summary>
        /// <returns></returns>
        public double[] HabitableZone() {
            double[] zone = new double[2];

            double absoluteLuminosity = 4.85 - 2.5 * Math.Log(Luminosity/Constants.SolarLuminosity);

            double outer = Math.Sqrt(absoluteLuminosity / 0.53);
            double inner = Math.Sqrt(absoluteLuminosity / 1.1);

            zone[0] = outer;
            zone[1] = inner;

            return zone;
        }
    }
}
