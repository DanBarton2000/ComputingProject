using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    public class CircleCollider : Collider2D
    {
        public Vector centre { get; private set; }
        public double radius { get; private set; }

        public CircleCollider(Vector centre, double radius) {
            Console.WriteLine("?");
            colliderType = ColliderType.Circle;
            this.centre = centre;
            this.radius = radius;
        }
    }
}
