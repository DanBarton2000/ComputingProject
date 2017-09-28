﻿using System;
using System.Windows.Shapes;
//using System.Windows.Media;
using System.Drawing;

namespace ComputingProject
{
    public class CelestialObject
    {
        #region Variables
        private string name;

        // Mass of the objects measured in kilogrammes
        private double mass;
        private double totalVelocity;

        public Brush colour;

        public double Mass { get { return mass; } set { if (value > 0) { mass = value; } } }
        public double TotalVelocity { get { return totalVelocity; } set {
                double radians = Bearing * Constants.DegreesToRadians;
                velocity.x = value * Math.Cos(radians);
                velocity.y = value * Math.Sin(radians);
                totalVelocity = value;
            }
        } // Split into X and Y 
        public Velocity velocity;

        public double Bearing { get; set; } // The bearing in degrees

        public Vector position { get; set; }
    
        public string Name { get { return name; } set { if (value != null || value != "") name = value; } }

        #endregion

        #region Methods

        public CelestialObject() { }
        
        public CelestialObject(string name, double mass, double velocity, double bearing, Vector position, Brush colour) {
            this.name = name;
            this.mass = mass;
            TotalVelocity = velocity;
            Bearing = bearing;
            this.position = position;
            this.colour = colour;
            ObjectManager.allObjects.Add(this);
        }

        public double[] Attraction(CelestialObject co) {
            double[] forces = new double[2];
            double distance = Vector.DistanceSqr(this.position, co.position);

            if (distance == 0) {
                Console.WriteLine("Objects {0} {1} are on top of each other!", this.Name, co.Name);
                return null;
            }

            // Using the formula F = GMm/d^2
            double force = (Constants.Gravitational * this.Mass * co.Mass) / distance;

            double differenceX = Vector.DifferenceX(this.position, co.position);
            double differenceY = Vector.DifferenceY(this.position, co.position);

            double theta = Math.Atan2(differenceY, differenceX);

            double forceX = force * Math.Cos(theta);
            double forceY = force * Math.Sin(theta);

            forces[0] = forceX;
            forces[1] = forceY;
            
            return forces;
        }

        #endregion

        #region Static Methods
        public static CelestialObject SpawnCopy(CelestialObject co, Vector v) {
            return new CelestialObject(co.Name + "_COPY", co.Mass, co.totalVelocity, co.Bearing, v, co.colour);
        }

        #endregion

        public struct Velocity{
            public double x;
            public double y;
            Velocity(double x, double y){
                this.x = x;
                this.y = y;
            }
        }
    }
}
