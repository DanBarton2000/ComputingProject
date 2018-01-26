using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    [Serializable]
    public class CircleCollider : Collider2D
    {
        [XmlElement(ElementName = "centre")]
        public Vector2 centre { get; set; }
        [XmlElement(ElementName = "radius")]
        public double radius { get; set; }

        public CircleCollider() { }

        public CircleCollider(Vector2 centre, double radius) {
            colliderType = ColliderType.Circle;
            this.centre = centre;
            this.radius = radius;
        }

        public override string ToString()
        {
            return "Centre: " + centre.ToString() + " Radius: " + radius;
        }
    }
}
