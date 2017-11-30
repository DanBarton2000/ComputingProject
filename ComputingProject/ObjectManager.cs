using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputingProject.Collision;

namespace ComputingProject
{
    public class ObjectManager
    {
        #region Variables

        private static Vector screenBounds;
        private static double fx, fy;

        public static List<IQuadtreeObject> AllObjects { get; private set; } = new List<IQuadtreeObject>();
        #endregion

        #region Static Methods
        /// <summary>
        /// Find the first object with the name "name" and then return it
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The object with the name</returns>
        public static IQuadtreeObject FindObjectWithName(string name) {
            IQuadtreeObject co = AllObjects.First(s => s.Name == name);
            return co;
        }

        /// <summary>
        /// Returns a list of objects of type "type"
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of objects of type "type"</returns>
        public static List<IQuadtreeObject> FindObjectsOfType<T>() where T : IQuadtreeObject {
            return AllObjects.Where(x => x.GetType() == typeof(T)).ToList();
        }

        public static void Update(double timeStep, double scale, QuadTree<IQuadtreeObject> tree, double velocityRebound = -1) {

            // Checking to make sure that there is a Quadtree available
            if (tree == null)
                throw new Exception("Tree is null!");

            // If the simulation is paused, don't update
            if (TimeController.isPaused) {
                return;
            }

            Dictionary<IQuadtreeObject, double[]> forces = new Dictionary<IQuadtreeObject, double[]>();
            foreach (IQuadtreeObject co in AllObjects) {
                fx = 0;
                fy = 0;
                foreach (IQuadtreeObject cobj in AllObjects) {
                    if (!EqualityComparer<IQuadtreeObject>.Default.Equals(co, cobj)) {
                        double[] force = co.Attraction(cobj);
                        fx += force[0];
                        fy += force[1];
                    }
                }
                double[] totalForces = new double[2] { fx, fy };
                forces.Add(co, totalForces);
            }

            foreach (IQuadtreeObject co in AllObjects) {
                double[] f = forces[co];
                double massTimeStep = co.Mass * timeStep;
                double x = f[0] / massTimeStep;
                double y = f[1] / massTimeStep;

                co.velocity = new Vector(co.velocity.x + x, co.velocity.y + y);

                double addPositionX = co.velocity.x * timeStep * scale;
                double addPositionY = co.velocity.y * timeStep * scale;

                co.position.x += addPositionX;
                co.position.y += addPositionY;

                if (co.collider != null) {
                    if (co.collider.colliderType == ColliderType.Circle) {
                        // Update the position of the circle collider.
                        CircleCollider cc = (CircleCollider)co.collider;
                        cc.centre.Set(co.position.x, co.position.y);
                    }
                    else if (co.collider.colliderType == ColliderType.Polygon) {
                        // Update the position of the vertices on the polygon collider
                        PolygonCollider pc = (PolygonCollider)co.collider;
                        foreach (Vector vert in pc.Vertices) {
                            vert.x += addPositionX;
                            vert.y += addPositionY;
                        }
                    }
                }

                if (DebugTools.UseCollision) {
                    UpdateCollision(tree);
                }

                // Check if the object is outside the screen. 
                // If it is, invert the velocity.
                if (co.position.x < 0 || co.position.x > screenBounds.x) {
                    co.velocity = new Vector(co.velocity.x * velocityRebound, co.velocity.y);
                }
                else if (co.position.y < 0 || co.position.y > screenBounds.y) {
                    co.velocity = new Vector(co.velocity.x, co.velocity.y * velocityRebound);
                }

                if (DebugTools.DebugMode) {
                    Console.WriteLine("OBJ: " + co.Name + " X: " + co.position.x + " Y: " + co.position.y);
                }
            }
        }

        public static void AddObject(IQuadtreeObject co) {
            AllObjects.Add(co);
        }

        public static void SetScreenBounds(Vector bounds) {
            screenBounds = bounds;
        }

