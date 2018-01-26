using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    [Serializable]
    [XmlInclude(typeof(CircleCollider))]
    [XmlInclude(typeof(PolygonCollider))]
    public class Collider2D
    {
        public bool isColliding = false;

        [XmlElement(ElementName = "colliderType")]
        public ColliderType colliderType { get; set; }

        public Collider2D() { }
    }
}
