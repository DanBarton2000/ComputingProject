using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    struct AABB
    {
        public Vector centre;
        public Vector halfDimension;

        public AABB(Vector centre, Vector halfDimension) {
            this.centre = centre;
            this.halfDimension = halfDimension;
        }

        public bool ContainsPoint(Vector point) {
            double minX = centre.x - halfDimension.x;
            double minY = centre.y - halfDimension.y;
            double maxX = centre.x + halfDimension.x;
            double maxY = centre.y + halfDimension.y;

            if (point.x < minX || point.x > maxX || point.y < minY || point.y > maxY) {
                return false;
            }
            return true;
        }

        public bool IntersectsAABB(AABB box) {
            bool minX = centre.x + halfDimension.x > box.centre.x - box.halfDimension.x;
            bool maxX = centre.x - halfDimension.x < box.centre.x + box.halfDimension.x;
            bool minY = centre.y + halfDimension.y > box.centre.y + box.halfDimension.y;
            bool maxY = centre.y - halfDimension.y < box.centre.y + box.halfDimension.y;

            if (minX || maxX || minY || maxY) {
                return true;
            }
            return false;
        }
    }
}
