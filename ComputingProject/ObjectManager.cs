using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputingProject.Collision;

namespace ComputingProject
{
    public class ObjectManager<T> where T : IQuadtreeObject 
    {
        #region Variables

        private static Vector screenBounds;
        private static double fx, fy;

        public static List<T> AllObjects { get; private set; } = new List<T>();
        #endregion

        #region Static Methods
        /// <summary>
        /// Find the first object with the name "name" and then return it
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The object with the name</returns>
        public static T FindObjectWithName(string name) {
            T co = AllObjects.First(s => s.Name == name);
            return co;
        }

        /// <summary>
        /// Returns a list of objects of type "type"
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of objects of type "type"</returns>
        public static List<T> FindObjectsOfType<K>() where K : IQuadtreeObject {
            return AllObjects.Where(x => x.GetType() == typeof(K)).ToList();
        }

        public static void Update(double timeStep, double scale, QuadTree<IQuadtreeObject> tree, double velocityRebound = -1) {

            if (TimeController.isPaused) {
                return;
            }

            Dictionary<T, double[]> forces = new Dictionary<T, double[]>();
            foreach (T co in AllObjects) {
                fx = 0;
                fy = 0;
                foreach (T cobj in AllObjects) {
                    if (!EqualityComparer<T>.Default.Equals(co, cobj)) {
                        double[] force = co.Attraction(cobj);
                        fx += force[0];
                        fy += force[1];
                    }
                }
                double[] totalForces = new double[2] { fx, fy };
                forces.Add(co, totalForces);
            }

            foreach (T co in AllObjects) {
                double[] f = forces[co];
                double massTimeStep = co.Mass * timeStep;
                double x = f[0] / massTimeStep;
                double y = f[1] / massTimeStep;

                co.velocity = new Velocity(co.velocity.x + x, co.velocity.y + y);

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

                UpdateCollision(tree);

                // Check if the object is outside the screen. 
                // If it is, invert the velocity.
                if (co.position.x < 0 || co.position.x > screenBounds.x) {
                    co.velocity = new Velocity(co.velocity.x * velocityRebound, co.velocity.y);
                }
                else if (co.position.y < 0 || co.position.y > screenBounds.y) {
                    co.velocity = new Velocity(co.velocity.x, co.velocity.y * velocityRebound);
                }

                if (DebugTools.DebugMode) {
                    Console.WriteLine("OBJ: " + co.Name + " X: " + co.position.x + " Y: " + co.position.y);
                }
            }
        }

        public static void AddObject(T co) {
            AllObjects.Add(co);
        }

        public static void SetScreenBounds(Vector bounds) {
            screenBounds = bounds;
        }

        public static void UpdateCollision(QuadTree<IQuadtreeObject> tree) {

            Vector centre = new Vector();
            Vector size = new Vector();

            AABB range = new AABB(centre, size);

            List<IQuadtreeObject> objects =  tree.QueryRange(range);

            bool hasCollided = false;

            foreach (IQuadtreeObject obj in objects) {
                foreach (IQuadtreeObject quadObj in objects) {
                    if (obj != quadObj) {
                        hasCollided = SAT.IsColliding(obj.collider, quadObj.collider);
                    }
                }
            }

            // Once every collision has been calculated, the quad is cleared.
            tree.ClearQuad();

            // Then insert the objects back into the quadtree.
            foreach (IQuadtreeObject obj in AllObjects) {
                tree.Insert(obj);
            }
        }
        #endregion
    }
}
