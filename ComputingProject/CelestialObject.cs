using System;
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

        private Vector _position;

        public Brush colour { get; set; }

        public double Mass { get { return mass; } set { if (value > 0) { mass = value; } } }
        public double TotalVelocity { get { return totalVelocity; } set {
                double radians = Bearing * Constants.DegreesToRadians;
                double x = value * Math.Cos(radians);
                double y = value * Math.Sin(radians);
                velocity = new Vector(x, y);

                if (DebugTools.DebugMode) {
                    Console.WriteLine("Celestial Object - Degrees to radians: " + Constants.DegreesToRadians);
                    Console.WriteLine("Celestial Object - Bearing: " + Bearing);
                    Console.WriteLine("Celestial Object - Radians: " + radians);
                    Console.WriteLine("Celestial Object - Velocity:" + velocity.ToString());
                }

                totalVelocity = value;
            }
        } // Split into X and Y 
        public Vector velocity { get; set; }

        public double Bearing { get; set; } // The bearing in degrees

        public Vector position { get { return _position; } set {
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

        public CelestialObject() { }
        
        /// <summary>
        /// Constuctor that takes in a bearing and a velocity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mass"></param>
        /// <param name="velocity"></param>
        /// <param name="bearing"></param>
        /// <param name="position"></param>
        /// <param name="colour"></param>
        /// <param name="col"></param>
        public CelestialObject(string name, double mass, double velocity, double bearing, Vector position, Brush colour, Collider2D col) {
            this.name = name;
            this.mass = mass;
            Bearing = bearing;
            TotalVelocity = velocity;
            this.position = position;
            this.colour = colour;
            collider = col;

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
        /// <param name="colour"></param>
        /// <param name="col"></param>
        public CelestialObject(string name, double mass, Vector vel,  Vector position, Brush colour, Collider2D col) {
            this.name = name;
            this.mass = mass;

            velocity = new Vector();

            velocity.x = vel.x;
            velocity.y = vel.y;
            this.position = position;
            this.colour = colour;
            collider = col;

            if (collider != null) {
                if (collider.colliderType == ColliderType.Circle) {
                    CircleCollider cc = (CircleCollider)collider;
                    cc.centre.Set(position.x, position.y);
                }
            }

            ObjectManager.AddObject(this);
        }

        /// <summary>
        /// Calculate the force between this object and another object
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public double[] Attraction(IQuadtreeObject co) {
            double[] forces = new double[2];
            double distance = Vector.DistanceSqr(position, co.position);

            if (distance == 0) {
                Console.WriteLine("Objects {0} {1} are on top of each other!", Name, co.Name);
                return null;
            }

            // Using the formula F = GMm/d^2
            double force = (Constants.Gravitational * Mass * co.Mass) / distance;

            double differenceX = Vector.DifferenceX(position, co.position);
            double differenceY = Vector.DifferenceY(position, co.position);

            double theta = Math.Atan2(differenceY, differenceX);

            double forceX = force * Math.Cos(theta);
            double forceY = force * Math.Sin(theta);

            forces[0] = forceX;
            forces[1] = forceY;

            return forces;
        }
    
        #endregion

        #region Static Methods
        /// <summary>
        /// Spawns a copy of an object
        /// </summary>
        /// <param name="co"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public static CelestialObject SpawnCopy(CelestialObject co, Vector newPosition) {
            CelestialObject coObj = new CelestialObject(co.Name + "_COPY", co.Mass, new Vector(co.velocity.x, co.velocity.y), newPosition, co.colour, co.collider);

            if (coObj.collider != null) {
                if (coObj.collider.colliderType == ColliderType.Circle) {
                    CircleCollider cc = (CircleCollider)coObj.collider;
                    cc.centre.Set(newPosition.x, newPosition.y);
                }
                else if (coObj.collider.colliderType == ColliderType.Polygon) {
                    Vector difference = co.position - newPosition;
                    PolygonCollider pc = (PolygonCollider)coObj.collider;

                    // Update the position of the vertices
                    foreach (Vector vert in pc.Vertices) {
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
