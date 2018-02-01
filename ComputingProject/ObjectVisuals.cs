using System;
using System.Xml.Serialization;
using System.Windows.Media;

namespace ComputingProject {
    [Serializable]
    [XmlRoot("ObjectVisuals")]
    public class ObjectVisuals {
        [XmlIgnore]
        public SolidColorBrush colour { get { return _colour;  } set { _colour = value; _colourName = new BrushConverter().ConvertToString(value); } }
        SolidColorBrush _colour;
        [XmlIgnore]
        int _size;
        [XmlElement(ElementName = "size")]
        public int size { get { return _size; } set { if (value > 0 && value < 100) { _size = value; } } }

        [XmlElement(ElementName = "colourName")]
        public string colourName { get { return _colourName; } set { _colourName = value; colour = (SolidColorBrush)new BrushConverter().ConvertFromString(colourName); } }
        private string _colourName;

        BrushConverter converter = new BrushConverter();

        public ObjectVisuals() {
            colour = Brushes.White;
            size = 20;
        }

        public ObjectVisuals(SolidColorBrush colour, int size) {
            Set(colour, size);
        }

        public ObjectVisuals(string colourName, int size) {
            Set(colourName, size);
        }

        public void Set(SolidColorBrush colour) {
            if (colour == null) {
                this.colour = Brushes.White;
            }
            this.colour = colour;
            colourName = converter.ConvertToString(this.colour);
        }

        public void Set(int size) {
            if (size < 5) {
                this.size = 5;
            }
            else if (size > 150) {
                this.size = 150;
            }
            else {
                this.size = size;
            }
        }

        public void Set(SolidColorBrush colour, int size) {
            Set(colour);
            Set(size);
        }

        public void Set(string colourName, int size) {
            try {
                colour = (SolidColorBrush)new BrushConverter().ConvertFromString(colourName);
            }
            catch {
                Console.WriteLine("Colour not valid.");
            }
            Set(size);
        }

        public override string ToString() {
            return "Colour: " + colourName + " Size: " + size.ToString();
        }
    }
}
