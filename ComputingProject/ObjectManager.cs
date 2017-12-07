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
        /// Returns a list of objects of type "T"
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
                        if (force != null) {
                            fx += force[0];
                            fy += force[1];
                        }
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

                if (DebugTools.UseCollision) {
                    UpdateCollision(tree);
                }


                // If the velocity becomes too high, set it to a lower value
                /*if (co.velocity.x > 500) {
                    co.velocity.x = 300;
                }
                else if (co.velocity.y > 500) {
                    co.velocity.y = 300;
                } */ 

                double addPositionX = co.velocity.x * timeStep * scale;
                double addPositionY = co.velocity.y * timeStep * scale;

                co.position += new Vector(addPositionX, addPositionX);

                // Check if the object is outside the screen. 
                // If it is, invert the velocity.
                if (co.position.x < 0 || co.position.x > screenBounds.x) {
                    co.velocity = new Vector(co.velocity.x * velocityRebound, co.velocity.y * Math.Abs(velocityRebound));
                }
                else if (co.position.y < 0 || co.position.y > screenBounds.y) {
                    co.velocity = new Vector(co.velocity.x * Math.Abs(velocityRebound), co.velocity.y * velocityRebound);
                }

                // Print the position of the object to the console
                if (DebugTools.DebugMode) {
                    Console.WriteLine("Object Manager -  OBJ: " + co.Name + " X: " + co.position.x + " Y: " + co.position.y);
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
                            // Update the velocity after collision

                            // Combined masses
                            double masses = quadObj.Mass + obj.Mass;
                   
                            Vector[] velocities = OnCollision(obj, quadObj);

                            obj.velocity = velocities[0];
                            quadObj.velocity = velocities[1];

                            obj.collider.isColliding = true;
                            quadObj.collider.isColliding = true;

                            if (DebugTools.PrintCollisionVelocities) {
                                // Print out the name of the class, the name of the objects and the velocity of each object
                                Console.WriteLine("(1) Object Manager - Name: " + obj.Name + "\t Velocity: " + obj.velocity.ToString());
                                Console.WriteLine("(2) Object Manager - Name: " + quadObj.Name + "\t Velocity: " + quadObj.velocity.ToString());
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

        // Update the velocites of the objects
        static Vector[] OnCollision(IQuadtreeObject obj1, IQuadtreeObject obj2) {

            // Implement MTV to fix issues
            // https://blogs.msdn.microsoft.com/faber/2013/01/09/elastic-collisions-of-balls/

            Vector[] velocities = new Vector[2];

            double combinedMasses = obj1.Mass + obj2.Mass;
            double differenceObj1Obj2Mass = obj1.Mass - obj2.Mass;
            double differenceObj2Obj1Mass = obj2.Mass - obj1.Mass;

            double obj1Horizontal = obj1.velocity.x * ((differenceObj1Obj2Mass) / (combinedMasses)) + ((2 * obj2.Mass * obj2.velocity.x) / (combinedMasses));
            double obj1Vertical = obj1.velocity.y * ((differenceObj1Obj2Mass) / (combinedMasses)) + ((2 * obj2.Mass * obj2.velocity.y) / (combinedMasses));

            double obj2Horizontal = obj2.velocity.x * ((differenceObj2Obj1Mass) / (combinedMasses)) + ((2 * obj1.Mass * obj1.velocity.x) / (combinedMasses));
            double obj2Vertical = obj2.velocity.y * ((differenceObj2Obj1Mass) / (combinedMasses)) + ((2 * obj1.Mass * obj1.velocity.y) / (combinedMasses));

            velocities[0] = new Vector(obj1Horizontal, obj1Vertical);
            velocities[1] = new Vector(obj2Horizontal, obj2Vertical);

            return velocities;
        }
        #endregion
    }
}
