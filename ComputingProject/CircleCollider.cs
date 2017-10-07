using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    class CircleCollider : Collider2D
    {
        public Vector centre { get; }
        public double radius { get; }

        CircleCollider() {
            colliderType = ColliderType.Circle;
        }
    }
}
