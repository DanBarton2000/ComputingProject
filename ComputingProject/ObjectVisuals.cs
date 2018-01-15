using System.Windows.Media;

namespace ComputingProject {
    public class ObjectVisuals {
        public SolidColorBrush colour { get; private set; }
        public int size { get; private set; }

        public ObjectVisuals() {
            colour = Brushes.White;
            size = 40;
        }

        public ObjectVisuals(SolidColorBrush colour, int size) {
            Set(colour, size);
        }

        public void Set(SolidColorBrush colour) {
            if (colour == null) {
                this.colour = Brushes.White;
            }
            this.colour = colour;
        }

        public void Set(int size) {
            if (size < 5) {
                this.size = 5;
            }
            else if (size > 200) {
                this.size = 200;
            }
            else {
                this.size = size;
            }
        }

        public void Set(SolidColorBrush colour, int size) {
            Set(colour);
            Set(size);
        }

        public override string ToString() {
            return "Colour: " + colour.ToString() + " Size: " + size.ToString();
        }
    }
}
