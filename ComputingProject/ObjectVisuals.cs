using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputingProject {
    public class ObjectVisuals {
        public SolidColorBrush colour;
        public int size;

        public ObjectVisuals(SolidColorBrush colour, int size) {
            if (colour == null) {
                this.colour = Brushes.White;
            }

            this.colour = colour;
            this.size = size;
        }
    }
}
