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

        private static Vector2 screenBounds;
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

        /// <summary>
        /// Called once per frame
        /// Updates the positions of the objects
        /// </summary>
        /// <param name="timeStep"></param>
        /// <param name="scale"></param>
        /// <param name="tree"></param>
        /// <param name="velocityRebound"></param>
        public static void Update(double timeStep, double scale, QuadTree<IQuadtreeObject> tree, double velocityRebound = -1) {

            // Checking to make sure that there is a Quadtree available
            if (tree == null)
                throw new Exception("Quadtree is null!");

            // If the simulation is paused, don't update
            if (TimeController.isPaused) {
                return;
            }

            // Check to see if there are objects in the list
            // if not return
            if (AllObjects == null) {
                return;
            }

            Dictionary<IQuadtreeObject, double[]> forces = new Dictionary<IQuadtreeObject, double[]>();
            foreach (IQuadtreeObject co in AllObjects.ToList()) {
                fx = 0;
                fy = 0;
                foreach (IQuadtreeObject cobj in AllObjects.ToList()) {

                    if (co != cobj) {

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

            if (DebugTools.DebugMode) {
                forces.ToList().ForEach(x => Console.WriteLine("Object Manager - Key: " + x.Key.Name + "\tValues: " + x.Value.GetValue(0) + " : " + x.Value.GetValue(1) + "\n"));
            }

            foreach (IQuadtreeObject co in AllObjects.ToList()) {
                double[] f = forces[co];
                double massTimeStep = co.Mass * timeStep;
                double x = f[0] / massTimeStep;
                double y = f[1] / massTimeStep;

                // Update the velocity
                co.velocity.Add(x, y);

                // Update collisions
                if (DebugTools.UseCollision) {
                    UpdateCollision(tree);
                }

                // Update the position of the object
                co.position.x += co.velocity.x * timeStep;
                co.position.y += co.velocity.y * timeStep;

                co.screenPosition = co.position * scale;

                // Check if the object is outside the screen. 
                // If it is, invert the velocity.
                if (screenBounds != null && co != null) {
                    if (co.position != null) {
                        if (co.position.x < 0 || co.position.x > screenBounds.x) {
                            co.velocity = new Vector2(co.velocity.x * velocityRebound, co.velocity.y * Math.Abs(velocityRebound));
                        }
                        else if (co.position.y < 0 || co.position.y > screenBounds.y) {
                            co.velocity = new Vector2(co.velocity.x * Math.Abs(velocityRebound), co.velocity.y * velocityRebound);
                        }
                    }
                }

                // Print the position of the object to the console
                if (DebugTools.DebugMode) {
                    Console.WriteLine("Object Manager - OBJ: " + co.Name + " \tPosition - " + (co.position * scale).ToString());
                    Console.WriteLine("Object Manager - OBJ: " + co.Name + " \tVelocity - " + co.velocity.ToString());
                    Console.WriteLine("Object Manager - OBJ: " + co.Name + " \tForces   - X: " + f[0] + " Y: " + f[1] + "\n");
                }
            }
        }

        /// <summary>
        /// Add an object to the list
        /// </summary>
        /// <param name="co"></param>
        public static void AddObject(IQuadtreeObject co) {
            AllObjects.Add(co);
        }

        public static void AddRange(List<IQuadtreeObject> objects) {
            AllObjects.AddRange(objects);
        }

        public static void ClearObjects() {
            AllObjects.Clear();
        }

        public static void SetScreenBounds(Vector2 bounds) {
            screenBounds = bounds;
        }

        /// <summary>
        /// Perform the necessary checks for collision between objects
        /// </summary>
        /// <param name="tree"></param>
        public static void UpdateCollision(QuadTree<IQuadtreeObject> tree) {

            // Insert objects into the tree
            foreach (IQuadtreeObject obj in AllObjects) {
                tree.Insert(obj);
            }

            // The centre of the query range
            // Currently will be just the size of the screen
            Vector2 centre = new Vector2(tree.Boundary.centre.x, tree.Boundary.centre.y);

            // The half size of the query range
            Vector2 size = centre;

            AABB range = new AABB(centre, size);

            // Getting the objects that are in range
            List<IQuadtreeObject> objects =  tree.QueryRange(range);

            bool hasCollided = false;

            foreach (IQuadtreeObject obj in objects) {
                foreach (IQuadtreeObject quadObj in objects) {
                    if (obj != quadObj) {
                        hasCollided = SAT.IsColliding(obj.collider, quadObj.collider);
                        if (hasCollided && !obj.collider.isColliding && !quadObj.collider.isColliding) {

                            // Update the velocity after collision

                            // Combined masses
                            double masses = quadObj.Mass + obj.Mass;
                   
                            Vector2[] velocities = OnCollision(obj, quadObj);

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
        static Vector2[] OnCollision(IQuadtreeObject obj1, IQuadtreeObject obj2) {

            // Implement MTV to fix issues
            // https://blogs.msdn.microsoft.com/faber/2013/01/09/elastic-collisions-of-balls/

            Vector2[] velocities = new Vector2[2];

            double combinedMasses = obj1.Mass + obj2.Mass;
            double differenceObj1Obj2Mass = obj1.Mass - obj2.Mass;
            double differenceObj2Obj1Mass = obj2.Mass - obj1.Mass;

            double obj1Horizontal = obj1.velocity.x * ((differenceObj1Obj2Mass) / (combinedMasses)) + ((2 * obj2.Mass * obj2.velocity.x) / (combinedMasses));
            double obj1Vertical = obj1.velocity.y * ((differenceObj1Obj2Mass) / (combinedMasses)) + ((2 * obj2.Mass * obj2.velocity.y) / (combinedMasses));

            double obj2Horizontal = obj2.velocity.x * ((differenceObj2Obj1Mass) / (combinedMasses)) + ((2 * obj1.Mass * obj1.velocity.x) / (combinedMasses));
            double obj2Vertical = obj2.velocity.y * ((differenceObj2Obj1Mass) / (combinedMasses)) + ((2 * obj1.Mass * obj1.velocity.y) / (combinedMasses));

            velocities[0] = new Vector2(obj1Horizontal, obj1Vertical);
            velocities[1] = new Vector2(obj2Horizontal, obj2Vertical);

            return velocities;
        }
        #endregion
    }
}
