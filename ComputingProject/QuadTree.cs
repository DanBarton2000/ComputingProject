using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class QuadTree<T> where T : IQuadtreeObject
    {
        readonly int maxNodeCount = 4;
        
        public AABB Boundary { get; }

        T[] points;

        QuadTree<T> northWest;
        QuadTree<T> northEast;
        QuadTree<T> southWest;
        QuadTree<T> southEast;

        QuadTree() {
            points = new T[maxNodeCount];
        }

        public QuadTree(AABB boundary)
        {
            points = new T[maxNodeCount];
            Boundary = boundary;
        }

        /// <summary>
        /// Insert a point into the quadtree
        /// </summary>
        /// <param name="point">The point being inserted into the quadtree</param>
        /// <returns></returns>
        public bool Insert(T point) {
            if (!Boundary.ContainsPoint(point.position)) {
                return false;
             }

            for (int i = 0; i < points.Length; i++) {
                if (points[i] == null) {
                    points[i] = point;
                    return true;
                }
            }

            if (northWest == null) {
                SubDivide();
            }

            if (northWest.Insert(point))
                return true;

            if (northEast.Insert(point))
                return true;

            if (southWest.Insert(point))
                return true;

            if (southEast.Insert(point))
                return true;

            return false;
        }

        /// <summary>
        /// Delete a point from the quadtree
        /// </summary>
        /// <param name="point"></param>
        public void Delete(T point) {
            for (int i = 0; i < points.Length; i++) {
                if (EqualityComparer<T>.Default.Equals(points[i], point)) {
                    points[i] = default(T);
                }
            }
        }
        
        /// <summary>
        /// SubDivide the current quadtree
        /// </summary>
        public void SubDivide() {
            Vector2 size = Boundary.halfDimension / 2;

            Vector2 centre = new Vector2(Boundary.centre.x - Boundary.halfDimension.x, Boundary.centre.y - Boundary.halfDimension.y);
            northWest = new QuadTree<T>(new AABB(centre, size));

            centre = new Vector2(Boundary.centre.x + Boundary.halfDimension.x, Boundary.centre.y - Boundary.halfDimension.y);
            northEast = new QuadTree<T>(new AABB(centre, size));
            
            centre = new Vector2(Boundary.centre.x - Boundary.halfDimension.x, Boundary.centre.y + Boundary.halfDimension.y);
            southWest = new QuadTree<T>(new AABB(centre, size));
            
            centre = new Vector2(Boundary.centre.x + Boundary.halfDimension.x, Boundary.centre.y + Boundary.halfDimension.y);
            southEast = new QuadTree<T>(new AABB(centre, size));
        }

        /// <summary>
        /// Query's a range using recursion
        /// </summary>
        /// <param name="range">What range should be queried</param>
        /// <returns></returns>
        public List<T> QueryRange(AABB range) {
            List<T> pointsInRange = new List<T>();

            if (!Boundary.IntersectsAABB(range)) {
                return pointsInRange;
            }

            foreach (T point in points) {
                if (!EqualityComparer<T>.Default.Equals(point, default(T))) {
                    if (range.ContainsPoint(point.position)) {
                        pointsInRange.Add(point);
                    }
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

        /// <summary>
        /// Clears the Quadtree by recursion
        /// </summary>
        public void ClearQuad() {

            if (northWest != null) {
                northWest.ClearQuad();
            }

            northEast = null;
            northWest = null;
            southEast = null;
            southWest = null;
        }
    }

}
