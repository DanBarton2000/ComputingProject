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

        public static List<CelestialObject> allObjects = new List<CelestialObject>();
        #endregion

        #region Methods
        public static CelestialObject FindObjectWithName(string name) {
            CelestialObject co = allObjects.First(s => s.Name == name);
            return co;
        }

        public static List<CelestialObject> FindObjectsOfType(Type type) {
            return allObjects.Where(x => x.GetType() == type).ToList();
        }

        public static void Update(double timeStep, double scale) {

            if (TimeController.isPaused) {
                return;
            }

            Dictionary<CelestialObject, double[]> forces = new Dictionary<CelestialObject, double[]>();
            foreach (CelestialObject co in allObjects) {
                fx = 0;
                fy = 0;
                foreach (CelestialObject cobj in allObjects) {
                    if (co != cobj) {
                        double[] force = co.Attraction(cobj);
                        fx += force[0];
                        fy += force[1];
                    }
                }
                double[] totalForces = new double[2] { fx, fy };
                forces.Add(co, totalForces);
            }

            foreach (CelestialObject co in allObjects) {
                double[] f = forces[co];
                double massTimeStep = co.Mass * timeStep;
                co.velocity.x += f[0] / massTimeStep;
                co.velocity.y += f[1] / massTimeStep;

                co.position.x += co.velocity.x * timeStep * scale;
                co.position.y += co.velocity.y * timeStep * scale;

                if (co.collider != null) {
                    if (co.collider.colliderType == ColliderType.Circle) {
                        // Update the position of the circle collider.
                        CircleCollider cc = (CircleCollider)co.collider;
                        cc.centre.Set(co.position.x, co.position.y);
                    }
                    else if (co.collider.colliderType == ColliderType.Polygon) {
                        // Update the position of the vertices
                        PolygonCollider pc = (PolygonCollider)co.collider;
                        foreach (Vector vert in pc.Vertices) {
                            vert.x += co.velocity.x * timeStep * scale;
                            vert.y += co.velocity.y * timeStep * scale;
                        }
                    }
                }

                Console.WriteLine("OBJ: " + co.Name + " X: " + co.position.x + " Y: " + co.position.y);
            }
        }
        #endregion
    }
}
