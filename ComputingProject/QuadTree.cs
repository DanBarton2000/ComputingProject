using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject.Collision
{
    class QuadTree
    {
        int maxNodeCount = 4;
        
        public AABB Boundary { get; }

        Vector[] points;

        QuadTree northWest;
        QuadTree northEast;
        QuadTree southWest;
        QuadTree southEast;
    }
}
