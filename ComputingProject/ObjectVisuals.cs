﻿using System;
using System.Xml.Serialization;
using System.Windows.Media;

namespace ComputingProject {
    [Serializable]
    [XmlRoot("ObjectVisuals")]
    public class ObjectVisuals {
        [XmlIgnore]
        public SolidColorBrush colour { get { return _colour;  } set { _colour = value; _colourName = new BrushConverter().ConvertToString(value); } }
        SolidColorBrush _colour;
        [XmlElement(ElementName = "size")]
        public int size { get; set; }

        [XmlElement(ElementName = "colourName")]
        public string colourName { get { return _colourName; } set { _colourName = value; colour = (SolidColorBrush)new BrushConverter().ConvertFromString(colourName); } }
        private string _colourName;

        BrushConverter converter = new BrushConverter();

        public ObjectVisuals() {
            colour = Brushes.White;
            size = 40;
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
            return "Colour: " + colour.ToString() + " Size: " + size.ToString();
        }
    }
}
