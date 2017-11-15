using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputingProject.Collision;

namespace ComputingProject {
    public interface IQuadtreeObject {
        Collider2D collider { get; set; }
        Vector position { get; set; }
    }
}
