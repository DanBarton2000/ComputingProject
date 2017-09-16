using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    class QuadTree
    {
        readonly int maxNodeCount = 4;
        
        public AABB Boundary { get; }

        Vector[] points;

        QuadTree northWest;
        QuadTree northEast;
        QuadTree southWest;
        QuadTree southEast;

        QuadTree() {
            points = new Vector[maxNodeCount];
        }

        bool Insert(Vector point) {
            if (!Boundary.ContainsPoint(point)) {
                return false;
            }

            for (int i = 0; i < points.Length; i++) {
                if (points[i] == null) {
                    points[i] = point;
                    return true;
                }
            }

            return false;
        }

        List<Vector> QueryRange(AABB range) {
            List<Vector> pointsInRange = new List<Vector>();

            if (!Boundary.IntersectsAABB(range)) {
                return pointsInRange;
            }

            for (int i = 0; i < points.Length; i++) {
                if (range.ContainsPoint(points[i])) {
                    pointsInRange.Add(points[i]);
                }
            }

            if (northWest == null) {
                return pointsInRange;
            }

            pointsInRange.AddRange(northWest.QueryRange(range));
            pointsInRange.AddRange(northEast.QueryRange(range));
            pointsInRange.AddRange(southWest.QueryRange(range));
            pointsInRange.AddRange(southEast.QueryRange(range));

            return pointsInRange;
        }
    }
}
