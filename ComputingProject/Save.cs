using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ComputingProject
{
    public class Save
    {
        [XmlArray(ElementName = "objects")]
        static List<CelestialObject> objects;

        /// <summary>
        /// Writes a xml file from a file path
        /// </summary>
        /// <param name="filePath"></param>
        public static void WriteXML(string filePath) {
            XmlSerializer writer = new XmlSerializer(typeof(List<CelestialObject>));

            Console.WriteLine("Path: " + filePath);

            objects = new List<CelestialObject>();

            ObjectManager.AllObjects.ForEach(x => objects.Add(x as CelestialObject));

            using (FileStream stream = File.Create(filePath)) {
                writer.Serialize(stream, objects);
            }
        }

        /// <summary>
        /// Writes a xml file from a file path and a filename
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filename"></param>
        public static void WriteXML(string filePath, string filename) {
            XmlSerializer writer = new XmlSerializer(typeof(List<CelestialObject>));

            string path = filePath + "\\" + filename + ".xml";

            Console.WriteLine("Path: " + path);

            objects = new List<CelestialObject>();

            ObjectManager.AllObjects.ForEach(x => objects.Add(x as CelestialObject));

            using (FileStream stream = File.Create(path)) {
                writer.Serialize(stream, objects);
            }
        }
    }
}
