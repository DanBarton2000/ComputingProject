using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class PolygonCollider : Collider2D
    {
        public List<Vector2> Vertices { get; private set; } = new List<Vector2>();

        PolygonCollider(List<Vector2> vertices) {
            colliderType = ColliderType.Polygon;
            Vertices = vertices;
        }
    }
}