        public static void UpdateCollision(QuadTree<IQuadtreeObject> tree) {

            // Insert objects into the tree
            foreach (IQuadtreeObject obj in AllObjects) {
                tree.Insert(obj);
            }

            // The centre of the query range
            // Currently will be just the size of the screen
            Vector centre = new Vector(tree.Boundary.centre.x, tree.Boundary.centre.y);

            // The half size of the query range
            Vector size = centre;

            AABB range = new AABB(centre, size);

            // Getting the objects that are in range
            List<IQuadtreeObject> objects =  tree.QueryRange(range);

            bool hasCollided = false;

            foreach (IQuadtreeObject obj in objects) {
                foreach (IQuadtreeObject quadObj in objects) {
                    if (obj != quadObj) {
                        hasCollided = SAT.IsColliding(obj.collider, quadObj.collider);
                        if (hasCollided && !obj.collider.isColliding && !quadObj.collider.isColliding) {

                            obj.colour = System.Drawing.Brushes.Black;
                            quadObj.colour = System.Drawing.Brushes.Black;

                            // Update the velocity

                            // Formula to work velocity after two objects collide.
                            // https://gamedevelopment.tutsplus.com/tutorials/when-worlds-collide-simulating-circle-circle-collisions--gamedev-769

                            /*newVelX1 = (firstBall.speed.x * (firstBall.mass – secondBall.mass) +(2 * secondBall.mass * secondBall.speed.x)) / (firstBall.mass + secondBall.mass);
                            newVelY1 = (firstBall.speed.y * (firstBall.mass – secondBall.mass) +(2 * secondBall.mass * secondBall.speed.y)) / (firstBall.mass + secondBall.mass);
                            newVelX2 = (secondBall.speed.x * (secondBall.mass – firstBall.mass) +(2 * firstBall.mass * firstBall.speed.x)) / (firstBall.mass + secondBall.mass);
                            newVelY2 = (secondBall.speed.y * (secondBall.mass – firstBall.mass) +(2 * firstBall.mass * firstBall.speed.y)) / (firstBall.mass + secondBall.mass);
                            */

                            // Combined masses
                            double masses = quadObj.Mass + obj.Mass;

                            // Work out the velocity of object 1
                            Vector vel1 = new Vector();
                            vel1.x = (obj.velocity.x * (obj.Mass - quadObj.Mass) + (2 * quadObj.Mass * quadObj.velocity.x)) / masses;
                            vel1.y = (obj.velocity.y * (obj.Mass - quadObj.Mass) + (2 * quadObj.Mass * quadObj.velocity.y)) / masses;
                            //obj.velocity = new Velocity(obj.velocity.x + vel1.x, obj.velocity.y + vel1.y);

                            // Then set the velocity to the new velocity

                            Console.WriteLine("Velocity 1: " + vel1.ToString());

                            obj.velocity += vel1;

                            // Work out the velocity of object 2
                            Vector vel2 = new Vector();
                            vel2.x = (quadObj.velocity.x * (quadObj.Mass - obj.Mass) + (2 * obj.Mass * obj.velocity.x)) / masses;
                            vel2.y = (quadObj.velocity.y * (quadObj.Mass - obj.Mass) + (2 * obj.Mass * obj.velocity.y)) / masses;
                            //quadObj.velocity = new Velocity(quadObj.velocity.x + vel2.x, quadObj.velocity.y + vel2.y);

                            Console.WriteLine("Velocity 2: " + vel2.ToString());

                            quadObj.velocity += vel2;

                            obj.collider.isColliding = true;
                            quadObj.collider.isColliding = true;

                            if (DebugTools.DebugMode) {
                                Console.WriteLine("Vel1: " + vel1.ToString());
                                Console.WriteLine("Vel2: " + vel2.ToString());
                            }
                        }
                        else {
                            obj.collider.isColliding = false;
                            quadObj.collider.isColliding = false;
                        }
                    }
                }
            }

            // Once every collision has been calculated, the quad is cleared.
            tree.ClearQuad();
        }
        #endregion
    }
}
