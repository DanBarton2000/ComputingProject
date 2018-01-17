﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    [Serializable]
    public class CircleCollider : Collider2D
    {
        public Vector2 centre { get; private set; }
        public double radius { get; private set; }

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
