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
        public static List<IQuadtreeObject> FindObjectsOfType<K>() where K : IQuadtreeObject {
            return AllObjects.Where(x => x.GetType() == typeof(K)).ToList();
        }

        public static void Update(double timeStep, double scale, QuadTree<IQuadtreeObject> tree, double velocityRebound = -1) {

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

                //UpdateCollision(tree);

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

            // Getting the objects that are 
            List<IQuadtreeObject> objects =  tree.QueryRange(range);

            bool hasCollided = false;

            foreach (IQuadtreeObject obj in objects) {
                foreach (IQuadtreeObject quadObj in objects) {
                    if (obj != quadObj) {
                        hasCollided = SAT.IsColliding(obj.collider, quadObj.collider);
                        if (hasCollided) {
                            // Update the velocity
                      
                            /*newVelX1 = (firstBall.speed.x * (firstBall.mass – secondBall.mass) +(2 * secondBall.mass * secondBall.speed.x)) / (firstBall.mass + secondBall.mass);
                            newVelY1 = (firstBall.speed.y * (firstBall.mass – secondBall.mass) +(2 * secondBall.mass * secondBall.speed.y)) / (firstBall.mass + secondBall.mass);
                            newVelX2 = (secondBall.speed.x * (secondBall.mass – firstBall.mass) +(2 * firstBall.mass * firstBall.speed.x)) / (firstBall.mass + secondBall.mass);
                            newVelY2 = (secondBall.speed.y * (secondBall.mass – firstBall.mass) +(2 * firstBall.mass * firstBall.speed.y)) / (firstBall.mass + secondBall.mass);
                            */

                            Velocity vel1 = new Velocity();
                            vel1.x = obj.velocity.x;
                            obj.velocity = new Velocity(obj.velocity.x + vel1.x, obj.velocity.y + vel1.y);

                            Velocity vel2 = new Velocity();
                            vel2.x = quadObj.velocity.x;
                            quadObj.velocity = new Velocity(quadObj.velocity.x + vel2.x, quadObj.velocity.y + vel2.y);                         
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
