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

        QuadTree(AABB boundary)
        {
            points = new Vector[maxNodeCount];
            Boundary = boundary;
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

        void SubDivide() {
            Vector size = Boundary.halfDimension / 2;

            Vector centre = new Vector(Boundary.centre.x - Boundary.halfDimension.x, Boundary.centre.y - Boundary.halfDimension.y);
            northWest = new QuadTree(new AABB(centre, size));

            centre = new Vector(Boundary.centre.x + Boundary.halfDimension.x, Boundary.centre.y - Boundary.halfDimension.y);
            northEast = new QuadTree(new AABB(centre, size));
            
            centre = new Vector(Boundary.centre.x - Boundary.halfDimension.x, Boundary.centre.y + Boundary.halfDimension.y);
            southWest = new QuadTree(new AABB(centre, size));
            
            centre = new Vector(Boundary.centre.x + Boundary.halfDimension.x, Boundary.centre.y + Boundary.halfDimension.y);
            southEast = new QuadTree(new AABB(centre, size));
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
