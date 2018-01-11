using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputingProject.Collision;

namespace ComputingProject {
    public interface IQuadtreeObject {
        Collider2D collider { get; set; }
        Vector2 position { get; set; }
        Vector2 screenPosition { get; set; }
        string Name { get; set; }
        Vector2 velocity { get; set; }
        double Mass { get; set; }

        // Used for both gravitational and eletrical fields
        double[] Attraction(IQuadtreeObject co);
    }
}
