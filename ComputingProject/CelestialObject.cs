﻿using System;
using System.Windows.Shapes;
//using System.Windows.Media;
using System.Drawing;
using ComputingProject.Collision;
using ComputingProject;

namespace ComputingProject
{
    public class CelestialObject : IQuadtreeObject
    {
        #region Variables
        private string name;

        // Mass of the objects measured in kilogrammes
        private double mass;
        private double totalVelocity;

        private Vector2 _position;
        private Vector2 _screenPosition;

        public double Mass { get { return mass; } set { if (value > 0) { mass = value; } } }
        public double TotalVelocity { get { return totalVelocity; } set {
                double radians = Bearing * Constants.DegreesToRadians;
                double x = value * Math.Cos(radians);
                double y = value * Math.Sin(radians);
                velocity = new Vector2(x, y);

                if (DebugTools.DebugMode) {
                    Console.WriteLine("Celestial Object - Degrees to radians: " + Constants.DegreesToRadians);
                    Console.WriteLine("Celestial Object - Bearing: " + Bearing);
                    Console.WriteLine("Celestial Object - Radians: " + radians);
                    Console.WriteLine("Celestial Object - Velocity:" + velocity.ToString());
                }

                totalVelocity = value;
            }
        } // Split into X and Y 
        public Vector2 velocity { get; set; }

        public double Bearing { get; set; } // The bearing in degrees

        public Vector2 position { get { return _position; } set {
                if (collider != null) {

                    // Update the position of the circle collider
                    if (collider.colliderType == ColliderType.Circle) {
                        CircleCollider col = (CircleCollider)collider;
                        col.centre.Set(value.x, value.y);
                    }

                    // Update the position of the polygon vertices
                    else if (collider.colliderType == ColliderType.Polygon) {
                        PolygonCollider col = (PolygonCollider)collider;

                        for (int i = 0; i < col.Vertices.Count; i++) {
                            col.Vertices[i] += value;
                        }
                    }
                }

                _position = value;
            }
        }

        public  Vector2 screenPosition { get { if (_screenPosition == null) { return new Vector2(); } else { return _screenPosition; } } set { _screenPosition = value; } }
    
        public string Name { get { return name; } set {
                if (value != null || value != "") {
                    name = value;
                }
                else {
                    Console.WriteLine("Not a valid name!");
                }
            }
        }

        public Collider2D collider { get; set; }

        // Radius of the graphic and the collider (if it is a circle collider)
        public double radius { get { return radius; } set { if (value > 0 && value < 100) { radius = value; } } } 
        #endregion
         
        #region Methods

        /// <summary>
        /// Empty constuctor, creates an empty object but does not assign any values too it
        /// </summary>
        public CelestialObject() { }
        
        /// <summary>
        /// Constuctor that takes in a bearing and a velocity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="velocity"></param>
        /// <param name="bearing"></param>
        /// <param name="position"></param>
        /// <param name="col"></param>
        public CelestialObject(string name, double mass, double velocity, double bearing, Vector2 position, Collider2D col) {
            this.name = name;
            this.mass = mass;
            Bearing = bearing;
            TotalVelocity = velocity;
            this.position = position;
            collider = col;

            screenPosition = new Vector2();

            if (collider != null) {
                if (collider.colliderType == ColliderType.Circle) {
                    CircleCollider cc = (CircleCollider)collider;
                    cc.centre.Set(position.x, position.y);
                }
            }

            ObjectManager.AddObject(this);
        }


        /// <summary>
        /// Constructor that take in a velocity of type Vector
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="vel"></param>
        /// <param name="position"></param>
        /// <param name="col"></param>
        public CelestialObject(string name, double mass, Vector2 velocity, Vector2 position, Collider2D col) {
            this.name = name;
            this.mass = mass;

            this.velocity = velocity;
            this.position = position;
            collider = col;

            screenPosition = new Vector2();

            if (collider != null) {
                if (collider.colliderType == ColliderType.Circle) {
                    CircleCollider cc = (CircleCollider)collider;
                    cc.centre.Set(position.x, position.y);
                }
            }

            ObjectManager.AddObject(this);
        }

        public CelestialObject(string name, double mass, Vector2 velocity, Collider2D col, Vector2 screenPosition) {
            this.name = name;
            this.mass = mass;

            this.velocity = velocity;
            this.screenPosition = screenPosition;

            position = new Vector2();

            collider = col;

            screenPosition = new Vector2();

            if (collider != null) {
                if (collider.colliderType == ColliderType.Circle) {
                    CircleCollider cc = (CircleCollider)collider;
                    cc.centre.Set(position.x, position.y);
                }
            }



            ObjectManager.AddObject(this);
        }

        /// <summary>
        /// Calculate the force between this object and another object then return the values
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public double[] Attraction(IQuadtreeObject co) {

            if (co == null) {
                return null;
            }

            double[] forces = new double[2];

            double distance = Vector2.DistanceSqr(position, co.position);


            // If the objects are on top of each other, a DivideByZero error would occur so return
            if (distance == 0) {
                Console.WriteLine("Objects {0} {1} are on top of each other!", Name, co.Name);
                return null;
            }

            // Using the formula F = GMm/d^2
            double force = (Constants.Gravitational * Mass * co.Mass) / distance;

            double differenceX = Vector2.DifferenceX(position, co.position);
            double differenceY = Vector2.DifferenceY(position, co.position);

            // Calculate the angle between the objects
            double theta = Math.Atan2(differenceY, differenceX);

            // Split the force in the x and y components
            double forceX = force * Math.Cos(theta);
            double forceY = force * Math.Sin(theta);

            forces[0] = forceX;
            forces[1] = forceY;

            return forces;
        }

        /// <summary>
        /// Returns the name, position, velocity and the mass of the object in a string format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Name: " + name + " Position: " + position.ToString() + " Velocity: " + velocity.ToString() + " Mass: " + mass;
        }

        #endregion

        #region Static Methods
        public static CelestialObject SpawnCopy(CelestialObject co, Vector2 newPosition) {
            CelestialObject coObj = new CelestialObject(co.Name + "_COPY", co.Mass, new Vector2(co.velocity.x, co.velocity.y), newPosition, co.collider);

            if (coObj.collider != null) {
                if (coObj.collider.colliderType == ColliderType.Circle) {
                    CircleCollider cc = (CircleCollider)coObj.collider;
                    cc.centre.Set(newPosition.x, newPosition.y);
                }
                else if (coObj.collider.colliderType == ColliderType.Polygon) {
                    Vector2 difference = co.position - newPosition;
                    PolygonCollider pc = (PolygonCollider)coObj.collider;

                    // Update the position of the vertices
                    foreach (Vector2 vert in pc.Vertices) {
                        vert.x += difference.x;
                        vert.y += difference.y;
                    }
                }
            }
            return coObj;
        }

        #endregion

    }
}
