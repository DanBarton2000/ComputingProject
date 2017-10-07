using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    class Collider2D
    {
        public bool isColliding;
        public CelestialObject attachedObject { get; protected set; }
        public ColliderType colliderType { get; protected set; }
    }
}
