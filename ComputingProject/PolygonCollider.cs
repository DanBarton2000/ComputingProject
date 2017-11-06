using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class PolygonCollider : Collider2D
    {
        public List<Vector> Vertices { get; }

        PolygonCollider() {
            colliderType = ColliderType.Polygon;
        }
    }
}
