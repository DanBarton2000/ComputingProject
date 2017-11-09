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

        private static double fx, fy;

        public static List<CelestialObject> AllObjects { get; private set; } = new List<CelestialObject>();
        #endregion

        #region Static Methods
        /// <summary>
        /// Find the first object with the name "name" and then return it
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The object with the name</returns>
        public static CelestialObject FindObjectWithName(string name) {
            CelestialObject co = AllObjects.First(s => s.Name == name);
            return co;
        }

        /// <summary>
        /// Returns a list of objects of type "type"
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of objects of type "type"</returns>
        public static List<CelestialObject> FindObjectsOfType(Type type) {
            return AllObjects.Where(x => x.GetType() == type).ToList();
        }

        public static void Update(double timeStep, double scale) {

            if (TimeController.isPaused) {
                return;
            }

            Dictionary<CelestialObject, double[]> forces = new Dictionary<CelestialObject, double[]>();
            foreach (CelestialObject co in AllObjects) {
                fx = 0;
                fy = 0;
                foreach (CelestialObject cobj in AllObjects) {
                    if (co != cobj) {
                        double[] force = co.Attraction(cobj);
                        fx += force[0];
                        fy += force[1];
                    }
                }
                double[] totalForces = new double[2] { fx, fy };
                forces.Add(co, totalForces);
            }

            foreach (CelestialObject co in AllObjects) {
                double[] f = forces[co];
                double massTimeStep = co.Mass * timeStep;
                co.velocity.x += f[0] / massTimeStep;
                co.velocity.y += f[1] / massTimeStep;

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
                if (DebugTools.DebugMode) {
                    Console.WriteLine("OBJ: " + co.Name + " X: " + co.position.x + " Y: " + co.position.y);
                }
            }
        }

        public static void AddObject(CelestialObject co) {
            AllObjects.Add(co);
        }
        #endregion
    }
}
