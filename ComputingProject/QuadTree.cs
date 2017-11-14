﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class QuadTree<T> where T : class
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
            Type type = typeof(T);
            if (type.GetType() != typeof(CelestialObject) || type.GetType() != typeof(Vector)) {
                throw new ApplicationException("Invalid type!");
            }
            points = new T[maxNodeCount];
            Boundary = boundary;
        }

        public bool Insert(T point) {
            if (point.GetType() == typeof(CelestialObject)) {
                CelestialObject co = point as CelestialObject;
                if (!Boundary.ContainsPoint(co.position)) {
                    return false;
                }
            }
            else if (point.GetType() == typeof(Vector)) {
                Vector vec = point as Vector;
                if (!Boundary.ContainsPoint(vec)) {
                        return false;
                }
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

        public void Delete(T point) {
            for (int i = 0; i < points.Length; i++) {
                if (points[i] == point) {
                    points[i] = null;
                }
            }
        }

        public void SubDivide() {
            Vector size = Boundary.halfDimension / 2;

            Vector centre = new Vector(Boundary.centre.x - Boundary.halfDimension.x, Boundary.centre.y - Boundary.halfDimension.y);
            northWest = new QuadTree<T>(new AABB(centre, size));

            centre = new Vector(Boundary.centre.x + Boundary.halfDimension.x, Boundary.centre.y - Boundary.halfDimension.y);
            northEast = new QuadTree<T>(new AABB(centre, size));
            
            centre = new Vector(Boundary.centre.x - Boundary.halfDimension.x, Boundary.centre.y + Boundary.halfDimension.y);
            southWest = new QuadTree<T>(new AABB(centre, size));
            
            centre = new Vector(Boundary.centre.x + Boundary.halfDimension.x, Boundary.centre.y + Boundary.halfDimension.y);
            southEast = new QuadTree<T>(new AABB(centre, size));
        }

        public List<T> QueryRange(AABB range) {
            List<T> pointsInRange = new List<T>();

            if (!Boundary.IntersectsAABB(range)) {
                return pointsInRange;
            }

            foreach (T point in points) {
                if (point.GetType() == typeof(CelestialObject)) {
                    CelestialObject co = point as CelestialObject;
                    if (range.ContainsPoint(co.position)) {
                        pointsInRange.Add(point);
                    }

                }
                else if (point.GetType() == typeof(Vector)) {
                    Vector vec = point as Vector;
                    if (range.ContainsPoint(vec)) {
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

        public void ClearQuad() {
            northEast = null;
            northWest = null;
            southEast = null;
            southWest = null;
        }
    }

}
