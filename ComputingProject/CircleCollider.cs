using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class CircleCollider : Collider2D
    {
        public Vector2 centre { get; private set; }
        public double radius { get; private set; }

        public CircleCollider(Vector2 centre, double radius) {
            colliderType = ColliderType.Circle;
            this.centre = centre;
            this.radius = radius;
        }
    }
}
