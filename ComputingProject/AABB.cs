using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    struct AABB
    {
        Vector centre;
        double halfDimension;

        public AABB(Vector centre, double halfDimension) {
            this.centre = centre;
            this.halfDimension = halfDimension;
        }

        public bool ContainsPoint(Vector point) {
            double minX = centre.x - halfDimension;
            double minY = centre.y - halfDimension;
            double maxX = centre.x + halfDimension;
            double maxY = centre.y + halfDimension;

            if (point.x < minX || point.x > maxX || point.y < minY || point.y > maxY) {
                return false;
            }
            return true;
        }

        public bool IntersectsAABB(AABB box) {
            return false;
        }
    }
}
