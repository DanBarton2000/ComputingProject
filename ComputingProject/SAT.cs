using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class SAT
    {

        public static bool IsColliding(Collider2D col1, Collider2D col2) {
            // Check for circle collision
            if (col1.colliderType == ColliderType.Circle && col2.colliderType == ColliderType.Circle) {
                return IsCollidingCircles((CircleCollider)col1, (CircleCollider)col2);
            }

            // Check for polygon circle collision
            else if (col1.colliderType == ColliderType.Polygon && col2.colliderType == ColliderType.Circle) {
                return IsCollidingPolygonCircle((PolygonCollider)col1, (CircleCollider)col2);
            }

            // Check for polygon collision
            else if (col1.colliderType == ColliderType.Polygon && col2.colliderType == ColliderType.Polygon) {
                return IsCollidingPolygons((PolygonCollider)col1, (PolygonCollider)col2);
            }

            // Should never reach here unless one of the colliders is null
            return false;
        }

        public static bool IsCollidingCircles(CircleCollider col1, CircleCollider col2) {
            Vector v = col1.centre - col2.centre;
            if (v.Magnitude < col1.radius + col2.radius) {
                return true;
            }
            return false;
        }

        public static bool IsCollidingPolygonCircle(PolygonCollider col1, CircleCollider col2) {
            List<Vector> edges = new List<Vector>();
            edges = CalculateEdges(col1.Vertices);
            for (int i = 0; i < edges.Count; i++) {
                if (Vector.Distance(col2.centre, edges[i]) < col2.radius) {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCollidingPolygons(PolygonCollider col1, PolygonCollider col2) {
            List<Vector> edges = CalculateEdges(col1.Vertices);
            edges.AddRange(CalculateEdges(col2.Vertices));

            List<Vector> normals = new List<Vector>();

            foreach (Vector edge in edges) {
                normals.Add(CalculateNormal(edge));
            }

            foreach (Vector normal in normals) {
                bool isSeparated = IsSeparatingAxis(normal, col1, col2);
                if (!isSeparated) {
                    return false;
                }
            }

            return true;
        }

        static List<Vector> CalculateEdges(List<Vector> vertices) {
            List<Vector> edges = new List<Vector>();
            int length = vertices.Count;

            for (int i = 0; i < length; i++) {
                Vector edge = vertices[(i + 1) % length] - vertices[i];
                edges.Add(edge);
            }
            return edges;
        }

        public static Vector CalculateNormal(Vector point) {
            return new Vector(-point.y, point.x);
        }

        static bool IsSeparatingAxis(Vector normal, PolygonCollider col1, PolygonCollider col2) {
            double min1 = double.PositiveInfinity;
            double max1 = double.NegativeInfinity;
            double min2 = double.PositiveInfinity;
            double max2 = double.NegativeInfinity;

            foreach (Vector point in col1.Vertices) {
                double projection = Vector.Dot(point, normal);
                min1 = Math.Min(min1, projection);
                max1 = Math.Max(max1, projection);
            }

            foreach (Vector point in col2.Vertices) {
                double projection = Vector.Dot(point, normal);
                min2 = Math.Min(min2, projection);
                max2 = Math.Max(max2, projection);
            }

            if (max1 >= min2 && max2 >= min1) {
                return false;
            }

            return true;
        }
    }
}
