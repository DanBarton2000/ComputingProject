using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    [Serializable]
    public class PolygonCollider : Collider2D
    {
        [XmlArray]
        public List<Vector2> Vertices { get; set; } = new List<Vector2>();

        public PolygonCollider() { }

        public PolygonCollider(List<Vector2> vertices) {
            colliderType = ColliderType.Polygon;
            Vertices = vertices;
        }
    }
}
